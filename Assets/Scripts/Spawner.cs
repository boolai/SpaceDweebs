/// <summary>
/// Spawner class
/// This is a component module
/// It will spawn specific or random objects
/// but only if the trigger is off.
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

namespace BoogieDownGames {

	[RequireComponent (typeof (BoxCollider))]
	public class Spawner : MonoBehaviour {

		[SerializeField]
		private GameObject m_objCollide;

		[SerializeField]
		private bool m_isOn;

		[SerializeField]
		private bool m_canSpawnSpecial;

		[SerializeField]
		private GameObject m_particleEffect;  //This is created when the spawner is activated

		[SerializeField]
		private bool m_isNetworked;

		[SerializeField]
		private bool m_canRandomized;

		[SerializeField]
		private Vector3 m_particleOffset;

		[SerializeField]
		private Vector3 m_particleRotOffset;

		[SerializeField]
		private int m_randomCeiling;

		[SerializeField]
		private List<GameObject> m_objBank;

		// Use this for initialization
		void Start () 
		{
			NotificationCenter.DefaultCenter.AddObserver(this, "SpawnSpecialItem");
			NotificationCenter.DefaultCenter.AddObserver(this, "SpawnItem");
			NotificationCenter.DefaultCenter.AddObserver(this, "SpawnRandom");
			NotificationCenter.DefaultCenter.AddObserver(this, "SpawnRandomMaybe");
			NotificationCenter.DefaultCenter.AddObserver(this, "increaseSpawnChance");

			if(m_randomCeiling > m_objBank.Count) {
				m_randomCeiling = m_objBank.Count;
			} else if(m_randomCeiling < 0) {
				m_randomCeiling = 0;
			}
		}

		public void increaseSpawnChance()
		{
			m_randomCeiling++;
			if(m_randomCeiling > m_objBank.Count) {
				m_randomCeiling = m_objBank.Count;
			} else if(m_randomCeiling < 0) {
				m_randomCeiling = 0;
			}
		}

		IEnumerator SpawnRandomMaybeDelayed(float p_delay)
		{
			yield return new WaitForSeconds(p_delay);

			var chance = UnityEngine.Random.Range(0, 100);
			//Make sure the trigger is good
			if(!m_isOn && m_canRandomized) {
				
				//Get a random number
				var r = UnityEngine.Random.Range(0, m_randomCeiling);
				//Grab the object from the memory manager
				var obj = MemoryPool.Instance.findAndGetObjs(m_objBank[r].name, false);
				
				//Check to see if we got a valid object
				if(obj) {
					//Place it
					obj.transform.position = gameObject.transform.position;
					obj.transform.rotation = gameObject.transform.rotation;
					//Wake it up
					obj.SetActive(true);
					
					//Set the particle effect
					if(m_particleEffect) {
						
						var pe = MemoryPool.Instance.findAndGetObjs(m_particleEffect.name, false);
						
						if(pe) {
							var pos = gameObject.transform.position;
							pos.x += m_particleOffset.x;
							pos.y += m_particleOffset.y;
							pos.z += m_particleOffset.z;
							pe.transform.position = pos;
							
							var rot = pe.transform.rotation;
							rot = Quaternion.Euler(m_particleRotOffset);
							pe.transform.rotation = rot;
							
							pe.SetActive(true);
						}
						
					}
				}
			}
		}

		/// <summary>
		/// Maybe spawns a random dweeb
		/// </summary>
		public void SpawnRandomMaybe()
		{
			//var chance = UnityEngine.Random.Range(0f, f);
			StartCoroutine(SpawnRandomMaybeDelayed(0f));
		}

