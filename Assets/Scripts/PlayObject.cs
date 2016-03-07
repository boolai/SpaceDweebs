using UnityEngine;
using System.Collections;

namespace BoogieDownGames {

	public class PlayObject : MonoBehaviour {

		public void Sleep()
		{
			//Debug.LogError(gameObject.name + " Im melting");
			transform.parent.gameObject.SetActive(false);
		}
		
	}
}