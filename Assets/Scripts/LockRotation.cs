using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour {

    private Quaternion _rot;

    void Start()
    {
        _rot = transform.rotation;
    }

    void Update()
    {
        transform.rotation = _rot;
    }
}
