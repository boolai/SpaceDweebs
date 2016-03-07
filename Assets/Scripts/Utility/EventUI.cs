using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BoogieDownGames {

	public class EventUI : MonoBehaviour {

		[SerializeField]
		private GameObject m_panel;

		[SerializeField]
		private GameObject m_obj;

		[SerializeField]
		private Text m_title;

		[SerializeField]
		private Text m_description;

		// Use this for initialization
		void Start () 
		{
			NotificationCenter.DefaultCenter.AddObserver(this,"DisplayEvent"); 
		}

		public void DisplayEvent( NotificationCenter.Notification p_note )
		{

			GameObject obj = MemoryPool.Instance.findAndGetObjs((string)p_note.data["name"],false);

			obj.transform.position = obj.transform.position;

			m_title.text = (string)p_note.data["title"];

			m_description.text = (string)p_note.data["description"];

		}

		public void ClosePanel()
		{
			m_panel.SetActive(false);
		}

		public void OpenPanel()
		{
			m_panel.SetActive(true);
		}
	}
}