/// <summary>
/// Time mode.
/// Used to adjust and keep track of the timeScale
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoogieDownGames {

	public enum CurrentTimeMode { Normal, Fast, SlowMo };

	[Serializable]
	public class TimeMode : PropertyAttribute {

		[SerializeField]
		private CurrentTimeMode m_currentMode;

		[SerializeField]
		private bool m_isPaused;

		//Following is the timelimits for each mode
		[SerializeField]
		private float m_normalTimeLimit;

		[SerializeField]
		private float m_fastTimeLimit;

		[SerializeField]
		private float m_SlowMoTimeLimit;

		[SerializeField]
		private float m_prevTimeScale;

		[SerializeField]
		private TimeKeeper m_modeClock; //The mode clock runs during modes and keeps track of the current time scale

		public delegate void MultiDelegate();
		public MultiDelegate RunTimeMode;

		#region PROPERTIES

		public bool IsPaused
		{
			get { return m_isPaused; }
			set { m_isPaused = value; }
		}

		public CurrentTimeMode MyCurrentTimeMode
		{
			get { return m_currentMode; }
			set { m_currentMode = value; }
		}

		public float PrevTimeScale
		{
			get { return m_prevTimeScale; }
			set { m_prevTimeScale = value; }
		}


		#endregion

		public void SetMode(CurrentTimeMode p_mode)
		{
			m_currentMode = p_mode;
			switch(m_currentMode) {

			case CurrentTimeMode.Normal:
				RunTimeMode = NormalMode;
				m_modeClock.ResetClock();

				break;

			case CurrentTimeMode.SlowMo:

				m_modeClock.Counter = m_SlowMoTimeLimit;
				m_modeClock.TimeLimit = m_SlowMoTimeLimit;

				RunTimeMode = SlowMode;

				m_modeClock.ResetClock();


				break;

			case CurrentTimeMode.Fast:

				m_modeClock.Counter = m_fastTimeLimit;
				m_modeClock.TimeLimit = m_fastTimeLimit;
				m_modeClock.ResetClock();
				RunTimeMode = RunFastMode;
			
				break;
			}
		}

		public void NoTimeModeRunning()
		{

			if(!m_isPaused) {
				Time.timeScale = 1f;
			}
		}

		public void NormalMode()
		{

			if(!m_isPaused) {
				Time.timeScale = 1f;
			}
		}

		public void RunFastMode()
		{

			if(!m_isPaused) {
				m_prevTimeScale = 2f;
				Time.timeScale = 2f;
				m_modeClock.Run();
				if(m_modeClock.IsDone) {
					m_modeClock.ResetClock();
					SetMode(CurrentTimeMode.Normal);

				}
			}
		}

		public void SlowMode()
		{
			if(!m_isPaused) {

				m_modeClock.Run();
				m_prevTimeScale = 0.5f;
				Time.timeScale = 0.5f;
				if(m_modeClock.IsDone) {

					m_modeClock.ResetClock();
					SetMode(CurrentTimeMode.Normal);
				}

			}
		}

		public void Pause()
		{
			m_prevTimeScale = Time.timeScale;
			Time.timeScale = 0f;
			m_isPaused = true;
		}

		public void UnPause()
		{
			m_isPaused = false;
			Time.timeScale = m_prevTimeScale;
		}
	}
}