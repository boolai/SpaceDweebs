using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BoogieDownGames {

	public class UiMenuController : UnitySingleton<UiMenuController> {

		[SerializeField]
		private GameObject m_altPanel;

		[SerializeField]
		private Animator m_anime;

		void Awake()
		{
			if(m_anime == null) {
				m_anime = GetComponent<Animator>();
			}
		}

		public void TurnOnPanel()
		{
			m_altPanel.SetActive(true);
			NotificationCenter.DefaultCenter.PostNotification(this, "PlayCurrentSong");
		}

		public void TurnOffPanel()
		{
			m_altPanel.SetActive(false);
		}

		public void SwitchAnimeState(string p_state)
		{
			m_anime.SetTrigger(p_state);
		}

		public void SwitchScene(string p_level)
		{
			SceneManager.LoadScene (p_level);
		}
	}
}