using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace BoogieDownGames {

	[Serializable]
	public class Achievement : PropertyAttribute {

		[SerializeField]
		private Sprite m_trophyPic;

		[SerializeField]
		private string m_name;

		[SerializeField]
		private int m_valueAchieve;

		[SerializeField]
		private int m_progressValue;

		[SerializeField]
		private string m_title;

		[SerializeField]
		private string m_description;

		[SerializeField]
		private string m_uncompleteDescription;

		[SerializeField]
		private bool m_isComplete;

		[SerializeField]
		private AudioClip m_sound;


		#region PROPERTIES

		public Sprite TrophyPic 
		{
			get { return m_trophyPic; }
			set { m_trophyPic = value; }
		}

		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		public int AchieveValue
		{
			get { return m_valueAchieve; }
			set { m_valueAchieve = value; }
		}

		public int ProgressValue
		{
			get { return m_progressValue; }
			set { m_progressValue = value; }
		}

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

		public string UnCompleteDescription
		{
			get { return m_uncompleteDescription; }
			set { m_uncompleteDescription = value; }
		}

		public bool IsComplete
		{
			get { return m_isComplete; }
			set { m_isComplete = value; }
		}

		#endregion

		public bool CheckForCompletion()
		{
			if( m_progressValue >= m_valueAchieve ) {
				m_isComplete = true;
			} 
			return m_isComplete;
		}

	
	}
}