using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStateEvent :  FSMState<BaseGameController> {
		
		static readonly GameStateEvent instance = new GameStateEvent();
		public static GameStateEvent Instance 
		{
			get { return instance; }
		}
		
		static GameStateEvent() { }
		private GameStateEvent() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"OnGameStateEventEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"OnGameStateEventUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(null,"GameStateIdleFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			
			NotificationCenter.DefaultCenter.PostNotification(p_game,"OnGameStateEventExit");
		}
	}
}