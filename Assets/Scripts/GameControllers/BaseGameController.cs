using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

namespace BoogieDownGames {

	[Serializable]
	public class BaseGameController : UnitySingletonPersistent<BaseGameController> {

		#region MEMBERS

		[SerializeField]
		private string m_startScene;

		[SerializeField]
		private bool m_isPaused;

		[SerializeField]
		private bool m_hasControl; //Does the player have control

		[SerializeField]
		private int m_PlayerOneModelIndex;
		
		[SerializeField]
		private int m_PlayerTwoModelIndex;

		[SerializeField]
		private int m_currentScore;
		
		[SerializeField]
		private int m_highScore; 

		public bool isTutorialOn;

		[SerializeField]
		private TimeMode m_timemode;

		[SerializeField]
		private int m_currentRound;//Stores the current round index


		private FiniteStateMachine<BaseGameController> m_sceneFsm = new FiniteStateMachine<BaseGameController>();

		private FiniteStateMachine<BaseGameController> m_gameFsm = new FiniteStateMachine<BaseGameController>();

		#endregion

		#region PROPERTIES

		public FiniteStateMachine<BaseGameController> SceneFSM
		{
			get { return m_sceneFsm; }
		}

		public FiniteStateMachine<BaseGameController> GameFSM
		{
			get { return m_gameFsm; }
		}

		public bool HasControl
		{
			get { return m_hasControl; }
			set { m_hasControl = value; }
		}

		public int PlayerOneModelIndex
		{
			get { return m_PlayerOneModelIndex; }
			set { m_PlayerOneModelIndex = value; }
		}
		
		public int PlayerTwoModelIndex
		{
			get { return m_PlayerTwoModelIndex; }
			set { m_PlayerTwoModelIndex = value; }
		}

		public int HighScore
		{
			get { return m_highScore; }
			set { m_highScore = value; }
		}

		public bool IsTutorialOn
		{
			get { return isTutorialOn; }
			set { isTutorialOn = value; }
		}

		public int CurrentScore
		{
			get { return m_currentScore; }
			set { m_currentScore = value; }
		}

		public int CurrentWave
		{
			get { return m_currentRound; }
			set { m_currentRound = value; }
		}

		public TimeMode MyTimeMode
		{
			get { return m_timemode; }
		}

		#endregion

		public override void Awake ()
		{
			base.Awake ();
			//Debug.LogError("Setting the states");
			//set the default states
			m_gameFsm.Configure(this,GameStateIdle.Instance);
			m_gameFsm.SetGlobalState(GameStateGlobal.Instance);
			m_sceneFsm.Configure(this,SceneStateIdle.Instance);
			m_sceneFsm.SetGlobalState(SceneStateGlobal.Instance);
		}
		
		// Update is called once per frame
		void Update () 
		{
			m_sceneFsm.runOnUpdate();
			m_gameFsm.runOnUpdate();
		
		}

		void FixedUpdate ()
		{
			m_sceneFsm.runOnFixedUpdate();
			m_gameFsm.runOnFixedUpdate();
		}

		//Overide this if you want to change the behaviour
		public virtual void StartGame()
		{
			SceneManager.LoadScene(m_startScene);
		}

		public virtual void SetDifficulty(int p_diff)
		{

		}

		public virtual float GetTimer()
		{
			return 0f;
		}

		//Changes the scene need a wrapper
		public void ChangeScene(int p_index)
		{
			SceneManager.LoadScene(p_index);
		}

		public void ChangeScene(string p_level)
		{
			SceneManager.LoadScene(p_level);
		}

		public virtual void Pause()
		{
			Time.timeScale = 0f;
		}

		public virtual void UnPause()
		{
			Time.timeScale = 1.0f;
		}

		public void TooglePause()
		{
			m_isPaused = !m_isPaused;
			if(m_isPaused) {
				Time.timeScale = 0f;
			} else {
				Time.timeScale = 1f;
			}
		}

		public void SetTimeScale(float p_scale)
		{
			Time.timeScale = p_scale;
		}

		public virtual void PlayerLost()
		{
			//Do something player has lost
		}

	}
}