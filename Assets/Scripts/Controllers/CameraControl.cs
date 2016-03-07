/*
 * CameraControls
 * Contains a group of methods with all of the popular types of 
 * Cinematic Camera Shots
 * It is coupled with a list of of camera positions, target positions.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace BoogieDownGames {

	[Serializable]
	public class CameraControl : MonoBehaviour {

		[SerializeField]
		private List<CameraPoint> m_camPoints;

		[SerializeField]
		private Dictionary <string,CameraPoint> m_camPointsBank = new Dictionary<string,CameraPoint>();

		[SerializeField]
		private int m_currentPos;

		[SerializeField]
		private string m_currentPosKey;

		[SerializeField]
		private float m_timeCounter;

		[SerializeField]
		private float m_width = 1;

		[SerializeField]
		private float m_height = 1;

		[SerializeField]
		private int m_travelPointsIndex;

		[SerializeField]
		private bool m_isIteratingForward; //For the travle points

		[SerializeField]
		private Text m_text;

		delegate void MultiDelegate();
		MultiDelegate cameraStateDelegate; //Which state we are currently in
		MultiDelegate camLookAtWhileInState; //The look state while we are in state
		MultiDelegate camLookAtWhileMoving;	//The look state while we are moving

		public float TimerCounter
		{
			get { return m_timeCounter; }
			set { m_timeCounter = value; }
		}

		private static CameraControl _instance;
		
		//This is the public reference that other classes will use
		public static CameraControl Instance
		{
			get
			{
				//If _instance hasn't been set yet, we grab it from the scene!
				//This will only happen the first time this reference is used.
				if(_instance == null)
					_instance = GameObject.FindObjectOfType<CameraControl>();
				return _instance;
			}
		}


		
		// Use this for initialization
		void Start () 
		{
			cameraStateDelegate = Encircle;

			//set some defaults
			/*
			foreach (var pair in m_camPointsBank)
			{
				m_camPoints.Add(pair.Key);
			}
			*/

			foreach(CameraPoint cp in m_camPoints) {

				cp.Name = cp.StationTrans.name;
				if( !m_camPointsBank.ContainsKey(cp.Name )) {
					m_camPointsBank.Add(cp.Name,cp);
				}
			}

			m_currentPosKey = m_camPoints[m_currentPos].Name;
			
			//This is while we are moving
			SetWhileMovingLookingAtState(m_camPointsBank[m_currentPosKey].LookWhileMoving);
			
			//This is for while we are stationed
			SetWhileInStationLookingAt(m_camPointsBank[m_currentPosKey].LookWhileInPoint);
			
			cameraStateDelegate = MoveToTarget;
		}
	
		public void Run()
		{
			//m_text.text = m_camPointsBank[m_currentPosKey].StationTrans.name;
			cameraStateDelegate();
		}
		
		// Update is called once per frame
		void Update () 
		{
			Run();

			if(Input.GetKeyDown(KeyCode.A)) {
				GoToNextPos();
				//gameObject.GetComponent<Machine>().ChangeState("ArcShot");
			}
		}

		void FixedUpdate()
		{

		}

		#region HelperMethods

		public void SetCameraPoint(string p_camPointKey) 
		{
			//first see if we got a key in there
			if(m_camPointsBank.ContainsKey(p_camPointKey)) {

				m_currentPosKey = p_camPointKey;

				//This is while we are moving
				SetWhileMovingLookingAtState(m_camPointsBank[m_currentPosKey].LookWhileMoving);

				//This is for while we are stationed
				SetWhileInStationLookingAt(m_camPointsBank[m_currentPosKey].LookWhileInPoint);

				//Send the camera off
				cameraStateDelegate = MoveToTarget;
			}
		}

		//Look at the Target
		public void LookAtPrimaryWhileMoving()
		{
			LerpLookat(m_camPointsBank[m_currentPosKey].TargetPrimaryTrans.position,transform.position, m_camPointsBank[m_currentPosKey].LookAtSpeedWhileMoving);
		}

		public void LookAtNone()
		{
			//LerpLookat( m_camPointsBank[m_currentPosKey].StartPos.position, transform.position, m_camPointsBank[m_currentPosKey].IsLookAtTargetWhileMoving);
		}

		//Look at the point where we are going
		public void LookAtStationWhileMoving()
		{
			LerpLookat(m_camPointsBank[m_currentPosKey].StationTrans.position, transform.position, m_camPointsBank[m_currentPosKey].LookAtSpeedWhileMoving);
		}

		public void LookAtSecondaryWhileMoving()
		{
			LerpLookat(m_camPointsBank[m_currentPosKey].TargetSecondaryTrans.position,transform.position, m_camPointsBank[m_currentPosKey].LookAtSpeedWhileMoving);
		}

		public void LookAtPrimaryWhileInStation()
		{
			LerpLookat(m_camPointsBank[m_currentPosKey].TargetPrimaryTrans.position,transform.position, m_camPointsBank[m_currentPosKey].SpeedWhileInState);
		}

		public void LookAtSecondaryWhileInStation()
		{
			LerpLookat(m_camPointsBank[m_currentPosKey].TargetSecondaryTrans.position,transform.position, m_camPointsBank[m_currentPosKey].SpeedWhileInState);
		}

		public void LookAtStationWhileInStation()
		{
			LerpLookat(m_camPointsBank[m_currentPosKey].StationTrans.position,transform.position, m_camPointsBank[m_currentPosKey].LookAtSpeedWhileMoving);
		}

		//Method just resets the travel points index we iterating positively
		public void Loop()
		{
			if(m_travelPointsIndex >= m_camPointsBank[m_currentPosKey].TravelPoints.Count) {
				m_travelPointsIndex = 0;
			} else {
				m_travelPointsIndex++;
			}
		}
		
		//Just plays the camera once
		public void Once()
		{
			if(m_travelPointsIndex < m_camPointsBank[m_currentPosKey].TravelPoints.Count -1) {
				m_travelPointsIndex++;
			}
		}
		
		//Ping Pong this plays backwards
		public void PingPong()
		{
			//Let's check to see if we are in the beginning or end
			if(m_isIteratingForward == true) {
				
				if(m_travelPointsIndex >= m_camPointsBank[m_currentPosKey].TravelPoints.Count) {
					m_isIteratingForward = false;
					m_travelPointsIndex--;
				}
				
			} else {
				if(m_travelPointsIndex <= 0) {
					m_isIteratingForward = true;
					m_travelPointsIndex++;
				}
			}
		}

		public void SetWhileMovingLookingAtState(LookAt p_lookingAt)
		{

			switch(p_lookingAt) {
				
			case LookAt.None:
				camLookAtWhileMoving = LookAtNone;
				break;
				
			case LookAt.Primary:
				camLookAtWhileMoving = LookAtPrimaryWhileMoving;
				break;
				
			case LookAt.Secondary:
				camLookAtWhileMoving = LookAtSecondaryWhileMoving;
				break;
				
			case LookAt.Station:
				camLookAtWhileMoving = LookAtStationWhileMoving;
				break;
			}
		}

		public void SetWhileInStationLookingAt(LookAt p_lookingAt)
		{
			//This is for while we are stationed
			switch(m_camPointsBank[m_currentPosKey].LookWhileInPoint) {
				
			case LookAt.None:
				camLookAtWhileInState = LookAtNone;
				break;

			case LookAt.Primary:
				camLookAtWhileInState = LookAtPrimaryWhileInStation;
				break;

			case LookAt.Secondary:
				camLookAtWhileInState = LookAtSecondaryWhileInStation;
				break;

			case LookAt.Station:
				camLookAtWhileInState = LookAtStationWhileInStation;
				break;
			}
		}



		public void GoToNextPos()
		{
			m_currentPos++;
			if(m_currentPos >= m_camPoints.Count) {
				//Debug.LogError("+++++++++++++ RESETING ++++++++++++++");
				m_currentPos = 0;
			}
			
			m_currentPosKey = m_camPoints[m_currentPos].Name;

			//This is while we are moving
			SetWhileMovingLookingAtState(m_camPointsBank[m_currentPosKey].LookWhileMoving);
			
			//This is for while we are stationed
			SetWhileInStationLookingAt(m_camPointsBank[m_currentPosKey].LookWhileInPoint);

			cameraStateDelegate = MoveToTarget;


			//Debug.Log( "Current postion ==>" + m_currentPosKey );
		}

		public void FollowPoints()
		{
			LerpFieldOfView();
			camLookAtWhileInState();
			//Check for the distance
			if(m_travelPointsIndex < m_camPointsBank[m_currentPosKey].TravelPoints.Count) {
				//Debug.LogError("index follow -----> " + m_travelPointsIndex);
				if(Vector3.Distance(transform.position,m_camPointsBank[m_currentPosKey].TravelPoints[m_travelPointsIndex].position) > m_camPointsBank[m_currentPosKey].MarginOfError) {

					transform.position = Vector3.Lerp(transform.position, m_camPointsBank[m_currentPosKey].TravelPoints[m_travelPointsIndex].position, m_camPointsBank[m_currentPosKey].SpeedWhileInState * Time.deltaTime);
		
				} else {
					m_travelPointsIndex++;
					//Debug.LogError("changing index follow -----> " + m_travelPointsIndex);
					//Debug.LogError("Im done");

					switch( m_camPointsBank[m_currentPosKey].MyLoop ) {
						
					case LoopTrv.Once:
						//
						break;
						
					case LoopTrv.Restart:
						m_travelPointsIndex = 0;
						break;
						
					case LoopTrv.PingPong:
						PingPong();
						break;
					}
				}
			}
		}

		public void LerpFieldOfView()
		{
			if(m_camPointsBank[m_currentPosKey].IsLerpingFieldOfView == true) {
				GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, m_camPointsBank[m_currentPosKey].FieldOfViewEnd, Time.deltaTime * m_camPointsBank[m_currentPosKey].FieldOfViewLerpSpeed);
			} else {
				GetComponent<Camera>().fieldOfView = 60;
			}
		}

		public void LerpLookat(Vector3 p_target, Vector3 p_lookie, float p_speed)
		{
			var newRotation = Quaternion.LookRotation(p_target - p_lookie).eulerAngles;
			//newRotation.x = 0;
			//newRotation.z = 0;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime * p_speed);
		}

		public void LerpLookAtTravel()
		{
			var newRotation = Quaternion.LookRotation(m_camPointsBank[m_currentPosKey].TravelPoints[m_travelPointsIndex].position - transform.position).eulerAngles;
			//newRotation.x = 0;
			//newRotation.z = 0;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime * m_camPointsBank[m_currentPosKey].SpeedWhileInState);

			if(Quaternion.Angle(transform.rotation, Quaternion.Euler(newRotation)) < 0.1f) {
				m_travelPointsIndex++;
			}

			if(m_travelPointsIndex >= m_camPointsBank[m_currentPosKey].TravelPoints.Count) {
				m_travelPointsIndex = 0;
			}
		}
		
		#endregion

		#region StateMethods
	
		public void Oscillate()
		{
			LerpFieldOfView();
			m_timeCounter += Time.deltaTime * m_camPointsBank[m_currentPosKey].SpeedWhileInState;
			float x = Mathf.Cos(m_timeCounter) * m_width;
			float y = m_camPointsBank[m_currentPosKey].StationTrans.position.y;
			float z = Mathf.Sin(m_timeCounter) * m_height;
			transform.position = Vector3.Lerp(m_camPointsBank[m_currentPosKey].StationTrans.position,new Vector3(x,y,z),m_camPointsBank[m_currentPosKey].SpeedWhileInState);
			//transform.LookAt(m_camPointsBank[m_currentPosKey].TargetPrimaryTrans.position);
			camLookAtWhileInState();
		}

		/*
		 * Arc Shot
		 * A shot in which the subject is circled by the camera. Beloved by Brian De Palma, Michael Bay.
		 * THE EXAMPLE
		 * The shot in De Palma's Carrie (1976) where Carrie White (Sissy Spacek) and Tommy Ross (William Katt) are dancing at the prom. The swirling camera move represents her giddy euphoria, see?
		*/
		public void Encircle()
		{

			LerpFieldOfView();
			switch(m_camPointsBank[m_currentPosKey].LookWhileInPoint) {

			case LookAt.None:

				break;

			case LookAt.Primary:
				transform.LookAt(m_camPointsBank[m_currentPosKey].TargetPrimaryTrans.position);
				break;

			case LookAt.Secondary:
				transform.LookAt(m_camPointsBank[m_currentPosKey].TargetSecondaryTrans.position);
				break;

			case LookAt.Station:
				transform.LookAt(m_camPointsBank[m_currentPosKey].StationTrans.position);
				break;
			}

			transform.RotateAround(m_camPointsBank[m_currentPosKey].TargetPrimaryTrans.position, Vector3.up, m_camPointsBank[m_currentPosKey].SpeedWhileInState *Time.deltaTime);
		}

		//This is the state in which the camera is moving to it's target position
		public void MoveToTarget()
		{
			GetComponent<Camera>().fieldOfView = 60;
			//Unattach the camera
			transform.parent = null;
			camLookAtWhileMoving();
			//LerpFieldOfView();
			//LerpLookat(m_camPointsBank[m_currentPosKey].LookAt.position,transform.position,1);
			//let's see if it reached the target
			if(Vector3.Distance(this.transform.position,m_camPointsBank[m_currentPosKey].StationTrans.position) > m_camPointsBank[m_currentPosKey].MarginOfError) {

				if(m_camPointsBank[m_currentPosKey].MoveType == Mobility.Fly) {
					//transform.LookAt( m_camPointsBank[m_currentPosKey].LookAt.position);
					transform.position = Vector3.Lerp(transform.position, m_camPointsBank[m_currentPosKey].StationTrans.position, m_camPointsBank[m_currentPosKey].SpeedGoingToStation * Time.deltaTime);
				} else {
					transform.position = m_camPointsBank[m_currentPosKey].StationTrans.position;
				}

			} else {

				//Ok we are here now set the shot type
				switch(m_camPointsBank[m_currentPosKey].MyCamActionType) {

				case CamState.Static :
					cameraStateDelegate = CamStatic;

					break;

				case CamState.Encircle:
					cameraStateDelegate = Encircle;
					break;

				case CamState.Dolly:
					m_travelPointsIndex = 0;
					//Debug.LogError("Dolly zoom ======> ");
					cameraStateDelegate = FollowPoints;
					break;

				case CamState.Dutch:

					cameraStateDelegate = Tilt;
					break;

				case CamState.Pan:
					cameraStateDelegate = Pan;
					break;
				}
			}
		}

		//The Camera Does Nothing
		public void CamStatic()
		{

			if(m_camPointsBank[m_currentPosKey].IsAttachedToPoint == true) {
				transform.parent = m_camPointsBank[m_currentPosKey].StationTrans.transform;
			}
			//Nothing but set the lookat
			camLookAtWhileInState();

			//set the field of view
			LerpFieldOfView();
		}

		public void Pan()
		{
			//Goes through the follows and pans continally between them
			LerpLookAtTravel();
		}



		//The Camera moves up
		public void CraneUp()
		{

		}


		public void IterateThroughPoints()
		{

		}

	
		//lerp Rect view
		public void LerpRectView()
		{

		}

		public void Tilt()
		{
			var rot = m_camPointsBank[m_currentPosKey].StationTrans.rotation;

			transform.rotation = rot;
		}

		#endregion
	}
}