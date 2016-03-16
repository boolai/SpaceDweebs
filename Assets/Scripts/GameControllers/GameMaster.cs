using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace BoogieDownGames {
	
	public class GameMaster : BaseGameController {

		#region MEMBERS

		[SerializeField]
		private int m_lives; //Holds the number of strikes the 

		[SerializeField]
		private int m_maxLives;
		
		[SerializeField]
		private int m_addSeqAtRound;//Stores the round in which to add another sequence

		[SerializeField]
		private int m_addNewSpawnAtRound;
		
		[SerializeField]
		private int m_currentNumOfSequences; //The current number of sequences

		[SerializeField]
		private int m_longestStreak;

		[SerializeField]
		private int m_playerSeq;
		
		[SerializeField]
		private float m_splashSceneDelay; //How long before splash screen switches to the menu
		
		[SerializeField]
		private float m_arenaTimeToStart;
		
		[SerializeField]
		private int m_sequenceCounter;//points to squence index

		[SerializeField]
		private bool m_isInSugarRush;

		[SerializeField]
		private int m_scoreMultiplier;
		
		[SerializeField]
		private string m_timerStringSpecifier; //Specifies the string format for the timer ui i.e. "00" or "00.00"

		[SerializeField]
		private string m_StringSpecifier;

		[SerializeField]
		private bool m_isIncreasingSeq;

		[SerializeField]
		private bool m_isCurrentRoundLost;

		[SerializeField]
		private float m_endTurnTransitionTime;

		[SerializeField]
		private float m_bufferTime;

		[SerializeField]
		private bool m_isHurry;

		[SerializeField]
		private TimeKeeper m_playerTurnClock;

		[SerializeField]
		private TimeKeeper m_beforePlayerStartBuffer;

		[SerializeField]
		private TimeKeeper m_sequenceClock; //The time between the sequences 

		[SerializeField]
		private TimeKeeper m_endTurnSequenceClock;

		[SerializeField]
		private TimeKeeper m_endTurnClock;

		[SerializeField]
		private List<int> m_sequence;

		[SerializeField]
		private List <bool> m_sequencePressed;

		[SerializeField]
		private TimeKeeper m_setUpClock;

		[SerializeField]
		private TimeKeeper m_startGameClock;

		[SerializeField]
		private List<GameObject> m_liveObjects;

		private Dictionary<int,int> m_inSeqBank = new Dictionary<int, int>(); 

		[SerializeField]
		private TimeKeeper m_ufoTimer;

		[SerializeField]
		private string m_difficultyNameForUrl;

        [SerializeField]
        private bool isBeatHighScore = false;

		[SerializeField]
		private string m_dbUrl;

        [SerializeField]
        private GameObject m_audioSource_applause;

		delegate void MultiDelegate();
		MultiDelegate myMultiDelegate;

		[SerializeField]
		private bool m_isGameSetupDone;

		private int SaveFileCreated = 0;
		#endregion

		#region PROPERTIES
	

		public int AddSeqAtRound
		{
			get { return m_addSeqAtRound; }
			set { m_addSeqAtRound = value; }
		}

		#endregion

		#region SETUPMETHODS

		public override void Awake ()
		{
			base.Awake ();
			//set the default states

			this.SetNotifications();
			
			this.HasControl = false;
			MyTimeMode.SetMode(CurrentTimeMode.Normal);

			loadPlayerData();

		}

		#endregion


		#region STATES

		/// <summary>
		/// Game the state idle enter.
		/// Upon the level being loaded this goes should be the first state the game goes to
		/// </summary>
		public void GameStateIdleEnter()
		{
			//Debug.LogError("I am in game idle state enter");

			if(SceneManager.GetActiveScene().name == "Game") {
				//Debug.LogError("Loading game");
				//StartCoroutine(StartTheGame(2));
				m_startGameClock.ResetClock();
				//Set all the defaults here
				m_lives = 3;
				CurrentWave = 1;
				m_currentNumOfSequences = 3;
				m_isInSugarRush = false;
				m_scoreMultiplier = 1;
				//Reset all the clocks
				m_playerTurnClock.ResetClock();
				m_sequenceClock.ResetClock();
				m_endTurnSequenceClock.ResetClock();
				m_endTurnClock.ResetClock();
				m_sequence.Clear();
				m_sequencePressed.Clear();
				m_setUpClock.ResetClock();
				m_startGameClock.ResetClock();
				m_liveObjects.Clear();
				m_inSeqBank.Clear();
				m_ufoTimer.ResetClock();
				MyTimeMode.NoTimeModeRunning();
				m_isGameSetupDone = true;
				CurrentScore = 0;
			}
			
			Hashtable dat = new Hashtable();
			dat.Add("dat", "Idle State");
			NotificationCenter.DefaultCenter.PostNotification(this, "SetText",dat);
		}

		/// <summary>
		/// Games the state idle update.
		/// Sets up the game
		/// </summary>
		public void GameStateIdleUpdate()
		{

			MyTimeMode.NoTimeModeRunning();
			if(Input.anyKey && SceneManager.GetActiveScene().name == "Splash") {
				UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
			}

			if(Input.GetKeyDown(KeyCode.R) && SceneManager.GetActiveScene().name == "Menu") {

				ResetScore();
			}
			
			if(SceneManager.GetActiveScene().name == "Game") {

				m_startGameClock.Run();
				if(m_startGameClock.IsDone) {
			
					m_startGameClock.ResetClock();
					GameFSM.ChangeState(GameStateSetUp.Instance);
					return;
				} 
			}
		}

		/// <summary>
		/// Games the state idle exit.
		/// </summary>
		public void GameStateIdleExit()
		{
			MyTimeMode.NoTimeModeRunning();
		}

		/// <summary>
		/// Games the state set up enter.
		/// This method drops the tiles into place
		/// So all the spawn points if able too till drop the 
		/// a new tile.
		/// </summary>
		public void GameStateSetUpEnter()
		{
			if(m_isGameSetupDone) {

				m_isGameSetupDone = false;
				m_isHurry = false;

				//Set up the ufo speed  mode
				NotificationCenter.DefaultCenter.PostNotification(this, "SetSlowMode");
				MyTimeMode.NoTimeModeRunning();
				

				if(CurrentWave == 1) {
					
					//send a message that we picked this object
					Hashtable dat = new Hashtable();
					dat.Add("event", "FirstRound");
					NotificationCenter.DefaultCenter.PostNotification(this, "PlayEvent",dat);
					NotificationCenter.DefaultCenter.PostNotification(this,"SpawnRandom");
					CurrentScore = 0;

				} else {
					//Spawn all the dweebs for this round

				
					//check to see if we have to update the number of sequences
					var md = CurrentWave % m_addSeqAtRound;
					
					if(md == 0) {
						//Debug.LogError("Incrementing sequence from " + m_currentNumOfSequences + " tp " + (m_currentNumOfSequences + 1).ToString() + " round " + CurrentWave);

						m_currentNumOfSequences++;
					}

					/*
					var p = CurrentWave % m_addNewSpawnAtRound;
					if(p == 0) {
						//Increase spawnable tiles chance
						NotificationCenter.DefaultCenter.PostNotification(this,"increaseSpawnChance");

					}

					*/

					if(m_currentNumOfSequences >= m_longestStreak) {

						m_longestStreak = m_currentNumOfSequences;
					}
					
					m_playerTurnClock.TimeLimit = (float)m_currentNumOfSequences + m_bufferTime;
					m_playerTurnClock.Counter = (float)m_currentNumOfSequences + m_bufferTime;
					
					NotificationCenter.DefaultCenter.PostNotification(this,"SpawnRandomMaybe");

				}


				//Delete the old sequence
				m_sequence.Clear();
				m_sequencePressed.Clear();
				
				//Reset the counter
				m_sequenceCounter = 0;
				
				m_liveObjects.Clear();
				m_inSeqBank.Clear();

				//reset the setupClock for the delays
				m_setUpClock.ResetClock();
				
				if(m_isInSugarRush) {
					
					m_setUpClock.Counter *= 2;
				} else {
					m_setUpClock.Counter = m_setUpClock.TimeLimit;
				}
			}
		}
		
		public void GameStateSetUpUpdate()
		{
			MyTimeMode.NoTimeModeRunning();
			////Debug.LogError("I am in setup");
			
			//Run the set up clock
			m_setUpClock.Run();
			
			if(m_setUpClock.IsDone) {
				//Find all of the live tiles
				FindLiveTiles();
				
				//Set the sequence
				SetSequence();
				GameFSM.ChangeState(GameStateCompTurn.Instance);
				return;
			}
		}
		
		/// <summary>
		/// Games the state set up exit.
		/// </summary>
		public void GameStateSetUpExit()
		{
			//Debug.LogError("Game setup  exit ****** being called");
			MyTimeMode.NoTimeModeRunning();
		}

		public void GameStateCompTurnEnter()
		{

			MyTimeMode.NoTimeModeRunning();
			//Reset the counter
			m_sequenceCounter = 0;
			//Reset the clock
			m_sequenceClock.ResetClock();

			Hashtable d = new Hashtable();
			d.Add("dat","Look");
			NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",d);
			Hashtable dat = new Hashtable();
			dat.Add("dat", "look");

			NotificationCenter.DefaultCenter.PostNotification(this, "PlayAudioAtIndex" ,dat);
			//NotificationCenter.DefaultCenter.PostNotification(this,"SpawnRandom");

			m_beforePlayerStartBuffer.ResetClock();
		}
		
		public void GameStateCompTurnUpdate()
		{
			//Debug.LogError("Computer turn update");
			MyTimeMode.NoTimeModeRunning();

			//Go through each sequence and play it
			m_sequenceClock.Run();
			
			if(m_sequenceClock.IsDone) {
				
				//MemoryPool.Instance.LiveObjects[m_sequence[m_sequenceCounter]].GetComponent<Tile>().PlayAnimation(TileAnimations.RightTouch);
				//Check to see if the sequence is over
				if(m_sequenceCounter >= m_currentNumOfSequences) {

					m_beforePlayerStartBuffer.Run();
					if(m_beforePlayerStartBuffer.IsDone) {
						GameFSM.ChangeState(GameStatePlayersTurn.Instance);
						return;
					}
						
				} else {
					
					var index = m_sequence[m_sequenceCounter];
					m_liveObjects[index].GetComponent<Tile>().PlayAnimation(TileAnimations.RightTouch);
					m_liveObjects[index].GetComponent<Tile>().PlayAudioClip(0);
					m_liveObjects[index].GetComponent<Tile>().PlayAParticle(ParticleType.RightTouch);
					m_sequenceClock.IsDone = false;
					m_sequenceClock.ResetClock();
					m_sequenceCounter++;
					//send a message that we picked this object
					Hashtable dat = new Hashtable();
					dat.Add("event", m_liveObjects[index].name);
					NotificationCenter.DefaultCenter.PostNotification(this, "PlayEvent",dat);
					
				}
			}
		}
		
		public void GameStateCompTurnExit()
		{

			Hashtable d = new Hashtable();
			d.Add("dat","ReadyGoPart");
			NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",d);
			Hashtable dat = new Hashtable();
			dat.Add("dat", "gogogo");

			NotificationCenter.DefaultCenter.PostNotification(this, "PlayAudioAtIndex" ,dat);
			MyTimeMode.NoTimeModeRunning();
		}

		/// <summary>
		/// Games the state players turn enter.
		/// </summary>
		public void GameStatePlayersTurnEnter()
		{

			//send a message that we picked this object
			Hashtable dat = new Hashtable();
			dat.Add("event", "PlayerTurn");
			NotificationCenter.DefaultCenter.PostNotification(this, "PlayEvent",dat);
			MyTimeMode.NoTimeModeRunning();

			if(GameFSM.PreviousState == GameStateCompTurn.Instance) {
				m_sequenceCounter = 0;
				m_playerTurnClock.ResetClock();
				RunRandomUfo();
			
			}
		}
		
		/// <summary>
		/// Games the state players turn update.
		/// called every Update
		/// </summary>
		public void GameStatePlayersTurnUpdate()
		{
			//Debug.LogError("Players turn update");
			MyTimeMode.RunTimeMode();
			//Check the clock 
			m_playerTurnClock.Run();
			if(m_playerTurnClock.Counter < (m_playerTurnClock.TimeLimit * 0.25f)) {

				if(!m_isHurry) {

					Hashtable dat = new Hashtable();
					dat.Add("dat", "hurry");

					NotificationCenter.DefaultCenter.PostNotification(this, "PlayAudioAtIndex" ,dat);
					m_isHurry = true;
				}

			}

			if(m_playerTurnClock.IsDone) {
				GameFSM.ChangeState(GameStateEndTurn.Instance);
				return;
			}
			
			//Check to see if player finished sequence
			if(m_sequenceCounter >= m_currentNumOfSequences) {
				//get the last index
				var lastOne = m_sequence[m_sequence.Count -1];
				var obj = m_liveObjects[lastOne];
				if(obj.GetComponent<Tile>().Special != TileSpecial.Repeatable) {
					GameFSM.ChangeState(GameStateEndTurn.Instance);

				}
				return;
			}

			//if the player clicks on a tile and its in the sequence then we got it
			RunHitDetections();
			/*
			if(CurrentScore > HighScore && !isBeatHighScore) 
			{
				HighScore = CurrentScore;
				NotificationCenter.DefaultCenter.PostNotification(this, "NewHighScore");
				//GameObject.Find("NHS-Holder").transform.GetChild(0).gameObject.SetActive(true);
				Hashtable d = new Hashtable();
				d.Add("dat","NewHighScore");
				NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",d);

				isBeatHighScore = true;
				if(m_audioSource_applause) {
					m_audioSource_applause.GetComponent<AudioSource>().Play();
				}

			} else if(CurrentScore > HighScore) {
				HighScore = CurrentScore;
			}
			*/

		}
		
		public void GameStatePlayersTurnExit()
		{



			MyTimeMode.NoTimeModeRunning();
		}

		/// <summary>
		/// Games the state end turn enter.
		/// </summary>
		public void GameStateEndTurnEnter()
		{

			MyTimeMode.NoTimeModeRunning();
			//Check to see if the sequence is done
			NotificationCenter.DefaultCenter.PostNotification(this,"DestroyUfo");

			if(!CheckForSequenceComplete()) {
				m_isCurrentRoundLost = true;
				m_lives--;
				Hashtable d = new Hashtable();
				d.Add("dat","TooSlow");
				NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",d);

				Hashtable dat = new Hashtable();
				dat.Add("dat", "aha");

				NotificationCenter.DefaultCenter.PostNotification(this, "PlayAudioAtIndex" ,dat);
			} else {
				
				m_isCurrentRoundLost = false;
				m_lives = 3;
			}
			
			if(m_lives <= 0) {
				
				GameFSM.ChangeState(GameStatePlayeOneLost.Instance);
				return;
			}

			//StartCoroutine(DelaySetUp(3));
			m_endTurnSequenceClock.ResetClock();
			m_sequenceCounter = 0;
			NotificationCenter.DefaultCenter.PostNotification(this, "SetFastMode");

		}
		
		public void GameStateEndTurnUpdate()
		{
			MyTimeMode.NoTimeModeRunning();
			//Go through each sequence and play it
			m_endTurnSequenceClock.Run();
			
			if(m_endTurnSequenceClock.IsDone) {
				
				//Check to see if the sequence is over
				if(m_sequenceCounter >= m_currentNumOfSequences) {

					if(m_isCurrentRoundLost == false) {
						ClearWinTiles();
					}


					//pick which to run 
					var md = CurrentWave % m_addNewSpawnAtRound;
					if(md == 0) {
						//Display the ship flying animation
						m_endTurnSequenceClock.ResetClock();
						m_endTurnSequenceClock.Counter = 100f;
						//Debug.LogError("New spawn and anime sequence");
						//GameFSM.ChangeState(GameStateUIAnime.Instance);
						StartCoroutine(DelaySetUp(m_endTurnTransitionTime));

					} else {
						StartCoroutine(DelaySetUp(m_endTurnTransitionTime));
					}

				 md = CurrentWave % m_addSeqAtRound;

					if(md == 0) {
						//Debug.LogError("Incrementing sequence from " + m_currentNumOfSequences + " tp " + (m_currentNumOfSequences + 1).ToString() + " round " + CurrentWave);

						m_currentNumOfSequences++;
					}
					return;
				} else {
					
					var index = m_sequence[m_sequenceCounter];
					var tile = m_liveObjects[index].GetComponent<Tile>();
					m_endTurnSequenceClock.IsDone = false;
					m_endTurnSequenceClock.ResetClock();
					m_sequenceCounter++;
					
					switch(tile.Special) {
						
					case TileSpecial.Bomb:
						
						if(m_isCurrentRoundLost == true && tile.Pressed) {
							
							tile.PlayAnimation(TileAnimations.Failed);
							//play a sound
							
						} else if( m_isCurrentRoundLost == false ) {

							tile.PlayAnimation(TileAnimations.Success);
							tile.PlayAudioClip(1);
							tile.PlayAParticle(ParticleType.Success);
							tile.BlowUp();
						}
						
						break;
						
					case TileSpecial.Bonus:

						if(m_isCurrentRoundLost == true && tile.Pressed) {
							
							tile.PlayAnimation(TileAnimations.Failed);
							//CurrentScore -= tile.Score;
							
						} else if( m_isCurrentRoundLost == false ) {
							
							CurrentScore += tile.Score;
							CurrentScore *= 2;
							tile.PlayAnimation(TileAnimations.Failed);
							tile.PlayAParticle(ParticleType.Failed);
							tile.PlayAudioClip(0);
						}
						
						break;
						
					case TileSpecial.ExtraLife:
						
						if(m_isCurrentRoundLost == true && tile.Pressed) {
							
							tile.PlayAnimation(TileAnimations.Failed);
							//play a sound
							
						} else if( m_isCurrentRoundLost == false ) {
							
							CurrentScore += tile.Score;
							tile.PlayAnimation(TileAnimations.RightTouch);
							tile.PlayAParticle(ParticleType.RightTouch);
							tile.PlayAudioClip(0);
							m_lives++;
						}
						
						break;
						
					case TileSpecial.Multiplier:
						
						if(m_isCurrentRoundLost == true && tile.Pressed) {
							
							tile.PlayAnimation(TileAnimations.Failed);
							//play a sound
							m_scoreMultiplier = 1;
							
						} else if( m_isCurrentRoundLost == false ) {
							
							CurrentScore += tile.Score * m_scoreMultiplier;
							tile.PlayAnimation(TileAnimations.RightTouch);
							tile.PlayAParticle(ParticleType.RightTouch);
							tile.PlayAudioClip(0);
						}
						
						break;
						
					case TileSpecial.None:
						
						if(m_isCurrentRoundLost == true && tile.Pressed) {
							
							tile.PlayAnimation(TileAnimations.Failed);
							//play a sound
							m_scoreMultiplier = 1;
							
						} else if( m_isCurrentRoundLost == false ) {
							
							CurrentScore += tile.Score * m_scoreMultiplier;
							tile.PlayAnimation(TileAnimations.RightTouch);
							tile.PlayAParticle(ParticleType.RightTouch);
							tile.PlayAudioClip(0);
						}
						
						break;

						
					case TileSpecial.Repeatable:
						
						if(m_isCurrentRoundLost == true && tile.Pressed) {
							
							tile.PlayAnimation(TileAnimations.Failed);
							//play a sound
							m_scoreMultiplier = 1;
							
						} else if( m_isCurrentRoundLost == false ) {
							
							CurrentScore += tile.Score * m_scoreMultiplier;
							tile.PlayAnimation(TileAnimations.RightTouch);
							tile.PlayAParticle(ParticleType.RightTouch);
							tile.PlayAudioClip(0);
						}
						
						break;
						
					case TileSpecial.SlowMo:
						
						if(m_isCurrentRoundLost == true && tile.Pressed) {
							
							tile.PlayAnimation(TileAnimations.Failed);
							//play a sound
							m_scoreMultiplier = 1;
							
						} else if( m_isCurrentRoundLost == false ) {
							
							CurrentScore += tile.Score * m_scoreMultiplier;
							tile.PlayAnimation(TileAnimations.RightTouch);
							tile.PlayAParticle(ParticleType.RightTouch);
							tile.PlayAudioClip(0);
						};
						
						break;

					case TileSpecial.UFO:
						
						
						break;
					}
				}
			}
		}
		
		public void GameStateEndTurnExit()
		{
			if(!m_isCurrentRoundLost) {
				//add extra points for timing if any
				var result = 100 * m_playerTurnClock.Counter;
				CurrentScore += Mathf.CeilToInt( result );

				if(CurrentScore > HighScore && !isBeatHighScore) 
				{
					HighScore = CurrentScore;
					NotificationCenter.DefaultCenter.PostNotification(this, "NewHighScore");
					//GameObject.Find("NHS-Holder").transform.GetChild(0).gameObject.SetActive(true);
					Hashtable dhs = new Hashtable();
					dhs.Add("dat","NewHighScore");
					NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",dhs);

					isBeatHighScore = true;
					if(m_audioSource_applause) {
						m_audioSource_applause.GetComponent<AudioSource>().Play();
					}

				} 
				else 
				{
					if(CurrentScore > HighScore) HighScore = CurrentScore;
					Hashtable d = new Hashtable();
					d.Add("dat","Yippe");
					NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",d);
					NotificationCenter.DefaultCenter.PostNotification(this,"PlayAudioAtIndex",d);
				}
			} 

			CurrentWave++;

			MyTimeMode.NoTimeModeRunning();
			m_isGameSetupDone = true;
			//Debug.LogError("Game end turn exit");

		}

		public void GameStateGlobalEnter()
		{
			
		}
		
		public void GameStateGlobalUpdate()
		{
			if(Input.GetKeyDown(KeyCode.A)) {
				NotificationCenter.DefaultCenter.PostNotification(this, "SpawnRandom");
				NotificationCenter.DefaultCenter.PostNotification(this, "RandomSpawn");
			}

			if (Input.GetKeyDown(KeyCode.Escape)) {
				Debug.LogError("Exiting App");
				Application.Quit(); 
			}
			
			UpdateUI();
		}
		
		public void GameStateGlobalFixedUpdate()
		{
			

		}
		
		public void GameStateGlobalExit()
		{
		}

		public void GameStateDisplaySpecialEnter()
		{
			var obj = MemoryPool.Instance.findAndGetObjs("SlowMoMessage",false);
			var pos = obj.transform.position;
			pos.x = 0;
			pos.y = 15;
			pos.z = 0;
			obj.transform.position = pos;
			obj.SetActive(true);
		
			StartCoroutine(DelaySequence(3,ChangeStateToPlayerTurn));
		}
		
		public void GameStateDisplaySpecialUpdate()
		{
			
		}
		
		public void GameStateDisplaySpecialExit()
		{

		}

		/// <summary>
		/// Games the state player one lost enter.
		/// </summary>
		public void GameStatePlayerOneLostEnter()
		{	
			savePlayerData();
			//Debug.LogError("Game over *******************");
			NotificationCenter.DefaultCenter.PostNotification(this,"GameOver");
			Hashtable dat = new Hashtable();
			dat.Add("state", "opengameover");
			NotificationCenter.DefaultCenter.PostNotification(this,"openPanel",dat);

			dat.Add("dat", "aww");

			NotificationCenter.DefaultCenter.PostNotification(this, "PlayAudioAtIndex" ,dat);

		}
		
		public void GameStatePlayerOneLostUpdate()
		{
			//NotificationCenter.DefaultCenter.PostNotification(this,"GameOver");
		}
		
		
		public void GameStatePauseEnter()
		{
			
		}
		
		public void GameStatePauseUpdate()
		{
			//Debug.LogError("Im in pause");
		}
		
		public void GameStatePauseExit()
		{
			
		}
		
		public void SceneStateIdleEnter()
		{
			
		}

		public void GameStateGameOverEnter()
		{
			savePlayerData();
		}

		public void GameStateGameOverUpdate()
		{
		}

		public void GameStateGameOverExit()
		{
		}

		public void GameStateUIAnimeEnter()
		{
			Hashtable d = new Hashtable();
			d.Add("dat","byebye");

			NotificationCenter.DefaultCenter.PostNotification(this,"PlayAudioAtIndex",d);

			Camera.main.GetComponent<CameraControls>().PlayAnime("zoomOut");
		}

		public void GameStateUIAnimeUpdate()
		{

		}

		public void GameStateUIAnimeExit()
		{
			Hashtable d = new Hashtable();
			d.Add("dat","awesome");

			NotificationCenter.DefaultCenter.PostNotification(this,"PlayAudioAtIndex",d);
			NotificationCenter.DefaultCenter.PostNotification(this, "NextSong");
			NotificationCenter.DefaultCenter.PostNotification(this,"increaseSpawnChance");
		}

		#endregion
	

		//All of the notfications we want to listen for. We have to set it on every level load
		void SetNotifications()
		{
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateIdleEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateIdleUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateIdleExit");
			NotificationCenter.DefaultCenter.AddObserver(this, "OnMenuStateEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateSetUpEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateSetUpUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateCompTurnEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateCompTurnUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateCompTurnExit");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayersTurnUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayersTurnEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayersTurnExit");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateGlobalUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateGlobalFixedUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateEndTurnEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateEndTurnUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateEndTurnExit");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerOneLostEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerOneLostUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePlayerOneLostExit");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePauseEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePauseUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStatePauseExit");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateDisplaySpecialEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateDisplaySpecialUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateDisplaySpecialExit");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateGameOverEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateGameOverUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateGameOverExit");
			NotificationCenter.DefaultCenter.AddObserver(this, "OnGameStateEventEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "OnGameStateEventUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "OnGameStateEventExit");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateUIAnimeEnter");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateUIAnimeUpdate");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateUIAnimeExit");

		}

		/// <summary>
		/// Sets the difficulty.
		/// This method is called via the UI delegates.
		/// </summary>
		/// <param name="p_diff">P_diff.</param>
		public override void SetDifficulty(int p_diff)
		{
			m_addSeqAtRound = (p_diff <= 0) ? 1 : p_diff;
		}

		public void RunHitDetections()
		{
			if (Input.GetButtonDown("Fire1")) {
				
				//Convert 3d to 2d space then cast the ray detect the hit
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				
				if (Physics.Raycast(ray, out hit,100,~(1 << LayerMask.NameToLayer ("Spawner")))) {
					Hashtable messDat = new Hashtable();
					var tile = hit.collider.GetComponent<Tile>();
					
					if(tile) {
						
						if(tile.Index == m_sequence[m_sequenceCounter]) {

							//Set the score
							//CurrentScore += tile.Score * m_scoreMultiplier;
							tile.Pressed = true;
							m_sequencePressed[m_sequenceCounter] = true;
							tile.PlayAnimation(TileAnimations.RightTouch);
							tile.PlayAudioClip(0);
							tile.PlayAParticle(ParticleType.RightTouch);

							
							switch(tile.Special) {
								
							case TileSpecial.None:
								
								m_sequenceCounter++;
								
								break;
							
								
							case TileSpecial.Repeatable:

								if(m_sequenceCounter < m_sequence.Count-1) {
									//offset by one
									m_sequenceCounter++;
								}
								
								//Add to the score
								//CurrentScore += tile.Score;
								
								break;
								
							case TileSpecial.Bomb:

								m_sequenceCounter++;
								
								tile.IsPrimed = true;
								
								break;
								
							case TileSpecial.UFO:


								if(m_scoreMultiplier == 1) {
									m_scoreMultiplier = 2;
								} else {
									m_scoreMultiplier += 2;
								}

								//CurrentScore += tile.Score;
								messDat.Clear();
								messDat.Add("dat","ScoreMultiMess");
								NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",messDat);

								break;
								
							case TileSpecial.ExtraLife:

								m_sequenceCounter++;
								
								break;

								
							case TileSpecial.Multiplier:
								
								m_sequenceCounter++;
								
								if(m_scoreMultiplier == 1) {
									m_scoreMultiplier = 2;
								} else {
									m_scoreMultiplier += 2;
								}
								
								break;
								
							case TileSpecial.Bonus:

								//CurrentScore += tile.Score *2;
								m_sequenceCounter++;
								
								break;
								
							case TileSpecial.SlowMo:

								MyTimeMode.SetMode(CurrentTimeMode.SlowMo);
								m_sequenceCounter++;
								messDat.Clear();
								messDat.Add("dat","SlowMoMess");
								NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",messDat);

								break;
							}
							
						} else {

							//Out of sequence
							switch(tile.Special) {

							case TileSpecial.None:
								NotificationCenter.DefaultCenter.PostNotification(this,"ShakeCamera");
								tile.PlayAnimation(TileAnimations.WrongTouch);
								tile.PlayAudioClip(1);
								tile.PlayAParticle(ParticleType.WrongTouch);
								//get rid of a second
								m_playerTurnClock.Counter -= 1.0f;
								//Player loses his/her score 

								if(m_scoreMultiplier > 1) {
									m_scoreMultiplier = 1;
									messDat.Clear();
									messDat.Add("dat","ScoreZeroMess");
									NotificationCenter.DefaultCenter.PostNotification(this,"SpawnAnItem",messDat);
								}

								break;
								
							case TileSpecial.Repeatable:
								
								//check to see if it is in the sequence
								if(m_inSeqBank.ContainsKey(tile.Index)) {
									//Debug.LogError("Again");
									tile.PlayAnimation(TileAnimations.RightTouch);
									tile.PlayAParticle(ParticleType.RightTouch);
									tile.PlayAudioClip(0);
									//CurrentScore += tile.Score;

									
								} else {
									NotificationCenter.DefaultCenter.PostNotification(this,"ShakeCamera");
									tile.PlayAnimation(TileAnimations.WrongTouch);
									tile.PlayAudioClip(1);
									m_playerTurnClock.Counter -= 1.0f;
									//Player loses his/her score 
									

									if(m_scoreMultiplier >1) {
										m_scoreMultiplier = 1;
										messDat.Clear();
										messDat.Add("dat","ScoreZeroMess");
										NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",messDat);
									}
								}
								
								break;
								
							case TileSpecial.Bomb:
								NotificationCenter.DefaultCenter.PostNotification(this,"ShakeCamera");
								tile.PlayAnimation(TileAnimations.WrongTouch);
								tile.PlayAudioClip(1);
								tile.IsPrimed = false;
								//Player loses his/her score 
								
								m_playerTurnClock.Counter -= 1.0f;

								if(m_scoreMultiplier > 1) {
									m_scoreMultiplier = 1;
									messDat.Clear();
									messDat.Add("dat","ScoreZeroMess");
									NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",messDat);
								}
							
								break;
								
							case TileSpecial.Bonus:

								//Player loses his/her score 
								NotificationCenter.DefaultCenter.PostNotification(this,"ShakeCamera");
								NotificationCenter.DefaultCenter.PostNotification(this,"SpawnRandom");
								Hashtable d = new Hashtable();
								d.Add("dat","Reinforcement");
								NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",d);
								m_playerTurnClock.Counter -= 1.0f;
								tile.PlayAnimation(TileAnimations.WrongTouch);
								tile.PlayAParticle(ParticleType.WrongTouch);
								tile.PlayAudioClip(1);

								if(m_scoreMultiplier > 1) {
									m_scoreMultiplier = 1;
									messDat.Clear();
									messDat.Add("dat","ScoreZeroMess");
									NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",messDat);
								}

								break;
								
							case TileSpecial.ExtraLife:
								NotificationCenter.DefaultCenter.PostNotification(this,"ShakeCamera");
								tile.PlayAnimation(TileAnimations.WrongTouch);
								tile.PlayAudioClip(1);
								//add a strike
								//Player loses his/her score 
								m_scoreMultiplier = 1;
								m_playerTurnClock.Counter -= 1.0f;

								if(m_scoreMultiplier > 1) {
									m_scoreMultiplier = 1;
									messDat.Clear();
									messDat.Add("dat","ScoreZeroMess");
									NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",messDat);
								}

								break;
								
							case TileSpecial.Multiplier:
								
								NotificationCenter.DefaultCenter.PostNotification(this,"ShakeCamera");
								tile.PlayAnimation(TileAnimations.WrongTouch);
								tile.PlayAudioClip(1);
								
								if(m_scoreMultiplier > 1 ) {
									m_scoreMultiplier = 1;
									messDat.Clear();
									messDat.Add("dat","ScoreZeroMess");
									NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",messDat);
								}
							
								break;
								
							case TileSpecial.UFO:

								tile.PlayAParticle(ParticleType.RightTouch);
								tile.PlayAudioClip(0);
								tile.PlayAnimation(TileAnimations.RightTouch);
								tile.gameObject.SetActive(false);
								if(m_scoreMultiplier == 1) {
									m_scoreMultiplier = 2;
								} else {
									m_scoreMultiplier += 2;
								}
								
								//CurrentScore += tile.Score;
								messDat.Clear();
								messDat.Add("dat","ScoreMultiMess");
								NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",messDat);
								
								break;
								
							case TileSpecial.SlowMo:
								NotificationCenter.DefaultCenter.PostNotification(this,"ShakeCamera");
								tile.PlayAnimation(TileAnimations.WrongTouch);
								tile.PlayAudioClip(1);
								
								MyTimeMode.SetMode(CurrentTimeMode.Fast);

								m_scoreMultiplier = 1;
								messDat.Clear();
								messDat.Add("dat","SugarRushMess");
								NotificationCenter.DefaultCenter.PostNotification(this,"SpawnSpecialItem",messDat);
								
								break;
								
							}
						
						}

					}
					if(tile) {
						SendEvent(tile.Special.ToString());
					}
				}
			}
		}

		public void ClearWinTiles()
		{
			foreach(int i in m_sequence) {
				//make sure the spawner it is in is cleared
				//m_liveObjects[i].SetActive(false);
				m_liveObjects[i].GetComponent<Tile>().PlayAnimation(TileAnimations.Success);
				m_liveObjects[i].transform.Translate(new Vector3(100000,100000,10000));
			}
		}

		public void ChangeStateToPlayerTurn()
		{
			GameFSM.ChangeState(GameStatePlayersTurn.Instance);
			return;
		}

		/// <summary>
		/// Checks for sequence complete.
		/// </summary>
		/// <returns><c>true</c>, if for sequence complete was checked, <c>false</c> otherwise.</returns>
		public bool CheckForSequenceComplete()
		{
			var result = true;

			for(int index = 0; index < m_sequence.Count; index++) {

				if(m_sequencePressed[index] != true) {
					//Debug.LogError("Returning false");
					return false;
				}
			}
			return result;
		}

		public void RunWinTiles()
		{
			foreach(GameObject t in m_liveObjects) {

				var tile = t.GetComponent<Tile>();
				if(m_inSeqBank.ContainsKey(tile.Index)) {

					// Add to score and any multiplier
					CurrentScore += (tile.Score * m_scoreMultiplier);

					switch(tile.Special) {

					case TileSpecial.None:

						tile.PlayAnimation(TileAnimations.RightTouch);
						tile.PlayAudioClip(0);

						break;

					case TileSpecial.Bomb:

						tile.PlayAnimation(TileAnimations.Success);
						tile.PlayAudioClip(3);
						tile.PlayAParticle(ParticleType.Success);
						tile.BlowUp();

						break;

					case TileSpecial.Bonus:

						CurrentScore += tile.Score;
						tile.PlayAnimation(TileAnimations.Success);
						tile.PlayAudioClip(0);

						break;

					case TileSpecial.ExtraLife:
						m_lives++;
						tile.PlayAnimation(TileAnimations.RightTouch);
						tile.PlayAudioClip(0);

						break;

					case TileSpecial.Multiplier:

						tile.PlayAnimation(TileAnimations.RightTouch);
						tile.PlayAudioClip(0);

						break;

					case TileSpecial.SlowMo:

						//GameFSM.ChangeState(GameStateDisplaySpecial.Instance);
						tile.PlayAnimation(TileAnimations.RightTouch);
						tile.PlayAudioClip(0);

						break;

					default:

						tile.PlayAnimation(TileAnimations.RightTouch);
						tile.PlayAudioClip(0);

						break;
					}
				}
			}
		}

		public void RunLoseTiles()
		{
			for(int index = 0; index < m_liveObjects.Count; ) {

				var t = m_liveObjects[index].GetComponent<Tile>();
				if(t.IsAnimeDone && t.IsAnimeRunning == false) {

					t.PlayAnimation(TileAnimations.Failed);
					t.IsAnimeRunning = true;
					 
				} else if(t.IsAnimeDone == true && t.IsAnimeRunning == true) {

					index++;
				}
			}
		}

		/// <summary>
		/// Clears the losing tiles.
		/// If they been pressed and have a negative effect
		/// </summary>
		public void ClearLosingTiles()
		{
			foreach(GameObject obj in m_liveObjects) {

				var tile = obj.GetComponent<Tile>();
				if(tile) {

					if(tile.Pressed == true) {
					
					}
				}
			}
		}

		public bool IsTileInSequence(Tile p_tile)
		{
			var result = false;
			foreach(GameObject obj in m_liveObjects) {

				var t = obj.GetComponent<Tile>();
				if(t == p_tile) {
					return true;
				}
			}
			return result;
		}

		public void SceneStateIdleUpdate()
		{
			if(SceneManager.GetActiveScene().name == "Splash") {
				SceneFSM.ChangeState(SceneStateSplash.Instance);
			}
		}

		public void SceneStateIdleFixedUpdate()
		{

		}

		public void SceneStateIdleExit()
		{

		}

		public void OnSplashEnter()
		{
			//CheckLevelVsState("Splash", SceneStateSplash.Instance);
			AudioController.Instance.playAtIndex(0);
			StartCoroutine(DelaySequence(m_splashSceneDelay,ChangeToMenuState));
			//StartCoroutine(DelaySequence(3, GainControl));

		}

		public void OnSplashUpdate()
		{
#if UNITY_STANDALONE
			if(Input.anyKeyDown) {

				if(this.HasControl == true) {

				SceneManager.LoadScene ("menu");
				}

			}
#endif
		}

		public void OnSplashFixedUpdate()
		{

		}

		public void OnSplashExit()
		{

		}

		public void OnMenuStateEnter()
		{
			//Debug.LogError("*****");

		}

		public void OnMenuStateUpdate()
		{

		}

		public void OnMenuStateFixedUpdate()
		{

		}

		public void OnMenuStateExit()
		{

		}

		public void OnGameStateEventEnter()
		{
			//Debug.LogError("On event enter");
		}

		public void OnGameStateEventUpdate()
		{

		}

		public void OnGameStateEventExit()
		{
			//Debug.LogError("On event exit");
		}


		#region HELPERMETHODS

		void OnLevelWasLoaded(int level)
		{
			this.SetNotifications();

            isBeatHighScore = false;
			m_currentNumOfSequences = 3;

			m_isCurrentRoundLost = false;
			CurrentWave = 1;
			m_lives = 3;
			CurrentScore = 0;
			m_playerTurnClock.ResetClock();

			//set the GameMaster states
			switch(SceneManager.GetActiveScene().name) {
				
			case "Splash":

				SceneFSM.ChangeState(SceneStateSplash.Instance);

				if(GameFSM.CurrentState != GameStateIdle.Instance) {
					GameFSM.ChangeState(GameStateIdle.Instance);
					return;
				}

				break;
				
			case "Menu":

				SceneFSM.ChangeState(SceneStateMenu.Instance);

				if(GameFSM.CurrentState != GameStateIdle.Instance) {
					GameFSM.ChangeState(GameStateIdle.Instance);
					return;
				}
				break;
				
			case "Game":

				//reset all the clocks
				ResetAllClocks();
				GameFSM.ChangeState(GameStateIdle.Instance);

				if(GameFSM.CurrentState != GameStateIdle.Instance) {
					GameFSM.ChangeState(GameStateIdle.Instance);
					return;
				}
				break;
				
			case "GameOver":

				//Debug.LogError(" I am starting the arena state");
				SceneFSM.ChangeState(SceneStateArena.Instance);

				if(GameFSM.CurrentState != GameStateIdle.Instance) {
					GameFSM.ChangeState(GameStateIdle.Instance);
					return;
				}
				//StartCoroutine(StartTheGame(m_arenaTimeToStart));
				break;
				
			case "Credits":
				SceneFSM.ChangeState(SceneStateCredits.Instance);
				GameFSM.ChangeState(GameStateIdle.Instance);
				break;
			}
		}

		//This function makes Sure we are in the right level
		public void CheckLevelVsState(string p_level, FSMState<BaseGameController> p_state)
		{
			//Debug.LogError(p_level);
			if(SceneManager.GetActiveScene().name != p_level) {
				SceneFSM.ChangeState(p_state);
			}
		}

		public void ChangeToMenuState()
		{
			//SceneFSM.ChangeState(SceneStateMenu.Instance);
			//Application.LoadLevel("Menu");
			NotificationCenter.DefaultCenter.PostNotification(this, "ChangingScene");
			NotificationCenter.DefaultCenter.PostNotification(this, "PlayCurrentSound");
			savePlayerData();
		}

		public void ChangeStateToSetup()
		{
			GameFSM.ChangeState(GameStateSetUp.Instance);
			return;
		}

		//Method that contains a delegate
		IEnumerator DelaySequence(float p_sec, MultiDelegate  p_method)
		{
			yield return new WaitForSeconds(p_sec);
			p_method();
		}

		IEnumerator StartTheGame(float p_sec)
		{
			//Debug.LogError("Co rountine");
			yield return new WaitForSeconds(p_sec);
			//Debug.LogError("I done");
			GameFSM.ChangeState(GameStateSetUp.Instance);
		}

		IEnumerator DelayCompTurn(float p_sec)
		{
			yield return new WaitForSeconds(p_sec);
			GameFSM.ChangeState(GameStateCompTurn.Instance);
		}

		IEnumerator DelaySetUp(float p_sec)
		{
			yield return new WaitForSeconds(p_sec);
			GameFSM.ChangeState(GameStateSetUp.Instance);
		}

		public void RunRandomUfo()
		{
			var r = UnityEngine.Random.Range(0,100);
			var c = UnityEngine.Random.Range(0,100);
			if(r >= c) {
				NotificationCenter.DefaultCenter.PostNotification(this, "RandomSpawn");
			}
		}

		/// <summary>
		/// Resets all clocks.
		/// </summary>
		public void ResetAllClocks() 
		{
			m_startGameClock.ResetClock();
			m_setUpClock.ResetClock();
			m_sequenceClock.ResetClock();
			m_playerTurnClock.ResetClock();
			m_endTurnClock.ResetClock();
		}

		/// <summary>
		/// Updates the UI
		/// </summary>
		public void UpdateUI()
		{
			Hashtable dat = new Hashtable();
			dat.Add("Score", CurrentScore.ToString("0000000"));
			dat.Add("HighScore",HighScore.ToString("0000000"));
			dat.Add("Sequence",m_currentNumOfSequences.ToString("0000000"));
			dat.Add("Multi", m_scoreMultiplier.ToString(m_StringSpecifier));
			dat.Add("Round", CurrentWave.ToString(m_StringSpecifier));
			dat.Add("Lives", m_lives.ToString("00"));
			dat.Add("LivesInt", m_lives);
			dat.Add("MaxLives", m_maxLives);
			dat.Add("Speed", Time.timeScale.ToString());
			var t = Mathf.Clamp(m_playerTurnClock.Counter,0,m_playerTurnClock.TimeLimit);
			dat.Add ("Time",t.ToString(m_timerStringSpecifier));
			dat.Add ("Time2",m_playerTurnClock.Counter);
			dat.Add("MaxTime", m_playerTurnClock.TimeLimit);
			NotificationCenter.DefaultCenter.PostNotification(this, "UpdateText", dat);
			NotificationCenter.DefaultCenter.PostNotification(this, "SetSlider",dat);
		}

		/// <summary>
		/// Finds the live tiles. and store the tiles in the liveobject list
		/// </summary>
		public void FindLiveTiles()
		{
			//Get all of the Live tiles
			var tiles = GameObject.FindGameObjectsWithTag("Tile");
			var i = 0;
			//go through each one finding the live ones
			foreach(GameObject tile in tiles) {
				if(tile.activeSelf == true) {
					if(tile.GetComponent<Tile>() == true && tile.GetComponent<Tile>().Special != TileSpecial.UFO) {
						m_liveObjects.Add(tile);
						tile.GetComponent<Tile>().Init(i,false);
						i++;
					}
				}
			}
		}

		public void SetSequence()
		{
			//For every sequence get a random sequence
			for(int index = 0; index < m_currentNumOfSequences; ++index) {
				//Lets get a random object index
				var r = UnityEngine.Random.Range(0,m_liveObjects.Count);
				//Now save it
				m_sequence.Add(r);
				m_sequencePressed.Add(false);
				//add it to the sequence bank
				if(!m_inSeqBank.ContainsKey(r)) {
					m_inSeqBank.Add(r,r);
				}
			}
		}

		public void SendEvent(string p_name)
		{
			Hashtable dat = new Hashtable();
			dat.Add("event", p_name);
			NotificationCenter.DefaultCenter.PostNotification(this, "PlayEvent", dat);
		}

		public void savePlayerData()
		{
			SaveFileCreated = 1;
			PlayerPrefs.SetInt ("HighScore", HighScore);
			PlayerPrefs.SetInt ("LongestStreak", m_longestStreak);
			PlayerPrefs.SetInt ("CurrentScore", CurrentScore);
			PlayerPrefs.SetInt ("SaveFileCreated", SaveFileCreated);
		}

		public void ResetScore()
		{
			CurrentScore = 0;
			HighScore = 0;
			m_longestStreak = 0;
			SaveFileCreated = 0;

			savePlayerData();
		}

		public void loadPlayerData()
		{
			SaveFileCreated = PlayerPrefs.GetInt ("SaveFileCreated");
			if (SaveFileCreated > 0) 
			{
				HighScore = PlayerPrefs.GetInt ("HighScore");
				m_longestStreak = PlayerPrefs.GetInt ("LongestStreak");
				CurrentScore = PlayerPrefs.GetInt ("CurrentScore");
			} 
			else
				ResetScore ();
		}

		#endregion
	}
}