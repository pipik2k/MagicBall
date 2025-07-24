using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button backtoMainMenu;
    public GameObject GameEndPanel;
    void Start()
    {
        backtoMainMenu.onClick.AddListener(BackToMenu);
        GameManager.Instance.onGameEnd += ActiveEndPanel;
    }

    private void ActiveEndPanel()
    {
        GameEndPanel.SetActive(true);
    }

    private void BackToMenu()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

}
