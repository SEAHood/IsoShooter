using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject Enemy; 
	public int Count;

	// Use this for initialization
	public void Start ()
	{
	    var origin = gameObject.transform.position;
		for (var i = 0; i < Count; i++) {
			var newZombie = Instantiate(Enemy);
            var position = new Vector3(Random.Range(origin.x - 50, origin.x + 50), 0.5f, Random.Range(origin.z - 50, origin.z + 50));
            Debug.Log("Creating zombie at position " + position);
		    newZombie.transform.position = position;
		}
	}

    // Update is called once per frame
    public void Update () {
		
	}
}
