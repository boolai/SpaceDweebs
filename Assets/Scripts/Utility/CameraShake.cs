using UnityEngine;
using System.Collections;

namespace BoogieDownGames {

	public class CameraShake : MonoBehaviour {

		[SerializeField]
		private Vector3 m_orginalPos;

		[SerializeField]
		private TimeKeeper m_timer;

		[SerializeField]
		private TimeKeeper m_innerTimer;

		[SerializeField]
		private float m_intensity;

		[SerializeField]
		private float m_speed;

		[SerializeField]
		private bool m_isShakeOn;

		[SerializeField]
		private Vector3 m_newPos;

		[SerializeField]
		private Transform Ship;

		[SerializeField]
		private Transform Platforms;
		[SerializeField]
		private Vector3 ShipDefaultPos;
		[SerializeField]
		private Vector3 PlatformsDefaultPos;

		// Use this for initialization
		void Start () 
		{
			NotificationCenter.DefaultCenter.AddObserver(this,"ShakeCamera");
			NotificationCenter.DefaultCenter.AddObserver(this,"GameStatePlayersTurnEnter");
			NotificationCenter.DefaultCenter.AddObserver(this,"GameStatePlayersTurnUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this,"GameStatePlayersTurnExit");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateSetUpSetUp");

			ShipDefaultPos = Ship.position;
			PlatformsDefaultPos = Platforms.position;
			m_orginalPos = transform.position;
		}

		public void ShakeCamera()
		{
			m_timer.ResetClock();
			m_isShakeOn = true;
		}

		public void GameStatePlayersTurnEnter()
		{
			m_orginalPos = transform.position;
			m_isShakeOn = false;
			transform.position = m_orginalPos;
			Platforms.position= PlatformsDefaultPos;
			Ship.position = ShipDefaultPos;
		}

		public void GameStatePlayersTurnUpdate() 
		{

			if(m_isShakeOn) {

				m_timer.Run();
				if(!m_timer.IsDone) {
					m_newPos.x = UnityEngine.Random.Range((-1*m_intensity),m_intensity);
					m_newPos.y = UnityEngine.Random.Range((-1*m_intensity),m_intensity);
					transform.position += m_newPos;
					Platforms.localPosition += m_newPos;
					Ship.localPosition += m_newPos;
				} else {
					transform.position = m_orginalPos;
					Platforms.position= PlatformsDefaultPos;
					Ship.position = ShipDefaultPos;
				}
			} else {
				transform.position = m_orginalPos;
				Platforms.position= PlatformsDefaultPos;
				Ship.position = ShipDefaultPos;
			}
		}

		public void GameStatePlayersTurnExit()
		{
			m_isShakeOn = false;
			transform.position = m_orginalPos;
			Platforms.position= PlatformsDefaultPos;
			Ship.position = ShipDefaultPos;
		}

		public void GameStateSetUpSetUp()
		{
			m_isShakeOn = false;
			transform.position = m_orginalPos;
			Platforms.position= PlatformsDefaultPos;
			Ship.position = ShipDefaultPos;
		}
	}
}