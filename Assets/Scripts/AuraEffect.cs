using UnityEngine;
using System.Collections;

namespace BoogieDownGames{

	public class AuraEffect : MonoBehaviour {

		// Use this for initialization
		void Start () 
		{
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayExit");
		}

		public void GameStatePlayEnter()
		{
			GetComponent<ParticleSystem>().Play();
		}

		public void GameStatePlayExit()
		{
			GetComponent<ParticleSystem>().Pause();
		}
	}

}