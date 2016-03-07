using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace  BoogieDownGames {

	public enum ListenMode { NONE, Rand, Item, BOTH }; 

	public class SpawnerRad : MonoBehaviour {

		[SerializeField]
		private Animator CanvasAnim;

		[SerializeField]
		private ListenMode m_listenMode;

		[SerializeField]
		private List<GameObject> m_objBank;

		private Dictionary<string,GameObject> m_bank = new Dictionary<string, GameObject>();

		// Use this for initialization
		void Start () 
		{
			NotificationCenter.DefaultCenter.AddObserver(this,"RandomSpawn");
			NotificationCenter.DefaultCenter.AddObserver(this,"SpawnAnItem");

			CanvasAnim = GameObject.Find ("Canvas").GetComponent<Animator> ();

			foreach(GameObject obj in m_objBank) {
				m_bank.Add(obj.name,obj);
			}
		}

		public void RandomSpawn()
		{
			//Make sure the trigger is good
				
			if(m_listenMode == ListenMode.BOTH || m_listenMode == ListenMode.Rand) {

				if (!CanvasAnim.IsInTransition(0)) {
					//Get a random number
					var c = UnityEngine.Random.Range (1, 100);

					if (c >= 50) {
						//Get a random index
						var r = UnityEngine.Random.Range (0, m_objBank.Count);
						//Grab the object from the memory manager
						var obj = MemoryPool.Instance.findAndGetObjs (m_objBank [r].name, false);
						//Check to see if we got a valid object
						if (obj) {
							//send a message that we picked this object
							Hashtable dat = new Hashtable ();
							dat.Add ("event", obj.name);
							NotificationCenter.DefaultCenter.PostNotification (this, "PlayEvent", dat);
							//Place it
							obj.transform.position = gameObject.transform.position;
							obj.transform.rotation = gameObject.transform.rotation;
							//Wake it up
							obj.SetActive (true);
							if (obj.GetComponent<ObjectLifetime> ()) {
								obj.GetComponent<ObjectLifetime> ().MyClock.ResetClock ();
							}
						}
					}
				}
			}
		}

		public void RandomSpawnMediated()
		{
			//Get a random index
			var r = UnityEngine.Random.Range(0, m_objBank.Count);
			//Grab the object from the memory manager
			var obj = MemoryPool.Instance.findAndGetObjs(m_objBank[r].name, false);
			//Check to see if we got a valid object
			if(obj) {
				//send a message that we picked this object
				Hashtable dat = new Hashtable();
				dat.Add("event", obj.name);
				NotificationCenter.DefaultCenter.PostNotification(this, "PlayEvent",dat);
				//Place it
				obj.transform.position = gameObject.transform.position;
				obj.transform.rotation = gameObject.transform.rotation;
				//Wake it up
				obj.SetActive(true);
				if(obj.GetComponent<ObjectLifetime>()) {
					obj.GetComponent<ObjectLifetime>().MyClock.ResetClock();
				}
			}
		}

		public void SpawnAnItem( NotificationCenter.Notification p_note )
		{
			try {

				if(m_listenMode == ListenMode.BOTH || m_listenMode == ListenMode.Item) {
					string item = (string)p_note.data["dat"];
					if(m_bank.ContainsKey(item)) {
						var obj = MemoryPool.Instance.findAndGetObjs(item,false);
						obj.transform.position = transform.position;
						obj.SetActive(true);
						obj.GetComponent<ObjectLifetime>().MyClock.ResetClock();
						//send a message that we picked this object
						Hashtable dat = new Hashtable();
						dat.Add("event", obj.name);
						NotificationCenter.DefaultCenter.PostNotification(this, "PlayEvent",dat);
					}
				}
			} catch(Exception p_err) {

				Debug.LogError(p_err.Message);
			}

		}
	}
}