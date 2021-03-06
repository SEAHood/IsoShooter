﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;


public class PlayerHealth : NetworkBehaviour
{
    public float StartingHealth = 100f;
    public Image HealthBar;
    public GameObject DeathEffect;


    private PlayerMovement _playerMovement;
    private PlayerShooting _playerShooting;
    private Coroutine _deathCoroutine;

    [SyncVar(hook = "UpdateHealth")]
    public float _currentHealth;

    [SyncVar(hook = "UpdateDead")]
    public bool _isDead;

    
    void Start ()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerShooting = GetComponent<PlayerShooting>();
        _currentHealth = StartingHealth;
    }
    
    void Update ()
    {
        if (isLocalPlayer && !GetComponent<PlayerHealth>().IsAlive() && Input.GetKeyDown(KeyCode.R))
            Respawn();
    }

    public bool IsFullHealth()
    {
        return Mathf.Approximately(_currentHealth, StartingHealth);
    }

    private void Respawn()
    {
        if (isServer)
            RpcRespawn();
        else
            CmdRespawn();
    }

    [Command]
    public void CmdRespawn()
    {
        RpcRespawn();
    }

    [ClientRpc]
    private void RpcRespawn()
    {

        _isDead = false;
        _currentHealth = 100;

        var spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        var point = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        var respawnPos = point.transform.position;
        respawnPos.y = 1;
        transform.position = respawnPos;
    }
    
    public void Heal(float amount)
    {
        var newHealth = _currentHealth + amount;
        _currentHealth = newHealth >= 100 ? 100 : newHealth;
    }

    /*[Command]
    public void CmdDeath()
    {
        StartCoroutine("DeathCoroutine");
    }

    private IEnumerator DeathCoroutine()
    {
        RpcDisableFloatUi();
        yield return new WaitForSeconds(1.5f);
        RpcDeath();
    }

    [ClientRpc]
    private void RpcDisableFloatUi()
    {
        transform.Find("FloatUI").gameObject.SetActive(false);
    }

    [ClientRpc]
    private void RpcDeath()
    {
        Instantiate(DeathEffect, transform.position, DeathEffect.transform.rotation);
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        transform.Find("Player").gameObject.SetActive(false);
    }*/
    
    private IEnumerator DeathCoroutine()
    {
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        transform.Find("FloatUI").gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        Instantiate(DeathEffect, transform.position, DeathEffect.transform.rotation);
        GetComponent<BoxCollider>().enabled = false;
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        transform.Find("Player").gameObject.SetActive(false);
    }

    // Todo: investigate whether or not i need to do the whole rpc / cmd shit
    public bool TakeDamage(int amount, GameObject shooter) // Returns if it was a kill shot
    {
        if (_isDead) return false;
        var oldHealth = _currentHealth;

        if (isServer)
            RpcTakeDamage(amount, shooter);
        else
            CmdTakeDamage(amount, shooter);


        return oldHealth > 0 && _isDead;
    }

    [Command]
    public void CmdTakeDamage(int amount, GameObject shooter)
    {
        RpcTakeDamage(amount, shooter);
    }

    [ClientRpc]
    private void RpcTakeDamage(int amount, GameObject shooter)
    {
        if (_isDead) return;

        var newHealth = _currentHealth - amount;
        _currentHealth = newHealth > 0 ? newHealth : 0;

        if (_currentHealth <= 0 && !_isDead)
        {
            _isDead = true;
            if (shooter != null)
                shooter.GetComponent<PlayerScore>().GrantPoints(1);

            //if (_isDead)
            //    CmdDeath();
        }
    }

    private void UpdateHealth(float newHealth)
    {
        if (isLocalPlayer)
            transform.Find("UI/Test2").GetComponent<Text>().text = "Health: " + newHealth + "/" + StartingHealth;
        HealthBar.fillAmount = newHealth / StartingHealth;
    }

    private void UpdateDead(bool isNowDead)
    {
        gameObject.layer = isNowDead ? LayerMask.NameToLayer("Default") : LayerMask.NameToLayer("Shootable");
        transform.Find("Player/Torch").gameObject.GetComponent<Light>().enabled = !isNowDead; 
        _playerMovement.Enabled = !isNowDead;
        _playerShooting.Enabled = !isNowDead;

        if (isNowDead)
            _deathCoroutine = StartCoroutine(DeathCoroutine());
        else
        {
            StopCoroutine(_deathCoroutine);
            transform.Find("Player").gameObject.SetActive(true);
            transform.Find("FloatUI").gameObject.SetActive(true);
            GetComponent<BoxCollider>().enabled = true;
            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        if (isLocalPlayer)
        {
            transform.Find("UI/Dead").GetComponent<Text>().enabled = isNowDead;
            transform.Find("UI/Respawn").GetComponent<Text>().enabled = isNowDead;
/*

            if (isNowDead)
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            else
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;*/
        }
    }

    public bool IsAlive()
    {
        return !_isDead;
    }
}
