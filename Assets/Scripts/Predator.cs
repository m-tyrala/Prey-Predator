using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Predator : MonoBehaviour {

	public float MaxSpeed;
	public float AngleSpeed;
	public float TimeToMaxSpeed;
	public float NitroTime;
	private float _acceleration;
	private float _currentSpeed;
	private float _remainingNitroTime;
	private float _traceTime = 0;
	private bool _seePrey = false;
	
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
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			LevelManager.LoadLevel("Menu");
		} else {
			// move
			KeyCode[] moveInput = ConstructInputTable();
			
			// howl
			if (Input.GetKeyDown(KeyCode.R)) {
				Howl();
			}
			// purr
			if (Input.GetKeyDown(KeyCode.F)) {
				Purr();
			}
			
			Move(moveInput);
			Score();
		}
	}

	KeyCode[] ConstructInputTable() {
		KeyCode[] moveInput = new KeyCode[3];
		// thrust
		if (Input.GetKey(KeyCode.W)) {
			moveInput[0] = KeyCode.W;
		}
		else {
			moveInput[0] = KeyCode.N;
		}
		// rotation
		if (Input.GetKey(KeyCode.A)) {
			moveInput[1] = KeyCode.A;
		}
		else if (Input.GetKey(KeyCode.D)) {
			moveInput[1] = KeyCode.D;
		}
		else {
			moveInput[1] = KeyCode.N;
		}
		// nitro
		if (Input.GetKey(KeyCode.E)) {
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
			//print(_remainingNitroTime);
			_currentSpeed += _acceleration * Time.deltaTime / TimeToMaxSpeed;
			_currentSpeed = (_currentSpeed > MaxSpeed + additionalSpeed) ? MaxSpeed + additionalSpeed : _currentSpeed;
		} else {
			_currentSpeed -= _acceleration * Time.deltaTime / ( TimeToMaxSpeed / 2 );
			_currentSpeed = (_currentSpeed < 0) ? 0 : _currentSpeed;
		}
		position = transform.position + transform.rotation * move * _currentSpeed * Time.deltaTime;
		position[0] = (position[0] < 10) ? 10 : position[0];
		position[0] = (position[0] > 5320) ? 5320 : position[0];
		position[1] = (position[1] > 890) ? 890 : position[1];
		position[1] = (position[1] < -4450) ? -4450 : position[1];
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
			}
			if (!_seePrey) {
				SpotCount += 1;
				_seePrey = true;
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
			_seePrey = false;
		}
	}

	public void SendScore(bool gotPrey, bool win) {
		print("send score");
		LevelManager.PlayerScore = new LevelManager.Score(SpotCount, FirstSpot, TraceScore, gotPrey, win);
		print("Overall: " + LevelManager.PlayerScore.Overall().ToString());
		print("Trace: " + LevelManager.PlayerScore.Trace.ToString());
		print("FirstSpot: " + LevelManager.PlayerScore.FirstSpot.ToString());
		print("Spots: " + LevelManager.PlayerScore.Spots.ToString());
		print("Catch: " + LevelManager.PlayerScore.Catch.ToString());
		print("Result: " + LevelManager.PlayerScore.Result.ToString());
		LevelManager.EndOfTheGame = true;
		print("end of send score");
	}
	
	void Howl() {
		print("HOWL");
	}

	void Purr() {
		print("PURR");
	}
}
