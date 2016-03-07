using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace BoogieDownGames {

	[Serializable]
	public class EventsProperty : PropertyAttribute {

		[SerializeField]
		private string m_name;

		[SerializeField]
		private string m_title;

		[SerializeField]
		private string m_description;

		[SerializeField]
		private bool m_hasPlayed;

		[SerializeField]
		private Sprite m_image;

		public string Title
		{
			get { return m_title; }
			set { m_title = value; }
		}

		public string Description
		{
			get { return m_description; }
			set { m_description = value; }
		}

		public bool HasPlayed
		{
			get { return m_hasPlayed; }
			set { m_hasPlayed = value; }
		}

		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		public Sprite EventImage
		{
			get { return m_image; }
		}

	}
}