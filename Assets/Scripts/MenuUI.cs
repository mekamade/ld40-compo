using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour {

	private enum MenuStates {Main, About};
	private MenuStates currentState;

	public GameObject mainPanel;
	public GameObject aboutPanel;

	void Start () {
		currentState = MenuStates.Main;
	}
	
	void Update () {
		switch (currentState) {
			case MenuStates.Main:
				mainPanel.SetActive(true);
				aboutPanel.SetActive(false);
				break;
			case MenuStates.About:
				mainPanel.SetActive(false);
				aboutPanel.SetActive(true);
				break;
		}
	}

	public void OpenScene(string scene) {
		SceneManager.LoadScene(scene);
	}

	public void pressedAbout() {
		currentState = MenuStates.About;
	}
		

	public void confirmQuit() {
		Application.Quit();
	}

	public void returnMain() {
		currentState = MenuStates.Main;
	}
}
