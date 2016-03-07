/*
 * A modular Stats system
 * This allows me to just add as many stats as I want per character
 * It contains a regen ablitiy
 */
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BoogieDownGames {

	[Serializable]
	public class Stat : PropertyAttribute {

		#region MEMBERS

		[SerializeField]
		private string m_name; //Stores the name of the Stat

		[SerializeField]
		private int m_currentValue;  //The current realtime value of the stat

		[SerializeField]
		private int m_maxValue; //The max value threshold. The state will never go pass this number as we used Unity's clamp method to keep it from so.

		[SerializeField]
		private bool m_canRegen; //A boolean designating if this stat can regen or not

		[SerializeField]
		private float m_regenRate; //The regenaration rate sets the times loop

		[SerializeField]
		private int m_regenIncrement; //The value amount we will use to increment with

		[SerializeField]
		private TimeKeeper m_timer; // The timer object

		[SerializeField]
		private Slider m_sliderUI; //The ui slider to display the values

		#endregion

		#region PROPERTIES

		public string Name
		{
			get { return m_name; }
		}

		public int CurrentValue
		{
			get { return m_currentValue; }
			set { m_currentValue = value; }
		}

		public int MaxValue
		{
			get { return m_maxValue; }
			set { m_maxValue = value; }
		}

		public bool CanRegen
		{
			get { return m_canRegen; }
			set { m_canRegen = value; }
		}

		public bool RegenRate
		{
			get { return m_canRegen; }
			set { m_canRegen = value; }
		}

		public TimeKeeper MyTimer
		{
			get { return m_timer; }
		}

		public int RegenIncrement
		{
			get { return m_regenIncrement; }
			set { m_regenIncrement = value; }
		}


		#endregion

		#region METHODS

		//Initializes the timer object
		public void SetTimer()
		{
			m_timer.Counter = m_regenRate;
			m_timer.TimeLimit = m_regenRate;
			m_timer.startClock();
		}

		//Runs the stat logic
		public void RunStat()
		{
			m_timer.Run();
			if(m_timer.IsDone) {

				Regenerate();
				m_timer.Counter = m_regenRate;
				m_timer.TimeLimit = m_regenRate;
				m_timer.ResetClock();
			}

			UpdateDisplay();
		}

		//Helper function so we do not have so have a tangle of if statements
		public void Regenerate()
		{
			if(m_canRegen == true) {
				m_currentValue = Mathf.Clamp((m_currentValue + m_regenIncrement),0,m_maxValue);
			}
		}

		//Send the info to the sliders ui
		public void UpdateDisplay()
		{
			if(m_sliderUI != null) {

				m_sliderUI.value = m_currentValue;
				m_sliderUI.maxValue = m_maxValue;
			}
		}

		#endregion
	}
}