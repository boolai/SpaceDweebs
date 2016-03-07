using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BoogieDownGames {

	public class TextMachine : MonoBehaviour {

		[SerializeField]
		private Text m_text;

		[SerializeField]
		private List<string> m_listenList;

		private Dictionary <string,string> m_listenBank = new Dictionary<string, string>();

		// Use this for initialization
		void Start () 
		{

			m_text = GetComponent<Text>();

			NotificationCenter.DefaultCenter.AddObserver(this, "UpdateText");

			foreach(string s in m_listenList) {

				if(!m_listenBank.ContainsKey(s)) {
					m_listenBank.Add(s,s);
				}
			}
		}

		public void UpdateText(NotificationCenter.Notification p_note)
		{
			//See if the note contains what we are listening for
			foreach(string s in m_listenList) {

				if(p_note.data.Contains(s)) {
					m_text.text = (string)p_note.data[s];

					return;
				}
			}
		}
	}

}