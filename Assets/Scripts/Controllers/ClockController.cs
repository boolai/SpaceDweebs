//Handle control of the clock
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BoogieDownGames {

	public class ClockController : MonoBehaviour {

		[SerializeField]
		private Image m_image;

		// Use this for initialization
		void Start () 
		{
			NotificationCenter.DefaultCenter.AddObserver(this,"UpdateText");
		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}

		public void UpdateText(NotificationCenter.Notification p_note)
		{

			float d = (float)p_note.data["MaxTime"];
			float n = (float)p_note.data["Time2"];
			float results = n/d;

			m_image.fillAmount = results;
		}
	}
}