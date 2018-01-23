using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class DrinkHandler : MonoBehaviour {

	public GameObject[] Belts;

	public Image[] dPad;
	public Sprite[] dPadSpriteIdle;
	public Sprite[] dPadSpriteGreen;
	public Sprite[] dPadSpriteRed;

	public Camera effectsCamera;
	public GameObject AudioManager;
	public GameObject MusicManager;

	public Image Blackout;
	public GameObject drinkPrefab;
	public Text timeText;
	public Image timeBar;
	public Image beerBar;
	public Image startPrompt;
	public GameObject endPrompt;
	public GameObject pausePanel;
	public Image logicLight;
	public Sprite logicSpriteGreen;
	public Sprite logicSpriteRed;

	public Text endTime;
	public Text encounter;
	public Text AT;
	public Text NAT;
	public Text AC;
	public Text NAC;
	public Text AL;
	public Text NAL;

	public float maxAlcoholLevel;
	public float greenLogicProbability;

	Stopwatch stopwatch;

	enum GameState {INIT, CREATE, ENTRY, WAITING, ACTIVE, DESTROY, PAUSED, ENDED, DONE};
	GameState currentState;
	GameState pausedState;
	GameObject activeDrink;
	int totalDrinkCount = 0; 
	int alcoholDrinkCount = 0;
	int nonAlcoholDrinkCount = 0;
	int alcoholConsumed = 0;
	int nonAlcoholConsumed = 0;
	int totalMinutes = 0;
	float alcoholLevel;
	float timerTotal;
	float timer;
	float ACTIVE_alcValue;
	bool ACTIVE_isAlc;
	bool ACTIVE_puzzleLogic;
	int[] ACTIVE_puzzleCode = new int[] { 0, 0, 0, 0 };
	int[] ACTIVE_puzzleColorCode = new int[] { 0, 0, 0, 0 };
	int ACTIVE_puzzlePointer = 0;
	bool ACTIVE_successFlag = false;
	bool gameActive = false;
	bool pauseBump = false;

	void Start () {
		stopwatch = new Stopwatch();
		conveyorBelt_initialSetup();
		UI_initialSetup();
		startPrompt.gameObject.SetActive(true);
	}

	void setGameState(GameState targetState) {
		currentState = targetState;
	}

	void Update() {
		if (gameActive){
			checkForPause ();
		}


		switch(currentState) {
		case GameState.INIT:
			if (Input.GetKeyDown(KeyCode.Space)) {
				stopwatch.Start ();
				startPrompt.gameObject.SetActive(false);
				gameActive = true;
				MusicManager.GetComponent<AudioSource> ().Play ();
				setGameState(GameState.CREATE);
			}
			break;
		case GameState.CREATE:
			logicLight.gameObject.SetActive (false);
			deactivate_Dpad ();
			if (activeDrink != null) {
				Destroy (activeDrink.gameObject);
			}
			activeDrink = createDrink ();
			setGameState (GameState.ENTRY);
			break;
		case GameState.ENTRY:
			activeDrink.GetComponent<Animator> ().SetTrigger ("enterDrink");
			moveBelt ();
			setGameState (GameState.WAITING);
			break;
		case GameState.WAITING:
			//Let Animation Play
			break;
		case GameState.ACTIVE:
			logicLight.gameObject.SetActive (true);
			activate_Dpad ();
			timer -= Time.deltaTime;
			recieveInput ();
			if (timer < 0) {
				if (!(ACTIVE_isAlc ^ ACTIVE_puzzleLogic)) {
					ACTIVE_successFlag = false;
					ACTIVE_puzzleColorCode = new int[] { -1, -1, -1, -1 };
					setGameState (GameState.DESTROY);
				} else {
					
					ACTIVE_successFlag = true;
					ACTIVE_puzzleColorCode = new int[] { 1, 1, 1, 1 };
					setGameState (GameState.DESTROY);
				}
			}
			break;
		case GameState.DESTROY:
			totalDrinkCount++;
			if (ACTIVE_isAlc)
				alcoholDrinkCount++;
			else
				nonAlcoholDrinkCount++;
			if (!(ACTIVE_successFlag ^ ACTIVE_isAlc)) {
				activeDrink.GetComponent<Animator> ().SetTrigger ("exitDrink");
				moveBelt ();
			} else {
				if (ACTIVE_isAlc)
					alcoholConsumed++;
				else
					nonAlcoholConsumed++;
				alcoholLevel = alcoholLevel + ACTIVE_alcValue;
				if (alcoholLevel < 0) alcoholLevel = 0;
				activeDrink.GetComponent<Animator> ().SetTrigger ("consumeDrink");
				AudioManager.GetComponent<SFXAudioManager> ().playSound ("Drink");
			}
			setGameState (GameState.WAITING);
			break;
		case GameState.ENDED:
			gameActive = false;
			effectsCamera.GetComponent<MotionBlur> ().blurAmount = 0;
			AudioManager.GetComponent<SFXAudioManager> ().updateEffects (5000.0f);
			effectsCamera.GetComponent<Animator> ().SetTrigger ("End");
			string hours = formatTime (totalMinutes / 60);
			string minutes = formatTime (totalMinutes % 60);
			endTime.text = "you survived for " + hours + ":" + minutes + " hrs.";
			encounter.text = "encountered " + totalDrinkCount.ToString () + " drinks.";
			AT.text = (alcoholDrinkCount).ToString ();
			NAT.text = (nonAlcoholDrinkCount).ToString ();
			AC.text = (alcoholConsumed).ToString ();
			NAC.text = (nonAlcoholConsumed).ToString ();
			AL.text = (alcoholDrinkCount - alcoholConsumed).ToString ();
			NAL.text = (nonAlcoholDrinkCount - nonAlcoholConsumed).ToString ();
			endPrompt.GetComponent<Animator> ().SetTrigger ("activate");
			setGameState (GameState.DONE);
			break;
		case GameState.PAUSED:
			if (Input.GetKeyDown (KeyCode.Escape) && !pauseBump)
				resumeGame ();
			pauseBump = false;
			break;
		case GameState.DONE:
			break;
		}



			
		UI_update();

		if (gameActive) {
			beerBarHandling ();
			addCameraEffects ();
		}
	}



	public void resumeGame() {
		gameActive = true;
		effectsCamera.GetComponent<Animator> ().SetTrigger ("Resume");
		pausePanel.gameObject.SetActive (false);
		stopwatch.Start ();
		Time.timeScale = 1;
		MusicManager.GetComponent<AudioSource> ().UnPause ();
		setGameState (pausedState);
	}


	void checkForPause(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			stopwatch.Stop ();
			effectsCamera.GetComponent<MotionBlur> ().blurAmount = 0;
			effectsCamera.GetComponent<DepthOfField> ().aperture = 0;
			pausePanel.gameObject.SetActive (true);
			gameActive = false;
			pauseBump = true;
			pausedState = currentState;
			Time.timeScale = 0;
			MusicManager.GetComponent<AudioSource> ().Pause ();
			setGameState (GameState.PAUSED);
		}
	}

	public void OpenScene(int scene) {
		Time.timeScale = 1;
		SceneManager.LoadScene(scene);
	}

	void addCameraEffects() {
		float drunkness = alcoholLevel / maxAlcoholLevel;
		Blackout.GetComponent<Image> ().color = new Vector4 (0, 0, 0, drunkness * 0.35f);
		effectsCamera.GetComponent<MotionBlur> ().blurAmount = 0.92f * (drunkness/ 1f);
		if (drunkness > 0.08f)
			effectsCamera.GetComponent<DepthOfField> ().aperture = ((drunkness - 0.08f) / 0.92f) * 0.30f;
		float random = Random.value;
		if ((random > (0.999f - ((0.009f)*((drunkness-0.3)/0.7f))) && drunkness > 0.3)) {
			effectsCamera.GetComponent<Animator> ().SetTrigger ("Iris");
		}
		if ((random > 0.9999f) && drunkness > 0.7) {
			effectsCamera.GetComponent<Animator> ().SetTrigger ("ZoneOut");
		}
		AudioManager.GetComponent<SFXAudioManager> ().updateEffects ((5000.0f - drunkness * 4700.0f));
		MusicManager.GetComponent<AudioLowPassFilter> ().cutoffFrequency = (5000.0f - drunkness * 4700.0f);

	}

	public void endEntry() {
		if(currentState != GameState.DONE)
			setGameState (GameState.ACTIVE);
	}

	public void endDestroy() {
		if(currentState != GameState.DONE)
			setGameState (GameState.CREATE);
	}
		

	void beerBarHandling() {
		alcoholLevel = 1.00005f * alcoholLevel;
		if (alcoholLevel > maxAlcoholLevel) {
			stopwatch.Stop ();
			setGameState (GameState.ENDED);
		}
	}

	GameObject createDrink() {
		var drink = Instantiate(drinkPrefab);
		drink.GetComponent<Drink>().Initialize(gameObject, totalMinutes);
		ACTIVE_alcValue = drink.GetComponent<Drink>().alcValue;
		ACTIVE_isAlc = drink.GetComponent<Drink>().isAlc;
		ACTIVE_puzzleCode = generatePuzzleCode();
		generatePuzzleLogic(totalMinutes);
		ACTIVE_puzzleColorCode = new int[] {0,0,0,0};
		ACTIVE_puzzlePointer = 0;
		timerTotal = generateTTL (totalMinutes);
		timer = timerTotal;
		return drink;
	}
		
	string formatTime(int x){
		if (x < 10)
			return "0" + x;
		else
			return x.ToString();
	}

