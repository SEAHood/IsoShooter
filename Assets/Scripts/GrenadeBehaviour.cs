﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
    }
	
	// Update is called once per frame
	void Update ()
	{
	    GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.VelocityChange);

    }
}
