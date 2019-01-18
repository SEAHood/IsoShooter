using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    [HideInInspector]
    public bool OptionSelected;

    public GameObject ExplosionFx;
    public Transform ExplosionTransform;

    public void SelectOption()
    {
        OptionSelected = true;
        Instantiate(ExplosionFx, ExplosionTransform.position, Quaternion.identity);
        GameObject.Find("Mine/MineAudio").GetComponent<AudioSource>().Play();
    }
}
