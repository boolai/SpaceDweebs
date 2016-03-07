/*
 * The initial state for the Arena
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GlobalState :  FSMState<BaseGameController> {
		
		static readonly GlobalState instance = new GlobalState();
		public static GlobalState Instance 
		{
			get { return instance; }
		}
		
		static GlobalState() { }
		private GlobalState() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GlobalStateEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GlobalStateUpdate");
			//Debug.LogError("running global state update");
			if(Input.GetKey(KeyCode.Escape)) {
				p_game.GameFSM.ChangeState(GameStatePause.Instance);
			}
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GlobalStateFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GlobaleStateExit");
			
		}
	}
}