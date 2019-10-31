using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float Speed;
    void Update()
    {
        transform.Rotate(0, 0, Speed * Time.deltaTime);
    }
}
