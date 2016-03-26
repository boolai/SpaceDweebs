/// <summary>
/// Tile. The play piece on the game
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoogieDownGames {

	/// <summary>
	/// TileAnimations
	/// Enum for playing the tile animations
	/// RightTouch = when a player hits the right tile in the right sequence
	/// WrongTouch = When a player hits the wrong tile in sequence
	/// Failed = when the Sequences fails
	/// Success = when the sequences succeeds
	/// </summary>
	public enum TileAnimations { Idle, RightTouch, WrongTouch, Failed, Success };
	public enum TileSpecial { None, Repeatable, Bomb, Bonus, UFO, Multiplier, SlowMo, ExtraLife };
	public enum ParticleType { RightTouch, WrongTouch, Success, Failed, LoseGame, WinGame };
	
	public class Tile : MonoBehaviour {

		[SerializeField]
		private int m_index;

		[SerializeField]
		private bool m_pressed;

		[SerializeField]
		private List<AudioClip> m_audioClips;

		private Dictionary<string,int> m_audioIndex = new Dictionary<string, int>();

		[SerializeField]
		private Animator m_anime;

		[SerializeField]
		private AudioSource m_audioSource;

		[SerializeField]
		private TileSpecial m_special;

		[SerializeField]
		private int m_score;

		[SerializeField]
		private bool m_isPrimed;

		[SerializeField]
		private TimeKeeper m_bombClock;

		[SerializeField]
		private bool m_isAnimeDone;

		[SerializeField]
		private List<ParticleAttributes> m_particleList;

		private Dictionary<string, GameObject> m_particlesBank = new Dictionary<string, GameObject>();

		[SerializeField]
		private bool m_isAnimeRunning;

		public bool IsAnimeRunning
		{
			get { return m_isAnimeRunning; }
			set { m_isAnimeRunning = value; }
		}

		public int Index
		{
			get { return m_index; }
			set { m_index = value; }
		}

		public TileSpecial Special
		{
			get { return m_special; }
		}

		public bool Pressed
		{
			get { return m_pressed; }
			set { m_pressed = value; }
		}

		public int Score
		{
			get { return m_score; }
		}

		public bool IsPrimed
		{
			get { return m_isPrimed; }
			set { m_isPrimed = value; }
		}

		public bool IsAnimeDone
		{
			get { return m_isAnimeDone; }
			set { m_isAnimeDone = value; }
		}
			
		[SerializeField]
		private Rigidbody m_rBody;

		void Start()
		{
			NotificationCenter.DefaultCenter.AddObserver(this, "PlayAnimationMessage");
			NotificationCenter.DefaultCenter.AddObserver(this, "DestroyUfo");
			NotificationCenter.DefaultCenter.AddObserver(this, "GameStateGlobalUpdate");
			//NotificationCenter.DefaultCenter.AddObserver(this, "PlayAudioAtIndex");

			foreach(ParticleAttributes p in m_particleList) {
				if(!m_particlesBank.ContainsKey(p.TypeParticle.ToString())) {
					m_particlesBank.Add(p.TypeParticle.ToString(),p.MyParticle);
				}
			}

			m_rBody = GetComponent<Rigidbody>();

			for(int index = 0; index < m_audioClips.Count; index++) {

				if(!m_audioIndex.ContainsKey(m_audioClips[index].name)) {
					m_audioIndex.Add(m_audioClips[index].name, index);
				}
			}
		}

		/// <summary>
		/// Init this instance.
		/// </summary>
		public void Init(int p_index, bool p_pressed)
		{
			m_pressed = p_pressed;
			m_index = p_index;
			m_bombClock.ResetClock();
			m_isAnimeDone = false;
			if( gameObject.GetComponent<SimpleMove>() ) {
				gameObject.GetComponent<SimpleMove>().SetSlowMode();
			}
		}

		void GameStateGlobalUpdate()
		{
			/*
			try {
				m_anime.SetFloat("ySpeed",m_rBody.velocity.y);
			} catch(UnassignedReferenceException p_err) {
				Debug.LogWarning(p_err.Message + " On " + gameObject.name); 
			}
			*/
		}

		/// <summary>
		/// Plays the animation.
		/// </summary>
		/// <param name="p_anime">P_anime.</param>
		public void PlayAnimation(TileAnimations p_anime)
		{
			if(m_anime != null) {
				m_anime.SetTrigger(p_anime.ToString());
			}
		}

		/// <summary>
		/// Plays the audio clip.
		/// </summary>
		/// <param name="p_index">P_index.</param>
		public void PlayAudioClip(int p_index)
		{
			if(m_audioSource != null) {
				m_audioSource.clip = m_audioClips[p_index];
				m_audioSource.Play();
			}
		}

		public void PlayAudioAtIndex(NotificationCenter.Notification p_note)
		{
			string name = (string)p_note.data["dat"];
			Debug.LogError("Playing " + name);
			if(m_audioIndex.ContainsKey(name)) {
				StartCoroutine(delaySound(UnityEngine.Random.Range(0.0f, 0.5f),m_audioIndex[name]));
			}
		}

		public void PlayAnimationMessage(NotificationCenter.Notification p_note)
		{
			string trigger = (string)p_note.data["dat"];
			m_anime.SetBool(trigger,true);
		}

		/// <summary>
		/// Blows up. The bomb it will cast a rays 360 degrees all around 
		/// when it does it will destroy all of the other tiles around it
		/// If one of the tiles is in the sequence the player would lost that 
		/// chance to get the tile.
		/// </summary>
		public void BlowUp()
		{
			ExplosionDamage(transform.position,5);
			gameObject.SetActive(false);
		}

		void ExplosionDamage(Vector3 center, float radius) 
		{
			Collider[] hitColliders = Physics.OverlapSphere(center, radius);
			int i = 0;
			while (i < hitColliders.Length) {

				Tile tile = hitColliders[i].GetComponent<Tile>();

				if(tile) {

					tile.PlayAnimation(TileAnimations.RightTouch);
					tile.PlayAudioClip(0);
					tile.PlayAParticle(ParticleType.RightTouch);
					tile.gameObject.SetActive(false);
				}
				i++;
			}
			m_pressed = false;
		}

		/// <summary>
		/// Plays A particle.
		/// </summary>
		/// <param name="p_type">P_type.</param>
		public void PlayAParticle(ParticleType p_type)
		{
			//See if the bank has the particle
			if( m_particlesBank.ContainsKey(p_type.ToString())) {

				var pe = MemoryPool.Instance.findAndGetObjs(m_particlesBank[p_type.ToString()].name,false);
				if(pe != null) {
					var pos = transform.position;
					pos.z -= 2.0f;
					pe.transform.position = pos;
					pe.GetComponent<ParticleSystem>().Play();
				}
			}
		}
		
		public void DestroyUfo()
		{
			if(m_special == TileSpecial.UFO) {
				//Debug.LogError("Beign destroyed");
				PlayAParticle(ParticleType.RightTouch);
				m_anime.SetTrigger("Success");
				PlayAudioClip(1);
				gameObject.SetActive(false);
			}
		}

		public void RunEndOfSequence()
		{
			m_anime.SetTrigger("Success");
		}

		public void Sleep()
		{
			gameObject.SetActive(false);
		}

		IEnumerator delaySound(float p_delay, int p_index)
		{
			yield return new WaitForSeconds(p_delay);
			var r = UnityEngine.Random.Range(0,100);
			if(r < 30) {
				PlayAudioClip(p_index);
			}
		}

	}
}