using System.Collections;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private AmoBar m_AmoBar;
    public MonoBehaviour CurrentActiveWeapon { get; private set; }
    private PlayerController m_PlayerController;
    private IWeapon m_currentWeapon;
    private float m_fireCooldown;
    private bool m_isAttack;

    private void Awake()
    {
        m_PlayerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        if (m_currentWeapon == null)
            return;

        if (!m_PlayerController.IsAttackHeld)
            return;

        m_fireCooldown -= Time.deltaTime;

        if (m_fireCooldown <= 0f)
        {
            if (GameManager.Instance.CurrentState != GameStateType.Gameplay)
            {
                return;
            }
            m_currentWeapon.Attack();
            m_fireCooldown = m_currentWeapon.FireRate;
        }
    }

    public void ReloadCurrentWeapon()
    {
        if (m_currentWeapon == null) return;

        if (m_currentWeapon is GunWeapon gun)
        {
            gun.Reload();
        }
    }

    public void RegisterPlayer(PlayerController player)
    {
        m_PlayerController = player;
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        m_currentWeapon = newWeapon as IWeapon;

        m_AmoBar.RegisterWeapon(newWeapon);

    }

    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
        m_currentWeapon = null;
    }

    public void ToggleAttack(bool value)
    {
        m_isAttack = value;
    }

}
