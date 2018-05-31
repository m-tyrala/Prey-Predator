using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor {

	void OnSceneGUI() {
		FieldOfView fow = (FieldOfView)target;
		Handles.color = Color.white;
		Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.up, 360, fow.ViewRadius);
		Vector3 viewAngleA = fow.GetDirectionVector(-fow.ViewAngle / 2);
		Vector3 viewAngleB = fow.GetDirectionVector(fow.ViewAngle / 2);
		
		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.ViewRadius);
		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.ViewRadius);
		
		Handles.color = Color.blue;
		Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.up, 360, fow.HearRadius);

		Handles.color = Color.red;
		foreach (Transform visibleTarget in fow.VisibleTargets) {
			Handles.DrawLine(fow.transform.position, visibleTarget.transform.position);
		}
	}
}
