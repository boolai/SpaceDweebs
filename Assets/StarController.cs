using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StarController : MonoBehaviour {

	public ParticleSystem Stars;
	public Button Pause;

	// Use this for initialization
	public void ToggleStars () {
		if (Stars.gravityModifier == 0)
			Stars.gravityModifier = 3;
		else
			Stars.gravityModifier = 0;
	}

	public void TogglePause ()
	{
		Pause.interactable = !Pause.interactable;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
