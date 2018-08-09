using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeBorderController : MonoBehaviour {

	public Prey Prey;
	public GameObject EscapeBorderLayer;

	[HideInInspector] 
	public LinkedList<Border> ActiveEscapeBorders;
	
	private bool _escapeShown = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (!_escapeShown && Prey.Detect) {
			ShowEscape();
		}
	}

	void ShowEscape() {
		LinkedList<Border> borders = new LinkedList<Border>();
		foreach (Transform child in EscapeBorderLayer.transform) {
			borders.AddLast(
				new Border(
					child,
					Vector3.Distance(child.position, Prey.transform.position),
					child.name)
			);
		}
		LinkedList<Border> choosenOnes = new LinkedList<Border>();

		for (int i = 0; i < 2; i++) {
			float max = 0.0f;
			Border farthestBorder = new Border(transform, 0.0f, "null");
			foreach (var border in borders) {
				if (border.DistanceFromPrey > max) {
					max = border.DistanceFromPrey;
					farthestBorder = border;
				}
			}

			borders.Remove(farthestBorder);
			choosenOnes.AddLast(farthestBorder);
		}

		foreach (var border in choosenOnes) {
			border.BorderBlockTransform.GetComponent<MeshRenderer>().enabled = true;
			border.BorderBlockTransform.GetComponent<BoxCollider>().enabled = true;
		}

		ActiveEscapeBorders = choosenOnes;
		
		_escapeShown = true;
	}

	public struct Border {
		public Transform BorderBlockTransform;
		public float DistanceFromPrey;
		public string Name;

		public Border(Transform borderBlockTransform, float distanceFromPrey, string name) {
			BorderBlockTransform = borderBlockTransform;
			DistanceFromPrey = distanceFromPrey;
			Name = name;
		}
	}
}
