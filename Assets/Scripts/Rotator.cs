using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public int RotationPerSecond = 50;
    public bool Bounce = true;
    public float BounceAmplitude = 0.5f;
    public float BounceFrequency = 1f;


    Vector3 _posOffset = new Vector3();
    Vector3 _tempPos = new Vector3();

    void Start()
    {
        _posOffset = transform.position;
    }

    void Update () {
	    gameObject.transform.Rotate(0, RotationPerSecond * Time.deltaTime, 0);
	    if (Bounce)
	    {
	        _tempPos = _posOffset;
	        _tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * BounceFrequency) * BounceAmplitude;

	        transform.position = _tempPos;
        }
	}
}