//CONTROL PANEL DATA

	void generatePuzzleLogic(int progression){
		float random = Random.value;
		if (progression < 30) {
			ACTIVE_puzzleLogic = true;
			logicLight.GetComponent<Image> ().sprite = logicSpriteGreen;
		}else if (random <= greenLogicProbability) {
			ACTIVE_puzzleLogic = true;
			logicLight.GetComponent<Image> ().sprite = logicSpriteGreen;
		} else {
			ACTIVE_puzzleLogic = false;
			logicLight.GetComponent<Image> ().sprite = logicSpriteRed;
		}
	}

	int[] generatePuzzleCode(){
		int[] dPad = new int[4];
		for(int i = 0; i < 4; i++){
			dPad[i] = Random.Range(0, 4);
		}
		return dPad;
	}

	float generateTTL(int progression) {
		float output = 0f;
		if (progression < 20) 
			output = 2.5f;
		else if (progression < 150) 
			output = 2.5f - ((progression - 20)/130.0f)*1.5f;
		else
			output = 1f;
		return output;
	}


// CONVEYOR BELT LOGIC

	void conveyorBelt_initialSetup() {
		Belts[0].GetComponent<Animator>().SetTrigger("A");
		Belts[1].GetComponent<Animator>().SetTrigger("B");
		Belts[3].GetComponent<Animator>().SetTrigger("D");
		Belts[4].GetComponent<Animator>().SetTrigger("E");
	}

	void moveBelt() {
		AudioManager.GetComponent<SFXAudioManager> ().playSound ("Belt");
		for (int i = 0; i < Belts.Length; i++) {
			Belts[i].GetComponent<Animator>().SetTrigger("move");
		}
	}

