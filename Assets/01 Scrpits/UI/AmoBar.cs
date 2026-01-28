using TMPro;
using UnityEngine;

public class AmoBar : MonoBehaviour
{

    [SerializeField] private TMP_Text ammoText;
    private IAmmoProvider ammo;


    public void RegisterWeapon(MonoBehaviour weapon)
    {
        ammo = weapon as IAmmoProvider;

        if (ammo == null || ammo.MagazineSize <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        UpdateUI();
    }

    private void Update()
    {
        if (ammo == null)
        {
            gameObject.SetActive(false);
            return;
        }

        bool noAmmoLeft = ammo.CurrentAmmo <= 0 && ammo.ReserveAmmo <= 0;
        gameObject.SetActive(!noAmmoLeft);

        if (!noAmmoLeft)
            UpdateUI();
    }

    private void UpdateUI()
    {
        ammoText.text = $"{ammo.CurrentAmmo} / {ammo.ReserveAmmo}";
    }
}
