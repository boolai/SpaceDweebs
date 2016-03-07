/*
 * The initial state for the Arena
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStateDisplaySpecial :  FSMState<BaseGameController> {
		
		static readonly GameStateDisplaySpecial instance = new GameStateDisplaySpecial();
		public static GameStateDisplaySpecial Instance 
		{
			get { return instance; }
		}
		
		static GameStateDisplaySpecial() { }
		private GameStateDisplaySpecial() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateDisplaySpecialEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateDisplayUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateDisplaySpecialFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateDisplaySpecialExit");
			
		}
	}
}