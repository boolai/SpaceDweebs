using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BoogieDownGames {

	public class SplashController : BaseSceneController {

		[SerializeField]
		private float m_delay;

		void Start()
		{
			StartCoroutine(delayChangeScene(m_delay));
		}

		IEnumerator delayChangeScene ( float p_delay )
		{
			yield return new WaitForSeconds(p_delay);
			SceneManager.LoadScene("Menu");
		}

		public void DelaySceneChange( float p_delay)
		{
			StartCoroutine(delayChangeScene(p_delay));
		}
	}
}