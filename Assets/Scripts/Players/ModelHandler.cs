/*
 * Handles the models that can be choosen by the player
 * 
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoogieDownGames{
	
	public class ModelHandler : MonoBehaviour {

		#region MEMEBERS


		private enum PlayerChoice { Player1, Player2 };

		[SerializeField]
		private PlayerChoice m_whichPlayer;


		[SerializeField]
		private List<GameObject> m_modelList;

		[SerializeField]
		private int m_currentIndex;

		#endregion

		#region PROPERTIES

		#endregion

		void Awake()
		{
			//Try and get the current index of the gamemaster
			if(m_whichPlayer == PlayerChoice.Player1) {
				m_currentIndex = GameMaster.Instance.PlayerOneModelIndex;
			} else {
				m_currentIndex = GameMaster.Instance.PlayerTwoModelIndex;
			}
			
			SetModelsActives(m_currentIndex);
		}

		// Use this for initialization
		void Start () 
		{


		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}

		//Goes to the next model if the current index overflows then restart
		public void NextModel()
		{
			if(m_modelList.Count > 0) {
				m_currentIndex++;
				if(m_currentIndex >= m_modelList.Count) {
					m_currentIndex = 0;
				}

				SetModelsActives(m_currentIndex);
				SetGameMasterIndex();
			}
		}

		//Goes to the previous model if current index is less than 0 then restart at the end
		public void PrevModel()
		{
			if(m_modelList.Count > 0) {
				m_currentIndex--;
				if(m_currentIndex < 0) {
					m_currentIndex = m_modelList.Count - 1;
				}

				SetModelsActives(m_currentIndex);
				SetGameMasterIndex();
			}
		}

		//Set all the models current active states only one can be active at a time
		public void SetModelsActives(int p_index)
		{
			//Make sure we not overbounding
			if(p_index >= 0 && p_index < m_modelList.Count) {

				for(int index = 0; index < m_modelList.Count; index++) {
					if(index != p_index) {
						m_modelList[index].SetActive(false);
					} else {
						m_modelList[index].SetActive(true);
					}
					
				}

			}
		}

		public void SetGameMasterIndex()
		{
			if(m_whichPlayer == PlayerChoice.Player1) {
				GameMaster.Instance.PlayerOneModelIndex = m_currentIndex;
			} else {
				GameMaster.Instance.PlayerTwoModelIndex = m_currentIndex;
			}
		}

	}
}