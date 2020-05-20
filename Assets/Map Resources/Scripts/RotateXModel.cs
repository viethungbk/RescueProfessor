using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateXModel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(new Vector3(Time.deltaTime * 60, 0, 0));
		
	}
}
