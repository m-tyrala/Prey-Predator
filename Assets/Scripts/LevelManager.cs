using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	[HideInInspector] public bool EndOfTheGame;
	[HideInInspector] public bool GameResult;
	[HideInInspector] public Score PlayerScore;
	[HideInInspector] public string SpotTime;
	
	private static bool _created = false;

	void Awake()
	{
		if (!_created)
		{
			DontDestroyOnLoad(this.gameObject);
			_created = true;
		}
	}
	
	private void Start() {
		Screen.SetResolution(900, 900, true);
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
		SceneManager.LoadScene(name);
		EndOfTheGame = false;
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
			Spots = spots;
			FirstSpot = firstSpot ? 2 : 0;
			Trace = trace;
			Catch = catchPrey ? 3 : 0;
			Result = result ? 2 : 0;
		}

		public int Overall() {
			return Catch + Trace + FirstSpot + Spots + Result;
		}
	}
}
