using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	[HideInInspector] public bool EndOfTheGame;
	
	private void Start() {
		Screen.SetResolution(900, 900, true);
	}

	public void LoadLevel(string name) {
		SceneManager.LoadScene(name);
		EndOfTheGame = false;
	}
	public void QuitRequest() {
		Application.Quit();
	}
}
