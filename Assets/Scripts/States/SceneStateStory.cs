/*
 * The initial state for the Arena
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class SceneStateStory :  FSMState<BaseGameController> {
		
		static readonly SceneStateStory instance = new SceneStateStory();
		public static SceneStateStory Instance 
		{
			get { return instance; }
		}
		
		static SceneStateStory() { }
		private SceneStateStory() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateStoryEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateStoryUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateGlobalFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateStoryExit");
			
		}
	}
}