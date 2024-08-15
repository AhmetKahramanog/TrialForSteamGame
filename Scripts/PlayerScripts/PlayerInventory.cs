using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform handWeaponSpawn;
    [SerializeField] private Transform backWeaponSpawn;
    [SerializeField] private List<GameObject> handWeapons;
    [SerializeField] private List<GameObject> backWeapons;
    private Dictionary<string, GameObject> handWeaponDict = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> backWeaponDict = new Dictionary<string, GameObject>();

    private void Start()
    {
        // El ve s�rt silahlar�n� dictionary'e ekleme
        foreach (var weapon in handWeapons)
        {
            handWeaponDict.Add(weapon.name, weapon);
        }
        foreach (var weapon in backWeapons)
        {
            backWeaponDict.Add(weapon.name, weapon);
        }
    }

    public void PickUpWeapon(GameObject weapon)
    {
        string weaponName = weapon.tag; // Silah t�r�n� tag ile belirle

        if (handWeaponDict.ContainsKey(weaponName))
        {
            ActivateWeapon(handWeaponDict[weaponName], handWeaponSpawn);
        }

        if (backWeaponDict.ContainsKey(weaponName))
        {
            ActivateWeapon(backWeaponDict[weaponName], backWeaponSpawn);
        }

        Destroy(weapon); // Yerden al�nan silah� yok et
    }

    private void ActivateWeapon(GameObject weapon, Transform parent)
    {
        // Mevcut aktif silahlar� devre d��� b�rak
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
        }

        // Yeni silah� aktif hale getir
        weapon.SetActive(true);
    }

    void Update()
    {
        // Silah de�i�tirme
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon("Sword"); // �lk silah� ku�an
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon("Axe"); // �kinci silah� ku�an
        }
    }

    private void EquipWeapon(string weaponName)
    {
        if (handWeaponDict.ContainsKey(weaponName))
        {
            ActivateWeapon(handWeaponDict[weaponName], handWeaponSpawn);
        }

        if (backWeaponDict.ContainsKey(weaponName))
        {
            ActivateWeapon(backWeaponDict[weaponName], backWeaponSpawn);
        }
    }
}
