using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TwitterShare : MonoBehaviour {

	const string Address = "http://twitter.com/intent/tweet";

	// Use this for initialization
	void Start () {
	}

	public void ShareTwitter ()
	{
		int score = BoogieDownGames.GameMaster.Instance.CurrentScore;
		int wave = BoogieDownGames.GameMaster.Instance.CurrentWave;
		Share("I got up to wave " + wave + " with a score of " + score + ", can you top that? #SpaceDweebs2 #BoogieDownGames", 
			"http://apple.co/1WqKatr", 
			"BoogieDownGames,ninjabit6,onizuka101,boolai007", 
			"en");
	}

	public static void Share(string text, string url,
		string related, string lang="en")
	{
		Application.OpenURL(Address +
			"?text=" + WWW.EscapeURL(text) +
			"&amp;url=" + WWW.EscapeURL(url) +
			"&amp;related=" + WWW.EscapeURL(related) +
			"&amp;lang=" + WWW.EscapeURL(lang));
	}
}
