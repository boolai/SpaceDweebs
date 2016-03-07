/// <summary>
/// Audio controller.
/// Designed to make it easy to add music to the game.
/// It is a singleton so you onley need one copy of it
/// Need to add a crossfade feature
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoogieDownGames {

	[RequireComponent(typeof(AudioSource))]
	public class AudioController : UnitySingleton<AudioController> {

		[SerializeField]
		private List<AudioClip> m_soundClips;
		
		[SerializeField]
		private int m_initialSound; //Index of the initial sound
		
		[SerializeField]
		private int m_currentIndex;

		[SerializeField]
		private int m_prevIndex;

		[SerializeField]
		private bool m_isPaused;

		[SerializeField]
		private AudioSource m_audioSource;

		#region PROPERTIES

		public List<AudioClip> AudioClips
		{
			get { return m_soundClips; }
		}

		public bool IsPaused
		{
			get { return m_isPaused; }
			set { m_isPaused = true; }
		}


		#endregion

		void Awake()
		{
			//We going to make sure that the list has something in it first
			if( m_soundClips.Count > 0 ) {
				GetComponent<AudioSource>().clip = m_soundClips[m_initialSound];
				m_currentIndex = m_initialSound;
				m_prevIndex = m_currentIndex;
			}

			m_audioSource = GetComponent<AudioSource>();
		}
		
		public bool DetectEndOfSong()
		{
			if(!m_audioSource.isPlaying) {
				return true;
			} else {
				return false;
			}
		}
		
		void Start()
		{
			NotificationCenter.DefaultCenter.AddObserver(this, "PlayCurrentSong");
			NotificationCenter.DefaultCenter.AddObserver(this, "PauseSong");
			NotificationCenter.DefaultCenter.AddObserver(this, "NextSong");



			//playAtIndex(m_initialSound);
			PlayRandomSong();
		}
		
		public void SetVolume(float p_vol)
		{
			GetComponent<AudioSource>().volume = p_vol;
		}
		
		public void playAtIndex(int p_index)
		{
			try{ 
				if(p_index > m_soundClips.Count - 1) {
					p_index = m_soundClips.Count -1;
				}
				////Debug.LogError("Playing a song");
				m_currentIndex = p_index;
				m_audioSource.clip = m_soundClips[p_index];
				m_audioSource.Play();
				PostMessage("SetText",m_soundClips[p_index].name);
				//GameMaster.Instance.CurrentSong = m_currentIndex;
			} catch (ArgumentOutOfRangeException e ) {
				Debug.LogError(e.Message);
			}
		}

		public void PlayCurrentSong()
		{
			m_audioSource.clip = m_soundClips[m_currentIndex];
			m_audioSource.Play();
			m_isPaused = false;
		}
		
		public AudioClip GetClipAtIndex(int p_index)
		{
			if(p_index > m_soundClips.Count - 1) {
				p_index = m_soundClips.Count -1;
			}
			m_currentIndex = p_index;
			PostMessage("SetText",m_soundClips[m_currentIndex].name);
			return m_soundClips[p_index];
		}
		
		public void NextSong()
		{
			m_currentIndex++;
			if(m_currentIndex > m_soundClips.Count -1) {
				m_currentIndex = 0;
			}
			m_audioSource.clip = m_soundClips[m_currentIndex];
			m_audioSource.Play();
			PostMessage("SetText",m_soundClips[m_currentIndex].name);
			//GameMaster.Instance.CurrentSong = m_currentIndex;
		}
		
		public void PrevSong()
		{
			m_currentIndex--;
			if(m_currentIndex < 0) {
				m_currentIndex = m_soundClips.Count -1;
			}
			m_audioSource.clip = m_soundClips[m_currentIndex];
			m_audioSource.Play();
			PostMessage("SetText",m_soundClips[m_currentIndex].name);
			//GameMaster.Instance.CurrentSong = m_currentIndex;
		}

		public void LastPlayedSong()
		{
			//Get the last song played
			m_audioSource.clip = m_soundClips[m_prevIndex];
			m_currentIndex = m_prevIndex;
			m_audioSource.Play();
		}
		
		public void StopSong()
		{
			m_audioSource.Stop();
			PostMessage("SetText",m_soundClips[m_currentIndex].name + " Stopped");
		}
		
		public void PauseSong()
		{
			m_audioSource.Pause();
			m_isPaused = true;
		}
		
		public void UnPauseSong()
		{
			GetComponent<AudioSource>().Play();
			m_isPaused = false;
		}
		
		public void PostMessage(string p_func, string p_message)
		{
			Hashtable dat = new Hashtable();
			dat.Add("msg",p_message);
			NotificationCenter.DefaultCenter.PostNotification(this,p_func,dat);
		}

		public void PlayRandomSong()
		{
			var r = UnityEngine.Random.Range(0, m_soundClips.Count);
			m_prevIndex = m_currentIndex;
			m_currentIndex = r;

			playAtIndex(r);
		}
	}
}