using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class LobbyHook : MonoBehaviour
    {
        public void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            var lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
            var player = gamePlayer.GetComponent<Player>();

            //player.PlayerName = lobby.PlayerName;
        }
    }
}
