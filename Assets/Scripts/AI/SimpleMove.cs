using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoogieDownGames {

	public class SimpleMove : MonoBehaviour {

		[SerializeField]
		private float m_speedTrajectory;

		[SerializeField]
		private float m_speedRot;

		[SerializeField]
		private float m_fastMode;

		[SerializeField]
		private float m_slowMode;

		[SerializeField]
		private bool m_isSleepOnExit;

		[SerializeField]
		private List<GameObject> m_particels;

		// Use this for initialization
		void Start () 
		{
			NotificationCenter.DefaultCenter.AddObserver(this,"GameStatePlayersTurnUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePauseEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePauseUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePauseExit");
			NotificationCenter.DefaultCenter.AddObserver(this,"SetFastMode");
			NotificationCenter.DefaultCenter.AddObserver(this,"SetSlowMode");
			NotificationCenter.DefaultCenter.AddObserver(this,"Sleep");


		}

		public void Sleep()
		{
			if(m_isSleepOnExit) {
				gameObject.SetActive(false);
			}
		}

		public void SetFastMode()
		{
			//m_speedTrajectory = m_fastMode;
		}

		public void SetSlowMode()
		{
			//m_speedTrajectory = m_slowMode;
		}

		public void GameStatePauseEnter()
		{
			foreach(GameObject obj in m_particels) {
				obj.SetActive(false);
			}

		}

		public void GameStatePauseExit()
		{
			foreach(GameObject obj in m_particels) {
				obj.SetActive(true);
			}
		}

		public void GameStatePlayersTurnUpdate()
		{
			//m_rigid2D.AddForce(new Vector2(m_speedTrajectory,0));
			//transform.Rotate(new Vector3(0,m_speedRot * Time.deltaTime,0));
			transform.Translate( new Vector3(m_speedTrajectory * Time.deltaTime,0,0));

			if(transform.position.x >= 4000) {
				gameObject.SetActive(false);
			} 

			if(transform.position.x <= -4000) {
				gameObject.SetActive(false);
			}
		}
	}
}