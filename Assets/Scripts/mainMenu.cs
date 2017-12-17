using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class mainMenu : MonoBehaviour {
	public Button startButton;
	public Dropdown mydropdown;
	public Button quitButton;
	string stringToEdit;
	public Canvas canvasB;
	float m_MySliderValue;
	public Slider musicSlider;
	public Slider speechSlider;
	public Slider effectsSlider;
	AudioSource speech,effect1,effect2,effect3,effect4;
	public AudioSource music;
	// Use this for initialization
	void Start () {
		Button btn = startButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
		Button btn1 = quitButton.GetComponent<Button>();
		btn1.onClick.AddListener(TaskOnClick1);
		musicSlider.value = 0.5f;
		speechSlider.value = 0.5f;
		effectsSlider.value = 0.5f;
		musicSlider.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
		speechSlider.onValueChanged.AddListener(delegate {ValueChangeCheck1(); });
		effectsSlider.onValueChanged.AddListener(delegate {ValueChangeCheck2(); });
		mydropdown.onValueChanged.AddListener(delegate{selectvalue(mydropdown);});
		music = GetComponent<AudioSource> ();
		//audio = GetComponent<AudioSource> ();
		
		//audio.Play();
		//audio.bypassEffects = false;
	    Camera.main.GetComponent<AudioSource>().Play();
	}
	public void ValueChangeCheck()
	{   
		music.volume = musicSlider.value;
		PlayerPrefs.SetFloat ("music", music.volume);
	}
	public void ValueChangeCheck1()
	{

        PlayerPrefs.SetFloat("speech", speechSlider.value);
    }
	public void ValueChangeCheck2()
	{
		PlayerPrefs.SetFloat ("effect1", effectsSlider.value);
	}
	private void selectvalue(Dropdown gdropdown)
	{

	}
	// Update is called once per frame
	void Update () {
		dropdownchk ();
	}
	void OnGUI() {
		GUI.color = Color.black;
		GUI.Label(new Rect(10,10,500,500), stringToEdit);
	}
	public void dropdownchk()
	{
		if (mydropdown.value == 0) {
			canvasB.gameObject.SetActive (true);
			stringToEdit = " ";

		} else if (mydropdown.value == 1) {
			canvasB.gameObject.SetActive (false);
			stringToEdit = "How to Play";

		} else if (mydropdown.value == 2) {
			canvasB.gameObject.SetActive(false);
			stringToEdit = "Credits";
		}
			

	}
	void TaskOnClick()
	{
		SceneManager.LoadScene("scenes/mainScene");

	}
	void TaskOnClick1()
	{
		Application.Quit();
	}

}
