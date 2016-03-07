
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStateNWSetup :  FSMState<GameNetworkMaster> {
		
		static readonly GameStateNWSetup instance = new GameStateNWSetup();
		public static GameStateNWSetup Instance 
		{
			get { return instance; }
		}
		
		static GameStateNWSetup() { }
		private GameStateNWSetup() { }
		
		public override void Enter (GameNetworkMaster p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateSetupEnter");
			
		}
		
		public override void ExecuteOnUpdate (GameNetworkMaster p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateSetupUpdate");
		}
		
		
		public override void ExecuteOnFixedUpdate (GameNetworkMaster p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(null,"GameStateIdleFixedUpdate");
			
		}
		
		public override void Exit(GameNetworkMaster p_game) 
		{
			
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateSetupExit");
			
		}
	}
}