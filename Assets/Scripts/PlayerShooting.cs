using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerShooting : NetworkBehaviour {
	public int DamagePerShot = 20;
	public float TimeBetweenBullets = 0.1f;
	public float Range = 100f;
    public bool Enabled = true;
    public GameObject Bullet;
    public GameObject PlayerUi;

    private Transform _bulletSpawnPosition;
    private float _timer;
    private int _shootableMask;
    private AudioSource _gunAudio;
    private Light _gunLight;

    private const float EffectsDisplayTime = 0.03f;


    void Start()
    {
        PlayerUi.SetActive(isLocalPlayer);
        _shootableMask = LayerMask.GetMask ("Shootable");

        var weapon = transform.Find("Weapon").gameObject;
        _gunAudio = weapon.GetComponent<AudioSource>();
		_gunLight = weapon.GetComponent<Light>();
        _bulletSpawnPosition = weapon.transform;
    }

    void Update ()
    {
        _timer += Time.deltaTime;
        if (_timer >= EffectsDisplayTime)
            DisableEffects();

        if (!isLocalPlayer) return;
        if (!Enabled) return;
        if (!GetComponent<PlayerHealth>().IsAlive()) return;

        if (Input.GetKey(KeyCode.Mouse0) && _timer >= TimeBetweenBullets)
        {
            PlayerUi.transform.Find("Bang").GetComponent<Text>().enabled = true;
            Shoot();
        }
	}

	public void DisableEffects ()
	{
	    PlayerUi.transform.Find("Bang").GetComponent<Text>().enabled = false;
		_gunLight.enabled = false;
	}

    private void Shoot()
    {
        CmdShoot();
    }
    
    [Command]
    public void CmdShoot()
    {
        var shootRay = new Ray();
        RaycastHit shootHit;
        shootRay.origin = _bulletSpawnPosition.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, Range, _shootableMask))
        {
            var enemyHealth = shootHit.collider.GetComponent<PlayerHealth>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(DamagePerShot, gameObject);
        }

        var bullet = Instantiate(Bullet.gameObject, _bulletSpawnPosition.position, transform.rotation);
        bullet.GetComponent<BulletBehaviour>().SetDestructionParams(shootRay.origin, shootHit.point, shootHit.distance);
        NetworkServer.SpawnWithClientAuthority(bullet, connectionToClient);

        RpcWeaponFired();
    }

    [ClientRpc]
    public void RpcWeaponFired()
    {
        _timer = 0f;
        _gunAudio.Play();
        _gunLight.enabled = true;
    }
}
