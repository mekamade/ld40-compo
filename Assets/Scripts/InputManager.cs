using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {
	
	public static bool Up_button() {
		return Input.GetButtonDown ("Up_BTN");
	}

	public static bool Left_button() {
		return Input.GetButtonDown ("Left_BTN");
	}

	public static bool Down_button() {
		return Input.GetButtonDown ("Down_BTN");
	}

	public static bool Right_button() {
		return Input.GetButtonDown ("Right_BTN");
	}
	public static bool Any_button() {
		return (Input.GetButtonDown ("Up_BTN") || Input.GetButtonDown ("Left_BTN") || Input.GetButtonDown ("Down_BTN") || Input.GetButtonDown ("Right_BTN"));
	}
}
