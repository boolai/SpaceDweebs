using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoogieDownGames {
	[Serializable]
	public class State : PropertyAttribute {

		[SerializeField]
		private string m_name;

	}
}