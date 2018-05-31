using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    
	private void Start() {
		Screen.SetResolution(900, 900, true);
	}

	public void LoadLevel(string name) {
		SceneManager.LoadScene(name);
	}
	public void QuitRequest() {
		Application.Quit();
	}
}
