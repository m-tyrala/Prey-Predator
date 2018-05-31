using System;
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

	public MeshFilter ViewMeshFilter;
	private Mesh viewMesh;

	[HideInInspector]
	public List<Transform> VisibleTargets = new List<Transform>();

	void Start() {
		viewMesh = new Mesh();
		viewMesh.name = "View Mesh";
		ViewMeshFilter.mesh = viewMesh;
		StartCoroutine("FindTargetsWithDelay", .03f);
	}

	private void LateUpdate() {
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
				VisibleTargets.Add(Prey.transform);
		} else if(distance < ViewRadius + Prey.Size){
			Vector3 direction = (Prey.transform.position - transform.position).normalized;
			if (Vector3.Angle(transform.right, direction) < ViewAngle / 2) {
				VisibleTargets.Add(Prey.transform);
			}
		}
	}

	void DrawFieldOfView() {
		int stepCount = Mathf.RoundToInt(ViewAngle * MeshResolution);
		float stepAngleSize = ViewAngle / stepCount;
		List<Vector3> viewPoints = new List<Vector3>();

		for (int i = 0; i <= stepCount; i++) {
			float angle = - ViewAngle / 2 + stepAngleSize * i;
			Vector3 viewCastRay = transform.position + GetDirectionVector(angle) * ViewRadius;
			
			viewPoints.Add(viewCastRay);
		}
		
		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount -2) * 3];
		
		vertices[0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; i++) {
			vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

			if (i < vertexCount - 2) {
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}
		viewMesh.Clear();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
	}
	
	public Vector3 GetDirectionVector(float angleDegrees) {
		
		return new Vector3(Mathf.Sin((-transform.eulerAngles.z + 90 -angleDegrees) * Mathf.Deg2Rad), Mathf.Cos((-transform.eulerAngles.z + 90 -angleDegrees) * Mathf.Deg2Rad), 0);
	}
}
