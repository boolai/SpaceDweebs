using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStateGameOver :  FSMState<BaseGameController> {
		
		static readonly GameStateGameOver instance = new GameStateGameOver();
		public static GameStateGameOver Instance 
		{
			get { return instance; }
		}
		
		static GameStateGameOver() { }
		private GameStateGameOver() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateGameOverEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateGameOveUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateGameOveFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateGameOveExit");
			
		}
	}
}