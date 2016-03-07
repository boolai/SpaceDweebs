/// <summary>
/// Canvas animations.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoogieDownGames {

	/// <summary>
	/// Canvas animations.
	/// </summary>
	public class CanvasAnimations : MonoBehaviour {

		[SerializeField]
		private GameObject m_tutorialPanel;

		[SerializeField]
		private Animator m_anime;

		[SerializeField]
		private float m_transitionTime;

		[SerializeField]
		private string m_nextTriggerName;

		[SerializeField]
		private string m_nextScene;

		public string NextTriggerName
		{
			get { return m_nextTriggerName; }
			set { m_nextTriggerName = value; }
		}

		public string NextScene
		{
			get { return m_nextScene; }
			set { m_nextScene = value; }
		}

		public float TransitionTime
		{
			get { return m_transitionTime; }
			set { m_transitionTime = value; }
		}

		/// <summary>
		/// Runs the event.
		/// </summary>
		public void runAnimation(string p_triggerName)
		{
			m_anime.SetTrigger(p_triggerName);
		}

		public void nextAnimation()
		{
			//Debug.LogError("Next animation " + m_nextTriggerName);
			m_anime.SetTrigger(m_nextTriggerName);
		}

		public void gotToScene()
		{
			StartCoroutine(delayScene(m_transitionTime));
		}

		public void toggleTutorial()
		{
			m_tutorialPanel.SetActive(!m_tutorialPanel.activeSelf);
		}

		IEnumerator delayScene(float p_secs)
		{
			yield return new WaitForSeconds(p_secs);
			MenuController.Instance.ChangeScene(m_nextScene);
		}

	}
}