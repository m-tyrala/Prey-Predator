using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassController : MonoBehaviour
{

	public Prey Prey;
	public Predator Predator;
	private const float TimeToSee = 1.1f;
	private float _currentTimeToSee;
	
	// Use this for initialization
	void Start ()
	{
		GetComponentInChildren<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	private void LateUpdate()
	{
		if (_currentTimeToSee > 0)
		{
			GetComponentInChildren<MeshRenderer>().enabled = true;
			_currentTimeToSee -= Time.deltaTime;
		}
		else
		{
			GetComponentInChildren<MeshRenderer>().enabled = false;
		}
		Vector3 direction = (Prey.transform.position - Predator.transform.position).normalized;
		float angle = Vector3.SignedAngle(Predator.transform.right, direction, Vector3.back);
		transform.rotation = Quaternion.Euler(0, 0, - angle + Predator.gameObject.transform.eulerAngles.z);
	}

	public void TurnCompassOn()
	{
		print("TURN COMPASS ON");
		_currentTimeToSee = TimeToSee;
	}
}
