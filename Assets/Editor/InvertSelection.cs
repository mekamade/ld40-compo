using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InvertSelection : MonoBehaviour {

	[MenuItem ("Edit/Selection/Invert")]
	static void static_InvertSelection() { 

		List< GameObject > oldSelection = new List< GameObject >();
		List< GameObject > newSelection = new List< GameObject >();


		foreach( GameObject obj in Selection.GetFiltered( typeof( GameObject ), SelectionMode.ExcludePrefab ) )
			oldSelection.Add( obj );

		foreach( GameObject obj in FindObjectsOfType( typeof( GameObject ) ) )
		{
			if ( !oldSelection.Contains( obj ) )
				newSelection.Add( obj );
		}

		Selection.objects = newSelection.ToArray();

	}
}
