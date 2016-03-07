using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

namespace BoogieDownGames {

	public class BaseSceneController : UnitySingleton<BaseSceneController> {

		public void GotToLevel(string p_level)
		{
			SceneManager.LoadScene(p_level);
		}

		public void GoToLevel(int p_level)
		{
			SceneManager.LoadScene(p_level);
		}
	}
}