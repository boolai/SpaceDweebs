using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

namespace BoogieDownGames {

	public class AudioMixerController : MonoBehaviour {

		[SerializeField]
		private AudioMixer m_masterMixer;

		[SerializeField]
		private Slider m_master;

		[SerializeField]
		private Slider m_sound;

		[SerializeField]
		private Slider m_music;

		void Awake()
		{
			/*
			var temp = 0.0f;

			m_masterMixer.GetFloat("MasterVolume",out temp);

			m_masterMixer.GetFloat("MusicVolume", out temp);

			m_masterMixer.GetFloat("SoundVolume", out temp);
		*/
		}

		void Start()
		{
			var temp = 0.0f;

			m_masterMixer.GetFloat("MasterVolume",out temp);
			m_master.value = temp;

			m_masterMixer.GetFloat("MusicVolume", out temp);
			m_music.value = temp;

			m_masterMixer.GetFloat("SoundVolume", out temp);
			m_sound.value = temp;
		}

		public void setMasterVolume(float p_lvl)
		{
			m_masterMixer.SetFloat("MasterVolume", p_lvl);
		}
		
		public void setMusicVolume(float p_lvl)
		{
			m_masterMixer.SetFloat("MusicVolume", p_lvl);
		}
		
		public void setSoundVolume(float p_lvl)
		{
			m_masterMixer.SetFloat("SoundVolume", p_lvl);
		}


	}
}