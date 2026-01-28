using UnityEngine;
using UnityEngine.UI;

public class IventorySlot : MonoBehaviour
{
    [SerializeField] private Image m_iconImage;
    [SerializeField] private WeaponData m_weaponData;

    private void Start()
    {
        UpdateSlotUI();
    }
    public void UpdateSlotUI()
    {
        if (m_weaponData != null && m_weaponData.weaponIcon != null)
        {
            m_iconImage.sprite = m_weaponData.weaponIcon;
            m_iconImage.color = Color.white; 
            m_iconImage.gameObject.SetActive(true);
        }
        else
        {
            //m_iconImage.sprite = null;
            m_iconImage.gameObject.SetActive(false);
        }
    }
    public void SetWeapon(WeaponData newWeapon)
    {
        m_weaponData = newWeapon;
        UpdateSlotUI();
    }
    public WeaponData GetWeaponData()
    {
        return m_weaponData;
    }
}
