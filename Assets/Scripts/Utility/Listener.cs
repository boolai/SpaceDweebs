using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace BoogieDownGames {

	public class Listener : MonoBehaviour {

		[SerializeField]
		private GameObject m_panel;

		// Use this for initialization
		void Start () 
		{
			NotificationCenter.DefaultCenter.AddObserver(this,"GameOver");
		}

		public void GameOver()
		{
			m_panel.SetActive(true);
		}

	}
}