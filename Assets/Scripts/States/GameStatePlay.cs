/*
 * The initial state for the Arena
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStatePlay :  FSMState<BaseGameController> {
		
		static readonly GameStatePlay instance = new GameStatePlay();
		public static GameStatePlay Instance 
		{
			get { return instance; }
		}
		
		static GameStatePlay() { }
		private GameStatePlay() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStatePlayEnter");

		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStatePlayUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{

			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStatePlayFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStatePlayExit");
		}
	}
}