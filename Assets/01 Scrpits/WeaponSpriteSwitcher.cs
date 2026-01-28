using UnityEngine;

public class WeaponSpriteSwitcher : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Weapon Sprites")]
    public Sprite upSprite;     // flamethrower_up
    public Sprite downSprite;   // flamethrower_down
    public Sprite sideSprite;   // flamethrower_side
    public Sprite diagUpSprite; // flamethrower_diag_up (nếu có)
    public Sprite diagDownSprite; // flamethrower_diag_down (nếu có)

    // Lấy góc quay (độ) từ script WeaponRotation
    private float currentAngle;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("SpriteRenderer not found on the weapon object!");
            enabled = false;
        }
    }

    void Update()
    {
        // 1. Tính toán Vị trí Chuột và Hướng (giống như script xoay)
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorldPosition - transform.position;

        // 2. Tính toán Góc (Angle) và chuẩn hóa về 0-360 độ hoặc -180 đến 180
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Chuẩn hóa góc về [0, 360]
        if (angle < 0)
        {
            angle += 360;
        }

        // 3. Thay đổi Sprite dựa trên Góc (Sử dụng 8 hướng cơ bản)
        // Chia 360 độ thành các khu vực: 0 (side/right), 90 (up), 180 (left), 270 (down)

        if (angle >= 337.5f || angle < 22.5f) // Right (Side)
        {
            sr.sprite = sideSprite;
        }
        else if (angle >= 22.5f && angle < 67.5f) // Up-Right (Diagonal)
        {
            sr.sprite = diagUpSprite;
        }
        else if (angle >= 67.5f && angle < 112.5f) // Up
        {
            sr.sprite = upSprite;
        }
        else if (angle >= 112.5f && angle < 157.5f) // Up-Left (Diagonal)
        {
            sr.sprite = diagUpSprite; // Vẫn dùng diag_up, nhưng script xoay sẽ lật nó
        }
        else if (angle >= 157.5f && angle < 202.5f) // Left (Side)
        {
            sr.sprite = sideSprite; // Vẫn dùng side, nhưng script xoay sẽ lật nó
        }
        else if (angle >= 202.5f && angle < 247.5f) // Down-Left (Diagonal)
        {
            sr.sprite = diagDownSprite;
        }
        else if (angle >= 247.5f && angle < 292.5f) // Down
        {
            sr.sprite = downSprite;
        }
        else if (angle >= 292.5f && angle < 337.5f) // Down-Right (Diagonal)
        {
            sr.sprite = diagDownSprite;
        }
    }
}