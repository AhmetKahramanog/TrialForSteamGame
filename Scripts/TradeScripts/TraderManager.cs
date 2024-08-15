using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class TraderManager : BaseEnemy
{
    [SerializeField] private MarketManager marketManager;
    [SerializeField] private GameObject sword, shield;
    private float axeAttackCD;
    private float shieldBlockCD;
    private float axeAttackTimer;

    private float marketInventory;


    private float blockTimer;

    private static bool traderIsDie = false;

    private int layerIndex;
    private void Awake()
    {
        sword.SetActive(false);
        shield.SetActive(false);
        axeAttackCD = Random.Range(4f, 8.2f);
        shieldBlockCD = Random.Range(1.2f, 3.7f);
    }

    private void Start()
    {
        currentHealth = Health;
        knightEnemyHealthBar.maxValue = Health;
        knightEnemyHealthBar.value = Health;
        collider = GetComponent<CapsuleCollider>();
        enemyRB = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!isDie)
        {
            Move();
        }
        if (currentHealth <= 0)
        {
            traderIsDie = true;
        }
    }

    private void Move()
    {
        marketInventory = marketManager.goldInventory;
        if (marketInventory == 0 || traderIsDie || currentHealth < Health)
        {
            sword.SetActive(true);
            shield.SetActive(true);
            LookNormal();
            if (Distance() > attackRange)
            {
                animator.SetBool("IsMove", true);
                transform.Translate(transform.forward * followSpeed * Time.deltaTime, Space.World);
                animator.SetLayerWeight(layerIndex, 0);
                animator.SetBool("IsBlock", false);
                IsBlocked = false;
                blockTimer = 0f;
            }
            else
            {
                animator.SetBool("IsMove", false);
                GreatAxeAttack();
                ShieldBlock();
                LookOnBlock();
            }
        }
    }

    private void GreatAxeAttack()
    {
        axeAttackTimer += Time.deltaTime;
        if (axeAttackTimer > axeAttackCD)
        {
            animator.Play("SwordAttack");
            RaycastAttack(transform.position).Forget();
            axeAttackCD = Random.Range(4f, 7f);
            IsBlocked = false;
            axeAttackTimer = 0;
        }
    }

    private void ShieldBlock()
    {
        blockTimer += Time.deltaTime;
        if (blockTimer > shieldBlockCD)
        {
            animator.SetBool("IsBlock", true);
            layerIndex = animator.GetLayerIndex("TopLayer");
            animator.SetLayerWeight(layerIndex, 0);
            IsBlocked = true;
            shieldBlockCD = Random.Range(2f, 5f);
            blockTimer = 0;
        }
    }

    private void LookOnBlock()
    {
        CancelInvoke(nameof(LookNormal));
        var direction = player.transform.position - shield.transform.forward;
        direction.y = 0f;
        float duration = 0.3f;
        transform.DOLookAt(direction, duration).OnComplete(() =>
        {
            Invoke(nameof(LookNormal),shieldBlockCD);
        });
    }

    private void LookNormal()
    {
        var lookAtPlayer = player.transform.position;
        lookAtPlayer.y = 0f;
        transform.LookAt(lookAtPlayer);
    }

}
