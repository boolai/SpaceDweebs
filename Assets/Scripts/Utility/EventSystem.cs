using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BoogieDownGames {

	public class EventSystem : UnitySingleton<EventSystem> {

		[SerializeField]
		private bool m_isRunning;

		[SerializeField]
		private GameObject m_panel;
		
		[SerializeField]
		private Text m_title;
		
		[SerializeField]
		private Text m_description;

		[SerializeField]
		private CanvasController m_canvasController;

		public bool IsRunning
		{
			get { return m_isRunning; }
			set { m_isRunning = value; }
		}


		[SerializeField]
		public List<EventsProperty> m_eventList;
		
		Dictionary<string, EventsProperty> m_eventBank = new Dictionary<string, EventsProperty>();

		// Use this for initialization
		void Start ()
		{
			//Lets see if this is running first

			NotificationCenter.DefaultCenter.AddObserver(this, "PlayEvent");
			gameObject.SetActive(GameMaster.Instance.isTutorialOn);

			foreach(EventsProperty e in m_eventList) {
				m_eventBank.Add(e.Name,e);
			}
		}

		public void SetTutorial(NotificationCenter.Notification p_note)
		{
			bool state = (bool)p_note.data["tutorial"];
			gameObject.SetActive(state);
		}
		
		public void ClosePanel()
		{
			m_panel.SetActive(false);

			GameMaster.Instance.MyTimeMode.UnPause();
		}
		
		public void OpenPanel()
		{
			m_panel.SetActive(true);
			GameMaster.Instance.MyTimeMode.Pause();
		}

		public void PlayEvent(NotificationCenter.Notification p_note)
		{

			//Get the name of the event
			string name = (string)p_note.data["event"];
			//Debug.LogError("Object sent message " + name); 
			//See if the event is in the bank
			if(m_eventBank.ContainsKey(name)) {

				EventsProperty prop = m_eventBank[name];

				if(prop.HasPlayed == false && m_isRunning == true) {

					//Set the properties
					
					m_title.text = prop.Title;
					
					m_description.text = prop.Description;
					
					m_canvasController.setAnime("displayevent");

					prop.HasPlayed = true;



					m_eventBank.Remove(name);

				}
			}

		}

		public void SetStateToViewEvent()
		{
			GameMaster.Instance.GameFSM.ChangeStateNoExit(GameStateEvent.Instance);
		}

		public void SetStateToPlayGame()
		{
			GameMaster.Instance.GameFSM.RevertToPreviousStateNoEnter();
		}
	}
}