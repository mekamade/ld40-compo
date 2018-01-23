using UnityEngine.Audio;
using UnityEngine;
using System;

public class SFXAudioManager : MonoBehaviour {

	public Sound[] sounds;
	public AudioLowPassFilter effects;

	// Use this for initialization
	void Awake () {
		foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource> ();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.loop = s.loop;
		}
		effects = gameObject.AddComponent<AudioLowPassFilter> ();
	}
	
	public void playSound(string name){
		Sound s = Array.Find (sounds, sound => sound.name == name);
		s.source.Play ();
	}

	public void pauseSound(string name){
		Sound s = Array.Find (sounds, sound => sound.name == name);
		s.source.Pause ();
	}

	public void unpauseSound(string name){
		Sound s = Array.Find (sounds, sound => sound.name == name);
		s.source.UnPause ();
	}


	public void updateEffects(float cutoff){
		effects.cutoffFrequency = cutoff;
	}
}
