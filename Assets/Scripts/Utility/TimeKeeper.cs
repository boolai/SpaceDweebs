/* Sometimes corroutines are not appropiate for the situations
 * So I made this simple timer to get stuff done in a more agile way.
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BoogieDownGames {
	[Serializable]
	public class TimeKeeper : PropertyAttribute {
		
		[SerializeField]
		private float m_timeLimit;
		
		[SerializeField]
		private float m_counter;
		
		[SerializeField]
		private bool m_isDone;

		[SerializeField]
		private bool m_isStopped;

		#region PROPERTIES

		public float TimeLimit
		{
			get { return m_timeLimit; }
			set { m_timeLimit = value; }
		}
		
		public float Counter
		{
			get { return m_counter; }
			set { m_counter = value; }
		}
		
		public bool IsDone
		{
			get { return m_isDone; }
			set { m_isDone = value;  }
		}

		public bool IsStopped
		{
			get { return m_isStopped; }
			set { m_isStopped = value; }
		}
	
		#endregion


		void Start()
		{
			startClock();
		}
		
		public bool Run()
		{
			if(m_isStopped == false) {

				if(m_counter > 0 ) {
					m_counter -= Time.deltaTime;
				} else {
					m_isDone = true;
				}
			}
			return m_isDone;
		}
		
		//Resets and starts the clock
		public void startClock()
		{
			m_counter = m_timeLimit;
			m_isDone = false;
			m_isStopped = false;
		}

		public void startClock(float p_sec)
		{
			m_timeLimit = p_sec;
			m_counter = m_timeLimit;
			m_isDone = false;
			m_isStopped = false;
		}

		public void StopClock()
		{
			m_isStopped = true;
		}

		public void ContinueClock()
		{
			m_isStopped = false;
		}


		public void ToggleClock()
		{
			m_isStopped = !m_isStopped;
		}

		public void ResetClock()
		{
			startClock();
		}
		
	}
}