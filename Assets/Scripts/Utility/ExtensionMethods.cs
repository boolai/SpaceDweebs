using UnityEngine;
using System.Collections;

namespace BoogieDownGames {

	public static class ExtensionMethods {

		public static void SetGameObjectActive(this GameObject p_gameobject, bool p_state)
		{
			p_gameobject.SetActive(p_state);


		}
	}
}