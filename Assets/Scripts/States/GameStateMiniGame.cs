/*
 * The initial state for the Arena
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStateMiniGame :  FSMState<BaseGameController> {
		
		static readonly GameStateMiniGame instance = new GameStateMiniGame();
		public static GameStateMiniGame Instance 
		{
			get { return instance; }
		}
		
		static GameStateMiniGame() { }
		private GameStateMiniGame() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"OnStateMiniGameEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"OnStateMiniGameUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"OnStateMiniGameFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"OnStateMiniGameExit");
			
		}
	}
}