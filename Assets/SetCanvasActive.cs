using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCanvasActive : MonoBehaviour {

	void Start ()
    {
        Canvas canvas = gameObject.GetComponent<Canvas>();
        canvas.enabled = true;
	}
	
}
