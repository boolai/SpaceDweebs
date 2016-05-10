using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BoogieDownGames {


	public enum CanvasTriggers { openmenu };

	public class CanvasController : MonoBehaviour {

		[SerializeField]
		private Animator m_anime;

		[SerializeField]
		private Button m_pauseButton;

		void Awake()
		{
			m_anime = GetComponent<Animator>();
			NotificationCenter.DefaultCenter.AddObserver(this,"openPanel");

			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerOneLostEnter");
		}

		public void setAnime(string p_trigger)
		{
			m_anime.SetTrigger(p_trigger);
		}

		public void openPanel(NotificationCenter.Notification p_note)
		{
			//Debug.LogError("Opening game over panel");
			m_anime.SetTrigger((string)p_note.data["state"]);
		}

		public void GameStatePlayerOneLostEnter()
		{
			m_anime.SetTrigger("opengameover");
		}

		public void PauseGame()
		{
			Time.timeScale = 0.0f;
			m_pauseButton.interactable = false;
		}

		public void UnPauseGame()
		{
			Time.timeScale = 1.0f;
			m_pauseButton.interactable = true;
		}
	}
}