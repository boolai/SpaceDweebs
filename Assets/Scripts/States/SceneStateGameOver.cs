/*
 * The initial state for the Arena
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class SceneStateGameOver :  FSMState<BaseGameController> {
		
		static readonly SceneStateGameOver instance = new SceneStateGameOver();
		public static SceneStateGameOver Instance 
		{
			get { return instance; }
		}
		
		static SceneStateGameOver() { }
		private SceneStateGameOver() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateGameOverEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateGameOverUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateGlobalFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateGameOverExit");
			
		}
	}
}