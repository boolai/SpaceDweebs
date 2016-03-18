using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public class AlienAnimeController : MonoBehaviour {

		[SerializeField]
		private Animator m_anime;

		// Use this for initialization
		void Start () 
		{
			if(!m_anime) {
				m_anime = GetComponent<Animator>();
			}
			NotificationCenter.DefaultCenter.AddObserver(this,"GameOver");
		}

		public void GameOver()
		{
			m_anime.SetTrigger("dance");
		}

	}
}