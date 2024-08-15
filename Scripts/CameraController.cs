using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float mouseX, mouseY;
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 distance;
    [SerializeField] private float sensitivity;
    [SerializeField] private float cameraFollowSpeed;
    private bool IsCameraLock = false;
    private List<BaseEnemy> targets = new();
    [SerializeField] private float cameraTargetRange;
    [SerializeField] private float cameraLockReturnSpeed;
    [SerializeField] private Vector3 cameraTargetOffset;
    private BaseEnemy currentTarget;
    [SerializeField] private float rotationSpeed;
    private BaseEnemy closestEnemy = null;
    public static CameraController Instance;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Cursor.lockState = CursorLockMode.Locked;
        BaseEnemy[] foundEnemies = FindObjectsOfType<BaseEnemy>();
        foreach (var foundEnemy in foundEnemies)
        {
            targets.Add(foundEnemy);
        }
    }

    void Update()
    {
        Distance();
        if (Input.GetKeyDown(KeyCode.Q) && Distance() < cameraTargetRange)
        {
            CameraLockAction();
        }
        if (IsCameraLock)
        {
            CameraTarget();
            if (Input.GetKeyDown(KeyCode.F))
            {
                SwitchTarget();
            }
        }
        else
        {
            currentTarget = null;
        }
    }

    private void LateUpdate()
    {
        if (!IsCameraLock)
        {
            CameraRotateHandle();
        }
    }

    private void CameraRotateHandle()
    {
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY += Input.GetAxis("Mouse Y") * sensitivity;
        transform.position = Vector3.Lerp(transform.position, player.transform.position + distance, cameraFollowSpeed * Time.deltaTime * 10);
        transform.eulerAngles = new Vector3(mouseY, mouseX, 0f);
        mouseY = Mathf.Clamp(mouseY, -12f, 45f);
    }

    private void CameraLockAction()
    {
        IsCameraLock = !IsCameraLock;
    }

    private void CameraTarget()
    {
        if (currentTarget == null)
        {
            currentTarget = closestEnemy;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + cameraTargetOffset, cameraLockReturnSpeed * Time.deltaTime);
            SmoothLookAt(currentTarget.transform.position);
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        }
    }

    private void SmoothLookAt(Vector3 targetPosition)
    {
        var direction = targetPosition - transform.position;
        var smoothLook = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation,smoothLook,rotationSpeed * Time.deltaTime);
    }

    private float Distance()
    {
        float range = Vector3.Distance(ReturnedEnemy().transform.position, player.transform.position);
        return range;
    }

    private void SwitchTarget()
    {
        currentTarget = ReturnedEnemy();
    }

    private BaseEnemy ReturnedEnemy()
    {
        float closesDistance = Mathf.Infinity;
        foreach (var target in targets)
        {
            if (target == currentTarget)
            {
                continue;
            }
            float distance = Vector3.Distance(player.transform.position, target.transform.position);
            if (distance < closesDistance)
            {
                closesDistance = distance;
                closestEnemy = target;
            }
        }
        return closestEnemy;
    }

    public void NotifyDeath(BaseEnemy enemy)
    {
        targets.Remove(enemy);
    }
}