		/// <summary>
		/// Spawns the item.
		/// </summary>
		/// <param name="p_note">P_note.</param>
		public void SpawnSpecialItem( NotificationCenter.Notification p_note )
		{
			if(m_canSpawnSpecial) {

				var name = (string)p_note.data["dat"];
				var obj = MemoryPool.Instance.findAndGetObjs(name,false);
				//Debug.LogError("Spawning " + name );
				//Check to see if we got a valid object
				if(obj) {
					//Place it
					obj.transform.position = gameObject.transform.position;
					//Wake it up
					obj.SetActive(true);
					//add it from live objects
					//face the camera
					obj.transform.Rotate(Vector3.zero);
					obj.transform.rotation = transform.rotation;
                    ParticleSystem part = obj.GetComponent<ParticleSystem>();
                    if(part)
                    {
						var em = part.emission;
						em.enabled = false;
                        part.Emit(1);
                    }
				}
			}

		}

		/// <summary>
		/// Spawns an item
		/// which the p_note will contain the item
		/// </summary>
		/// <param name="p_note">P_note.</param>
		public void SpawnItem( NotificationCenter.Notification p_note )
		{

			//check to see if we can spawn
			if(!m_isOn) {

				var name = (string)p_note.data["name"];
				var obj = MemoryPool.Instance.findAndGetObjs(name,false);
				//Debug.LogError("Spawning " + name );
				//Check to see if we got a valid object
				if(obj) {
					//Place it
					obj.transform.position = gameObject.transform.position;
					//Wake it up
					obj.SetActive(true);
					if(m_isNetworked) {
						NetworkServer.Spawn(obj);
					}
					//add it from live objects
					//face the camera
					obj.transform.Rotate(Vector3.zero);
					obj.transform.rotation = transform.rotation;
				}
			}

		}

		/// <summary>
		/// Spawns random item
		/// </summary>
		public void SpawnRandom()
		{
			float delayedSecs = UnityEngine.Random.Range(0.0f,1.0f);
			StartCoroutine(delaySpawn(delayedSecs));
		}

		void OnTriggerEnter(Collider other)
		{

		}

		void OnTriggerStay(Collider other)
		{
			if(other) {

				m_objCollide = other.gameObject;
				
				if(other.gameObject.tag == "Tile" && other.gameObject.activeSelf == true) {
					m_isOn = true;
				} else {
					m_isOn = false;
					m_objCollide = null;
				}

			} else {
				m_objCollide = null;
			}

		}

		void OnTriggerExit(Collider other) 
		{
			m_objCollide = null;
		
			m_isOn = false;
		}

		void Update()
		{
			if(m_objCollide) {

				if(m_objCollide.activeSelf == false) {

					m_objCollide.transform.Translate(new Vector3(1000,1000,0));
					m_objCollide = null;
					m_isOn = false;
				}
			}
		}

		IEnumerator delaySpawn( float p_delay )
		{

			yield return new WaitForSeconds(p_delay);

			if(m_canRandomized) {
				try {
					
					//Make sure the trigger is good
					if(!m_isOn) {
						
						//Get a random number
						var r = UnityEngine.Random.Range(0,m_randomCeiling);
						//Grab the object from the memory manager
						var obj = MemoryPool.Instance.findAndGetObjs(m_objBank[r].name, false);
						
						//Check to see if we got a valid object
						if(obj) {
							//Place it
							obj.transform.position = gameObject.transform.position;
							obj.transform.rotation = gameObject.transform.rotation;
							//Wake it up
							obj.SetActive(true);
							
							//Set the particle effect
							if(m_particleEffect) {
								
								var pe = MemoryPool.Instance.findAndGetObjs(m_particleEffect.name, false);
								
								if(pe) {
									
									var pos = gameObject.transform.position;
									pos.x += m_particleOffset.x;
									pos.y += m_particleOffset.y;
									pos.z += m_particleOffset.z;
									pe.transform.position = pos;
									
									var rot = pe.transform.rotation;
									rot = Quaternion.Euler(m_particleRotOffset);
									pe.transform.rotation = rot;
									
									pe.SetActive(true);
								}
								
							}
						}
					}
				} catch(Exception p_err) {
					Debug.LogError("Error on Spawner " + p_err.Message);
				}
			}

		}



	}
}