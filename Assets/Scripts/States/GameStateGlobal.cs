/*
 * The initial state for the Arena
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStateGlobal :  FSMState<BaseGameController> {
		
		static readonly GameStateGlobal instance = new GameStateGlobal();
		public static GameStateGlobal Instance 
		{
			get { return instance; }
		}
		
		static GameStateGlobal() { }
		private GameStateGlobal() { }
		
		public override void Enter (BaseGameController p_game)
		{

			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateGlobalEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{

			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateGlobalUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{

			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateGlobalFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
		
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateGlobalExit");
			
		}
	}
}