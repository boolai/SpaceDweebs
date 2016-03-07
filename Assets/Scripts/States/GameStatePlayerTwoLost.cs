/*
 * The initial state for the Arena
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStatePlayerTwoLost :  FSMState<BaseGameController> {
		
		static readonly GameStatePlayerTwoLost instance = new GameStatePlayerTwoLost();
		public static GameStatePlayerTwoLost Instance 
		{
			get { return instance; }
		}
		
		static GameStatePlayerTwoLost() { }
		private GameStatePlayerTwoLost() { }
		
		public override void Enter (BaseGameController p_entity)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_entity, "GameStatePlayerTwoLostEnter");

		}
		
		public override void ExecuteOnUpdate (BaseGameController p_entity) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_entity, "GameStatePlayerTwoLostUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_entity)
		{
			
		}
		
		public override void Exit(BaseGameController p_entity) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_entity, "GameStatePlayerTwoLostExit");
			
		}
	}
}