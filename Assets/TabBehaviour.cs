using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TabBehaviour : MonoBehaviour
{
    public GameObject PlayerRowPrefab;

    private List<KeyValuePair<string, int>> _scores = new List<KeyValuePair<string, int>>();
    private float _basePanelHeight;


	void Start ()
	{
        _basePanelHeight = GetComponent<RectTransform>().sizeDelta.y;
    }

    public void AddOrUpdateScore(string player, int newScore)
    {
        Debug.Log("TabUI - adding score of " + player + ": " + newScore);
        if (_scores.Any(s => s.Key == player))
        {
            var existingScore = _scores.First(s => s.Key == player);
            var newScoreKv = new KeyValuePair<string, int>(player, newScore);
            _scores.Remove(existingScore);
            _scores.Add(newScoreKv);
        }
        else
        {
            var newScoreKv = new KeyValuePair<string, int>(player, newScore);
            _scores.Add(newScoreKv);
        }

        RenderTabList();
    }

    private void RenderTabList()
    {
        var padding = 1;
        var totalContentsHeight = 0f;

        foreach (Transform child in transform.Find("TabList"))
        {
            Destroy(child.gameObject);
        }

        _scores.Sort((x, y) => y.Value.CompareTo(x.Value));

        foreach (var s in _scores)
        {
            var playerRow = Instantiate(PlayerRowPrefab, transform.Find("TabList"));
            playerRow.transform.Find("PlayerName").GetComponent<Text>().text = s.Key;
            playerRow.transform.Find("PlayerScore").GetComponent<Text>().text = s.Value.ToString();

            var rowHeight = playerRow.GetComponent<RectTransform>().sizeDelta.y;

            var position = playerRow.transform.position;
            position.y -= (rowHeight + padding) * _scores.IndexOf(s);

            Debug.Log("Setting y position of row for player '" + s.Key + "' to " + position.y);

            playerRow.transform.position = position;
            totalContentsHeight += rowHeight + padding;
        }

        var panelRectTrans = GetComponent<RectTransform>();
        var size = panelRectTrans.sizeDelta;
        size.y = _basePanelHeight + totalContentsHeight;

        panelRectTrans.sizeDelta = size;
        panelRectTrans.anchoredPosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update ()
	{
	    GetComponent<CanvasGroup>().alpha = Input.GetKey(KeyCode.Tab) ? 1f : 0f;
	}
}
