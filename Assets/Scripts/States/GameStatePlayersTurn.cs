/*
 * 
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStatePlayersTurn :  FSMState<BaseGameController> {
		
		static readonly GameStatePlayersTurn instance = new GameStatePlayersTurn();
		public static GameStatePlayersTurn Instance 
		{
			get { return instance; }
		}
		
		static GameStatePlayersTurn() { }
		private GameStatePlayersTurn() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStatePlayersTurnEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStatePlayersTurnUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(null,"GameStateIdleFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStatePlayersTurnExit");
		}
	}
}