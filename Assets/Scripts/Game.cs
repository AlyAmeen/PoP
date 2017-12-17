using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
	AudioSource a;
	public AudioSource effect1,effect2,effect3,effect4;

    Animation anim;

    float timer = 0;

    public float tourTime = 90;
	// Use this for initialization
	void Start () {
		a = GetComponent<AudioSource> ();
		effect1 = GetComponent<AudioSource> ();
		effect2 = GetComponent<AudioSource> ();
		effect3 = GetComponent<AudioSource> ();
		effect4 = GetComponent<AudioSource> ();
		effect1.volume=PlayerPrefs.GetFloat("effect1");
		effect2.volume=PlayerPrefs.GetFloat("effect1");
		effect3.volume=PlayerPrefs.GetFloat("effect1");
		effect4.volume=PlayerPrefs.GetFloat("effect1");
		a.volume=PlayerPrefs.GetFloat("music");
		Debug.Log ("MUSIIIC IN L111" +a.volume);
		a.Play ();

        anim = GetComponent<Animation>();
		//Debug.Log("VOLUME"+a.volume);
	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;
        if (timer >= tourTime ||  Input.anyKeyDown)
        {
            if (anim != null)
            anim.enabled = false;
            GetComponent<TPSCamera>().enabled = true;
        }
		
	}
}
