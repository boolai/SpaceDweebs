/*
 * 
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStateIdle :  FSMState<BaseGameController> {
		
		static readonly GameStateIdle instance = new GameStateIdle();
		public static GameStateIdle Instance 
		{
			get { return instance; }
		}
		
		static GameStateIdle() { }
		private GameStateIdle() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateIdleEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateIdleUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(null,"GameStateIdleFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateIdleExit");
		}
	}
}