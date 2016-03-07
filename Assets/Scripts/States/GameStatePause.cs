/*
 * The initial state for the Arena
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStatePause :  FSMState<BaseGameController> {
		
		static readonly GameStatePause instance = new GameStatePause();
		public static GameStatePause Instance 
		{
			get { return instance; }
		}
		
		static GameStatePause() { }
		private GameStatePause() { }
		
		public override void Enter (BaseGameController p_game)
		{
			//Debug.Log("Im in Arena Idle State");
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStatePauseEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStatePauseUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStatePauseFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStatePauseExit");
		}
	}
}