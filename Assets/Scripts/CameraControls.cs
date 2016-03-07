using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoogieDownGames {

	public class CameraControls : MonoBehaviour {

		public Animator m_anime;

		void Awake()
		{
			m_anime = GetComponent<Animator>();
		}

		public void SwitchToOrtho()
		{
			GetComponent<Camera>().orthographic = true;
		}

		public void SwitchToProjection()
		{
			GetComponent<Camera>().orthographic = false;
		}

		public void PlayAnime(string p_anime)
		{
			m_anime.SetTrigger(p_anime);
		}

		public void BackToGame()
		{

			GameMaster.Instance.GameFSM.ChangeState(GameStateSetUp.Instance);
		}
	}
}