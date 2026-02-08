using UnityEngine;

public class WeaponSpriteSwitcher : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Weapon Sprites")]
    public Sprite upSprite;     
    public Sprite downSprite;   
    public Sprite sideSprite;   
    public Sprite diagUpSprite; 
    public Sprite diagDownSprite; 


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

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorldPosition - transform.position;


        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += 360;
        }


        if (angle >= 337.5f || angle < 22.5f)
        {
            sr.sprite = sideSprite;
        }
        else if (angle >= 22.5f && angle < 67.5f) 
        {
            sr.sprite = diagUpSprite;
        }
        else if (angle >= 67.5f && angle < 112.5f) 
        {
            sr.sprite = upSprite;
        }
        else if (angle >= 112.5f && angle < 157.5f) 
        {
            sr.sprite = diagUpSprite; 
        }
        else if (angle >= 157.5f && angle < 202.5f) 
        {
            sr.sprite = sideSprite;
        }
        else if (angle >= 202.5f && angle < 247.5f) 
        {
            sr.sprite = diagDownSprite;
        }
        else if (angle >= 247.5f && angle < 292.5f) 
        {
            sr.sprite = downSprite;
        }
        else if (angle >= 292.5f && angle < 337.5f) 
        {
            sr.sprite = diagDownSprite;
        }
    }
}