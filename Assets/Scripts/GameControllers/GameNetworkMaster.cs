using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace BoogieDownGames {

	public class GameNetworkMaster : NetworkBehaviour {

		[SerializeField]
		private bool isPlayerOneTurn; //Player one is always the server

		public TimeKeeper m_startGameTimer;
		
		public FiniteStateMachine<GameNetworkMaster> m_gameFSM = new FiniteStateMachine<GameNetworkMaster>();


		public void Awake()
		{
			m_gameFSM.Configure(this,GameStateNWIdle.Instance);
			SetNotifications();
		}
			// Use this for initialization
		void Start () 
		{
			//SetNotifications();
		}

		//All of the notfications we want to listen for. We have to set it on every level load
		void SetNotifications()
		{
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateIdleEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateIdleUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateIdleExit");

			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateSetUpEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateSetUpUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateSetUpExit");

			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerOneTurnEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerOneTurnUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerOneTurnExit");

			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerOneTurnEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerOneTurnUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerOneTurnExit");

			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerTwoTurnEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerTwoTurnUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerTwoTurnEnter");

			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateStartingGameEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateStartingGameUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateStartingGameExit");

			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateSetupEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateSetUpUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateSetUpExit");


			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerOneTurnEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerOneTurnUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerOneTurnExit");

			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerTwoTurnEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerTwoTurnUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerTwoTurnExit");

			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateEndRoundEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateEndRoundUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateEndRoundExit");

			NotificationCenter.DefaultCenter.AddObserver(this, "PlayerJoinedGame");

		}

		public void GameStateIdleEnter()
		{

		}

		public void GameStateIdleUpdate()
		{

		}

		public void GameStateIdleExit()
		{

		}

		public void GameStateStartingGameEnter()
		{
			//Debug.LogError("GameStateStartGameEnter");
			m_startGameTimer.startClock();
		}
		
		public void GameStateStartingGameUpdate()
		{
			//Debug.LogError("GameStateStartGameUpdate");

			m_startGameTimer.Run();
			//Debug.LogError("Count down ==> " + m_startGameTimer.Counter);
			if(!m_startGameTimer.IsDone) {

				m_gameFSM.ChangeState(GameStateNWSetup.Instance);
			}
		}
		
		public void GameStateStartingGameExit()
		{
			//Debug.LogError("GameStateStartGameExit");
		}

		public void GameStateSetUpEnter()
		{
			NotificationCenter.DefaultCenter.PostNotification(this, "SpawnRandom");
		}

		public void GameStateSetUpUpdate()
		{

		}

		public void GameStateSetUpExit()
		{

		}

		//Server's turn
		public void GameStatePlayerOneTurnEnter()
		{

		}

		public void GameStatePlayerOneTurnUpdate()
		{
	
		}

		public void GameStatePlayerOneTurnExit()
		{
			
		}

		public void GameStatePlayerTwoTurnEnter()
		{
			
		}

		public void GameStatePlayerTwoTurnUpdate()
		{

		}

		public void GameStatePlayerTwoTurnExit()
		{
			
		}

		public void GameStateEndRoundEnter()
		{
			
		}

		public void GameStateEndRoundUpdate()
		{
			
		}

		public void GameStateEndRoundExit()
		{
			
		}

		void Update () 
		{
			m_gameFSM.runOnUpdate();
			
		}
		
		void FixedUpdate ()
		{
			m_gameFSM.runOnFixedUpdate();
		
		}

		public void PlayerJoinedGame()
		{
			//Debug.LogError("Player joined the game");
			m_gameFSM.ChangeState(GameStateNWStartingGame.Instance);
		}

	}
}