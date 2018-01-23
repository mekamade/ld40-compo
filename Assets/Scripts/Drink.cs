using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drink : MonoBehaviour {

	public Sprite[] spriteArray;
	public float[] alcoholValue;
	public bool[] isAlcoholic;



	int drinkID;
	public float alcValue;
	public bool isAlc;
	public GameObject drinkHandler;

	public void Initialize(GameObject handler, int progression) {
		drinkHandler = handler;
		drinkID = generateDrinkID(progression);
		GetComponent<SpriteRenderer>().sprite = spriteArray[drinkID];
		alcValue = alcoholValue[drinkID];
		isAlc = isAlcoholic[drinkID];
	}

	int generateDrinkID(int progression){
		if (progression < 15)
			return (Random.Range(0, 7));
		else
			return (Random.Range(0, spriteArray.Length));
	}

	public void endEntry() {
		drinkHandler.GetComponent<DrinkHandler> ().endEntry ();
	}

	public void endDestroy() {
		drinkHandler.GetComponent<DrinkHandler> ().endDestroy ();
	}

}

