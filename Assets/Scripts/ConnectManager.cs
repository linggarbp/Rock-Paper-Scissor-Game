using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField usernameInput;

    [SerializeField] TMP_Text feedbackText;

    private void Start()
    {
        usernameInput.text = PlayerPrefs.GetString(PropertyName.Player.NickName, "");
    }

    public void ClickConnect()
    {
        feedbackText.text = "";
        if (usernameInput.text.Length < 3)
        {
            feedbackText.text = "Username min. 3 characters";
            return;
        }

        //save username
        PlayerPrefs.SetString(PropertyName.Player.NickName, usernameInput.text);
        PhotonNetwork.NickName = usernameInput.text;
        PhotonNetwork.AutomaticallySyncScene = true;

        //connect ke server
        PhotonNetwork.ConnectUsingSettings();
        feedbackText.text = "Connecting...";
    }

    //run ketika sudah connect
    public override void OnConnectedToMaster()
    {
        feedbackText.text = "Connected to Master";
        SceneManager.LoadScene("Lobby");
        StartCoroutine(LoadLevelAfterConnectedAndReady());
    }

    IEnumerator LoadLevelAfterConnectedAndReady()
    {
        while (PhotonNetwork.IsConnectedAndReady == false)
            yield return null;

        SceneManager.LoadScene("Lobby");
    }

    public void ClickPvE()
    {
        SceneManager.LoadScene("GamePlayCardBot");
    }

    public void ClickQuit()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
