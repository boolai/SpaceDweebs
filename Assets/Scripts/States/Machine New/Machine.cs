using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoogieDownGames {

	public class Machine : MonoBehaviour {

		[SerializeField]
		private List<string> m_states;

		private Dictionary<string,string> m_statesBank = new Dictionary<string,string >();

		[SerializeField]
		private string m_current;

		[SerializeField]
		private int m_index;

		[SerializeField]
		private string m_previous;

		[SerializeField]
		private bool m_isTransitioning;

		// Use this for initialization
		void Start () 
		{
			foreach(string state in m_states) {
				m_statesBank.Add(state,state);
			}

			if(m_statesBank.Count > 0) {
				m_current = m_states[0];
			}
		}
		
		// Update is called once per frame
		void Update () 
		{
			if(!m_isTransitioning) {
				NotificationCenter.DefaultCenter.PostNotification(this,m_statesBank[m_current] + "Update");
			}
		}

		void FixedUpdate()
		{
			NotificationCenter.DefaultCenter.PostNotification(this,m_statesBank[m_current] + "FixedUpdate");
		}

		public void ChangeState(string p_newState)
		{
			m_isTransitioning = true;
			NotificationCenter.DefaultCenter.PostNotification(this,m_statesBank[m_current] + "Exit");
			m_current = p_newState;
			//enter the new state
			NotificationCenter.DefaultCenter.PostNotification(this,m_statesBank[m_current] + "Enter");
			m_isTransitioning = false;
		}

	}
}