using UnityEngine;

public class PlayerFollow : MonoBehaviour {

	private float _distance = 12f;
    private Quaternion _rot;
    private GameObject _target;

	// Use this for initialization
	void Start ()
	{
	    _target = transform.parent.gameObject;
	    _rot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		var pos = _target.transform.position;
		pos.x -= _distance;
		pos.z -= _distance;
		pos.y = 10f;
		transform.position = pos;
	    transform.rotation = _rot;
	}
}
