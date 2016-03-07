using UnityEngine;
using System.Collections;
using System;

namespace BoogieDownGames {

	[Serializable]
	public class ParticleAttributes : PropertyAttribute {

		#region MEMBERS

		[SerializeField]
		private ParticleType m_particleType;

		[SerializeField]
		private GameObject m_particle;

		#endregion

		#region PROPERTIES

		public ParticleType TypeParticle
		{
			get { return m_particleType; }
			set { m_particleType = value; }
		}

		public GameObject MyParticle
		{
			get { return m_particle; }
			set { m_particle = value; }
		}

		#endregion

	}
}