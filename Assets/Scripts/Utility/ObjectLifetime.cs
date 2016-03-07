/*
 * Misael Aponte Feb 4, 2014
 * Object life time gotten tired of rewrtiting the same script 
 * over and over.  So now you can set the object's life time it can either be 
 * destroyed or inactive.
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoogieDownGames {

	public class ObjectLifetime : MonoBehaviour {

		enum DEATHTYPE { None, Destroy, Sleep };

		enum  CLOCKTYPE { Enumarative, Iterative };

		[SerializeField]
		DEATHTYPE m_deathType;

		[SerializeField]
		CLOCKTYPE m_myClockType;

		[SerializeField]
		private float m_timeTillDeath;

		[SerializeField]
		private bool m_isAutomated;

		[SerializeField]
		private bool m_isOnFIxedUpdate;

		delegate void deathTypeDelegate();

		deathTypeDelegate m_deathTypeDelegate;

		delegate void ClockTypeDelegate();
		ClockTypeDelegate ClockDelagate;

		[SerializeField]
		private TimeKeeper m_itrClock;

		public TimeKeeper MyClock
		{
			get { return m_itrClock; }
		}

		void Start()
		{
			if(m_isAutomated == true) {
				NotificationCenter.DefaultCenter.AddObserver(this, "GameStateGlobalUpdate");
				NotificationCenter.DefaultCenter.AddObserver(this, "GameStateGlobalFixedUpdate");
			}

			Init();
		}

		void SetClockType()
		{
			switch (m_myClockType) {

			case CLOCKTYPE.Enumarative:
				ClockDelagate = EnumarativeClock;
				StartCoroutine(myTimeIsOver(m_timeTillDeath,m_deathTypeDelegate));
				break;
			
			case CLOCKTYPE.Iterative:
				//Set the clock defaults
				m_itrClock.Counter = m_timeTillDeath;
				m_itrClock.TimeLimit = m_timeTillDeath;
				m_itrClock.startClock();
				ClockDelagate = IterativeClock;
				break;
			}
		}

		public void EnumarativeClock()
		{
			//Nothing cause we already called it on start
		}

		public void IterativeClock()
		{
			m_itrClock.Run();
			if(m_itrClock.IsDone == true) {
				m_itrClock.ResetClock();
				m_deathTypeDelegate();
			}
		}

		void setTheDeathType()
		{
			switch(m_deathType) {
			case DEATHTYPE.None:
				m_deathTypeDelegate = DeathByNone;
				break;
			
			case DEATHTYPE.Sleep:
				m_deathTypeDelegate = DeathBySleeping;
				break;

			case DEATHTYPE.Destroy:
				m_deathTypeDelegate = DeathByDestruction;
				break;
			}
		}

		void OnEnable()
		{
			setTheDeathType();
		}

		public void Init()
		{
			setTheDeathType();
			SetClockType();
			m_itrClock.ResetClock();
		}

		public void Init( float p_counter )
		{
			m_itrClock.Counter = p_counter;
		}
	
		//Deletes the object by destroying
		void DeathByDestruction ()
		{
			DestroyObject(this.gameObject);
		}

		//Death by setting inactive
		void DeathBySleeping ()
		{
			this.gameObject.SetActive(false);
		}

		void DeathByNone()
		{

		}

		//Used this to automate the clock
		public void GameStateGlobalUpdate()
		{
			ClockDelagate();
		}

		public void GameStateGlobalFixedUpdate()
		{
		}

		//Rund the clock when not automated
		public void RunDeathClock()
		{
			ClockDelagate();
		}


		IEnumerator myTimeIsOver(float p_sec,deathTypeDelegate p_del )
		{
			yield return new WaitForSeconds(p_sec);
			p_del();
		}
	}
}