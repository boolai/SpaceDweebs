/*
 * 
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStateCompTurn :  FSMState<BaseGameController> {
		
		static readonly GameStateCompTurn instance = new GameStateCompTurn();
		public static GameStateCompTurn Instance 
		{
			get { return instance; }
		}
		
		static GameStateCompTurn() { }
		private GameStateCompTurn() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateCompTurnEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateCompTurnUpdate");
		}

		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(null,"GameStateIdleFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateCompTurnExit");
		}
	}
}