using UnityEngine;
using System.Collections;

namespace BoogieDownGames {

	public class AnimateMaterial : MonoBehaviour {

		[SerializeField]
		private float m_speed;

		[SerializeField]
		private Renderer m_renderer;


		// Use this for initialization
		void Start () {
			m_renderer = GetComponent<Renderer>();
		}
		
		// Update is called once per frame
		void Update () {

			float y = Mathf.Repeat(Time.time * m_speed,0.20f);
			Vector2 offSet = new Vector2(0,y);
			m_renderer.sharedMaterial.SetTextureOffset("_MainTex",offSet);
		}
	}
}