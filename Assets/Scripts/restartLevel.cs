using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class restartLevel : MonoBehaviour {
	static string lastScene;
	static string currentScene;
	void Start(){
		changeScene ("scenes/gameOver");
	}
	public static void changeScene(string sceneName){
		lastScene = "scenes/"+SceneManager.GetActiveScene().name;
		currentScene = sceneName;
		SceneManager.LoadScene(currentScene);
	}
	public static void LoadLastScene(){
		string last = lastScene;
		lastScene = currentScene;
		currentScene = last;
		SceneManager.LoadScene(currentScene);
	}

}