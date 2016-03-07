using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine.UI;
using System;

public class FBShare : MonoBehaviour {

	private Text textComponent; 

	// Use this for initialization
	void Start () {
		textComponent = this.transform.GetChild (0).GetComponent<Text> ();
	}

	public void ShareFB ()
	{
		if (FB.IsLoggedIn) 
		{
			int score = BoogieDownGames.GameMaster.Instance.CurrentScore;
			int wave = BoogieDownGames.GameMaster.Instance.CurrentWave;
			FB.ShareLink (
				new Uri ("http://apple.co/1WqKatr"),
				"Space Dweebs 2",
				"Hey everyone, I got up to wave " + wave + " with a score of " + score + ", if you can't top that, then you're all a bunch of Space Dweebs! #SpaceDweebs2 #BoogieDownGames",
				new Uri ("http://bit.ly/20D20e1"),
				callback: ShareCallback
			);
		}
		else
		{
			var perms = new List<string>(){"public_profile", "email", "user_friends"};
			FB.LogInWithReadPermissions(perms, AuthCallback);
		}
	}

	private void AuthCallback (ILoginResult result) {
		if (FB.IsLoggedIn) {
			// AccessToken class will have session details
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log(aToken.UserId);
			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions) {
				Debug.Log(perm);
			}
		} else {
			Debug.Log("User cancelled login");
		}
	}

	private void ShareCallback (IShareResult result) {
		if (result.Cancelled || !String.IsNullOrEmpty(result.Error)) {
			Debug.Log("ShareLink Error: "+result.Error);
		} else if (!String.IsNullOrEmpty(result.PostId)) {
			// Print post identifier of the shared content
			Debug.Log(result.PostId);
		} else {
			// Share succeeded without postID
			Debug.Log("ShareLink success!");
		}
	}

	// Update is called once per frame
	void Update () {
		if (FB.IsLoggedIn) 
		{
			textComponent.text = "Share on Facebook";
		}
		else
		{
			textComponent.text = "Connect to Facebook";
		}
	}
}
	