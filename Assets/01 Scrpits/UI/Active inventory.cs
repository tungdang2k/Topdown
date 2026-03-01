using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Activeinventory : MonoBehaviour
{
    private int m_activeSlotIndexNum = 0;
    private ActiveWeapon m_activeWeapon;

    private Dictionary<WeaponData, MonoBehaviour> spawnedWeapons;
    private void Awake()
    {
        spawnedWeapons = new Dictionary<WeaponData, MonoBehaviour>();
    }
    private void OnEnable()
    {
        PlayerController.OnInventorySlotSelected += UpdateInventory;
    }

    private void OnDisable()
    {
        PlayerController.OnInventorySlotSelected -= UpdateInventory;
    }

    private void Start()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();

        if (player != null)
        {
            RegisterPlayer(player);
        }
    }

    public void UpdateInventory(int numValue)
    {
        int index = numValue - 1;

        if (index < 0 || index >= transform.childCount)
        {
            Debug.LogWarning($"Invalid inventory slot: {numValue}");
            return;
        }
        ToggleActiveHighlight(index);
    }

    private void ToggleActiveHighlight(int indexNum)
    {

        m_activeSlotIndexNum = indexNum;
        foreach (Transform inventorySlot in this.transform)
        {

            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }
        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    public void RegisterPlayer(PlayerController player)
    {
        m_activeWeapon = player.GetComponentInChildren<ActiveWeapon>();
        if (m_activeWeapon == null)
        {
            return;
        }

        ToggleActiveHighlight(m_activeSlotIndexNum);
       
    }

    private void ChangeActiveWeapon()
    {

        if (m_activeWeapon == null) {
            return;
        }
        if (m_activeSlotIndexNum < 0 || m_activeSlotIndexNum >= transform.childCount) {
            return;
        }

        Transform slotTf = transform.GetChild(m_activeSlotIndexNum);
        if (slotTf == null) { 
            return;
        }

        IventorySlot slot = slotTf.GetComponentInChildren<IventorySlot>();
        if (slot == null || slot.GetWeaponData() == null)
        {
            m_activeWeapon.WeaponNull();
            return;
        }

        WeaponData data = slot.GetWeaponData();
        
        foreach (var w in spawnedWeapons.Values)
        {
            if (w != null)
                w.gameObject.SetActive(false);
        }

        if (!spawnedWeapons.ContainsKey(data))
        {
           
            GameObject obj = Instantiate(data.weaponPrefab, m_activeWeapon.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            MonoBehaviour weapon = obj.GetComponent<MonoBehaviour>();
            spawnedWeapons.Add(data, weapon);

            if (weapon is GunWeapon gun)
                gun.InitializeGun(data);
        }


        MonoBehaviour activeWeapon = spawnedWeapons[data];
        if (activeWeapon == null) return;

        activeWeapon.gameObject.SetActive(true);
        m_activeWeapon.NewWeapon(activeWeapon);
    }



}
