using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour  {

	public float Speed = 15f;
	public bool Enabled = true;

    public GameObject PlayerCamera;

	// Use this for initialization
	void Start ()
	{
	    PlayerCamera.SetActive(isLocalPlayer);
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!isLocalPlayer) return; 
	    if (!Enabled) return;

        var dir = new Vector3();
		if ( Input.GetKey(KeyCode.W) ) {
			dir += new Vector3(1, 0, 1);
		}
		if ( Input.GetKey(KeyCode.A) ) {
			dir += new Vector3(-1, 0, 1);
		}
		if ( Input.GetKey(KeyCode.S) ) {
			dir += new Vector3(-1, 0, -1);
		}
		if ( Input.GetKey(KeyCode.D) ) {
			dir += new Vector3(1, 0, -1);
		}

	    var rb = GetComponent<Rigidbody>();
        var movement = dir.normalized * Speed * Time.deltaTime;
        rb.AddForce(movement, ForceMode.VelocityChange);

		var plane = GameObject.Find("Ground");
		var cPlane = plane.GetComponent<Collider>();

		var ray = PlayerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (cPlane.Raycast (ray, out hit, 1000f)) 
		{
			var targetPoint = hit.point;
			targetPoint.y += 0.5f;
			var targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Speed * Time.deltaTime);

		}

		//Debug 
		var rayStart = transform.position;
		Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
	}
}
