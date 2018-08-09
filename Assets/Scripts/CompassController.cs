using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassController : MonoBehaviour
{

	public Prey Prey;
	public Predator Predator;
	
	private void LateUpdate()
	{
		Vector3 direction = (Prey.transform.position - Predator.transform.position).normalized;
		float angle = Vector3.SignedAngle(Predator.transform.right, direction, Vector3.back);
		transform.rotation = Quaternion.Euler(0, 0, - angle + Predator.gameObject.transform.eulerAngles.z);
	}
}
