using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MineBehaviour : NetworkBehaviour {

    public GameObject ExplosionVfx;
    public GameObject ExplosionSfx;

    private bool _hasExploded;

    public void Trigger()
    {
        OnTriggerEnter(null);
    }

    void OnTriggerEnter(Collider entityCollider)
    {
        if (_hasExploded) return;

        var impactInst = Instantiate(ExplosionVfx, transform.position, transform.rotation);
        NetworkServer.Spawn(impactInst);
        ExplosionSfx.GetComponent<AudioSource>().Play();

        if (entityCollider != null)
        {
            entityCollider.GetComponentInParent<Rigidbody>().AddExplosionForce(50f, transform.position, 5f, 0f, ForceMode.Impulse);
            entityCollider.GetComponentInParent<PlayerHealth>().TakeDamage(200, null);
        }

        Destroy(gameObject.transform.Find("Graphics").gameObject);
        Destroy(impactInst, 5f);
        Destroy(gameObject, 5f);

        _hasExploded = true;
    }
}
