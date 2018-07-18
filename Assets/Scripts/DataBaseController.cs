using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite; 
using System.Data; 
using System;
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
		
	void Start () {

		string conn = "URI=file:" + Application.dataPath + "/Database/Database.s3db"; //Path to database.
		_dbconn = (IDbConnection) new SqliteConnection(conn);
		_dbconn.Open(); //Open connection to the database.
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene().name == "Game") {
			if (_newGame) {
				_gameId = RecordNewGame();
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

	public int RecordNewGame() {
		IDbCommand dbcmd = _dbconn.CreateCommand();
		String startDate = DateTime.Now.ToLongDateString();
		print("start date: " + startDate);
		string sqlCmd = "INSERT INTO matches (start) VALUES (" + startDate + ");";
		dbcmd.CommandText = sqlCmd;
		dbcmd.ExecuteNonQuery();
		
		string sqlQuery = "SELECT gameID FROM matches WHERE start=" + startDate + ";";
		dbcmd.CommandText = sqlQuery;
		IDataReader reader = dbcmd.ExecuteReader();

		int gameID = 0;
			
		while (reader.Read())
		{
			gameID = reader.GetInt32(0);
		}
		reader.Close();
		reader = null;
		dbcmd.Dispose();
		dbcmd = null;

		return gameID;
	}

	private void RecordFrame() {
		if (Predator1 != null) RecordPlayerFrame(Predator1.gameObject);
		if (Predator2 != null) RecordPlayerFrame(Predator2.gameObject);
		if (Predator3 != null) RecordPlayerFrame(Predator3.gameObject);
		if (Predator4 != null) RecordPlayerFrame(Predator4.gameObject);
		if (Prey != null) RecordPlayerFrame(Prey.gameObject);	
	}

	private void RecordPlayerFrame(GameObject Player) {
		IDbCommand dbcmd = _dbconn.CreateCommand();
		string sqlCmd = "CREATE TABLE `gameIdPX` (" +
							"`time`	REAL NOT NULL," +
							"`positionX`	REAL NOT NULL," +
							"`positionY`	REAL NOT NULL," +
							"`angleZ`	REAL NOT NULL," +
							"`find`	INTEGER NOT NULL DEFAULT 0," +
							"`see`	INTEGER NOT NULL DEFAULT 0," +
							"`howl`	INTEGER NOT NULL DEFAULT 0," +
							"`purr`	INTEGER NOT NULL DEFAULT 0" +
		                ");";
		dbcmd.CommandText = sqlCmd;
		dbcmd.ExecuteNonQuery();
		
		sqlCmd = "INSERT INTO matches (time, positionX, positionY, angleZ, find, see, howl, purr) VALUES (" + startDate + ");";
		dbcmd.CommandText = sqlCmd;
		dbcmd.ExecuteNonQuery();
		
		string sqlQuery = "SELECT gameID FROM matches WHERE start=" + startDate + ";";
		dbcmd.CommandText = sqlQuery;
		IDataReader reader = dbcmd.ExecuteReader();

		int gameID = 0;
			
		while (reader.Read())
		{
			gameID = reader.GetInt32(0);
		}
		reader.Close();
		reader = null;
		dbcmd.Dispose();
		dbcmd = null;
	}
}

//	CREATE TABLE `matches` (
//	`gameID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
//	`P1Score`	INTEGER,
//	`P2Score`	INTEGER,
//	`P3Score`	INTEGER,
//	`P4Score`	INTEGER,
//	`duration`	REAL,
//	`spottime`	REAL,
//	`howlcount`	INTEGER,
//	`purrcount`	INTEGER,
//	`catch`	INTEGER,
//  `start` TEXT NOT NULL
//	);


//	CREATE TABLE `gameIdPX` (
//	`time`	REAL NOT NULL,
//	`positionX`	REAL NOT NULL,
//	`positionY`	REAL NOT NULL,
//	`angleZ`	REAL NOT NULL,
//	`find`	INTEGER NOT NULL DEFAULT 0,
//	`see`	INTEGER NOT NULL DEFAULT 0,
//	`howl`	INTEGER NOT NULL DEFAULT 0,
//	`purr`	INTEGER NOT NULL DEFAULT 0
//	);