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
        ToggleActiveHighlight(0);
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

        ChangeActiveWeapon();
    }

    //private void ChangeActiveWeapon()
    //{
    //    if (m_activeWeapon == null)
    //        return;

    //    if (m_activeSlotIndexNum < 0 || m_activeSlotIndexNum >= transform.childCount)
    //        return;

    //    var slot = transform.GetChild(m_activeSlotIndexNum).GetComponentInChildren<IventorySlot>();

    //    if (slot == null || slot.GetWeaponData() == null)
    //    {
    //        m_activeWeapon.WeaponNull();
    //        return;
    //    }

    //    if (m_activeWeapon.CurrentActiveWeapon != null)
    //    {
    //        Destroy(m_activeWeapon.CurrentActiveWeapon.gameObject);
    //    }

    //    WeaponData data = slot.GetWeaponData();

    //    GameObject newWeapon = Instantiate(
    //        data.weaponPrefab,
    //        m_activeWeapon.transform.position,
    //        Quaternion.identity,
    //        m_activeWeapon.transform
    //    );
    //    GunWeapon gunScript = newWeapon.GetComponent<GunWeapon>();

    //    if (gunScript != null)
    //    {

    //        gunScript.InitializeGun(data);
    //    }

    //    m_activeWeapon.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    //}
    private void ChangeActiveWeapon()
    {
        if (m_activeWeapon == null) return;
        if (m_activeSlotIndexNum < 0 || m_activeSlotIndexNum >= transform.childCount) return;

        Transform slotTf = transform.GetChild(m_activeSlotIndexNum);
        if (slotTf == null) return;

        IventorySlot slot = slotTf.GetComponentInChildren<IventorySlot>();
        if (slot == null || slot.GetWeaponData() == null)
        {
            m_activeWeapon.WeaponNull();
            return;
        }

        WeaponData data = slot.GetWeaponData();

        // Tắt tất cả weapon đã spawn
        foreach (var w in spawnedWeapons.Values)
        {
            if (w != null)
                w.gameObject.SetActive(false);
        }

        // Spawn nếu chưa có
        if (!spawnedWeapons.ContainsKey(data))
        {
            GameObject obj = Instantiate(data.weaponPrefab, m_activeWeapon.transform);

            // Lấy weapon gốc (GunWeapon, MeleeWeapon, SkillWeapon...)
            MonoBehaviour weapon = obj.GetComponent<MonoBehaviour>();
            spawnedWeapons.Add(data, weapon);

            // Nếu là súng thì init ammo
            if (weapon is GunWeapon gun)
                gun.InitializeGun(data);
        }

        // Bật weapon được chọn
        MonoBehaviour activeWeapon = spawnedWeapons[data];
        if (activeWeapon == null) return;

        activeWeapon.gameObject.SetActive(true);
        m_activeWeapon.NewWeapon(activeWeapon);
    }



}
