using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace BoogieDownGames {

	[Serializable]
	public class UIController : MonoBehaviour {

		[SerializeField]
		private Button m_myButton;

		[SerializeField]
		private Animator m_anime;

		// Use this for initialization
		void Start () 
		{

			NotificationCenter.DefaultCenter.AddObserver(this, "ChangingScene");
			if(m_myButton == null) {
				m_myButton = GetComponent<Button>();
			}
			if(m_anime == null) {
				m_anime = GetComponent<Animator>();
			}
			m_anime.SetTrigger("Open");
		}

		public void ChangingScene()
		{
			m_anime.SetTrigger("Close");
		}

		public void InteractiveOn()
		{
			m_myButton.interactable = true;
		}

		public void InteractiveOff()
		{
			m_myButton.interactable = false;
		}

		public void SwitchAnimationState(string p_state)
		{
			m_anime.SetTrigger(p_state);
		}

		public void GoToLevel(string p_level)
		{
			//Debug.LogError("Switching scene");
			GameMaster.Instance.ChangeScene(p_level);
		}
	}
}