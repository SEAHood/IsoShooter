using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombo : MonoBehaviour {

	public float speed;
	public float strength;

	Transform player;
	UnityEngine.AI.NavMeshAgent nav;
	EnemyHealth health;

	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
		health = GetComponent<EnemyHealth>();
	}
	
	// Update is called once per frame
	void Update () {

		if ( health.CurrentHealth > 0 )
			nav.SetDestination(player.position);
		return;
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, player.position, step);

		Quaternion targetRotation = Quaternion.LookRotation (player.position - transform.position);
		float str = Mathf.Min (strength * Time.deltaTime, 1);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, str);

		//Debug 
		Vector3 rayStart = transform.position;
		Debug.DrawRay(transform.position, transform.forward * 3, Color.red);
	}
}
