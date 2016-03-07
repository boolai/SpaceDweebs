/*
 * Property attritbute for the Camera focus points
 * Stores the locations for the placement of the camera
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoogieDownGames {

	public enum Mobility { Instant, Fly}; //Is the camera flying or instanty transported
	//Which State will the camera be when the camera is stationed 
	public enum CamState { Static, Encircle, Dolly, Pan, Dutch ,TiltUpAndDown, RotateX, RotateY, RotateZ, TiltX, TiltY, TiltZ, CraneUp, CraneDown, ItrPoints};
	//Will the camera be repeating and how
	public enum LoopTrv { Once, Restart, PingPong };
	//What will the camera be looking at
	public enum LookAt { Station, Primary, Secondary , None };

	[Serializable]
	public class CameraPoint : PropertyAttribute {

		[SerializeField]
		private string m_name;  //The name of the Cam point

		[SerializeField]
		private bool m_isStartPoint;

		[SerializeField]
		private Transform m_stationTransform; //The position the camera is going to be set at.

		[SerializeField]
		private Transform m_primaryTargetTransform; //The primary target

		[SerializeField]
		private Transform m_secondaryTargetTransform;//The secondary Target

		[SerializeField]
		private List<Transform> m_travelPoints;//In between points while in stationed mode

		[SerializeField]
		private float m_speedWhileGoingToStation; //The speed when in this mode

		[SerializeField]
		private float m_speedWhileInState; //The speed when in the state

		[SerializeField]
		private Mobility m_travelType;

		[SerializeField]
		private float m_marginOfError;

		[SerializeField]
		private float m_fieldOfView;

		[SerializeField]
		private float m_rectX;

		[SerializeField]
		private float m_rectY;

		[SerializeField]
		private float m_rectW;

		[SerializeField]
		private float m_rectH;

		[SerializeField]
		private bool m_isLerpingFieldOfView;

		[SerializeField]
		private float m_fieldOfViewStart;

		[SerializeField]
		private float m_fieldOfViewEnd;

		[SerializeField]
		private float m_fieldOfViewLerpSpeed;

		[SerializeField]
		private bool m_isLerpingViewRect;

		[SerializeField]
		private CamState m_CamActionType;

		[SerializeField]
		private float m_lookAtSpeedWhileMoving;

		[SerializeField]
		private bool m_isRepeatingMovement;

		[SerializeField]
		private LoopTrv m_loopType;

		[SerializeField]
		private bool m_isAttachedToPoint;//Is this attached to the point

		[SerializeField]
		private float m_rotX;

		[SerializeField]
		private float m_rotY;

		[SerializeField]
		private float m_rotZ;

		[SerializeField]
		private LookAt m_lookWhileMoving;

		[SerializeField]
		private LookAt m_lookWhileInPoint;

		#region PROPERTIES

		public LookAt LookWhileMoving
		{
			get { return m_lookWhileMoving; }
		}

		public LookAt LookWhileInPoint
		{
			get { return m_lookWhileInPoint; }
		}

		public float LookAtSpeedWhileMoving
		{
			get { return m_lookAtSpeedWhileMoving; }
		}

		public float RotX
		{
			get { return m_rotX; }
		}

		public float RotY
		{
			get { return m_rotY; }
		}

		public float RotZ
		{
			get { return m_rotZ; }
		}

		public bool IsAttachedToPoint
		{
			get { return m_isAttachedToPoint; }
			set { m_isAttachedToPoint = value; }
		}

		public List<Transform> TravelPoints
		{
			get { return m_travelPoints; }
		}

		public LoopTrv MyLoop
		{
			get { return m_loopType;}
		}

		public bool IsRepeatingMovement
		{
			get { return m_isRepeatingMovement; }
		}

		public CamState MyCamActionType
		{
			get { return m_CamActionType; }
		}

		public bool IsLerpingViewRect
		{
			get { return m_isLerpingViewRect; }
		}

		public float FieldOfViewLerpSpeed
		{
			get { return m_fieldOfViewLerpSpeed; }
		}

		public float FieldOfViewEnd
		{
			get { return m_fieldOfViewEnd; }
		}

		public float FieldOfViewStart
		{
			get { return m_fieldOfViewStart; }
		}

		public bool IsLerpingFieldOfView
		{
			get { return m_isLerpingFieldOfView; }
		}

		public float RectH
		{
			get { return m_rectH; }

		}
		public float RectW
		{
			get { return m_rectW; }

		}
		public float RectY
		{
			get { return m_rectY; }
		}

		public float FieldOfView
		{
			get { return m_fieldOfView; }
		}

		public float RectX
		{
			get { return m_rectX; }
		}

		public float SpeedWhileInState
		{
			get { return m_speedWhileInState; }
		}

		public float MarginOfError
		{
			get { return m_marginOfError; }
		}

		public Transform StationTrans
		{
			get { return m_stationTransform; }
		}

		public Transform TargetPrimaryTrans
		{
			get { return m_primaryTargetTransform; }
		}

		public Transform TargetSecondaryTrans
		{
			get { return m_secondaryTargetTransform;}
		}

		public bool IsStartingPoint
		{
			get { return m_isStartPoint; }
			set { m_isStartPoint = true; }
		}

		public float SpeedGoingToStation
		{
			get { return m_speedWhileGoingToStation; }
		}

		public Mobility MoveType
		{
			get { return m_travelType; }
		}

		public string Name
		{
			get { return m_name; }
			set { m_name = value; } 
		}

		#endregion

	}
}