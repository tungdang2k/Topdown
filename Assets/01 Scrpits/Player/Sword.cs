using System.Collections;
using UnityEngine;


public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private Transform m_swordPivot;
    [SerializeField] private int m_damage = 1;
    [SerializeField] private WeaponData m_weaponData;
    public float FireRate => m_weaponData.fireRate;
    private Animator m_animator;
    private float m_nextAttackTime = 0f;
    private const string SWORD_PARAM = "Attack";

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
      
    }
    void Update()
    {
        RotationSword();
     }
   
    public void Attack()
    {
        if(Time.time < m_nextAttackTime) return;
        m_animator.SetTrigger(SWORD_PARAM);
        AudioManager.Instance.PlaySwordSound();
        m_nextAttackTime = Time.time + m_weaponData.fireRate;
    }
    
    private void RotationSword()
    {

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorldPos - m_swordPivot.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Kiếm mặc định hướng UP → trừ 90 độ
        float baseAngle = angle - 90f;
        if (angle > 90f || angle < -90f)
        {

            m_swordPivot.localRotation = Quaternion.Euler(0f, 180f, -baseAngle);
        }
        else
        {
            m_swordPivot.localRotation = Quaternion.Euler(0f, 0f, baseAngle);

        }
    }

    public WeaponData GetWeaponData()
    {
        return m_weaponData;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ITakeDame takeDame = other.GetComponent<ITakeDame>();

        if (takeDame != null)
        {
            
            takeDame.TakeDame(m_damage, this.transform);
            
        }

    }

   
}

    

    

