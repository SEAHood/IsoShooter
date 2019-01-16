using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerScore : NetworkBehaviour
{
    [HideInInspector]
    [SyncVar(hook = "OnScoreChange")]
    public int CurrentScore;

	// Use this for initialization
	void Start ()
	{
	}

    public void GrantPoints(int points)
    {
        Debug.Log("GRANTING POINTZ");
        CurrentScore += points;
    }

    public void OnScoreChange(int newScore)
    {
        transform.Find("FloatUI/Score").gameObject.GetComponent<Text>().text = newScore.ToString();
        transform.Find("UI/TabPanel").GetComponent<TabBehaviour>().AddOrUpdateScore(GetComponent<Player>().PlayerName ?? "Unknown", newScore);

        if (isLocalPlayer)
        {
            // do something
        }
    }
}
