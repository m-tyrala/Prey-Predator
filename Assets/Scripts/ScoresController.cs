using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoresController : MonoBehaviour {

	[HideInInspector] public LevelManager LevelManager;

	private void Awake() {
		LevelManager = GameObject.Find("/LevelManager").GetComponent<LevelManager>();
	}

	// Use this for initialization
	void Start () {
		GameObject.Find("/Canvas/Score/Overall/Score").GetComponent<Text>().text = LevelManager.PlayerScore.Overall().ToString();
		GameObject.Find("/Canvas/Score/Trace/Score").GetComponent<Text>().text = LevelManager.PlayerScore.Trace.ToString();
		GameObject.Find("/Canvas/Score/Find/Score").GetComponent<Text>().text = LevelManager.PlayerScore.FirstSpot.ToString();
		GameObject.Find("/Canvas/Score/Spot/Score").GetComponent<Text>().text = LevelManager.PlayerScore.Spots.ToString();
		GameObject.Find("/Canvas/Score/Catch/Score").GetComponent<Text>().text = LevelManager.PlayerScore.Catch.ToString();
		GameObject.Find("/Canvas/Score/Result/Score").GetComponent<Text>().text = LevelManager.PlayerScore.Result.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
