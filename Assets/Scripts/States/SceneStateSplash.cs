/*
 * The initial state for the Arena
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class SceneStateSplash :  FSMState<BaseGameController> {
		
		static readonly SceneStateSplash instance = new SceneStateSplash();
		public static SceneStateSplash Instance 
		{
			get { return instance; }
		}
		
		static SceneStateSplash() { }
		private SceneStateSplash() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"OnSplashEnter");

		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"OnSplashUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(p_game,"OnSplashFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"OnSplashExit");
		}
	}
}