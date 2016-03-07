using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BoogieDownGames {

	public class RandomImageColors : MonoBehaviour {

        [SerializeField]
        private SpriteRenderer m_spRend;

        public float m_red;

        public float m_green;

        public float m_blue;

		// Use this for initialization
		void Start () 
		{
     
			SetRandomColor();
		}

		public void SetRandomColor()
		{
			m_red = UnityEngine.Random.Range(0,255);
			m_green = UnityEngine.Random.Range(0,255);
			m_blue = UnityEngine.Random.Range(0,255);
            m_spRend.color = new Color(m_red/255, m_green/255, m_blue/255, 235);
         
		}
	}
}