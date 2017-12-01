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
	//public AudioSource main;
	// Use this for initialization
	void Start () {
		pause = false;
		//GetComponent<AudioSource>().Stop();
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
				//GetComponent<AudioSource>().Stop();
				//Camera.main.GetComponent<AudioSource> ().UnPause();
			} else {
				Time.timeScale = 0;
				//Camera.main.GetComponent<AudioSource> ().Pause();
				//GetComponent<AudioSource> ().Play();
			}
			can.SetActive(pause);

		}


	}
	}

