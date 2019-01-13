using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthpackBehaviour : NetworkBehaviour
{
    public float HealthAmount = 20f;
    public float RespawnRate = 5f;

    private bool _isRespawning;
    private float _respawnTimer;

    void Update()
    {
        if (!_isRespawning) return;

        _respawnTimer += Time.deltaTime;
        if (_respawnTimer >= RespawnRate)
        {
            _isRespawning = false;
            RpcRespawn();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (_isRespawning) return;
        if (other.tag == "Player")
        {
            var playerHealth = other.GetComponentInParent<PlayerHealth>();
            if (playerHealth.IsFullHealth() || !playerHealth.IsAlive()) return;
            playerHealth.Heal(HealthAmount);
            _isRespawning = true;
            _respawnTimer = 0f;
            RpcCollected();
        }
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        transform.Find("Graphics").gameObject.SetActive(true);
        // todo
        /*var meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var mr in meshRenderers)
        {
            var color = mr.material.color;
            color.a = 1f;
            mr.material.color = color;
        }*/
    }

    [ClientRpc]
    private void RpcCollected()
    {
        Debug.Log("Healthpack collected");
        transform.Find("Graphics").gameObject.SetActive(false);
        transform.Find("CollectEffect").GetComponent<ParticleSystem>().Play();
        // todo
        /*var meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var mr in meshRenderers)
        {
            var color = mr.material.color;
            Debug.Log(color);
            color.a = 0.1f;
            mr.material.color = color;
            Debug.Log(mr.material.color);
        }*/
    }
}
