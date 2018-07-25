using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite; 
using System.Data; 
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataBaseController : MonoBehaviour {

	private IDbConnection _dbconn;

	[HideInInspector] public Predator Predator1;
	[HideInInspector] public Predator Predator2;
	[HideInInspector] public Predator Predator3;
	[HideInInspector] public Predator Predator4;
	[HideInInspector] public Prey Prey;

	private int _gameId;
	private bool _newGame = true;
	private static bool _created = false;

	void Awake()
	{
		if (!_created)
		{
			DontDestroyOnLoad(this.gameObject);
			_created = true;
		}
	}
	void Start () {

		string conn = "URI=file:" + Application.dataPath + "/Database/Database.s3db"; //Path to database.
		print(conn);
		_dbconn = (IDbConnection) new SqliteConnection(conn);
		_dbconn.Open(); //Open connection to the database.
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene().name == "Game") {
			if (_newGame) {
				RecordNewGame();
				_newGame = false;
			}
			RecordFrame();
		}
		else {
			_newGame = true;
		}
	}

	private void OnApplicationQuit() {
		_dbconn.Close();
	}

	public void RecordNewGame() {
		IDbCommand dbcmd = _dbconn.CreateCommand();
		String startDate = DateTime.Now.ToString();
		print("start date: " + startDate);
		
		string sqlQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='matches';";
		dbcmd.CommandText = sqlQuery;
		IDataReader reader = dbcmd.ExecuteReader();

		string sqlCmd;
		
		if (!reader.Read()) {
			if (!reader.IsClosed) {
				reader.Close();
			}
			sqlCmd = 
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
					"`catch` INTEGER," +
					"`start` TEXT NOT NULL" +
				");";
			dbcmd.CommandText = sqlCmd;
			dbcmd.ExecuteNonQuery();
		}
		if (!reader.IsClosed) {
			reader.Close();
		}
		
		sqlCmd = "INSERT INTO matches (start) VALUES (\"" + startDate + "\");";
		dbcmd.CommandText = sqlCmd;
		dbcmd.ExecuteNonQuery();
		
		sqlQuery = "SELECT gameID FROM matches WHERE start=\"" + startDate + "\";";
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
	}

	private void RecordFrame() {
		print("record frame");
		if (Predator1 != null) RecordPredatorFrame(Predator1.gameObject);
		else Predator1 =  GameObject.Find("/Canvas/Predator1").GetComponent<Predator>();
//		if (Predator2 != null) RecordPredatorFrame(Predator2.gameObject);
//		else Predator2 =  GameObject.Find("/Canvas/Predator2").GetComponent<Predator>();
//		if (Predator3 != null) RecordPredatorFrame(Predator3.gameObject);
//		else Predator3 =  GameObject.Find("/Canvas/Predator3").GetComponent<Predator>();
//		if (Predator4 != null) RecordPredatorFrame(Predator4.gameObject);
//		else Predator4 =  GameObject.Find("/Canvas/Predator4").GetComponent<Predator>();
//		if (Prey != null) RecordPreyFrame(Prey.gameObject);	
//		else Prey =  GameObject.Find("/Canvas/Prey").GetComponent<Prey>();
	}

	private void RecordPredatorFrame(GameObject Player) {
		IDbCommand dbcmd = _dbconn.CreateCommand();

		print("1: " +  DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
		string sqlQuery = string.Format(
			"SELECT name FROM sqlite_master WHERE type='table' AND name='{0}{1}';",
			_gameId,
			Player.name
		);
		dbcmd.CommandText = sqlQuery;
		IDataReader reader = dbcmd.ExecuteReader();

		print("2: " +  DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
		string sqlCmd;
		
		if (!reader.Read()) {
			if (!reader.IsClosed) {
				reader.Close();
			}
			sqlCmd = string.Format(
				"CREATE TABLE `{0}{1}` (" +
				"`time`	REAL NOT NULL," +
				"`positionX`	REAL NOT NULL," +
				"`positionY`	REAL NOT NULL," +
				"`angleZ`	REAL NOT NULL," +
				"`find`	INTEGER NOT NULL DEFAULT 0," +
				"`see`	INTEGER NOT NULL DEFAULT 0," +
				"`howl`	INTEGER NOT NULL DEFAULT 0," +
				"`purr`	INTEGER NOT NULL DEFAULT 0" +
				");",
				_gameId, Player.name
			);
			dbcmd.CommandText = sqlCmd;
			
			dbcmd.ExecuteNonQuery();
		}
		if (!reader.IsClosed) {
			reader.Close();
		}

		print("3: " +  DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
		sqlCmd = string.Format(
			"INSERT INTO `{0}{1}` (time, positionX, positionY, angleZ, see, howl, purr) VALUES (\"{2}\", {3}, {4}, {5}, {6}, {7}, {8});",
			_gameId, Player.name,
			DateTime.Now.ToString(), 
			Player.transform.position.x, 
			Player.transform.position.y, 
			Player.transform.eulerAngles.z, 
			Player.GetComponent<Predator>().SeePrey, 
			Player.GetComponent<Predator>().IsHowling(),
			Player.GetComponent<Predator>().IsPurring()
			);
		dbcmd.CommandText = sqlCmd;
		dbcmd.ExecuteNonQuery();
		
		print("4: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
		dbcmd.Dispose();
		dbcmd = null;
	}
	
	private void RecordPreyFrame(GameObject Player) {
		IDbCommand dbcmd = _dbconn.CreateCommand();

		string sqlQuery = string.Format(
			"SELECT name FROM sqlite_master WHERE type='table' AND name='{0}{1}';",
			_gameId,
			Player.name
		);
		dbcmd.CommandText = sqlQuery;
		IDataReader reader = dbcmd.ExecuteReader();

		string sqlCmd;
		
		if (!reader.Read()) {
			if (!reader.IsClosed) {
				reader.Close();
			}
			sqlCmd = string.Format(
				"CREATE TABLE `{0}{1}` (" +
				"`time`	REAL NOT NULL," +
				"`positionX`	REAL NOT NULL," +
				"`positionY`	REAL NOT NULL," +
				"`angleZ`	REAL NOT NULL," +
				"`find`	INTEGER NOT NULL DEFAULT 0," +
				"`see`	INTEGER NOT NULL DEFAULT 0," +
				"`howl`	INTEGER NOT NULL DEFAULT 0," +
				"`purr`	INTEGER NOT NULL DEFAULT 0" +
				");",
				_gameId, Player.name
			);
			dbcmd.CommandText = sqlCmd;
			dbcmd.ExecuteNonQuery();
		}
		if (!reader.IsClosed) {
			reader.Close();
		}

		sqlCmd = string.Format(
			"INSERT INTO `{0}{1}` (time, positionX, positionY, angleZ, see, howl, purr) VALUES (\"{0}\", {1}, {2}, {3}, {4}, {5}, {6});",
			_gameId, Player.name,
			DateTime.Now.ToString(), 
			Player.transform.position.x, 
			Player.transform.position.y, 
			Player.transform.eulerAngles.z, 
			Player.GetComponent<Prey>().Notice(), 
			"0",
			"0"
		);
		dbcmd.CommandText = sqlCmd;
		dbcmd.ExecuteNonQuery();
		
		dbcmd.Dispose();
		dbcmd = null;
	}
}