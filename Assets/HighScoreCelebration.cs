using UnityEngine;
using System.Collections;

namespace BoogieDownGames {

	public class HighScoreCelebration : MonoBehaviour {

	    [SerializeField]
	    private GameObject confettiHolder;
	

		[SerializeField]
		private bool m_isPlayingSound;

		// Use this for initialization
		void Start () 
	    {
	        NotificationCenter.DefaultCenter.AddObserver(this, "NewHighScore");
		}
		
		// Update is called once per frame
		void Update () {
		
		}

	    public void NewHighScore()
	    {
	        confettiHolder.transform.GetChild(0).gameObject.SetActive(true);
            
			if(m_isPlayingSound) {

				gameObject.GetComponent<AudioSource>().Play();
			}
	    }

	}
}