using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BoogieDownGames {
		
	public enum ScoreType { Score, HighScore };

	public class ScoreText : MonoBehaviour {

		[SerializeField]
		private ScoreType m_scoreType;

		[SerializeField]
		private Text m_text;

		// Use this for initialization
		void Start ()
		{
			NotificationCenter.DefaultCenter.AddObserver(this, "UpdateScore");
		}

		public void UpdateScore(NotificationCenter.Notification p_note)
		{
			var i = 0;
			if(m_scoreType == ScoreType.Score ) {
				i = (int)p_note.data["Score"];
				m_text.text = i.ToString();
			} else {
				i = (int)p_note.data["HighScore"];
				m_text.text = i.ToString();
			}
		}
	}
}
