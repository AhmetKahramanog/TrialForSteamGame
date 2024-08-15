using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum EnemyState
{
    Movement,
    Attack
}

public abstract class BaseEnemy : MonoBehaviour
{
    public float Health;
    protected float currentHealth;
    [SerializeField] private float attackCooldown;
    private float attackTimer;
    protected GameObject player;
    [SerializeField] protected float followSpeed = 2f;
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] protected float noticeRange = 5f;
    [SerializeField] protected float attackRange = 2f;
    protected Animator animator;
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private string deathAnimationName;
    private RaycastHit hit;
    [SerializeField] private GameObject hitEffect;
    protected PlayerMovement playerMovement;
    protected bool IsNoticed = false;
    protected string playerLayerName = "Player";
    [SerializeField] private float attackSphereRadius;

    public bool IsBlocked { get; set; }
    public bool isDie { get; set; } = false;


    [SerializeField] private float deathPushForce;
    [SerializeField] private GameObject deathEffect;

    protected Rigidbody enemyRB;
    protected CapsuleCollider collider;

    [SerializeField] protected Slider knightEnemyHealthBar;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    

    protected virtual void HandleMovement(string animationName)
    {
        if (Distance() < noticeRange && Distance() > attackRange)
        {
            var moveDirection = transform.forward * followSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDirection, followSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool(animationName, false);
        }
    }

    protected void HandleLookRotation()
    {
        if (Distance() < noticeRange)
        {
            var direction = player.transform.position - transform.position;
            direction.y = 0f;
            var smoothLook = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, smoothLook, turnSpeed * Time.deltaTime);
        }
    }

    protected void Attack(string animationName,string idleName)
    {
        if (Time.time - attackTimer > attackCooldown)
        {
            attackTimer = Time.time;
            RaycastAttack(transform.position).Forget();
            animator.SetTrigger(animationName);
        }
        if (Distance() <= attackRange)
        {
            animator.SetBool(idleName, false);
        }
    }


    protected float Distance()
    {
        float distance = Vector3.Distance(transform.position,player.transform.position);
        return distance;
    }


    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        knightEnemyHealthBar.value = currentHealth;
        if (currentHealth > 0)
        {
            var bloodEffect = Instantiate(hitEffect, transform.position + Vector3.up, Quaternion.identity);
            Destroy(bloodEffect, 0.5f);
        }
        else
        {
            var effect = Instantiate(deathEffect,transform.position + Vector3.up / 2, Quaternion.identity);
            var backDirection = (transform.position - player.transform.position).normalized;
            backDirection.y = 0f;
            enemyRB.AddForce(backDirection * deathPushForce, ForceMode.Impulse);
            animator.SetTrigger(deathAnimationName);
            isDie = true;
            GoldDownPosibility();
            Debug.Log("Enemy is death");
            collider.height = 0f;
            CameraController.Instance.NotifyDeath(this);
            Destroy(effect, 2f);
            knightEnemyHealthBar.value = 0;
            Destroy(gameObject, 5f);
        }
    }

    protected async UniTaskVoid RaycastAttack(Vector3 startPosition)
    {
        await UniTask.Delay(400);
        var direction = player.transform.position - transform.position;
        float distance = direction.magnitude;
        Vector3 castDirection = direction.normalized;
        bool isHit = Physics.SphereCast(startPosition, attackSphereRadius, castDirection, out hit, distance);
        if (isHit)
        {
            if (hit.transform.TryGetComponent<PlayerHealthBar>(out PlayerHealthBar playerHealthBar))
            {
                playerHealthBar.TakeDamage(attackDamage);
            }
        }
    }
    
    protected void NoticePlayer()
    {
        if (Distance() < noticeRange && CanSee() || playerMovement.MoveSpeed() > 1.5f)
        {
            IsNoticed = true;
        }
        if (Distance() > noticeRange)
        {
            IsNoticed = false;
        }
    }

    private float NoticeAngle()
    {
        var direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, direction);
        return angle;
    }

    private bool CanSee()
    {
        RaycastHit hitInfo;
        bool isHit = Physics.Linecast(transform.position + Vector3.up, player.transform.position + Vector3.up, out hitInfo);
        if (isHit)
        {
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer(playerLayerName) && NoticeAngle() < 45f)
            {
                return true;
            }
        }
        return false;
    }

    protected virtual void GoldDownPosibility()
    {

    }


}
