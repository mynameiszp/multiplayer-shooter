using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private GameObject skinsPanel;
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
