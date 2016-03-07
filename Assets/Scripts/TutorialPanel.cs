using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BoogieDownGames {

public class TutorialPanel : MonoBehaviour {

		public Text m_text;

		public string m_narrationText;

		public void changeText(string p_text)
		{
			//Debug.LogError("Updating text " + p_text);
			m_text.text = p_text;
		}
	}
}