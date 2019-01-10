using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class PlayerHealth : NetworkBehaviour
{
    public float StartingHealth = 100f;
    public Image HealthBar;
    public GameObject ImpactEffect;
    public GameObject ImpactExitEffect;
    
    private PlayerMovement _playerMovement;
    private PlayerShooting _playerShooting;

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
        var respawnPos = Vector3.zero;
        respawnPos.y = 1;
        transform.position = respawnPos;
    }
    
    public void Heal(float amount)
    {
        var newHealth = _currentHealth + amount;
        _currentHealth = newHealth >= 100 ? 100 : newHealth;
    }

    // Todo: investigate whether or not i need to do the whole rpc / cmd shit
    public void TakeDamage(int amount)
    {
        if (isServer)
            RpcTakeDamage(amount);
        else
            CmdTakeDamage(amount);
    }

    [Command]
    public void CmdTakeDamage(int amount)
    {
        RpcTakeDamage(amount);
    }

    [ClientRpc]
    private void RpcTakeDamage(int amount)
    {
        if (_isDead) return;

        var newHealth = _currentHealth - amount;
        _currentHealth = newHealth > 0 ? newHealth : 0;

        if (_currentHealth <= 0 && !_isDead)
            _isDead = true;
    }

    private void UpdateHealth(float newHealth)
    {
        if (isLocalPlayer)
            transform.Find("UI/Test2").GetComponent<Text>().text = "Health: " + newHealth + "/" + StartingHealth;
        HealthBar.fillAmount = newHealth / StartingHealth;
    }

    private void UpdateDead(bool isNowDead)
    {
        transform.Find("Player/Torch").gameObject.GetComponent<Light>().enabled = !isNowDead; //todo
        _playerMovement.Enabled = !isNowDead;
        _playerShooting.Enabled = !isNowDead;

        if (isLocalPlayer)
        {
            transform.Find("UI/Dead").GetComponent<Text>().enabled = isNowDead;

            if (isNowDead)
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            else
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public bool IsAlive()
    {
        return !_isDead;
    }
}
