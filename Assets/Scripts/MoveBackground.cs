using UnityEngine;
using System.Collections;

namespace BoogieDownGames {

	public class MoveBackground : MonoBehaviour {

		[SerializeField]
		private float m_speedMin;

		[SerializeField]
		private float m_speedMax;

		[SerializeField]
		private float m_currentSpeed;

		[SerializeField]
		private float m_minSize;

		[SerializeField]
		private float m_maxSize;

		[SerializeField]
		private float m_currentSize;

		// Use this for initialization
		void Start () 
		{
			SetObject();
		}

		public void SetObject()
		{
			m_currentSpeed = UnityEngine.Random.Range(m_speedMin, m_speedMax);
			
			m_currentSize = UnityEngine.Random.Range(m_minSize, m_maxSize);
			transform.localScale = new Vector3(m_currentSize,m_currentSize,1);
		}

		// Update is called once per frame
		void Update () 
		{
			transform.Translate( new Vector3(0,m_currentSpeed * Time.deltaTime,0));
		}
	}
}