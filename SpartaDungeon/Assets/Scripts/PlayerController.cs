using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpPower;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    public float minYLook;
    public float maxYLook;
    private float camCurXRot;
    private float camCurYRot;
    public float lookSensitivity;

    // 점프게이지
    [Header("JumpGauge")]
    public float usejumpgauge;
    private bool possiblejump;

    // 벽타기
    [Header("Climb")]
    public float wallCheckDistance = 0.5f;
    public float climbForce;
    public LayerMask wallLayerMask;

    

    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;

    public Action inventory;
    
    private Rigidbody _rigidbody;
    private PlayerCondition condition; 
    private Condition _condition;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        condition = GetComponent<PlayerCondition>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void LateUpdate()
    {
        if(canLook)
        {
            CameraLook();
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    private void Move()
    {
        Vector3 camForward = cameraContainer.forward;
        Vector3 camRight = cameraContainer.right;
        Vector3 dir = camForward * curMovementInput.y + camRight * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;
        _rigidbody.velocity = dir;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurYRot += mouseDelta.x * lookSensitivity;
        // Debug.Log($"마우스델타값 : {mouseDelta}");
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        camCurYRot = Mathf.Clamp(camCurYRot, minYLook, maxYLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, camCurYRot, 0);
    }

    // 땅일때는 Jump, 벽일때는 Climb
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && IsGrounded() && condition.possiblejump(usejumpgauge))
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            condition.MinusJumpGauge(usejumpgauge);
        }
        else if(context.phase == InputActionPhase.Started && IsWall())
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(Vector3.up * climbForce, ForceMode.Impulse);
        }
    }
    
    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.5f) + (transform.up * 0.05f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };
        for (int i = 0; i< rays.Length; i++)
        {
            if(Physics.Raycast(rays[i], 0.6f, groundLayerMask))
            {
                Debug.DrawRay(rays[i].origin, rays[i].direction * 1f, Color.red);
                return true;
            }
        }
        return false;     
    }


    // wall인지 체크
    private bool IsWall()
    {
        Ray ray = new Ray(transform.position + (transform.forward * 0.5f) + (transform.up * 0.05f), Vector3.forward);
        if(Physics.Raycast(ray, wallCheckDistance, wallLayerMask))
        {
            return true;
        }
        return false;
    }

    public void OnInventoryButton(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }


}
