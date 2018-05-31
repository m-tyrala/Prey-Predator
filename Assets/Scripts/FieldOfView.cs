using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

	public float ViewRadius;
	public float HearRadius;
	[Range(0,360)]
	public float ViewAngle;

	public Prey Prey;

	public LayerMask UnitsLayerMask;

	public float MeshResolution;

	[HideInInspector]
	public List<Transform> VisibleTargets = new List<Transform>();

	void Start() {
		StartCoroutine("FindTargetsWithDelay", .1f);
	}

	private void Update() {
		DrawFieldOfView();
	}

	IEnumerator FindTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}
	
	void FindVisibleTargets() {
		VisibleTargets.Clear();

		float distance = Vector3.Distance(transform.position, Prey.transform.position);
		if (distance < HearRadius + Prey.Size) {
			print("Found prey");
				VisibleTargets.Add(Prey.transform);
		} else if(distance < ViewRadius + Prey.Size){
			print("Found prey");
			Vector3 direction = (Prey.transform.position - transform.position).normalized;
			if (Vector3.Angle(transform.right, direction) < ViewAngle / 2) {
				VisibleTargets.Add(Prey.transform);
			}
		}
	}

	void DrawFieldOfView() {
		int stepCount = Mathf.RoundToInt(ViewAngle * MeshResolution);
		float stepAngleSize = ViewAngle / stepCount;

		for (int i = 0; i < stepCount; i++) {
			float angle = - ViewAngle / 2 + stepAngleSize * i;
			Debug.DrawLine(transform.position, transform.position + GetDirectionVector(angle) * ViewRadius, Color.green);
		}
	}
	
	public Vector3 GetDirectionVector(float angleDegrees) {
		
		return new Vector3(Mathf.Sin((-transform.eulerAngles.z + 90 -angleDegrees) * Mathf.Deg2Rad), Mathf.Cos((-transform.eulerAngles.z + 90 -angleDegrees) * Mathf.Deg2Rad), 0);
	}
}
