using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Predator : MonoBehaviour {

	public float MaxSpeed;
	public float AngleSpeed;
	public float TimeToMaxSpeed;
	public float NitroTime;
	public bool SeePrey = false;
	private float _acceleration;
	private float _currentSpeed;
	private float _remainingNitroTime;
	private float _traceTime = 0;
	private bool _howl = false;
	private bool _purr = false;
	private Controlls _controlls;
	
	[HideInInspector] public LevelManager LevelManager;
	[HideInInspector] public int TraceScore;
	[HideInInspector] public int SpotCount;
	[HideInInspector] public bool FirstSpot;

	[HideInInspector] public float Size;
	
	// Use this for initialization
	private void Awake() {
		LevelManager = GameObject.Find("/LevelManager").GetComponent<LevelManager>();
		_acceleration = MaxSpeed;
		_currentSpeed = 0;
		_remainingNitroTime = NitroTime;		
		Size = 20.0f;
		_controlls = new Controlls(name);
	}
	
	// Update is called once per frame
	void Update () {
		_howl = false;
		_purr = false;
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
			LevelManager.LoadLevel("Menu");
		} else {
			// move
			KeyCode[] moveInput = ConstructInputTable();
			
			// howl
			if (Input.GetKeyDown((KeyCode) _controlls.get["howl"])) {
				Howl();
			}
			// purr
			if (Input.GetKeyDown((KeyCode) _controlls.get["purr"])) {
				Purr();
			}
			
			Move(moveInput);
			Score();
		}
	}

	KeyCode[] ConstructInputTable() {
		KeyCode[] moveInput = new KeyCode[3];
		// thrust
		if (Input.GetKey((KeyCode) _controlls.get["thrust"])) {
			moveInput[0] = KeyCode.W;
		}
		else {
			moveInput[0] = KeyCode.N;
		}
		// rotation
		if (Input.GetKey((KeyCode) _controlls.get["left"])) {
			moveInput[1] = KeyCode.A;
		}
		else if (Input.GetKey((KeyCode) _controlls.get["right"])) {
			moveInput[1] = KeyCode.D;
		}
		else {
			moveInput[1] = KeyCode.N;
		}
		// nitro
		if (Input.GetKey((KeyCode) _controlls.get["nitro"])) {
			moveInput[2] = KeyCode.E;
		}
		else {
			moveInput[2] = KeyCode.N;
		}

		return moveInput;
	}

	void Move(KeyCode[] inputTable) {
		Vector3 move;
		Vector3 position;
		float rotation;
		if (inputTable[1] == KeyCode.A) {
			rotation = AngleSpeed * Time.deltaTime;
		} else if (inputTable[1] == KeyCode.D) {
			rotation = - AngleSpeed * Time.deltaTime;
		} else {
			rotation = 0;
		}

		transform.rotation = transform.rotation * Quaternion.Euler(0, 0, rotation);
		
		move = Vector3.right;
		if (inputTable[0] == KeyCode.W) {
			float additionalSpeed = 0;
			if (inputTable[2] == KeyCode.E && _remainingNitroTime > 0) {
				additionalSpeed = MaxSpeed;
				_remainingNitroTime -= Time.deltaTime;
			}
			_currentSpeed += _acceleration * Time.deltaTime / TimeToMaxSpeed;
			_currentSpeed = (_currentSpeed > MaxSpeed + additionalSpeed) ? MaxSpeed + additionalSpeed : _currentSpeed;
		} else {
			_currentSpeed -= _acceleration * Time.deltaTime / ( TimeToMaxSpeed / 2 );
			_currentSpeed = (_currentSpeed < 0) ? 0 : _currentSpeed;
		}
		position = transform.position + transform.rotation * move * _currentSpeed * Time.deltaTime;
		position[0] = (position[0] < 20) ? 20 : position[0];
		position[0] = (position[0] > 5380) ? 5380 : position[0];
		position[1] = (position[1] > 1070) ? 1070 : position[1];
		position[1] = (position[1] < -4310) ? -4310 : position[1];
		transform.position = position;
	}

	void Score() {
		FieldOfView fieldOfView = GetComponent<FieldOfView>();
		if (fieldOfView.VisibleTargets.Count > 0) {

			Transform prey = fieldOfView.VisibleTargets[0];
			Prey preyObject = prey.gameObject.GetComponent<Prey>();
			if (preyObject.Unnoticed) {
				FirstSpot = true;
				preyObject.Unnoticed = false;
				LevelManager.SpotTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
			}
			if (!SeePrey) {
				SpotCount += 1;
				SeePrey = true;
			}
			else {
				_traceTime += Time.deltaTime;
				if (_traceTime >= 1.0f) {
					TraceScore += 1;
					_traceTime = .0f;
				}
			}
			
		}
		else {
			SeePrey = false;
		}
	}

	public void SendScore(bool gotPrey, bool win) {
		LevelManager.PlayerScore = new LevelManager.Score(SpotCount, FirstSpot, TraceScore, gotPrey, win);
		LevelManager.EndOfTheGame = true;
	}
	
	private void Howl() {
		_howl = true;
		print("HOWL");
	}

	private void Purr() {
		_purr = true;
		print("PURR");
	}

	public bool IsHowling() {
		return _howl;
	}

	public bool IsPurring() {
		return _purr;
	}
}