// UI Logic 

	void UI_initialSetup() {
		timeBar.gameObject.SetActive (true);
		logicLight.gameObject.SetActive(false);
		deactivate_Dpad();
		timer = 2.5f;
		timerTotal = 2.5f;
		alcoholLevel = 0;
	}


	void UI_update(){
		beerBar.fillAmount = alcoholLevel/maxAlcoholLevel;
		timeBar.fillAmount = timer/timerTotal;

		totalMinutes = Mathf.FloorToInt((float)stopwatch.Elapsed.TotalSeconds);
		string hours = formatTime(totalMinutes/60);
		string minutes = formatTime(totalMinutes%60);
		timeText.text = hours + ":" + minutes;

		render_Dpad();
	}


//D-Pad related functions

	void deactivate_Dpad() {
		for(int i = 0; i < 4; i++){
			dPad[i].gameObject.SetActive(false);
		}
	}

	void activate_Dpad() {
		for(int i = 0; i < 4; i++){
			dPad[i].gameObject.SetActive(true);
		}
	}


	void render_Dpad() {
		for (int i = 0; i < 4; i++) {
			if (ACTIVE_puzzleColorCode [i] == 0) {
				dPad [i].GetComponent<Image> ().sprite = dPadSpriteIdle [ACTIVE_puzzleCode[i]];
			} else if (ACTIVE_puzzleColorCode [i] == 1) {
				dPad [i].GetComponent<Image> ().sprite = dPadSpriteGreen [ACTIVE_puzzleCode[i]];
			} else {
				dPad [i].GetComponent<Image> ().sprite = dPadSpriteRed [ACTIVE_puzzleCode[i]];
			}
		}
	}

	void recieveInput() {
		if (!(ACTIVE_puzzleLogic ^ ACTIVE_isAlc)) {
			if (translateDirection (ACTIVE_puzzleCode [ACTIVE_puzzlePointer], true)) {
				ACTIVE_puzzleColorCode [ACTIVE_puzzlePointer] = 1;
				ACTIVE_puzzlePointer++;
				if (ACTIVE_puzzlePointer == 4) {
					ACTIVE_successFlag = true;
					setGameState (GameState.DESTROY);
				}
			} else if (translateDirection (ACTIVE_puzzleCode [ACTIVE_puzzlePointer], false)) {
					ACTIVE_puzzleColorCode [ACTIVE_puzzlePointer] = -1;
					ACTIVE_successFlag = false;
					setGameState (GameState.DESTROY);
			}
		} else {
			if (InputManager.Any_button ()) {
				ACTIVE_puzzleColorCode = new int[] { -1, -1, -1, -1 };
				ACTIVE_successFlag = false;
				setGameState (GameState.DESTROY);
			}
		}
	}

	bool translateDirection (int ID, bool positive)
	{
		bool output = false;
		bool alt_output = false;
		switch (ID) {
		case 0:
			output = InputManager.Right_button ();
			break;
		case 1:
			output = InputManager.Down_button ();
			break;
		case 2:
			output = InputManager.Left_button ();
			break;
		case 3:
			output = InputManager.Up_button ();
			break;
		}
		if (InputManager.Any_button () && output != true) {
			alt_output = true;
		}
		if (positive)
			return output;
		else
			return alt_output;

	}
}
