using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class KnightEnemy : BaseEnemy
{
    private EnemyState currentState;

    [SerializeField] private float radius = 5f;
    [SerializeField] private float patrolSpeed = 2f;
    private bool isPatrolling = false;

    [Inject] private GoldManager goldManager;

    private int goldDown;

    private Vector3 initialPos;


    void Start()
    {
        animator = GetComponent<Animator>();
        enemyRB = GetComponent<Rigidbody>();
        currentHealth = Health;
        knightEnemyHealthBar.maxValue = Health;
        knightEnemyHealthBar.value = Health;
        initialPos = transform.position;
        collider = GetComponent<CapsuleCollider>();
        goldDown = Random.Range(0, 9);
        Debug.Log(goldManager);
    }

    void Update()
    {
        if (!isDie)
        {
            NoticePlayer();
            if (IsNoticed)
            {
                StateHandle();
                HandleLookRotation();
            }
            else
            {
                animator.SetBool("IsMove", false);
            }
        }
    }

    private void StateHandle()
    {
        switch (currentState)
        {
            case EnemyState.Movement:
                //HandleMovement("IsMove");
                if (!isPatrolling)
                {
                    Patrol().Forget();
                }
                break;
            case EnemyState.Attack:
                Attack("Attack","IsMove");
                break;
        }

        if (Distance() <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if(Distance() < noticeRange)
        {
            currentState = EnemyState.Movement;
        }
    }

    private async UniTaskVoid Patrol()
    {
        isPatrolling = true;
        while (Distance() >= attackRange && Distance() < noticeRange && !isDie)
        {
            Vector3 randomPoint = GetRandomInCircle(initialPos, radius);
            await Arrive(randomPoint, patrolSpeed);
            await UniTask.Delay(1200);
        }
        isPatrolling = false;
    }

    private Vector3 GetRandomInCircle(Vector3 center,float radius)
    {
        Vector2 randomPoint = Random.insideUnitSphere.normalized * radius;
        return new Vector3(center.x + randomPoint.x,center.y,center.z + randomPoint.y);
    }

    private async UniTask Arrive(Vector3 target,float speed)
    {
        while (Vector3.Distance(transform.position,target) > 0.2f)
        {
            Vector3 direction = (target - transform.position).normalized;
            var movementDirection = direction * speed * Time.deltaTime;
            var calculatedDirection = direction * speed;
            transform.position += movementDirection;
            animator.SetFloat("HorizontalMovement", calculatedDirection.x);
            animator.SetFloat("VerticalMovement",calculatedDirection.z);
            await UniTask.Yield();
        }
        animator.SetFloat("HorizontalMovement", 0);
        animator.SetFloat("VerticalMovement", 0);
    }

    protected override void GoldDownPosibility()
    {
        goldManager.GoldKeeper(goldDown);
    }
}
