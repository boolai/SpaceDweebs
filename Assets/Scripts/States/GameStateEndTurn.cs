using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStateEndTurn :  FSMState<BaseGameController> {
		
		static readonly GameStateEndTurn instance = new GameStateEndTurn();
		public static GameStateEndTurn Instance 
		{
			get { return instance; }
		}
		
		static GameStateEndTurn() { }
		private GameStateEndTurn() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateEndTurnEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateEndTurnUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			//sNotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateEndTurnFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateEndTurnExit");
			
		}
	}
}