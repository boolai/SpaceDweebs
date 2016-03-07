/*
 * 
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStateSetUp :  FSMState<BaseGameController> {
		
		static readonly GameStateSetUp instance = new GameStateSetUp();
		public static GameStateSetUp Instance 
		{
			get { return instance; }
		}
		
		static GameStateSetUp() { }
		private GameStateSetUp() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateSetUpEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateSetUpUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(null,"GameStateIdleFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateSetUpExit");
		}
	}
}