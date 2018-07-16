using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite; 
using System.Data; 
using System;
using UnityEngine;

public class DataBaseController : MonoBehaviour {

	void Start () {

		string conn = "URI=file:" + Application.dataPath + "/Database/Database.s3db"; //Path to database.
		IDbConnection dbconn;
		dbconn = (IDbConnection) new SqliteConnection(conn);
		dbconn.Open(); //Open connection to the database.
		IDbCommand dbcmd = dbconn.CreateCommand();
		string sqlQuery = "SELECT value,name, randomSequence " + "FROM PlaceSequence";
		dbcmd.CommandText = sqlQuery;
		IDataReader reader = dbcmd.ExecuteReader();
		while (reader.Read())
		{
			int value = reader.GetInt32(0);
			string name = reader.GetString(1);
			int rand = reader.GetInt32(2);
        
			Debug.Log( "value= "+value+"  name ="+name+"  random ="+  rand);
		}
		reader.Close();
		reader = null;
		dbcmd.Dispose();
		dbcmd = null;
		dbconn.Close();
		dbconn = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

//	CREATE TABLE `matches` (
//	`gameID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
//	`P1Score`	INTEGER,
//	`P2Score`	INTEGER,
//	`P3Score`	INTEGER,
//	`P4Score`	INTEGER,
//	`duration`	REAL NOT NULL,
//	`spottime`	REAL NOT NULL,
//	`howlcount`	INTEGER NOT NULL,
//	`purrcount`	INTEGER NOT NULL,
//	`catch`	INTEGER NOT NULL
//	);


//	CREATE TABLE `gameIdPX` (
//	`time`	REAL NOT NULL,
//	`positionX`	REAL NOT NULL,
//	`positionY`	REAL NOT NULL,
//	`angleZ`	REAL NOT NULL,
//	`spot`	INTEGER NOT NULL DEFAULT 0,
//	`see`	INTEGER NOT NULL DEFAULT 0,
//	`howl`	INTEGER NOT NULL DEFAULT 0,
//	`purr`	INTEGER NOT NULL DEFAULT 0
//	);