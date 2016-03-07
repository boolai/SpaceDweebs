/*
 * The initial state for the Arena
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class SceneStateGlobal :  FSMState<BaseGameController> {
		
		static readonly SceneStateGlobal instance = new SceneStateGlobal();
		public static SceneStateGlobal Instance 
		{
			get { return instance; }
		}
		
		static SceneStateGlobal() { }
		private SceneStateGlobal() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateGlobalSceneEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateGlobalUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateGlobalFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateGlobalExit");
			
		}
	}
}