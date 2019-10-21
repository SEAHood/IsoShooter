using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour  {

	public float Speed;
	public bool Enabled = true;
    public float GravityScale;
    public float JumpForce;

    public GameObject PlayerCamera;

    private Vector3 _moveDirection = Vector3.zero;
    private CharacterController _controller;

	// Use this for initialization
	void Start ()
	{
	    PlayerCamera.SetActive(isLocalPlayer);
	    _controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!Enabled) return;

	    if (isLocalPlayer)
	    {
	        var previousY = _moveDirection.y;
            //_moveDirection = new Vector3(Input.GetAxis("Horizontal") * Speed, _moveDirection.y, Input.GetAxis("Vertical") * Speed);
            //_moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection = new Vector3();
	        if (Input.GetKey(KeyCode.W))
	            _moveDirection += new Vector3(1, _moveDirection.y, 1);
	        if (Input.GetKey(KeyCode.A))
	            _moveDirection += new Vector3(-1, _moveDirection.y, 1);
	        if (Input.GetKey(KeyCode.S))
	            _moveDirection += new Vector3(-1, _moveDirection.y, -1);
	        if (Input.GetKey(KeyCode.D))
	            _moveDirection += new Vector3(1, _moveDirection.y, -1);
            
	        _moveDirection *= Speed;
	        _moveDirection.y = previousY;

            if (_controller.isGrounded && Input.GetButtonDown("Jump"))
	        {
	            _moveDirection.y = JumpForce;
	        }

        }

	    _moveDirection.y = _moveDirection.y + (Physics.gravity.y * GravityScale) * Time.deltaTime;
	    _controller.Move(_moveDirection * Time.deltaTime);

        if (!isLocalPlayer) return; 

        /*var dir = new Vector3();
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
        rb.AddForce(movement, ForceMode.VelocityChange);*/


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
