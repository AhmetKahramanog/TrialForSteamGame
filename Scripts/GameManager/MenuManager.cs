using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameNameText;

    private void Start()
    {
        gameNameText.transform.DOScale(2f, 7f).From(0f);
    }

    public void GameStartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void GameQuitButton()
    {
        Application.Quit();
    }
}
