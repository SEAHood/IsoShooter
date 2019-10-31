using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

// ReSharper disable InconsistentNaming
[Serializable]
public class LobbyDto
{
    public string name;
    public int maxPlayers;
    public int players;
    public string map;
    public string ip;
}

public class LobbiesDto
{
    public List<LobbyDto> lobbies;
}
// ReSharper enable InconsistentNaming

namespace Assets.Scripts.Helpers
{
    public static class Api
    {
        private const string Domain = "https://rootytootywebservices.azurewebsites.net/api";
        private static readonly string LobbiesUrl = $"{Domain}/lobbies?code=2Qub98B6zk11J6MT3dabRSmXJQh1fsoTawsIf2rZMrZyhmyZVZnPvQ==";
/*

        public static IEnumerator SubmitScore(string username, string pbToken, int score)
        {
            var payload = JsonUtility.ToJson(new AccountDto()
            {
                username = username,
                pb = score,
                pbToken = pbToken
            });

            var request = new UnityWebRequest(LeaderboardUrl)
            {
                uploadHandler = new UploadHandlerRaw(Encoding.ASCII.GetBytes(payload)),
                downloadHandler = new DownloadHandlerBuffer(),
                method = UnityWebRequest.kHttpVerbPOST
            };
            request.SetRequestHeader("UpMetadata-Version", Config.GameVersion);

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Score submitted");
            }
        }*/
        
        public static IEnumerator GetLobbies(Action<List<LobbyDto>> successCallback, Action failCallback)
        {
            var request = UnityWebRequest.Get(Api.LobbiesUrl);

            yield return request.SendWebRequest();

            var lobbyList = new List<LobbyDto>();
            if (request.isNetworkError || request.isHttpError)
            {
                failCallback();
            }
            else
            {
                var body = request.downloadHandler.text;
                var lobbies = JsonUtility.FromJson<LobbiesDto>(body);
                lobbyList = lobbies.lobbies;
            }

            successCallback(lobbyList);
        }


        public static IEnumerator CreateLobby(string name, Action<LobbyDto> successCallback, Action<string> failCallback)
        {
            var payload = JsonUtility.ToJson(new LobbyDto
            {
                name = name
            });

            var request = new UnityWebRequest(LobbiesUrl)
            {
                uploadHandler = new UploadHandlerRaw(Encoding.ASCII.GetBytes(payload)),
                downloadHandler = new DownloadHandlerBuffer(),
                method = UnityWebRequest.kHttpVerbPOST
            };

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                failCallback("Invalid request");
            }
            else
            {
                var body = request.downloadHandler.text;
                var account = JsonUtility.FromJson<LobbyDto>(body);
                successCallback(account);
            }
        }

        public static IEnumerator ClearLobby(Action callback)
        {
            Debug.Log("Clearing lobby");
            var request = new UnityWebRequest(LobbiesUrl)
            {
                method = UnityWebRequest.kHttpVerbDELETE
            };

            yield return request.SendWebRequest();

            callback();
        }
    }

}
