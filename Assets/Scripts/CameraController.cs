using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject Player;

	void Start () {
		Vector2 playerPosition = checkPosition();
		Vector3 newPosition = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
		transform.position = newPosition;
	}
    
	void LateUpdate () 
	{
		Vector2 playerPosition = checkPosition();
		Vector3 newPosition = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
		transform.position = newPosition;
	}

	Vector3 checkPosition() {
		Vector3 playerPosition = Player.transform.position;
		
		float x = (playerPosition[0] < 875) ? 875 : playerPosition[0];
		x = (x > 4540) ? 4540 : x;
		
		float y = (playerPosition[1] > 590) ? 590 : playerPosition[1];
		y = (y < -3820) ? -3820 : y;

		return new Vector2(x, y);
	}
}