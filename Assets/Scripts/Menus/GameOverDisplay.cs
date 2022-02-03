using System;
using Mirror;
using TMPro;
using UnityEngine;


public class GameOverDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverDisplayParent;
    [SerializeField] private TextMeshProUGUI _winnerNameText;
    
    private void Start()
    {
        GameOverHandler.OnClientGameOver += ClientHandleGameOver;
    }

    private void OnDestroy()
    {
        GameOverHandler.OnClientGameOver -= ClientHandleGameOver;
    }

    public void LeaveGame()
    {
        if(NetworkServer.active && NetworkClient.isConnected)
            NetworkManager.singleton.StopHost();
        else
            NetworkManager.singleton.StopClient();
    }

    private void ClientHandleGameOver(string winner)
    {
        _winnerNameText.text = $"{winner} Has Won!!!";

        _gameOverDisplayParent.SetActive(true);
    }
}