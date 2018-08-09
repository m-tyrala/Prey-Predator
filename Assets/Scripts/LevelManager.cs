using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	[HideInInspector] public bool EndOfTheGame;
	[HideInInspector] public bool GameResult;
	[HideInInspector] public Hashtable PlayerScores;
	[HideInInspector] public string SpotTime;
	[HideInInspector] public Vector3[] Positions;
	
	private static bool _created = false;

	void Awake()
	{
		if (!_created)
		{
			DontDestroyOnLoad(this.gameObject);
			_created = true;
		}
		
		PlayerScores = new Hashtable();
	}
	
	private void Start() {
		Screen.SetResolution(1920, 1080, true);
	}

	private void LateUpdate() {
		if (EndOfTheGame) {
			if (GameResult) {
				LoadLevel("Win");
			} else {
				LoadLevel("Lose");
			}
		}
	}

	public void LoadLevel(string name) {
		if (name == "Game")
		{
			Vector3 position1 = new Vector3(Random.Range(20, 5380), Random.Range(-4310, 1070), -37);
			
			float temp = (position1[0] + Random.Range(-200, 200));
			float temp2 = (temp < 20) ? 20 : temp;
			float x = (temp2 > 5380) ? 5380 : temp2;
			temp = (position1[1] + Random.Range(-200, 200));
			temp2 = (temp < -4310) ? -4310 : temp;
			float y = (temp2 > 1070) ? 1070 : temp2;
			Vector3 position2 = new Vector3(x, y , -37);
			
			
			temp = (position1[0] + Random.Range(-200, 200));
			temp2 = (temp < 20) ? 20 : temp;
			x = (temp2 > 5380) ? 5380 : temp2;
			temp = (position1[1] + Random.Range(-200, 200));
			temp2 = (temp < -4310) ? -4310 : temp;
			y = (temp2 > 1070) ? 1070 : temp2;
			Vector3 position3 = new Vector3(x, y , -37);
			
			
			temp = (position1[0] + Random.Range(-200, 200));
			temp2 = (temp < 20) ? 20 : temp;
			x = (temp2 > 5380) ? 5380 : temp2;
			temp = (position1[1] + Random.Range(-200, 200));
			temp2 = (temp < -4310) ? -4310 : temp;
			y = (temp2 > 1070) ? 1070 : temp2;
			Vector3 position4 = new Vector3(x, y , -37);

			x = (position1[0] > 2700) ? 
				position1[0] - Random.Range(1700, 2680) :
				position1[0] + Random.Range(1700, 2680)
				;
			
			y = (position1[1] > -1620) ?
				position1[0] - Random.Range(1700, 2680) :
				position1[0] + Random.Range(1700, 2680);
			Vector3 position0 = new Vector3(x, y, -37);
			

			Positions = new Vector3[5];
			Positions[0] = position0;
			Positions[1] = position1;
			Positions[2] = position2;
			Positions[3] = position3;
			Positions[4] = position4;
		}
		
		EndOfTheGame = false;
		SceneManager.LoadScene(name);
	}
	
	public void QuitRequest() {
		Application.Quit();
	}

	public struct Score {
		public int Spots;
		public int FirstSpot;
		public int Trace;
		public int Catch;
		public int Result;

		public Score(int spots, bool firstSpot, int trace, bool catchPrey, bool result) {
			Spots = (spots > 0) ? spots : -10;
			FirstSpot = firstSpot ? 2 : 0;
			Trace = trace;
			Catch = catchPrey ? 5 : 0;
			Result = result ? 20 : -20;
		}

		public int Overall() {
			return Catch + Trace + FirstSpot + Spots + Result;
		}
	}
}
