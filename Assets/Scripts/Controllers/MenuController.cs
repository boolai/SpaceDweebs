/// <summary>
/// Menu controller.
/// Mediatary between the scene and the gamemaster. as the gamemaster 
/// is a permanate object we cant depend on it to link it to the ui as 
/// the address may not be the same all the time.
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BoogieDownGames {

	public class MenuController : UnitySingleton<MenuController> {

		[SerializeField]
		private float m_transitionDelay;

		[SerializeField]
		private float m_controlsDelay; //On started up the play can accidentaly press a menu button this delays that

		[SerializeField]
		private bool m_canTouchMenu;

		[SerializeField]
		private Slider m_transitionSlider;

		[SerializeField]
		private GameObject m_loadingScreenPanel;

		void Awake()
		{

			float r = UnityEngine.Random.Range(0.5f, 4.0f);
			StartCoroutine( RandSpawn(r) );
		}

		public void ChangeScene(string p_scene)
		{
			/*
			if(m_canTouchMenu) {
				StartCoroutine(Transition(m_transitionDelay, p_scene));
			}*/
			if(m_canTouchMenu == true) {

				StartCoroutine(DisplayLoadingScreen(p_scene));
			}
		}

		public void SetDifficulty(int p_diff)
		{
			GameMaster.Instance.SetDifficulty(p_diff);
		}

		public void QuitGame(float p_inSeconds)
		{
			if(m_canTouchMenu) {
				StartCoroutine(TransitionQuit(p_inSeconds));
			}
		}

		IEnumerator TransitionQuit(float p_delay)
		{
			yield return new WaitForSeconds(p_delay);

			Application.Quit();
		}

		IEnumerator Transition(float p_delay,string p_scene)
		{
			yield return new WaitForSeconds(p_delay);
			GameMaster.Instance.SceneFSM.ChangeState(SceneStateIdle.Instance);
			GameMaster.Instance.ChangeScene(p_scene);
		}

		IEnumerator DisplayLoadingScreen(string p_level)
		{
			//turn on loading screen panel
			m_loadingScreenPanel.SetActive(true);
			AsyncOperation aysc = SceneManager.LoadSceneAsync(p_level);
			while(!aysc.isDone) {
				m_transitionSlider.value = aysc.progress * 100;
				yield return null;
			}
		}

		IEnumerator DelayControls(float p_sec)
		{
			yield return new WaitForSeconds(p_sec);

			m_canTouchMenu = true;

		}

		IEnumerator RandSpawn( float p_sec )
		{
			yield return new WaitForSeconds(p_sec);
			NotificationCenter.DefaultCenter.PostNotification(this, "RandomSpawn");
			float r = UnityEngine.Random.Range(0.5f, 4.0f);
			StartCoroutine( RandSpawn( r ));
		}

		void OnLevelWasLoaded(int level) 
		{
			StartCoroutine(DelayControls(m_controlsDelay));
		}

		public void SetTutorial(bool p_state)
		{
			GameMaster.Instance.isTutorialOn = p_state;
		}

	}
}