/*
 * The initial state for the Arena
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStatePlayeOneLost :  FSMState<BaseGameController> {
		
		static readonly GameStatePlayeOneLost instance = new GameStatePlayeOneLost();
		public static GameStatePlayeOneLost Instance 
		{
			get { return instance; }
		}
		
		static GameStatePlayeOneLost() { }
		private GameStatePlayeOneLost() { }
		
		public override void Enter (BaseGameController p_entity)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_entity, "GameStatePlayerOneLostEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_entity) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_entity, "GameStatePlayerOneLostUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_entity)
		{

		}
		
		public override void Exit(BaseGameController p_entity) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_entity, "GameStatePlayerOneLostExit");
			
		}
	}
}