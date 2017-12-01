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
	//public AudioSource x;
	// Use this for initialization
	void Start () {
		//x=GetComponent<AudioSource>();
		Button btn = startButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
		Button btn1 = quitButton.GetComponent<Button>();
		btn1.onClick.AddListener(TaskOnClick1);
		musicSlider.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
		speechSlider.onValueChanged.AddListener(delegate {ValueChangeCheck1(); });
		effectsSlider.onValueChanged.AddListener(delegate {ValueChangeCheck2(); });
		mydropdown.onValueChanged.AddListener(delegate{selectvalue(mydropdown);});
	}
	public void ValueChangeCheck()
	{
		Debug.Log("MUSICCCC"+musicSlider.value);
		//x.volume = musicSlider.value;
	}
	public void ValueChangeCheck1()
	{
		Debug.Log("SPEECH"+speechSlider.value);
		//x.volume = speechSlider.value;
	}
	public void ValueChangeCheck2()
	{
		Debug.Log("EFFECTSS"+effectsSlider.value);
		//x.volume = effectsSlider.value;
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
		SceneManager.LoadScene("scenes/levelOne");

	}
	void TaskOnClick1()
	{
		Application.Quit();
	}

}
