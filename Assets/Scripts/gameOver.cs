using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class gameOver : MonoBehaviour {
	public Button restartButton;
	public Button quitButton;
	AudioSource audio;
	//public nav navScript;
	// Use this for initialization

	void Start () {
		Button btn = restartButton.GetComponent<Button>(); 
		btn.onClick.AddListener(TaskOnClick);
		Button btn1 = quitButton.GetComponent<Button>(); 
		btn1.onClick.AddListener(TaskOnClick1);
		audio = GetComponent<AudioSource> ();
		audio.Play();
	}
	void TaskOnClick(){
		restartLevel.LoadLastScene();
	}
	void TaskOnClick1(){
		SceneManager.LoadScene("scenes/mainMenu");
	}

}