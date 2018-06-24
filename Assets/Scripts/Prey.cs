using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Prey : MonoBehaviour {

	public LevelManager LevelManager;
	public EscapeBorderController EscapeBorderController;
	public Predator Predator;
	
	public float MaxSpeed;
	public float AngleSpeed;
	public float TimeToMaxSpeed;
	public float NitroTime;
	
	private KeyCode[] _inputTable = new KeyCode[3];
	private float _acceleration;
	private float _currentSpeed;
	private float _currentRotation;
	private float _remainingNitroTime;
	private float _minMoveTime;
	private float _maxMoveTime;
	private float _currentMoveTime;
	
	[HideInInspector]
	public float Size;
	public bool Detect;
	
	// Use this for initialization
	void Start () {
		_acceleration = MaxSpeed;
		_currentSpeed = 0;
		_remainingNitroTime = NitroTime;
		_minMoveTime = 3.0f;
		_maxMoveTime = 8.0f;
		_currentMoveTime = 8.0f;
		Size = 20.0f;
		Detect = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!Detect)
			Notice();
	}

	void LateUpdate() {
		Act();
	}

	private void OnTriggerEnter(Collider other) {
		print("striggerowalo kolizje");
		if(!LevelManager.EndOfTheGame) {
			print("nazwa kolizji [" + other.GetComponent<Collider>().name + "]");
			if (
				other.GetComponent<Collider>().name == EscapeBorderController.ActiveEscapeBorders.First.Value.Name ||
				other.GetComponent<Collider>().name == EscapeBorderController.ActiveEscapeBorders.Last.Value.Name
				) {
				LevelManager.LoadLevel("Lose");
				LevelManager.EndOfTheGame = true;
			} else if (other.GetComponent<Collider>().name == Predator.name) {
				LevelManager.LoadLevel("Win");
				LevelManager.EndOfTheGame = true;
			}
		}
	}

	void Notice() {
		FieldOfNotice fieldOfNotice = GetComponent<FieldOfNotice>();
		if (fieldOfNotice.VisibleTargets.Count > 0) {
			Detect = true;
		} 
	}

	void Act() {
		if (Detect)
			RunForYourLife();
		else
			Move();
	}

	void RunForYourLife() {
		Escape();
		SetRotation();
		SetSpeed();
		SetPosition(1);
	}

	void Move() {
		_currentMoveTime += Time.deltaTime;
		RandomMove();
		SetRotation();
		SetSpeed();
		SetPosition(6);
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

	void SetPosition(int velocityDivider) {
		Vector3 move;
		Vector3 position;
		move = Vector3.right;
		
		position = transform.position + transform.rotation * move * _currentSpeed * Time.deltaTime / velocityDivider;
		position[0] = (position[0] < 10) ? 10 : position[0];
		position[0] = (position[0] > 5320) ? 5320 : position[0];
		position[1] = (position[1] > 890) ? 890 : position[1];
		position[1] = (position[1] < -4450) ? -4450 : position[1];
		transform.position = position;
	}

	void Escape() {
		float Angle1 = Vector3.SignedAngle(
			EscapeBorderController.ActiveEscapeBorders.First.Value.BorderBlockTransform.position - transform.position,
			new Vector3(Mathf.Sin((-transform.eulerAngles.z + 90) * Mathf.Deg2Rad),Mathf.Cos((-transform.eulerAngles.z + 90) * Mathf.Deg2Rad), 0),
			Vector3.back
		);
		float Angle2 = Vector3.SignedAngle(
			EscapeBorderController.ActiveEscapeBorders.Last.Value.BorderBlockTransform.position - transform.position,
			new Vector3(Mathf.Sin((-transform.eulerAngles.z + 90) * Mathf.Deg2Rad),Mathf.Cos((-transform.eulerAngles.z + 90) * Mathf.Deg2Rad), 0),
			Vector3.back
		);
		//print("Angle 1 = [" + Angle1 + "], Angle 2 = [" + Angle2 + "]");
		if (Angle1 < 0 && Angle2 < 0)
			_inputTable[1] = KeyCode.D;
		else if (Angle1 > 0 && Angle2 > 0)
			_inputTable[1] = KeyCode.A;
		else if (Angle1 > 0 && Angle2 < 0)
			_inputTable[1] = KeyCode.A;
		else if (Angle1 < 0 && Angle2 > 0)
			_inputTable[1] = KeyCode.D;
		
		_inputTable[0] = KeyCode.W;
		_inputTable[2] = KeyCode.N;
	}
	
	void RandomMove() {
		bool toss = false;

		float tossToToss = UnityEngine.Random.Range(0.0f, 1.0f);
		if (_currentMoveTime > _maxMoveTime)
			toss = true;
		else if (_currentMoveTime > _minMoveTime && tossToToss < 0.01f)
			toss = true;

		if (toss) {
			_currentMoveTime = 0.0f;
			float rotationToss = UnityEngine.Random.Range(0.0f, 1.0f);
			float thrustToss = UnityEngine.Random.Range(0.0f, 1.0f);

			if (rotationToss < 0.40f)
				_inputTable[1] = KeyCode.A;
			else if (rotationToss > 0.60f)
				_inputTable[1] = KeyCode.D;
			else
				_inputTable[1] = KeyCode.N;

			if (thrustToss > 0.3f)
				_inputTable[0] = KeyCode.W;
			else
				_inputTable[0] = KeyCode.N;

			_inputTable[2] = KeyCode.N;
		}
	}
}
