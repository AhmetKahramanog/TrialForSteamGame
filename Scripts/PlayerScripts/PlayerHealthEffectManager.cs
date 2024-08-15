using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthEffectManager : MonoBehaviour
{
    [SerializeField] private Image redScreen;
    [SerializeField] private GameObject redScreenUI;
    [SerializeField] private float flashDuration = 1f;
    private bool isRedScreenOpen = false;
    [SerializeField] private ParticleSystem healParticleEffect;

    private void Awake()
    {
        healParticleEffect.Stop();
        redScreenUI.SetActive(false);
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        PlayerHealthBar.OnHealthChanged += PlayerHealthBar_OnHealthChanged;
        PlayerHealthBar.OnHealthIncreased += PlayerHealthBar_OnHealthIncreased;
    }


    private void OnDisable()
    {
        PlayerHealthBar.OnHealthChanged -= PlayerHealthBar_OnHealthChanged;
        PlayerHealthBar.OnHealthIncreased -= PlayerHealthBar_OnHealthIncreased;
    }

    private void PlayerHealthBar_OnHealthChanged()
    {
        //redScreenUI.SetActive(true);
        //redScreen.CrossFadeAlpha(7f, 30f, false);
        StartFlash();
    }

    private void PlayerHealthBar_OnHealthIncreased()
    {
        isRedScreenOpen = false;
        redScreenUI.SetActive(isRedScreenOpen);
        healParticleEffect.Play();
        // can doldurma particle iþlemleri
        // ses efektleri
    }

    private async void StartFlash()
    {
        isRedScreenOpen = true;

        while (isRedScreenOpen)
        {
            await FlashRedScreen();
        }
    }

    private async UniTask FlashRedScreen()
    {
        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            redScreenUI.SetActive(isRedScreenOpen);
            redScreen.color = new Color(1f, 0f, 0f, Mathf.Lerp(0f, 0.5f, elapsedTime / flashDuration));
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
        elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            redScreen.color = new Color(1f,0f,0f, Mathf.Lerp(0.5f,0f,elapsedTime / flashDuration));
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }
}
