
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStateNWIdle :  FSMState<GameNetworkMaster> {
		
		static readonly GameStateNWIdle instance = new GameStateNWIdle();
		public static GameStateNWIdle Instance 
		{
			get { return instance; }
		}
		
		static GameStateNWIdle() { }
		private GameStateNWIdle() { }
		
		public override void Enter (GameNetworkMaster p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateIdleEnter");

		}
		
		public override void ExecuteOnUpdate (GameNetworkMaster p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateIdleUpdate");
		}
		
		
		public override void ExecuteOnFixedUpdate (GameNetworkMaster p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(null,"GameStateIdleFixedUpdate");

		}
		
		public override void Exit(GameNetworkMaster p_game) 
		{
			
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateIdleExit");
		
		}
	}
}