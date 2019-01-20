using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button PlayButton;
    public Button NameSelectButton;
    public Button QuitButton;
    public InputField NameInput;
    
    public GameObject ExplosionFx;
    public Transform ExplosionTransform;

    private bool _mineExploded;

    void Start()
    {
        PlayButton.onClick.AddListener(OnClickPlay);
        NameSelectButton.onClick.AddListener(OnClickNameSelect);
        QuitButton.onClick.AddListener(OnClickQuit);
    }

    void Update()
    {

    }


    IEnumerator SlideInNameSelect()
    {
        const float slideDistance = -1295f;
        var slidingMenu = transform.Find("SlidingMenu");
        while (slidingMenu.transform.localPosition.x > slideDistance)
        {
            slidingMenu.transform.Translate(-50f * transform.localScale.x, 0, 0, Space.Self);
            yield return null;
        }

    }

    void OnClickPlay()
    {
        StartCoroutine(SlideInNameSelect());
        // Todo Lerp this so it slides
        //transform.Find("SlidingMenu").transform.Translate(-1295 * transform.localScale.x, 0, 0, Space.Self);
    }

    void OnClickNameSelect()
    {
        if (string.IsNullOrEmpty(NameInput.text)) return;

        if (!_mineExploded)
        {
            Instantiate(ExplosionFx, ExplosionTransform.position, Quaternion.identity);
            GameObject.Find("Mine/MineAudio").GetComponent<AudioSource>().Play();
            _mineExploded = true;
        }

        CrossScene.PlayerName = NameInput.text;
        Initiate.Fade("Lobby", Color.black, 0.5f);
    }

    void OnClickQuit()
    {
        Application.Quit();
    }
}
