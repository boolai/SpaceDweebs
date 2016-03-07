
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStateNWStartingGame:  FSMState<GameNetworkMaster> {
		
		static readonly GameStateNWStartingGame instance = new GameStateNWStartingGame();
		public static GameStateNWStartingGame Instance 
		{
			get { return instance; }
		}
		
		static GameStateNWStartingGame() { }
		private GameStateNWStartingGame() { }
		
		public override void Enter (GameNetworkMaster p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateStartingGameEnter");
			
		}
		
		public override void ExecuteOnUpdate (GameNetworkMaster p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateStartingGameUpdate");
		}
		
		
		public override void ExecuteOnFixedUpdate (GameNetworkMaster p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(null,"GameStateIdleFixedUpdate");
			
		}
		
		public override void Exit(GameNetworkMaster p_game) 
		{
			
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateStartingGameExit");
			
		}
	}
}