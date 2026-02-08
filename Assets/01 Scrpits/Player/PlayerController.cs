using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public static Action<int> OnInventorySlotSelected;
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public bool IsAttackHeld { get; private set; }


    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float smoothTime = 0.1f;


    private KnockBack m_knockBack;
    private Vector2 m_moveInput;
    private Rigidbody2D m_rb;
    private Animator m_animator;
    private Camera m_mainCamera;
    private SpriteRenderer m_spriteRenderer;
    private Vector2 m_currentBlendVector;
    private Vector2 m_blendVelocity = Vector2.zero;
    private InputSystem_Actions m_actions;
    private PlayerDash m_dash;
    private ActiveWeapon m_activeWeapon;
    private HealthControler m_healthControler;



    private const string INPUT_X_PARAM = "BlendX";
    private const string INPUT_Y_PARAM = "BlendY";
    private const string WALK_PARAM = "walk";

     void Awake()
    {
       

        //GameManager.Instance.RegisterPlayer(transform);
        //GameManager.Instance.RegisterPlayerHealth(GetComponent<HealthControler>());
        //ActiveWeapon.Instance.RegisterPlayer(this);
        m_activeWeapon = GetComponentInChildren<ActiveWeapon>();
        
        m_rb = GetComponent<Rigidbody2D>();
        m_animator = GetComponentInChildren<Animator>();
        m_mainCamera = Camera.main;
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_knockBack = GetComponent<KnockBack>();
        m_dash = GetComponent<PlayerDash>();
        m_healthControler = GetComponent<HealthControler>();
        
    }

    private void OnEnable()
    {
        if (m_actions == null)
            m_actions = new InputSystem_Actions();
        
        m_actions.Player.Enable();
        m_actions.Inventory.Enable();

        m_actions.Player.Move.performed += OnMove;
        m_actions.Player.Move.canceled += OnMove;

        m_actions.Player.Attack.started += ctx => IsAttackHeld = true;
        m_actions.Player.Attack.canceled += ctx => IsAttackHeld = false;

        m_actions.Player.Jump.started += Dash;

        m_actions.Inventory.Keyboard.performed += Inventory;

    }

 
    private void OnDisable()
    {
        m_actions.Player.Disable();
        m_actions.Inventory.Disable();
        m_actions.Player.Move.performed -= OnMove;
        m_actions.Player.Move.canceled -= OnMove;

        m_actions.Player.Attack.started -= ctx => IsAttackHeld = true;
        m_actions.Player.Attack.canceled -= ctx => IsAttackHeld = false;

        m_actions.Player.Jump.started -= Dash;

        m_actions.Inventory.Keyboard.performed -= Inventory;

    }
    void Start()
    {
        Activeinventory inventory = FindFirstObjectByType<Activeinventory>();
        if (inventory != null)
        {
            inventory.RegisterPlayer(this);
        }

    }

    void Update()
    {
        if (GameManager.Instance.IsPaused)
            return;
        
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            m_activeWeapon.ReloadCurrentWeapon();
        }
        UpdateAnimationDirection();
        PlayerFlip();
    }

    private void FixedUpdate()
    {
        if (m_knockBack.gettingKnockBack) return;
        if (GameManager.Instance.CurrentState != GameStateType.Gameplay)
            return;
        m_rb.MovePosition(m_rb.position + m_moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    private void UpdateAnimationDirection()
    {

        Vector2 targetDirectionForAnimation;


        Vector2 mouseDirection = Vector2.zero;
        if (m_mainCamera != null)
        {
            Vector3 mouseWorld = m_mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = transform.position.z;

             mouseDirection = (mouseWorld - transform.position).normalized;
        }


        // QUYẾT ĐỊNH HƯỚNG CHO ANIMATION:
        if (m_moveInput.sqrMagnitude > 0.05f) // A. Nếu đang di chuyển
        {
            targetDirectionForAnimation = m_moveInput.normalized;
        }
        else // B. Nếu đang đứng yên
        {
            
            targetDirectionForAnimation = mouseDirection;
          
        }


        if (m_animator != null)
        {

            m_currentBlendVector.x = Mathf.SmoothDamp(
                m_currentBlendVector.x,
                targetDirectionForAnimation.x,
                ref m_blendVelocity.x,
                smoothTime
            );

            m_currentBlendVector.y = Mathf.SmoothDamp(
                m_currentBlendVector.y,
                targetDirectionForAnimation.y,
                ref m_blendVelocity.y,
                smoothTime
            );

            m_animator.SetFloat(INPUT_X_PARAM, m_currentBlendVector.x);
            m_animator.SetFloat(INPUT_Y_PARAM, m_currentBlendVector.y);
            float inputX = targetDirectionForAnimation.x; 

            if (inputX > 0.05f) 
            {
                m_spriteRenderer.flipX = false;
            }
            else if (inputX < -0.05f) 
            {
                m_spriteRenderer.flipX = true;
            }
        }

        
    }

    private void PlayerFlip()
    {
        if (m_spriteRenderer != null)
        {
            float inputX = m_moveInput.x;

            if (inputX > 0.05f) 
            {
              
                m_spriteRenderer.flipX = false;
            }
            else if (inputX < -0.05f) 
            {
               
                m_spriteRenderer.flipX = true;
            }
         
        }
        if (m_animator != null)
        {

            float currentSpeed = m_moveInput.magnitude;

            m_animator.SetBool(WALK_PARAM, currentSpeed > 0.05f);
        }
    }


    

     private void OnMove(InputAction.CallbackContext context)
    {
        m_moveInput = context.ReadValue<Vector2>();
    }

    private void Dash(InputAction.CallbackContext context)
    {
       
        m_dash.Dash();
    }

    private void Inventory (InputAction.CallbackContext context)
    {
        
        if (!context.performed) return;

        string path = context.control.path;
        if (int.TryParse(path[^1].ToString(), out int slot))
        {
            OnInventorySlotSelected?.Invoke(slot);
        }
    }


}
