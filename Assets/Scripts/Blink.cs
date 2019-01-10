using System.Collections;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public float Frequency = 0.5f;

    IEnumerator Start()
    {
        var blinkLight = GetComponent<Light>();
        while (true)
        {
            blinkLight.enabled = !(blinkLight.enabled);
            yield return new WaitForSeconds(Frequency);
        }
    }
}
