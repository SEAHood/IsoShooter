using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletBehaviour : NetworkBehaviour
{
    public float BulletSpeed = 150f;
    public GameObject BulletImpactEffect;

    private float _distanceToTravel = 1000f;
    private Vector3 _hitPoint;
    private Vector3 _hitOrigin;
    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
    }

	void FixedUpdate ()
	{
        //GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed;

        transform.Translate(Vector3.forward * Time.deltaTime * BulletSpeed);
	    if (Vector3.Distance(transform.position, _startPosition) > _distanceToTravel)
	    {
	        var awayDirection = _hitOrigin - _hitPoint;
	        var awayRotation = Quaternion.LookRotation(awayDirection);
            var impact = Instantiate(BulletImpactEffect, _hitPoint, awayRotation);
	        NetworkServer.Spawn(impact);
            Destroy(impact, 5f);
            Destroy(gameObject);
        }
    }

    public void SetDestructionParams(Vector3 hitOrigin, Vector3 hitPoint, float distance)
    {
        _hitPoint = hitPoint;
        _hitOrigin = hitOrigin;
        _distanceToTravel = distance;
    }
/*

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGERED");
        if (other.gameObject.layer == 8) // Shootable layer
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("awgagwgawagw");
    }*/
}
