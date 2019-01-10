using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{

    public GameObject TeleportTarget;

    private GameObject _player;

    // Use this for initialization
    void Start()
    {
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
    }


    void OnTriggerEnter(Collider par)
    {
        if (par.CompareTag("Player"))
        {
            _player.transform.position = TeleportTarget.transform.position;
        }
    }
}
