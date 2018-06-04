using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Predator : MonoBehaviour {

	public LevelManager LevelManager;
	public float MaxSpeed;
	public float AngleSpeed;
	public float TimeToMaxSpeed;
	public float NitroTime;
	private float _acceleration;
	private float _currentSpeed;
	private float _remainingNitroTime;
	
	// Use this for initialization
	void Start () {
		_acceleration = MaxSpeed;
		_currentSpeed = 0;
		_remainingNitroTime = NitroTime;
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
		position = transform.position + transform.rotation * move * _currentSpeed * Time.deltaTime;;
		position[0] = (position[0] < 10) ? 10 : position[0];
		position[0] = (position[0] > 5320) ? 5320 : position[0];
		position[1] = (position[1] > 890) ? 890 : position[1];
		position[1] = (position[1] < -4450) ? -4450 : position[1];
		transform.position = position;
	}

	void Howl() {
		print("HOWL");
	}

	void Purr() {
		print("PURR");
	}
}
