using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BoogieDownGames {

	public class GameController : UnitySingleton<GameController> {

		[SerializeField]
		private Canvas canvas;

		void Start()
		{
			Time.timeScale = 1.0f;
		}

		public void PauseGame()
		{
			//GameMaster.Instance.Pause();
			//GameMaster.Instance.MyTimeMode.Pause();
			//GameMaster.Instance.GameFSM.ChangeState(GameStatePause.Instance);

			Time.timeScale = Time.timeScale == 0 ? 1 : 0;
			Debug.LogError(Time.timeScale);
		}

		public void UnPauseGame()
		{
			//Time.timeScale = 1f;
			//GameMaster.Instance.MyTimeMode.UnPause();
			GameMaster.Instance.GameFSM.RevertToPreviousStateNoEnter();
		}

		public void GoToScene(string p_scene)
		{

			SceneManager.LoadScene(p_scene);
		}

		public void QuitGame()
		{
			Application.Quit();
		}


	}
}