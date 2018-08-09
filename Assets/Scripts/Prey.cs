using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Prey : MonoBehaviour {

	public EscapeBorderController EscapeBorderController;
	public Predator Predator1;
	public Predator Predator2;
	public Predator Predator3;
	public Predator Predator4;
	
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
	
	[HideInInspector] public LevelManager LevelManager;
	[HideInInspector] public float Size;
	[HideInInspector] public bool Detect;
	[HideInInspector] public bool Unnoticed = true;
	
	// Use this for initialization
	void Awake () {
		LevelManager = GameObject.Find("/LevelManager").GetComponent<LevelManager>();
		_acceleration = MaxSpeed;
		_currentSpeed = 0;
		_remainingNitroTime = NitroTime;
		_minMoveTime = 3.0f;
		_maxMoveTime = 8.0f;
		_currentMoveTime = 8.0f;
		Size = 20.0f;
		Detect = false;
	}
	
	void Start()
	{
		transform.position = LevelManager.Positions[0];
	}
	
	// Update is called once per frame
	void Update () {
		if(!Detect)
			Notice();
		else
			GetComponent<SphereCollider>().enabled = true;
			
	}

	void LateUpdate() {
		Act();
	}

	private void OnTriggerEnter(Collider other) {
		//print("striggerowalo kolizje");
		
		Hashtable gotPrey = new Hashtable();
		if(!LevelManager.EndOfTheGame) {
			//print("nazwa kolizji [" + other.GetComponent<Collider>().name + "]");
			if (
				other.name == EscapeBorderController.ActiveEscapeBorders.First.Value.Name ||
				other.name == EscapeBorderController.ActiveEscapeBorders.Last.Value.Name
				) {
				LevelManager.GameResult = false;
				gotPrey.Add("Predator1", false);
				gotPrey.Add("Predator2", false);
				gotPrey.Add("Predator3", false);
				gotPrey.Add("Predator4", false);
			} else if (other.name == Predator1.name || other.name == Predator2.name || other.name == Predator3.name || other.name == Predator4.name) {
				LevelManager.GameResult = true;
				gotPrey.Add("Predator1", (other.name == Predator1.name));
				gotPrey.Add("Predator2", (other.name == Predator2.name));
				gotPrey.Add("Predator3", (other.name == Predator3.name));
				gotPrey.Add("Predator4", (other.name == Predator4.name));
			}
			Predator1.SendScore((bool) gotPrey["Predator1"], LevelManager.GameResult);
			Predator2.SendScore((bool) gotPrey["Predator2"], LevelManager.GameResult);
			Predator3.SendScore((bool) gotPrey["Predator3"], LevelManager.GameResult);
			Predator4.SendScore((bool) gotPrey["Predator4"], LevelManager.GameResult);
		}
	}

	public bool Notice() {
		FieldOfNotice fieldOfNotice = GetComponent<FieldOfNotice>();
		if (fieldOfNotice.VisibleTargets.Count > 0) {
			Detect = true;
		}

		return fieldOfNotice.VisibleTargets.Count > 0;
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
			if (_inputTable[2] == KeyCode.E && _remainingNitroTime > 0) {
				additionalSpeed = MaxSpeed;
				_remainingNitroTime -= Time.deltaTime;
			}
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
		position[0] = (position[0] > 5390) ? 5390 : position[0];
		position[1] = (position[1] > 1080) ? 1080 : position[1];
		position[1] = (position[1] < -4310) ? -4310 : position[1];
		transform.position = position;
	}

	void Escape() {
		float AngleToFirstBorder = Vector3.SignedAngle(
			EscapeBorderController.ActiveEscapeBorders.First.Value.BorderBlockTransform.position - transform.position,
			new Vector3(Mathf.Sin((-transform.eulerAngles.z + 90) * Mathf.Deg2Rad),Mathf.Cos((-transform.eulerAngles.z + 90) * Mathf.Deg2Rad), 0),
			Vector3.back
		);
		float AngleToSecondBorder = Vector3.SignedAngle(
			EscapeBorderController.ActiveEscapeBorders.Last.Value.BorderBlockTransform.position - transform.position,
			new Vector3(Mathf.Sin((-transform.eulerAngles.z + 90) * Mathf.Deg2Rad),Mathf.Cos((-transform.eulerAngles.z + 90) * Mathf.Deg2Rad), 0),
			Vector3.back
		);
		//print("Angle 1 = [" + Angle1 + "], Angle 2 = [" + Angle2 + "]");
		if (AngleToFirstBorder < 0 && AngleToSecondBorder < 0)
			_inputTable[1] = KeyCode.D;
		else if (AngleToFirstBorder > 0 && AngleToSecondBorder > 0)
			_inputTable[1] = KeyCode.A;
		else if (AngleToFirstBorder > 0 && AngleToSecondBorder < 0)
			_inputTable[1] = KeyCode.A;
		else if (AngleToFirstBorder < 0 && AngleToSecondBorder > 0)
			_inputTable[1] = KeyCode.D;
		
		_inputTable[0] = KeyCode.W;
		_inputTable[2] = KeyCode.E;
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
