using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite; 
using System.Data; 
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEditor;
using UnityEditor.iOS;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class DataBaseController : MonoBehaviour {

	private IDbConnection _dbconn;
	
	private const string CreateMatchesTableQuery = 
		"CREATE TABLE `matches` (" +
		"`gameID` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
		"`P1Score` INTEGER," +
		"`P2Score` INTEGER," +
		"`P3Score` INTEGER," +
		"`P4Score` INTEGER," +
		"`duration` REAL," +
		"`spottime` TEXT," +
		"`howlcount` INTEGER," +
		"`purrcount` INTEGER," +
		"`start` TEXT NOT NULL" +
		");";
	
	private const string CreateMatchTableQuery = 
		"CREATE TABLE `{0}{1}` (" +
	    "`time`	REAL NOT NULL," +
	    "`positionX`	REAL NOT NULL," +
	    "`positionY`	REAL NOT NULL," +
	    "`angleZ`	REAL NOT NULL," +
	    "`see`	INTEGER NOT NULL DEFAULT 0," +
	    "`howl`	INTEGER NOT NULL DEFAULT 0," +
	    "`purr`	INTEGER NOT NULL DEFAULT 0" +
	    ");";
	
	private const string InsertMatchRecordStartQuery = 
		"INSERT INTO matches [(start)] VALUES (\"{0}\");";
	
	private const string InsertMatchRecordEndQuery = 
		"INSERT INTO matches [(P1Score, P2Score, P3Score, P4Score, duration, spottime, howlcount, purrcount)] VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7});";
	
	private const string InsertMatchFrameRecordQuery =
		
		"INSERT INTO `{0}{1}` (time, positionX, positionY, angleZ, see, howl, purr) VALUES (\"{2}\", {3}, {4}, {5}, {6}, {7}, {8});";
	
	private const string SelectMatchIdQuery = 
		"SELECT gameID FROM matches WHERE start=\"{0}\";";
	
	private const string SelectMatchesTableExistance = 
		"SELECT name FROM sqlite_master WHERE type='table' AND name='matches';";
	
	private const string SelectMatchTableExistence = 
		"SELECT name FROM sqlite_master WHERE type='table' AND name='{0}{1}';";
	
	private const string DatabaseConnectionDefinition = "URI=file:{0}/Database/Database.s3db";

	private Hashtable _playersQueryCommands;
	
	[HideInInspector] public Predator Predator1;
	[HideInInspector] public Predator Predator2;
	[HideInInspector] public Predator Predator3;
	[HideInInspector] public Predator Predator4;
	[HideInInspector] public Prey Prey;

	private int _gameId;
	private bool _newGame = true;
	private static bool _created = false;
	private bool _threadRunning = false;
	private string _dataPath;
	private LinkedList<Thread> _threads;

	void Awake()
	{
		if (!_created)
		{
			DontDestroyOnLoad(this.gameObject);
			_created = true;
		}

		_threads = new LinkedList<Thread>();
		_dataPath = Application.dataPath;
		_playersQueryCommands = new Hashtable();
	}
	void Start ()
	{
		_playersQueryCommands.Add("Predator1", new LinkedList<string>());
		_playersQueryCommands.Add("Predator2", new LinkedList<string>());
		_playersQueryCommands.Add("Predator3", new LinkedList<string>());
		_playersQueryCommands.Add("Predator4", new LinkedList<string>());
		_playersQueryCommands.Add("Prey", new LinkedList<string>());
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene().name == "Game") {
			if (_newGame) {
				ClearQueries();
				RecordNewGame();
				_newGame = false;
			}
			RecordFrame();
		}
		else
		{
			if (!_newGame)
			{
				_threads.AddLast(new Thread(FillDatabase));
				_threads.Last.Value.Start();

				_newGame = true;
			}
		}
	}

	private void OnDisable()
	{
		if(_threadRunning)
		{
			_threadRunning = false;
			foreach (var thread in _threads)
			{
				thread.Join();
			}
		}
	}

	private void ClearQueries()
	{
		_playersQueryCommands["Predator1"] = new LinkedList<string>();
		_playersQueryCommands["Predator2"] = new LinkedList<string>();
		_playersQueryCommands["Predator3"] = new LinkedList<string>();
		_playersQueryCommands["Predator4"] = new LinkedList<string>();
		_playersQueryCommands["Prey"] = new LinkedList<string>();
	}
	
	public void FillDatabase()
	{
		_threadRunning = true;
		
		string conn = string.Format(DatabaseConnectionDefinition, _dataPath);
		_dbconn = (IDbConnection) new SqliteConnection(conn);
		_dbconn.Open();
		
		foreach (var command in (LinkedList<string>)_playersQueryCommands["Predator1"])
		{
			print(command);
			InsertIntoTable(command);
		}
		foreach (var command in (LinkedList<string>)_playersQueryCommands["Predator2"])
		{
			print(command);
			InsertIntoTable(command);
		}
		foreach (var command in (LinkedList<string>)_playersQueryCommands["Predator3"])
		{
			print(command);
			InsertIntoTable(command);
		}
		foreach (var command in (LinkedList<string>)_playersQueryCommands["Predator4"])
		{
			print(command);
			InsertIntoTable(command);
		}
		foreach (var command in (LinkedList<string>)_playersQueryCommands["Prey"])
		{
			print(command);
			InsertIntoTable(command);
		}
		
		_dbconn.Close();
		
		_threadRunning = false;
	}
	
	public void InsertIntoTable(string command)
	{
		IDbCommand dbcmd = _dbconn.CreateCommand();
		
		dbcmd.CommandText = command;
		dbcmd.ExecuteNonQuery();
		dbcmd.Dispose();
		dbcmd = null;
	}

	public void RecordNewGame() {
		string conn = string.Format(DatabaseConnectionDefinition, _dataPath);
		IDbConnection dbconn = new SqliteConnection(conn);
		dbconn.Open();
		
		IDbCommand dbcmd = dbconn.CreateCommand();
		String startDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
		
		string sqlQuery = SelectMatchesTableExistance;
		dbcmd.CommandText = sqlQuery;
		IDataReader reader = dbcmd.ExecuteReader();

		string sqlCmd;
		
		if (!reader.Read()) {
			if (!reader.IsClosed) {
				reader.Close();
			}

			sqlCmd = CreateMatchesTableQuery;
			dbcmd.CommandText = sqlCmd;
			dbcmd.ExecuteNonQuery();
		}
		if (!reader.IsClosed) {
			reader.Close();
		}
		
		sqlCmd = string.Format(InsertMatchRecordStartQuery, startDate);
		dbcmd.CommandText = sqlCmd;
		dbcmd.ExecuteNonQuery();
		
		sqlQuery = string.Format(SelectMatchIdQuery, startDate);
		dbcmd.CommandText = sqlQuery;
		reader = dbcmd.ExecuteReader();
		
		int gameID = 0;
			
		while (reader.Read())
		{
			gameID = reader.GetInt32(0);
		}
		reader.Close();
		reader = null;
		dbcmd.Dispose();
		dbcmd = null;

		_gameId = gameID;
		dbconn.Close();
	}

	private void RecordFrame() {
		if (Predator1 != null) RecordPredatorFrame(Predator1.gameObject);
		else Predator1 =  GameObject.Find("/Canvas/Predator1").GetComponent<Predator>();
		try
		{
			if (Predator2 != null) RecordPredatorFrame(Predator2.gameObject);
			else Predator2 = GameObject.Find("/Canvas/Predator2").GetComponent<Predator>();
			if (Predator3 != null) RecordPredatorFrame(Predator3.gameObject);
			else Predator3 = GameObject.Find("/Canvas/Predator3").GetComponent<Predator>();
			if (Predator4 != null) RecordPredatorFrame(Predator4.gameObject);
			else Predator4 = GameObject.Find("/Canvas/Predator4").GetComponent<Predator>();
		}
		catch (Exception ignore)
		{
		}

		if (Prey != null) RecordPreyFrame(Prey.gameObject);	
		else Prey =  GameObject.Find("/Canvas/Prey").GetComponent<Prey>();
	}

	private void RecordPredatorFrame(GameObject player) {
		string see = player.GetComponent<Predator>().SeePrey ? "1" : "0";
		string howl = player.GetComponent<Predator>().IsHowling() ? "1" : "0";
		string purr = player.GetComponent<Predator>().IsPurring() ? "1" : "0";
		
		RecordFrame(player, see, howl, purr);
	}
	
	private void RecordPreyFrame(GameObject player)
	{
		string see = player.GetComponent<Prey>().Notice() ? "1" : "0";
		
		RecordFrame(player, see, "0", "0");
	}
	
	private void RecordFrame(GameObject player, string see, string howl, string purr) {
		string conn = string.Format(DatabaseConnectionDefinition, _dataPath);
		IDbConnection dbconn = new SqliteConnection(conn);
		dbconn.Open();
		
		IDbCommand dbcmd = dbconn.CreateCommand();

		string sqlQuery = string.Format(
			SelectMatchTableExistence,
			_gameId,
			player.name
		);
		dbcmd.CommandText = sqlQuery;
		IDataReader reader = dbcmd.ExecuteReader();

		string sqlCmd;
		
		if (!reader.Read()) {
			if (!reader.IsClosed) {
				reader.Close();
			}
			sqlCmd = string.Format(CreateMatchTableQuery, _gameId, player.name);
			dbcmd.CommandText = sqlCmd;
			dbcmd.ExecuteNonQuery();
		}
		if (!reader.IsClosed) {
			reader.Close();
		}

		dbcmd.Dispose();
		dbcmd = null;
		
		dbconn.Close();
		
		LinkedList<string> queryList = (LinkedList<string>) _playersQueryCommands[player.name]; 
		
		queryList.AddLast(
			string.Format(
				InsertMatchFrameRecordQuery,
				_gameId, player.name,
				DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture), 
				player.transform.position.x, 
				player.transform.position.y, 
				player.transform.eulerAngles.z,  
				see, 
				howl,
				purr
			)
		);
	}
}