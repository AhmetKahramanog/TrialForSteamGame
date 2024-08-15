using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class PlayerInteractObjects : MonoBehaviour,IInteract
{
    private bool isInteract = false;
    private GameObject interactedObject;
    public int HandObjectCount = 0;
    private Animator animator;
    public static event Action OnUpdadetItemIcon;

    public GameObject activeItem { get; private set; }

    public float HandSwordTimer { get; set; } = 0f;

    private int currentObjectIndex = 0;
    [SerializeField] private int drawSwordCooldown = 3;
    [SerializeField] private List<GameObject> handWeapons,backWeapons;

    [SerializeField] private GameObject showInteractTextUI;

    public bool IsWeaponHand { get; set; } = false;

    private readonly string[] weaponNames = { "Sword", "Axe" };

    public CancellationTokenSource cancellationTokenSource;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (isInteract && Input.GetKeyDown(KeyCode.E) && !Inventory.Instance.IsInventoryFull())
        {
            Interact();
        }
        ChangeWeaponSlot();
        if (IsWeaponHand)
        {
            HandWeaponOn(currentObjectIndex);
            backWeapons.ForEach(item => item.SetActive(false));
            DrawSword(Time.deltaTime);
        }
        else
        {
            BackWeaponOn(currentObjectIndex);
            handWeapons.ForEach(item => item.SetActive(false));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.transform.CompareTag("Sword"))
        //{
        //    isInteract = true;
        //    interactedObject = other.gameObject;
        //}
        //if (other.transform.CompareTag("Axe"))
        //{
        //    isInteract = true;
        //    interactedObject = other.gameObject;
        //}
        foreach (var weaponTag in weaponNames)
        {
            if (other.transform.CompareTag(weaponTag))
            {
                showInteractTextUI.SetActive(true);
                isInteract = true;
                interactedObject = other.gameObject;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //if (other.transform.CompareTag("Sword"))
        //{
        //    CloseInteractWeapons();
        //}
        //if (other.transform.CompareTag("Axe"))
        //{
        //    CloseInteractWeapons();
        //}
        foreach (var weapontTag in weaponNames)
        {
            if (other.transform.CompareTag(weapontTag))
            {
                showInteractTextUI.SetActive(false);
                CloseInteractWeapons();
            }
        }
    }

    public void Interact()
    {
        Inventory.Instance.AddWeapon(interactedObject);
        interactedObject.SetActive(false);
        HandObjectCount++;
        CloseInteractWeapons();
        if (interactedObject == null)
        {
            OnUpdadetItemIcon?.Invoke();
        }
    }


    public void DrawSword(float drawSwordTimer)
    {
        HandSwordTimer += drawSwordTimer;
        if (HandSwordTimer > drawSwordCooldown)
        {
            animator.Play("SwordDraw");
            HandSwordTimer = 0f;
            IsWeaponHand = false;
        }
    }

    private void ChangeWeaponSlot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentObjectIndex = 0;
            IsWeaponHand = true;
            HandWeaponOn(currentObjectIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentObjectIndex = 1;
            IsWeaponHand = true;
            HandWeaponOn(currentObjectIndex);
        }
    }

    private void CloseInteractWeapons()
    {
        showInteractTextUI.SetActive(false);
        isInteract = false;
        interactedObject = null;
    }

    public void HandWeaponOn(int index)
    {
        foreach (var item in handWeapons)
        {
            if (Inventory.Instance.weapons[index].tag == item.tag)
            {
                item.SetActive(true);
                activeItem = item;
            }
            else
            {
                item.SetActive(false);
            }
        }
    }
    
    private void BackWeaponOn(int index)
    {
        try
        {
            foreach (var item in backWeapons)
            {
                if (Inventory.Instance.weapons[index].tag == item.tag)
                {
                    item.SetActive(true);
                }
                else
                {
                    item.SetActive(false);
                }
            }
        }
        catch (ArgumentOutOfRangeException e)
        {
            //Debug.Log(e.Message);
        }
    }


}
