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

	public Predator Predator1;
	public Predator Predator2;
	public Predator Predator3;
	public Predator Predator4;

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

		float distance = Vector3.Distance(transform.position, Predator1.transform.position);
		if (distance < HearRadius + Predator1.Size) {
				VisibleTargets.Add(Predator1.transform);
		} else if(distance < ViewRadius + Predator1.Size){
			Vector3 direction = (Predator1.transform.position - transform.position).normalized;
			if (Vector3.Angle(transform.right, direction) < ViewAngle / 2) {
				VisibleTargets.Add(Predator1.transform);
			}
		}
		
		distance = Vector3.Distance(transform.position, Predator2.transform.position);
		if (distance < HearRadius + Predator2.Size) {
			VisibleTargets.Add(Predator2.transform);
		} else if(distance < ViewRadius + Predator2.Size){
			Vector3 direction = (Predator2.transform.position - transform.position).normalized;
			if (Vector3.Angle(transform.right, direction) < ViewAngle / 2) {
				VisibleTargets.Add(Predator2.transform);
			}
		}
		
		distance = Vector3.Distance(transform.position, Predator3.transform.position);
		if (distance < HearRadius + Predator3.Size) {
			VisibleTargets.Add(Predator3.transform);
		} else if(distance < ViewRadius + Predator3.Size){
			Vector3 direction = (Predator3.transform.position - transform.position).normalized;
			if (Vector3.Angle(transform.right, direction) < ViewAngle / 2) {
				VisibleTargets.Add(Predator3.transform);
			}
		}
		
		distance = Vector3.Distance(transform.position, Predator4.transform.position);
		if (distance < HearRadius + Predator4.Size) {
			VisibleTargets.Add(Predator4.transform);
		} else if(distance < ViewRadius + Predator4.Size){
			Vector3 direction = (Predator4.transform.position - transform.position).normalized;
			if (Vector3.Angle(transform.right, direction) < ViewAngle / 2) {
				VisibleTargets.Add(Predator4.transform);
			}
		}
	}
}
