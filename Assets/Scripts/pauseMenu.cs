using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class pauseMenu : MonoBehaviour {

	public bool pause;
	public GameObject can;
	public Button restartButton;
	public Button quitButton;
	// Use this for initialization
	Button btn,btn1;
	public AudioSource main;
	AudioSource pauseAudio;
	// Use this for initialization
	void Start () {
		pause = false;
		pauseAudio=GetComponent<AudioSource>();
		pauseAudio.Stop ();
		can.SetActive(pause);
		btn = restartButton.GetComponent<Button>(); 
		btn.onClick.AddListener(TaskOnClick);
		btn1 = quitButton.GetComponent<Button>(); 
		btn1.onClick.AddListener(TaskOnClick1);
	
	}
	public void Awake(){
		pause = false;
		Time.timeScale = 1;
	}
	public void TaskOnClick(){
		SceneManager.LoadScene("scenes/"+SceneManager.GetActiveScene().name);
	}
	public void TaskOnClick1(){
		//pause = false;
		SceneManager.LoadScene("scenes/mainMenu");
	}
	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.P)){

			pause = !pause;
			if (!pause) {
				Time.timeScale = 1;
				pauseAudio.Stop();
				main.UnPause();
			} else {
				Time.timeScale = 0;
				main.Pause();
				Debug.Log ("MAINNN" + main.isPlaying);
				pauseAudio.Play();
			}
			can.SetActive(pause);

		}


	}
	}

