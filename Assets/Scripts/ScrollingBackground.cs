using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BoogieDownGames {

	public class ScrollingBackground : MonoBehaviour {

		[SerializeField]
		private Vector2 m_speed;

		[SerializeField]
		private Vector2 m_offSet;

		[SerializeField]
		private Material m_mat;

		[SerializeField]
		private Image m_image;

		public Vector2 Speed 
		{
			get { return m_speed; }
			set { m_speed = value; }
		}

		// Use this for initialization
		void Start () 
		{
			m_mat = GetComponent<Image>().material;
			m_offSet = m_mat.GetTextureOffset("_MainTex");
		}
		
		// Update is called once per frame
		void Update ()
		{
			m_offSet += m_speed * Time.deltaTime;
			m_mat.SetTextureOffset("_MainTex",m_offSet);
		}
	}

}