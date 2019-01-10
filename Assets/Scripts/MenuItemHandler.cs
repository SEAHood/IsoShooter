using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuItemHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private MainMenu _fullMenu;

    void Start()
    {
        _fullMenu = GetComponentInParent<MainMenu>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_fullMenu.OptionSelected) return;
        _fullMenu.OptionSelected = true;

        switch (gameObject.name)
        {
            case "Play":
                GameObject.Find("Mine").GetComponent<MineBehaviour>().Trigger();
                Initiate.Fade("ArenaMode", Color.black, 0.5f);
                break;
            case "Quit":
                Application.Quit();
                break;
            default:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_fullMenu.OptionSelected) return;
        GetComponent<Text>().color = Color.gray;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_fullMenu.OptionSelected) return;
        GetComponent<Text>().color = Color.white;
    }
    
}
