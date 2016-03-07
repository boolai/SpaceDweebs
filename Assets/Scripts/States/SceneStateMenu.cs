/*
 * The initial state for the scenes
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class SceneStateMenu:  FSMState<BaseGameController> {
		
		static readonly SceneStateMenu instance = new SceneStateMenu();
		public static SceneStateMenu Instance 
		{
			get { return instance; }
		}
		
		static SceneStateMenu() { }
		private SceneStateMenu() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"OnMenuStateEnter");
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"OnMenuStateUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			//NotificationCenter.DefaultCenter.PostNotification(p_game,"OnMenuStateFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"OnMenuStateExit");
		}
	}
}