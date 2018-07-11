using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

	[HideInInspector] public LevelManager LevelManager;
	
	void Awake () {
		LevelManager = GameObject.Find("/LevelManager").GetComponent<LevelManager>();
	}
	
	void Start () {
		Button btnStart = GameObject.Find("/Canvas/Start").GetComponent<Button>();
		Button btnMenu = GameObject.Find("/Canvas/Menu").GetComponent<Button>();

		btnStart.onClick.AddListener(delegate {LoadLevel("Game"); });
		btnMenu.onClick.AddListener(delegate {LoadLevel("Menu"); });
	}
	
	void LoadLevel(string levelName)
	{
		LevelManager.LoadLevel(levelName);
	}
}
