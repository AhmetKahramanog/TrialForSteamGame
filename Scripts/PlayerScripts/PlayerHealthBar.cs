using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : BasePlayerManager
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    [SerializeField] private float healAmount;

    [SerializeField] private Slider sliderHealthBar;

    [SerializeField] private GameObject getHitEffect;

    private Animator animator;

    [SerializeField] private Image panel;

    [SerializeField] private TextMeshProUGUI deathText;

    [SerializeField] private GameObject DeathUI;

    public static bool isDie = false;

    public static event Action OnHealthChanged;

    public static event Action OnHealthIncreased;

    [SerializeField] private GameObject deathScreenUI;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealhtBarSlider();
        animator = GetComponent<Animator>();
        sliderHealthBar.maxValue = maxHealth;
    }

    private void Update()
    {
        if (isDie) { return; }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Heal(healAmount);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDie) { return; }

        if (currentHealth > 0)
        {
            currentHealth -= damage;
            var effect = Instantiate(getHitEffect, transform.position + Vector3.up, Quaternion.identity);
            Destroy(effect, 1f);
            animator.SetTrigger("GetHit");
            UpdateHealhtBarSlider();
        }
        if (currentHealth <= 20f)
        {
            OnHealthChanged?.Invoke();
        }
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Death");
            DeathUI.SetActive(true);
            panel.CrossFadeAlpha(10f,100f,false);
            deathText.CrossFadeAlpha(200f, 10f, true);
            DeathScreen().Forget();
            Destroy(gameObject, 5f);
            isDie = true;
        }
    }

    private void Heal(float amount)
    {
        if (currentHealth > 0)
        {
            currentHealth += amount;
            if (currentHealth > 20f)
            {
                OnHealthIncreased?.Invoke();
            }
            UpdateHealhtBarSlider();
        }
    }

    private void UpdateHealhtBarSlider()
    {
        sliderHealthBar.value = currentHealth;
    }

    private async UniTaskVoid DeathScreen()
    {
        await UniTask.DelayFrame(600);
        panel.DOFade(300f, 3f);
        Cursor.lockState = CursorLockMode.None;
        deathScreenUI.SetActive(true);
    }

}
