using UnityEngine;
using System.Collections;

public class HitEffect : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	const float decreaseValue = 1;
	// Use this for initialization
	void Start () {
	
	}
	public void GotHit()
	{
		spriteRenderer.color = new Color(1,0,0, 0.5f);
	}
	// Update is called once per frame
	void Update () {
		Color spriteColor = spriteRenderer.color;

		spriteColor.a -= decreaseValue * Time.deltaTime;

		spriteRenderer.color = spriteColor;
	}
}
