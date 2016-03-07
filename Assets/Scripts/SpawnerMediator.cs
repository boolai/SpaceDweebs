using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoogieDownGames {

	public class SpawnerMediator : MonoBehaviour {

		[SerializeField]
		private List<SpawnerRad> m_spawners; 


		// Use this for initialization
		void Start () 
		{
			NotificationCenter.DefaultCenter.AddObserver(this,"RandomSpawn");
		}

		public void RandomSpawn()
		{
			var index = UnityEngine.Random.Range( 0, m_spawners.Count);
			m_spawners[index].RandomSpawnMediated();

		}
		
		// Update is called once per frame
		void Update () 
		{
		
		}
	}

}
