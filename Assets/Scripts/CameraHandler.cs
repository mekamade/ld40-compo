using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;



public class CameraHandler : MonoBehaviour {

	public Camera effectsCamera;
	public GameObject AudioManager;

	public void turnOffBlur() {
		effectsCamera.GetComponent<DepthOfField> ().aperture = 0;
	}

	public void playWasted() {
		AudioManager.GetComponent<SFXAudioManager> ().playSound ("Wasted");
	}

}
