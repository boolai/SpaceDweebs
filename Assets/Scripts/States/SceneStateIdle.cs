/*
 * The initial state for the scenes
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {

	public sealed class SceneStateIdle :  FSMState<BaseGameController> {
		
		static readonly SceneStateIdle instance = new SceneStateIdle();
		public static SceneStateIdle Instance 
		{
			get { return instance; }
		}
		
		static SceneStateIdle() { }
		private SceneStateIdle() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateIdleEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateIdleUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateIdleFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateIdleExit");
			
		}
	}
}