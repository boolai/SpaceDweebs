/*
 * The initial state for the Arena
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class SceneStateArena :  FSMState<BaseGameController> {
		
		static readonly SceneStateArena instance = new SceneStateArena();
		public static SceneStateArena Instance 
		{
			get { return instance; }
		}
		
		static SceneStateArena() { }
		private SceneStateArena() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateArenaEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateArenaUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateGlobalFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"SceneStateArenaExit");
			
		}
	}
}