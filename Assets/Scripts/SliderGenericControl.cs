using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BoogieDownGames {

	public class SliderGenericControl : MonoBehaviour {

		[SerializeField]
		private Slider m_slider;

		// Use this for initialization
		void Start () 
		{
			NotificationCenter.DefaultCenter.AddObserver(this, "SetSlider");
		}
		
		// Update is called once per frame
		public void SetSlider (NotificationCenter.Notification p_note) 
		{
			m_slider.maxValue = (int)p_note.data["MaxLives"];
			m_slider.value = (int)p_note.data["LivesInt"];
		}
	}
}