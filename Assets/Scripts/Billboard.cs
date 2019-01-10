using UnityEngine;

public class Billboard : MonoBehaviour
{

    public Camera MainCamera;
	// Use this for initialization
	void Start () {
		//_mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
	    transform.LookAt(transform.position + MainCamera.transform.rotation * Vector3.forward, MainCamera.transform.rotation * Vector3.up);
    }
}
