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
		GameObject.Find("/Canvas/Score/Overall/Score1").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator1"]).Overall().ToString();
		GameObject.Find("/Canvas/Score/Trace/Score1").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator1"]).Trace.ToString();
		GameObject.Find("/Canvas/Score/Find/Score1").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator1"]).FirstSpot.ToString();
		GameObject.Find("/Canvas/Score/Spot/Score1").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator1"]).Spots.ToString();
		GameObject.Find("/Canvas/Score/Catch/Score1").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator1"]).Catch.ToString();
		GameObject.Find("/Canvas/Score/Result/Score1").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator1"]).Result.ToString();
		
		GameObject.Find("/Canvas/Score/Overall/Score2").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator2"]).Overall().ToString();
		GameObject.Find("/Canvas/Score/Trace/Score2").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator2"]).Trace.ToString();
		GameObject.Find("/Canvas/Score/Find/Score2").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator2"]).FirstSpot.ToString();
		GameObject.Find("/Canvas/Score/Spot/Score2").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator2"]).Spots.ToString();
		GameObject.Find("/Canvas/Score/Catch/Score2").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator2"]).Catch.ToString();
		GameObject.Find("/Canvas/Score/Result/Score2").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator2"]).Result.ToString();
		
		GameObject.Find("/Canvas/Score/Overall/Score3").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator3"]).Overall().ToString();
		GameObject.Find("/Canvas/Score/Trace/Score3").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator3"]).Trace.ToString();
		GameObject.Find("/Canvas/Score/Find/Score3").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator3"]).FirstSpot.ToString();
		GameObject.Find("/Canvas/Score/Spot/Score3").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator3"]).Spots.ToString();
		GameObject.Find("/Canvas/Score/Catch/Score3").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator3"]).Catch.ToString();
		GameObject.Find("/Canvas/Score/Result/Score3").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator3"]).Result.ToString();
		
		GameObject.Find("/Canvas/Score/Overall/Score4").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator4"]).Overall().ToString();
		GameObject.Find("/Canvas/Score/Trace/Score4").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator4"]).Trace.ToString();
		GameObject.Find("/Canvas/Score/Find/Score4").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator4"]).FirstSpot.ToString();
		GameObject.Find("/Canvas/Score/Spot/Score4").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator4"]).Spots.ToString();
		GameObject.Find("/Canvas/Score/Catch/Score4").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator4"]).Catch.ToString();
		GameObject.Find("/Canvas/Score/Result/Score4").GetComponent<Text>().text = ((LevelManager.Score) LevelManager.PlayerScores["Predator4"]).Result.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
