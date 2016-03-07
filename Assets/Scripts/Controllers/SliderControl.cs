using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BoogieDownGames {

	public class SliderControl : MonoBehaviour {

		[SerializeField]
		private Slider m_myslider;

		[SerializeField]
		private Color m_normalColor;

		[SerializeField]
		private GameObject m_fastEffect;
		[SerializeField]
		private Color m_fastColor;

		[SerializeField]
		private GameObject m_slowEffect;

		[SerializeField]
		private Color m_slowColor;

		[SerializeField]
		private Image m_image; //The actual bar

		[SerializeField]
		private GameObject m_firePanel;

		[SerializeField]
		private GameObject m_icePanel;

		// Use this for initialization
		void Start () 
		{
			NotificationCenter.DefaultCenter.AddObserver(this, "SetSlider");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayersTurnUpdate");
		}

		public void SetSlider(NotificationCenter.Notification p_note)
		{
			m_myslider.value = (float)p_note.data["Time2"];
			m_myslider.maxValue = (float)p_note.data["MaxTime"];
		}

		public void GameStatePlayersTurnUpdate()
		{
			if(Time.timeScale > 1.0f) {
				m_fastEffect.SetActive(true);
				m_fastEffect.GetComponent<ParticleSystem>().Play();
				m_slowEffect.SetActive(false);
				m_image.color = m_fastColor;
				m_firePanel.SetActive(true);
				m_icePanel.SetActive(false);
			} else if(Time.timeScale < 1.0f) {
				m_image.color = m_slowColor;
				m_fastEffect.SetActive(false);
				m_slowEffect.SetActive(true);
				m_slowEffect.GetComponent<ParticleSystem>().Play();
				m_firePanel.SetActive(false);
				m_icePanel.SetActive(true);
			} else {
				m_image.color = m_normalColor;
				m_fastEffect.SetActive(false);
				m_slowEffect.SetActive(false);
				m_firePanel.SetActive(false);
				m_icePanel.SetActive(false);
			}
		}
	}
}