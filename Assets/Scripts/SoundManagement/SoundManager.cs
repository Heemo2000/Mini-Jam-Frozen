using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using Game.Core;

using Random = UnityEngine.Random;

namespace Game.SoundManagement
{
    public class SoundManager : GenericSingleton<SoundManager>
    {
        
        [SerializeField]private AudioSource musicAudioSource;
        [SerializeField]private AudioSource sfxAudioSource;
        [SerializeField]private Sound[] sounds;

        [SerializeField]private SoundType startingMusic;
        [SerializeField]private AudioMixer musicAudioMixer;
        [SerializeField]private AudioMixer sfxAudioMixer;
        private Dictionary<SoundType,Sound> soundDictionary;

        public void PlayMusic(SoundType soundType)
        {
            Play(soundType,musicAudioSource);
        }

        public void PlayRandomSFX(List<SoundType> soundTypes)
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            PlaySFX(soundTypes[Random.Range(0, soundTypes.Count)]);
        }
        public void PlaySFX(SoundType soundType)
        {
            Play(soundType,sfxAudioSource);
        }

        
        public void PlaySFXInstantly(SoundType soundType)
        {
            Play(soundType,sfxAudioSource,true);
        }

        public void PauseMusic()
        {
            musicAudioSource.Pause();
        }

        public void ResumeMusic()
        {
            musicAudioSource.Play();
        }

        public void StopMusic()
        {
            musicAudioSource.Stop();
        }


        public void ResumeSFX()
        {
            sfxAudioSource.Play();
        }

        public void PauseSFX()
        {
            sfxAudioSource.Pause();
        }

        public void StopSFX()
        {
            sfxAudioSource.Stop();
        }

        public void SetMusicVolume(float volume)
        {
            musicAudioMixer.SetFloat("musicVol",volume);
        }

        public void SetSFXVolume(float volume)
        {
            sfxAudioMixer.SetFloat("sfxVol",volume);
        }

        protected override void Awake() {
            
            base.Awake();
            //DontDestroyOnLoad(gameObject);
            soundDictionary = new Dictionary<SoundType, Sound>();        
            foreach(Sound sound in sounds)
            {
                Debug.Log("sound type: " + sound.soundType);
                soundDictionary.Add(sound.soundType,sound);
            }
        }

        private void Play(SoundType soundType,AudioSource source,bool oneShot = false)
        {
            if(soundDictionary.TryGetValue(soundType,out Sound sound))
            {
                source.clip = sound.clip;
                source.pitch = sound.pitch;
                source.loop = sound.loop;
                source.volume = sound.volume;
                if(oneShot)
                {
                    source.PlayOneShot(sound.clip);
                }
                else
                {
                    source.Play();
                }
                
            }
            else
            {
                Debug.LogError("Sound to play not found: " + soundType.ToString());
            }        
        }


        private void Start() 
        {
            PlayMusic(startingMusic);
        }

    }

}
