using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoogieDownGames {

	[RequireComponent (typeof (AudioSource))]
	public class SoundController : UnitySingleton<SoundController> {

		[SerializeField]
		private List<AudioClip> m_soundClips;

		Dictionary<string, int> m_audioIndex = new Dictionary<string, int>();

		[SerializeField]
		private int m_initialSound; //Index of the initial sound

		[SerializeField]
		private int m_currentIndex;

		public bool DetectEndOfSound()
		{
			if(!GetComponent<AudioSource>().isPlaying) {
				return true;
			} else {
				return false;
			}
		}

		void Start()
		{
			//playAtIndex(m_initialSound);
			NotificationCenter.DefaultCenter.AddObserver(this, "PlayCurrentSound");
			NotificationCenter.DefaultCenter.AddObserver(this, "PlayAudioAtIndex");
			for(int index = 0; index < m_soundClips.Count; index++) {

				if(!m_audioIndex.ContainsKey(m_soundClips[index].name)) {
					m_audioIndex.Add(m_soundClips[index].name, index);
				}
			}
		}

		public void PlayCurrentSound()
		{
			GetComponent<AudioSource>().clip = m_soundClips[m_currentIndex];
			GetComponent<AudioSource>().Play();
		}

		public void SetVolume(float p_vol)
		{
			GetComponent<AudioSource>().volume = p_vol;
		}

		public void PlayAudioAtIndex(NotificationCenter.Notification p_note)
		{
			string name = (string)p_note.data["dat"];
			//Debug.LogError("Playing " + name);
			if(m_audioIndex.ContainsKey(name)) {
				playAtIndex(m_audioIndex[name]);
			}
		}

		public void playAtIndex(int p_index)
		{
			if(p_index > m_soundClips.Count - 1) {
				p_index = m_soundClips.Count -1;
			}
			//Debug.Log("Playing a clip");
			m_currentIndex = p_index;
			GetComponent<AudioSource>().PlayOneShot(m_soundClips[p_index]);
			//PostMessage("SetText",m_soundClips[p_index].name);
			//GameMaster.Instance.CurrentSong = m_currentIndex;
		}

		public AudioClip GetClipAtIndex(int p_index)
		{
			if(p_index > m_soundClips.Count - 1) {
				p_index = m_soundClips.Count -1;
			}
			m_currentIndex = p_index;
			//PostMessage("SetText",m_soundClips[m_currentIndex].name);
			return m_soundClips[p_index];
		}

		public void NextSound()
		{
			m_currentIndex++;
			if(m_currentIndex > m_soundClips.Count -1) {
				m_currentIndex = 0;
			}
			//audio.clip = m_soundClips[m_currentIndex];
			GetComponent<AudioSource>().PlayOneShot(m_soundClips[m_currentIndex]);
			//PostMessage("SetText",m_soundClips[m_currentIndex].name);
		}

		public void PrevSound()
		{
			m_currentIndex--;
			if(m_currentIndex <= 0) {
				m_currentIndex = m_soundClips.Count -1;
			}
			//audio.clip = m_soundClips[m_currentIndex];
			GetComponent<AudioSource>().PlayOneShot(m_soundClips[m_currentIndex]);
			//PostMessage("SetText",m_soundClips[m_currentIndex].name);
			//GameMaster.Instance.CurrentSong = m_currentIndex;
		}

		public void StopSound()
		{
			GetComponent<AudioSource>().Stop();
			PostMessage("SetText",m_soundClips[m_currentIndex].name + " Stopped");
		}

		public void PauseSound()
		{
			GetComponent<AudioSource>().Pause();
		}

		public void UnPauseSound()
		{
			GetComponent<AudioSource>().Play();
		}

		public void PostMessage(string p_func, string p_message)
		{
			Hashtable dat = new Hashtable();
			dat.Add("msg",p_message);
			NotificationCenter.DefaultCenter.PostNotification(this,p_func,dat);
		}

	}
}