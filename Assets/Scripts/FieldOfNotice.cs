using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldOfNotice : MonoBehaviour {

	public float ViewRadius;
	public float HearRadius;
	[Range(0,360)]
	public float ViewAngle;

	public Predator Predator;

	[HideInInspector]
	public List<Transform> VisibleTargets = new List<Transform>();

	void Start() {
		StartCoroutine("FindTargetsWithDelay", .03f);
	}

	IEnumerator FindTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}
	
	void FindVisibleTargets() {
		VisibleTargets.Clear();

		float distance = Vector3.Distance(transform.position, Predator.transform.position);
		if (distance < HearRadius + Predator.Size) {
				VisibleTargets.Add(Predator.transform);
		} else if(distance < ViewRadius + Predator.Size){
			Vector3 direction = (Predator.transform.position - transform.position).normalized;
			if (Vector3.Angle(transform.right, direction) < ViewAngle / 2) {
				VisibleTargets.Add(Predator.transform);
			}
		}
	}
}
