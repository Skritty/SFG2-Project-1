using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    #region Animation Events
    public event Action Idle = delegate { };
    public event Action StartRunning = delegate { };
    public event Action Jump = delegate { };
    public event Action Fall = delegate { };
    public event Action Land = delegate { };
    public event Action Sprint = delegate { };
    public event Action Die = delegate { };
    public event Action Hurt = delegate { };
    #endregion
    #region Animation Booleans
    bool _isGrounded = true;
    bool _isMoving = false;
    bool _isSprinting = false;
    bool _isJumping = false;
    bool _beganFall = false;
    bool _isDead = false;
    #endregion

    [SerializeField]
    public CharacterController controller;
    [SerializeField]
    Camera cam;

    #region Stats
    [Header("Stats")]
    [SerializeField]
    float gravity = 9.8f;
    [SerializeField]
    float movementSpeed = 5;
    [SerializeField]
    float sprintSpeed = 10;
    [SerializeField] [Tooltip("How much does the character move per second?")]
    float jumpForce = 10;
    [SerializeField] [Tooltip("How long is the jump force applied for?")]
    float jumpTime = .4f;
    [SerializeField]
    Vector2 cameraSensitivity = new Vector2(1, 1);
    #endregion

    #region Abilities
    [Header("Abilities")]
    [SerializeField]
    Transform magicSpawnFront;
    [SerializeField]
    AbilityLoadout loadout;
    [SerializeField]
    Ability startingAbility;
    [SerializeField]
    Ability _newAbilityToTest;
    #endregion

    public bool scrollCam = false;
    public Vector3 down = Vector3.down;
    Transform abilityTarget;
    public Vector3 cameraOffset;
    float _airTime = 0;
    bool cooldown = false;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (startingAbility != null) loadout?.EquipAbility(startingAbility);
        abilityTarget = transform;
        cameraOffset = cam.transform.position - controller.transform.position;
    }

    private void Start()
    {
        Idle?.Invoke();
    }

    private void Update()
    {
        CameraRotation();
        if (_isDead) return;
        Inputs();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Inputs()
    {
        if (Input.GetKey(KeyCode.Space) && _isGrounded)
        {
            _isJumping = true;
            Jump?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_beganFall && _isMoving)
        {
            _isSprinting = true;
            Sprint?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !_beganFall)
        {
            _isSprinting = false;
            if (_isMoving)
                StartRunning?.Invoke();
            else Idle?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !cooldown)
        {
            loadout.UseEquppiedAbility(transform, abilityTarget);
            StartCoroutine(Cooldown());
        }
        //if (Input.GetKeyDown(KeyCode.F)) loadout.EquipAbility(_newAbilityToTest);
        if (Input.GetKeyDown(KeyCode.Tab)) abilityTarget = transform;
    }

    private IEnumerator Cooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(.1f);
        cooldown = false;
    }

    private void Movement()
    {
        _isGrounded = controller.isGrounded;
        if (!_isGrounded && _airTime > 0.3f && !_isJumping && !_beganFall)
        {
            Fall?.Invoke();
            _beganFall = true;
        }
        if (!_isGrounded) _airTime += Time.fixedDeltaTime;
        else
        {
            _airTime = 0;
            _isJumping = false;
            if (_beganFall) Land?.Invoke();
            _beganFall = false;
        }
        if (_isJumping && jumpTime < _airTime)
        {
            _isJumping = false;
            Fall?.Invoke();
        }

        Vector3 camForward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
        Vector3 dir = camForward * Input.GetAxis("Vertical") * (scrollCam ? 0 : 1) + cam.transform.right * Input.GetAxis("Horizontal");
        if (_isDead) dir = Vector3.zero;
        Vector3 toMove = dir * (_isSprinting ? sprintSpeed : movementSpeed) + down * gravity * _airTime * (_isGrounded ? 0 : 1) + (_isJumping ? -down : Vector3.zero) * jumpForce * (1 - (_airTime / jumpTime) / 2);
        if (scrollCam) toMove = Vector3.ProjectOnPlane(toMove, cam.transform.forward);

        if (_isMoving) controller.transform.rotation = Quaternion.LookRotation(camForward * (Input.GetAxis("Vertical") < 0 ? -1 : 1), Vector3.up) * Quaternion.AngleAxis(90 * Input.GetAxis("Horizontal") * (Input.GetAxis("Vertical") < 0 ? -1 : 1), Vector3.up);

        controller.Move(Time.fixedDeltaTime * toMove);
        cam.transform.position = controller.transform.position + cameraOffset;

        if(down != Vector3.down)
        {
            controller.enabled = false;
            transform.position = Vector3.ProjectOnPlane(transform.position, Camera.main.transform.forward);
            controller.enabled = true;
        }

        if (dir.magnitude > 0)
        {
            CheckIfStartedMoving();
        }
        else
        {
            CheckIfStoppedMoving();
        }
    }

    private void CameraRotation()
    {
        if (cam == null || !Input.GetMouseButton(1) || scrollCam) return;
        cam.transform.RotateAround(controller.transform.position, Vector3.up, Input.GetAxis("Mouse X") * cameraSensitivity.x);
        cam.transform.RotateAround(controller.transform.position, cam.transform.right, Input.GetAxis("Mouse Y") * cameraSensitivity.y);
        cameraOffset = cam.transform.position - controller.transform.position;
    }

    private void CheckIfStartedMoving()
    {
        if (_isJumping || _beganFall || _isSprinting) return;
        if (!_isMoving)
        {
            StartRunning?.Invoke();
        }
        _isMoving = true;
    }

    private void CheckIfStoppedMoving()
    {
        if (_isJumping || _beganFall) return;
        if (_isMoving)
        {
            Idle?.Invoke();
        }
        _isMoving = false;
    }

    public void DoDie()
    {
        _isDead = true;
        Die?.Invoke();
    }

    public void Oof()
    {
        Hurt?.Invoke();
    }

    
}
