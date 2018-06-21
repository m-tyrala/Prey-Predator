using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeBorderController : MonoBehaviour {

	public Prey Prey;
	public GameObject EscapeBorderLayer;
	
	private bool _escapeShown = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (!_escapeShown && Prey.Detect) {
			ShowEscape();
		}
	}

	void ShowEscape() {
		foreach (Transform child in transform) {
			
		}
	}
}
