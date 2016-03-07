/*
 * The initial state for the Arena
 */
using UnityEngine;
using System.Collections;

namespace BoogieDownGames {
	
	public sealed class GameStateUIAnime :  FSMState<BaseGameController> {
		
		static readonly GameStateUIAnime instance = new GameStateUIAnime();
		public static GameStateUIAnime Instance 
		{
			get { return instance; }
		}
		
		static GameStateUIAnime() { }
		private GameStateUIAnime() { }
		
		public override void Enter (BaseGameController p_game)
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateUIAnimeEnter");
			
		}
		
		public override void ExecuteOnUpdate (BaseGameController p_game) 
		{
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateUIAnimeUpdate");
		}
		
		public override void ExecuteOnFixedUpdate (BaseGameController p_game)
		{
			
			//NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateUIAnimeFixedUpdate");
		}
		
		public override void Exit(BaseGameController p_game) 
		{
			
			NotificationCenter.DefaultCenter.PostNotification(p_game,"GameStateUIAnimeExit");
		}
	}
}