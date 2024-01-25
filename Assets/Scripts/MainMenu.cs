using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject skinsPanel;
    public void OnHost()
    {
        SceneManager.LoadScene("Game");
    }
    public void OnJoin()
    {
        SceneManager.LoadScene("Game");
    }
    private void Awake()
    {
        skinsPanel.SetActive(false);
    }
    public void OnSkins()
    {
        skinsPanel.SetActive(true);
    }
    public void OnExit()
    {
        Application.Quit();
    }
    public void OnSaveSkins()
    {
        //save skin
        skinsPanel.SetActive(false);
    }
}
