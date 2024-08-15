using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Zenject;

public class PlayerAttack : BasePlayerManager
{
    private Animator animator;
    [SerializeField] private List<AttackSO> combos = new();
    private int attackIndex = 0;
    private float attackTimer;
    [SerializeField] private float attackCoolDown;
    private Rigidbody playerRB;
    [SerializeField] private float AttackPushForce;
    [SerializeField] private float ComboResetCooldown;
    public bool IsAttacking { get; set; } = false;

    public bool CanAttack { get; set; } = true;

    private PlayerInteractObjects interactObjects;

    [Inject] private List<Weapon> weapons;


    void Start()
    {
        animator = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody>();
        interactObjects = GetComponent<PlayerInteractObjects>();
    }

    void Update()
    {
        if (PlayerHealthBar.isDie) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        EndAttack();
        Debug.Log(ReturnedWeapon());
    }


    public void Attack()
    {
        if (Time.time - attackTimer > attackCoolDown && CanAttack && interactObjects.HandObjectCount > 0)
        {
            interactObjects.IsWeaponHand = true;
            interactObjects.HandSwordTimer = 0f;
            animator.runtimeAnimatorController = combos[attackIndex].animatorOverrideController;
            ReturnedWeapon().damage = combos[attackIndex].attackDamage;
            IsAttacking = true;
            attackTimer = Time.time;
            CombatEffect();
            animator.Play("MeleeAttack", 0, 0);
            ReturnedWeapon().OpenCollision();
            attackIndex++;
            if (attackIndex >= combos.Count || Time.time - attackTimer > ComboResetCooldown)
            {
                attackIndex = 0;
            }
        }
    }

    private void EndAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsTag("OneHandAttack")) 
        {
            ReturnedWeapon().CloseCollision();
            IsAttacking = false;
        }
    }

    private void CombatEffect()
    {
        playerRB.AddForce(transform.forward * AttackPushForce, ForceMode.Force);
    }

    private Weapon ReturnedWeapon()
    {
        foreach (var weapon in weapons)
        {
            if (interactObjects.activeItem.activeSelf && weapon.tag == interactObjects.activeItem.tag)
            {
                return weapon;
            }
        }
        return null;
    }

}
