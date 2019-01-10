using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int StartingHealth = 100;
    public int CurrentHealth;
    public float SinkSpeed = 2.5f;
    public Image HealthBar;
    public GameObject ImpactEffect;
    public GameObject ImpactExitEffect;

    public GameObject DebugSphere;

    private bool _isDead;
    private bool _isSinking;


    private void Start()
    {
        CurrentHealth = StartingHealth;
    }


    private void Update()
    {
        if(_isSinking)
            transform.Translate (-Vector3.up * SinkSpeed * Time.deltaTime);
    }


    public void TakeDamage(int amount, Vector3 hitPoint, Vector3 hitOrigin)
    {
        if(_isDead)
            return;

        /*var debSphOrigin = Instantiate(DebugSphere, hitOrigin, transform.rotation);
        Destroy(debSphOrigin, 3f);
        
        var debSphHit = Instantiate(DebugSphere, hitPoint, transform.rotation);
        Destroy(debSphHit, 3f);*/
        
        var impactInst = Instantiate(ImpactEffect, hitPoint, transform.rotation);
        impactInst.transform.LookAt(hitOrigin);

        var awayDirection = hitPoint - hitOrigin;
        var awayRotation = Quaternion.LookRotation(awayDirection);
        var impactExitInst = Instantiate(ImpactExitEffect, hitPoint, awayRotation);

        Destroy(impactInst, 3f);
        Destroy(impactExitInst, 3f);

        CurrentHealth -= amount;
        HealthBar.fillAmount = (float) CurrentHealth / StartingHealth;
        
        if (CurrentHealth <= 0)
            Death ();
    }


    private void Death()
    {
        _isDead = true;
		StartSinking();
    }


    public void StartSinking ()
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        _isSinking = true;
        Destroy (gameObject, 2f);
    }
}
