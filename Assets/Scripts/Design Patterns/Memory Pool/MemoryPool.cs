/*
 * Memory Pool controls the activation and deactivation of objects
 * in the game.  It can also creat more objects if need be. Can be
 * set as an option.  Works in tangent with the notification center 
 * To enchance usage.
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


namespace BoogieDownGames
{
	
	public class MemoryPool : UnitySingleton<MemoryPool>{
		
		[SerializeField]
		private bool m_loadOnLevel;

		[SerializeField]
		private bool m_isOnNetwork;

		public Dictionary<string,int> m_keyBank = new Dictionary<string,int>();//Keep a bank of keys

		[SerializeField]
		private Dictionary <GameObject, GameObject> m_liveObjects = new Dictionary<GameObject,GameObject>();

		[SerializeField]
		private Dictionary <GameObject, GameObject> m_deadObjects =  new Dictionary<GameObject,GameObject>();

		public Builder[] m_builders;

		public Dictionary<GameObject, GameObject> LiveObjects
		{
			get { return m_liveObjects; }
		}

		public Dictionary<GameObject, GameObject>  DeadObjects
		{
			get { return m_deadObjects; }
		}


		// Use this for initialization
		void Awake()
		{
			//search for all the objects
			createBuilderBanks();
			NotificationCenter.DefaultCenter.AddObserver(this,"findMyObjectNotification");
			
			////Debug.LogError("I've been called")
		}
		
		void Start () 
		{
			/*
			if(m_loadOnLevel == false) {
				//Create the storage banks
				createBuilderBanks();
			}
			NotificationCenter.DefaultCenter.AddObserver(this,"findMyObjectNotification");
			*/
		}
		
		public GameObject findObjs(string p_key,bool p_state)
		{
			GameObject obj = null;
			if(p_state == false) {
				obj = m_builders[m_keyBank[p_key]].findDead();
				//obj.SetActive(true);
			} else {
				obj = m_builders[m_keyBank[p_key]].findAlive();
				//obj.SetActive(false);
			}

			return obj;
		}
		
		public GameObject findAndGetObjs(string p_key,bool p_state)
		{
			GameObject obj = null;
			if(p_state == false) {
				if(m_keyBank.ContainsKey(p_key)) {
					obj = m_builders[m_keyBank[p_key]].findDead();
				} else {
					//Debug.LogError("Key not found => " + p_key);
				}
				//obj.SetActive(true);
			} else {
				if(m_keyBank.ContainsKey(p_key)) {
					obj = m_builders[m_keyBank[p_key]].findAlive();
				} else {
					//Debug.LogError("Key not found => " + p_key);
				}
				//obj.SetActive(false);
			}
			return obj;
		}
		
		public void findMyObjectNotification(NotificationCenter.Notification p_not)
		{
			if(p_not.data.ContainsKey("name")) {
				
				string name = (string)p_not.data["name"];
				
				Vector3 pos = new Vector3();
				pos = Vector3.zero;
				if(p_not.data.ContainsKey("pos")) {
					pos = (Vector3)p_not.data["pos"];
				}
				
				//get the state 
				bool state = false;
				if(p_not.data.ContainsKey("state")) {
					state = (bool)p_not.data["state"];
				}
				//make sure we have the data for the postioning if not set it to the calling object position
				GameObject obj = null;
				if(state == true) { 
					obj = m_builders[m_keyBank[name]].findAlive();
					if(obj != null) {
						obj.SetActive(false);
					}
				} else {
					obj = m_builders[m_keyBank[name]].findDead();
					if(obj != null) {
						obj.SetActive(true);
					}
				}

				obj.transform.position = pos;
			}
		}
		
		//Starts off building the bank

		public void createBuilderBanks()
		{
			for(int index =0; index < m_builders.Length; ++index) {
				m_builders[index].build(m_isOnNetwork);
				if(!m_keyBank.ContainsKey(m_builders[index].Name) && !m_keyBank.ContainsKey(m_builders[index].PreFab.name)) {
					if(m_builders[index].Name != null) {
						m_keyBank.Add(m_builders[index].Name,index);
					} else {
						m_keyBank.Add(m_builders[index].PreFab.name,index);
					}
				}
			}
		}
		
		void OnLevelWasLoaded(int level)
		{
			if(m_loadOnLevel == true) {
				createBuilderBanks();
			}
		}

		public void SetLiveDeadBank(GameObject p_object, bool p_state)
		{
			//Delete from both list
			if(LiveObjects.ContainsKey(p_object)) {
				LiveObjects.Remove(p_object);
			}

			if(DeadObjects.ContainsKey(p_object)) {
				DeadObjects.Remove(p_object);
			}

			switch(p_state) {

			case true:

				m_liveObjects.Add(p_object,p_object);
				break;

			case false:
				m_deadObjects.Add(p_object,p_object);
				break;
			}
		}

		public GameObject[] GetLiveObjects()
		{
			GameObject[] m_buffer = new GameObject[m_liveObjects.Count];
			m_liveObjects.Values.CopyTo(m_buffer,0);
			return m_buffer;
		}

		public GameObject[] GetDeadObjects()
		{
			GameObject[] m_buffer = new GameObject[m_deadObjects.Count];
			m_deadObjects.Values.CopyTo(m_buffer,0);
			return m_buffer;
		}

	}
}