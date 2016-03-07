using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoogieDownGames {
	
	public class SequencePressed {

		[SerializeField]
		private bool m_isPressed;

		[SerializeField]
		private int m_tileNumber;

		public bool IsPressed
		{
			get { return m_isPressed; }
			set { m_isPressed = value; }
		}

		public int TileNumber
		{
			get { return m_tileNumber; }
			set { m_tileNumber = value; }
		}

		public SequencePressed (bool p_isPressed, int p_tileNumber) {
			this.m_isPressed = p_isPressed;
			this.m_tileNumber = p_tileNumber;
		}
	}
}