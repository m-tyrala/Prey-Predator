using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Prey : MonoBehaviour {

	public LevelManager LevelManager;
	public float MaxSpeed;
	public float AngleSpeed;
	public float TimeToMaxSpeed;
	public float NitroTime;
	private KeyCode[] _inputTable = new KeyCode[3];
	private float _acceleration;
	private float _currentSpeed;
	private float _currentRotation;
	private float _remainingNitroTime;
	private bool _detect;
	private float _minMoveTime;
	private float _maxMoveTime;
	private float _currentMoveTime;
	
	[HideInInspector]
	public float Size;
	
	// Use this for initialization
	void Start () {
		_acceleration = MaxSpeed;
		_currentSpeed = 0;
		_remainingNitroTime = NitroTime;
		_detect = false;
		_minMoveTime = 3.0f;
		_maxMoveTime = 8.0f;
		_currentMoveTime = 8.0f;
		Size = 20.0f;
	}
	
	// Update is called once per frame
	void Update () {
		//Notice();
		//Act();
	}

	void Notice() {
		
	}

	void Act() {
		if (_detect)
			RunForYourLife();
		else
			Move();
	}

	void RunForYourLife() {
		Escape();
		SetRotation();
		SetSpeed();
		SetPosition();
	}

	void Move() {
		_currentMoveTime += Time.deltaTime;
		RandomMove();
		SetRotation();
		SetSpeed();
		SetPosition();
	}

	void SetRotation() {
		if (_inputTable[1] == KeyCode.A) {
			_currentRotation = AngleSpeed * Time.deltaTime;
		} else if (_inputTable[1] == KeyCode.D) {
			_currentRotation = - AngleSpeed * Time.deltaTime;
		} else {
			_currentRotation = 0;
		}
		
		transform.rotation = transform.rotation * Quaternion.Euler(0, 0, _currentRotation);
	}

	void SetSpeed() {
		if (_inputTable[0] == KeyCode.W) {
			float additionalSpeed = 0;
			_currentSpeed += _acceleration * Time.deltaTime / TimeToMaxSpeed;
			_currentSpeed = (_currentSpeed > MaxSpeed + additionalSpeed) ? MaxSpeed + additionalSpeed : _currentSpeed;
		} else {
			_currentSpeed -= _acceleration * Time.deltaTime / ( TimeToMaxSpeed / 2 );
			_currentSpeed = (_currentSpeed < 0) ? 0 : _currentSpeed;
		}
	}

	void SetPosition() {
		Vector3 move;
		Vector3 position;
		move = Vector3.right;
		
		position = transform.position + transform.rotation * move * _currentSpeed * Time.deltaTime;;
		position[0] = (position[0] < 110) ? 110 : position[0];
		position[0] = (position[0] > 4980) ? 4980 : position[0];
		position[1] = (position[1] > 790) ? 790 : position[1];
		position[1] = (position[1] < -4080) ? -4080 : position[1];
		transform.position = position;
	}

	void Escape() {
		
	}
	
	void RandomMove() {
		bool toss = false;

		float tossToToss = UnityEngine.Random.Range(0.0f, 1.0f);
		if (_currentMoveTime > _maxMoveTime)
			toss = true;
		else if (_currentMoveTime > _minMoveTime && tossToToss > 0.2f)
			toss = true;

		if (toss) {
			_currentMoveTime = 0.0f;
			float rotationToss = UnityEngine.Random.Range(0.0f, 1.0f);
			float thrustToss = UnityEngine.Random.Range(0.0f, 1.0f);

			if (rotationToss < 0.33f)
				_inputTable[1] = KeyCode.A;
			else if (rotationToss > 0.66f)
				_inputTable[1] = KeyCode.D;
			else
				_inputTable[1] = KeyCode.N;

			if (thrustToss > 0.5f)
				_inputTable[0] = KeyCode.W;
			else
				_inputTable[0] = KeyCode.N;

			_inputTable[2] = KeyCode.N;
		}
	}
}
