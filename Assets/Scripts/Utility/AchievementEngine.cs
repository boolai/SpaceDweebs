using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using FullInspector;

namespace BoogieDownGames {
	
	public class AchievementEngine : MonoBehaviour {

		public GameObject m_panel;

		public Text m_title;

		public Text m_description;

		public Image m_trophyPic;

		public float m_displayTime; //How long to display achievement panel

		public List<Achievement> m_bank = new List<Achievement>();

		public Dictionary<string, int> m_achievementBankKey = new Dictionary<string,int>();
		
		// Use this for initialization
		void Start () 
		{
			for(int index = 0; index < m_bank.Count; ++index) {

				if(!m_achievementBankKey.ContainsKey(m_bank[index].Name)) {
					m_achievementBankKey.Add( m_bank[index].Name , index);
				}
			}
			NotificationCenter.DefaultCenter.AddObserver(this,"ListenToEvent");
		}

		void Update()
		{
			if(Input.GetKeyDown(KeyCode.Space)) {
				//Debug.LogError("sending message");
				Hashtable dat = new Hashtable();
				dat.Add("name","test");
				dat.Add("value", 1);
				NotificationCenter.DefaultCenter.PostNotification(this ,"ListenToEvent", dat);
			}
		}

		public void ListenToEvent(NotificationCenter.Notification p_note)
		{
			//Debug.LogError("I got the error");
			//See if we have the event
			if( p_note.data.ContainsKey("name" ) ) {

				string n = (string)p_note.data["name"];

				if( m_achievementBankKey.ContainsKey( n ) ) {

					Achievement achieve = m_bank[ m_achievementBankKey[n] ];
					achieve.ProgressValue += (int)p_note.data["value"];
					if(achieve.CheckForCompletion()) {

						StartCoroutine(DisplayAchievement(m_displayTime,achieve));
					}
				}
			}
		}

		IEnumerator DisplayAchievement(float p_sec, Achievement p_a)
		{

			m_panel.SetActive(true);
			//m_trophyPic = p_a.TrophyPic;
			m_title.text = p_a.Title;
			m_description.text = p_a.Description + " Has been achieved " + " " + p_a.AchieveValue; 
			m_trophyPic.sprite = p_a.TrophyPic;
			yield return new WaitForSeconds(p_sec);

			m_panel.SetActive(false);

		}
	}

}