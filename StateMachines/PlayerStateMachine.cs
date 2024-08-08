using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
      PlayerInput  _playerInput;
      Vector2 _currentMovementInput;
      public Vector3 _currentMovement;
      public Vector3 _currentRunMovement;
      public Vector3 _appliedMovement;
      Vector3 _cameraRelativeMovement;
      bool _isMovementPressed;
      bool _isRunPressed;
      bool _isAttackPressed = false;
    bool _isEquipPressed  = false;
    bool _isAttacking = false;
    bool _isEquiping = false;
    bool _isActiveSword = false;
    bool _isThrowing = false;
      public CharacterController _characterController;
      public Animator _animator;
      public float _rotationFactorPerFrame = 15f;
      public float _runMultiplier = 3.0f;
      public float _speed = 3.0f;
      int _isWalkingHash;
      int _isRunningHash;
      int _isFallingHash;
      int _isAttackingHash;
    int _isEquipingHash;
    int _isThrowingHash;
    public int _attackCountHash;
      public int _isJumpingHash;
      public int _jumpCountHash;
      public bool _requireNewJumpPress = false;
    bool _requireNewAttackPress = false;
      public float _groundedGravity = -0.5f;
      public float _gravity = -9.8f;
      public bool _isJumpPressed = false;
      public float _initialJumpVelocity;
      public float _maxJumpHeight;
      public float _maxJumpTime;
      public bool _isJumping = false;
      public int _jumpCount = 0;
    int _attackCount = 0;
      Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
      Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
      Coroutine _currentJumpResetRoutine = null;
    Coroutine _currentAttackResetRoutine = null;


      PlayerBaseState _currentState;
      PlayerStateFactory _states;


    public bool _isCombating=false;
    public GameObject ActiveSword;
    public GameObject EquippedSword;


    public Rigidbody bullet;
    public Transform spawnPoint;
    public int multiplier;

    public Sword _sword;
    public Sword Sword { get { return _sword; } set { _sword = value; } }
    public bool isDamaging;

      public CharacterController CharacterController { get { return _characterController; } set { _characterController = value; } }
      public Animator Animator { get { return _animator; } }
      public Coroutine CurrentJumpResetRoutine { get { return _currentJumpResetRoutine; } set { _currentJumpResetRoutine = value; } }
    public Coroutine CurrentAttackResetRoutine { get { return _currentAttackResetRoutine; } set { _currentAttackResetRoutine = value; } }
      public Dictionary<int, float> InitialJumpVelocities { get { return _initialJumpVelocities; } }
      public Dictionary<int, float> JumpGravities { get { return _jumpGravities; } }
      public int JumpCount { get { return _jumpCount; } set { _jumpCount = value; } }
    public int AttackCount { get { return _attackCount; } set { _attackCount = value; } }
      public int IsWalkingHash { get { return _isWalkingHash; } }
      public int IsRunningHash { get { return _isRunningHash; } }
      public int IsJumpingHash { get { return _isJumpingHash; } }
      public int IsFallingHash { get { return _isFallingHash; } }
    public int IsEquipingHash { get { return _isEquipingHash; } }
      public int JumpCountHash { get { return _jumpCountHash; } }
    public int IsAttackingHash { get { return _isAttackingHash; } }
    public int AttackCountHash { get { return _attackCountHash; } }
    public int IsThrowingHash { get { return _isThrowingHash; } }
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }
    public bool IsEquiping { get { return _isEquiping; } set { _isEquiping = value; } }
    public bool IsCombating { get { return _isCombating; } set { _isCombating = value; } }
    public bool IsThrowing { get { return _isThrowing; } set { _isThrowing = value; } }
    public bool IsActiveSword { get { return _isActiveSword; }set { _isActiveSword = value; } }
      public bool IsMovementPressed { get { return _isMovementPressed; } }
      public bool IsRunPressed { get { return _isRunPressed; } }
    public bool IsEquipPressed { get { return _isEquipPressed; } }
      public bool RequireNewJumpPress { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; } }
    public bool RequireNewAttackPress { get { return _requireNewAttackPress;  } set { _requireNewAttackPress = value; } }
      public bool IsJumping { get { return _isJumping; } set { _isJumping = value; } }
      public bool IsJumpPressed { get { return _isJumpPressed; } }
    public bool IsAttackPressed { get { return _isAttackPressed; } set { _isAttackPressed = value; } }
      public float GroundedGravity { get { return _groundedGravity; } }
      public float Gravity { get { return _gravity; } }
      public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
      public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
      public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
      public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }
      public float RunMultiplier { get { return _runMultiplier; } }
      public Vector2 CurrentMovementInput { get { return _currentMovementInput; } }
      public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
void Awake()
{
_playerInput = new PlayerInput();
_characterController = GetComponent<CharacterController>();
_animator = GetComponent<Animator>();

//set up state
_states = new PlayerStateFactory(this);
_currentState = _states.Grounded();
_currentState.EnterState();
        
_isWalkingHash = Animator.StringToHash("isWalking");
_isRunningHash = Animator.StringToHash("isRunning");
_isFallingHash = Animator.StringToHash("isFalling");
_isJumpingHash = Animator.StringToHash("isJumping");
        _isEquipingHash = Animator.StringToHash("isEquiping");
_jumpCountHash = Animator.StringToHash("jumpCount");
        _isAttackingHash = Animator.StringToHash("isAttacking");
        _attackCountHash = Animator.StringToHash("attackCount");
        _isThrowingHash = Animator.StringToHash("isThrowing");
_playerInput.CharacterControls.Movement.started += onMovementInput;
_playerInput.CharacterControls.Movement.canceled += onMovementInput;
_playerInput.CharacterControls.Walk.started += onRun;
_playerInput.CharacterControls.Walk.canceled += onRun;
_playerInput.CharacterControls.Movement.performed += onMovementInput;
_playerInput.CharacterControls.Jump.started += onJump;
_playerInput.CharacterControls.Jump.canceled += onJump;
        _playerInput.CharacterControls.Attack.started += onAttack;
        _playerInput.CharacterControls.Attack.canceled += onAttack;
        _playerInput.CharacterControls.Equip.started += onEquip;
        _playerInput.CharacterControls.Equip.canceled += onEquip;
        setupJumpVariables();
}
void setupJumpVariables()
{
float timeToApex = _maxJumpTime / 2;
float innitialGravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
_initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
float secondJumpGravity = (-2 * (_maxJumpHeight + 2)) / Mathf.Pow(timeToApex * 1.25f, 2);
float secondJumpInitialVelocity = (2 * (_maxJumpHeight + 2)) / (timeToApex * 1.25f);
float thirdJumpGravity = (-2 * (_maxJumpHeight + 4)) / Mathf.Pow(timeToApex * 1.5f, 2);
float thirdJumpInitialVelocity = (2 * (_maxJumpHeight + 4)) / (timeToApex * 1.5f);

        _initialJumpVelocities.Add(1, secondJumpInitialVelocity);
        _initialJumpVelocities.Add(2, _initialJumpVelocity);



_jumpGravities.Add(0, innitialGravity);
_jumpGravities.Add(1, secondJumpGravity);
_jumpGravities.Add(2, secondJumpGravity);

}
    void onEquip(InputAction.CallbackContext context)
    {
        _isEquipPressed = context.ReadValueAsButton();
       
    }
    void onAttack(InputAction.CallbackContext context)
    {
        _isAttackPressed = context.ReadValueAsButton();
        _requireNewAttackPress = false;
    }
void onJump(InputAction.CallbackContext context)
{
_isJumpPressed = context.ReadValueAsButton();

}
void onRun(InputAction.CallbackContext context)
{
_isRunPressed = context.ReadValueAsButton();
_requireNewJumpPress = false;
}
void onMovementInput(InputAction.CallbackContext context)
{
_currentMovementInput = context.ReadValue<Vector2>();
_currentMovement.x = _currentMovementInput.x;
_currentMovement.z = _currentMovementInput.y;
_currentRunMovement.x = _currentMovementInput.x * _speed;
_currentRunMovement.z = _currentMovementInput.y * _speed;
_isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;

}
void handleRotation()
{
Vector3 positionToLookAt;
positionToLookAt.x = _cameraRelativeMovement.x;
positionToLookAt.y = 0.0f;
positionToLookAt.z = _cameraRelativeMovement.z;
Quaternion currentRotation = transform.rotation;
if (_isMovementPressed)
{
Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
}
}
void Start()
{
_characterController.Move(_appliedMovement * Time.deltaTime);
}

// Update is called once per frame
void Update()
{

handleRotation();
_currentState.UpdateStates();
_cameraRelativeMovement = ConvertToCameraSpace(_appliedMovement);
_characterController.Move(_cameraRelativeMovement * Time.deltaTime);

}
Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
{
float currentYValue = vectorToRotate.y;

Vector3 cameraForward = Camera.main.transform.forward;
Vector3 cameraRight = Camera.main.transform.right;

cameraForward.y = 0;
cameraRight.y = 0;

cameraForward = cameraForward.normalized;
cameraRight = cameraRight.normalized;

Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

Vector3 vectorRotatedToCameraSpace = cameraForwardZProduct + cameraRightXProduct;
vectorRotatedToCameraSpace.y = currentYValue;
return vectorRotatedToCameraSpace;
}
    public void ResetAttack()
    {
        
        if (!_isAttackPressed)
        {
            _isAttacking = false;
            _sword._canDamage = false;
        }
             
        if (_isAttackPressed && AttackCount == 4)
        {
            _isAttacking = false;
            _sword._canDamage = false;
        }
    }
    public void ResetDamage()
    {
        
       // Debug.Log("can damage");
    }
    public void ActiveWeapon()
    {
        
        if (!_isCombating)
        {
            ActiveSword.SetActive(true);
            EquippedSword.SetActive(false);
            _isCombating = !_isCombating;
            _isActiveSword = true;
        }
        else
        {
            ActiveSword.SetActive(false);
            EquippedSword.SetActive(true);
            _isCombating = !_isCombating;
            _isActiveSword= false;
        }
    }
    
    public void Equipped()
    {
        
         _isEquiping = false;
    }
    private void OnEnable()
    {
    _playerInput.CharacterControls.Enable();
    }
    private void OnDisable()
    {
    _playerInput.CharacterControls.Disable();
    }

    public void FireWeapon()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Rigidbody bulletInstance = Instantiate(bullet,spawnPoint.position,bullet.transform.rotation) as Rigidbody;
        bulletInstance.velocity = ray.direction * multiplier;
        _isThrowing = false;
    }
    public void beDamage()
    {
        if (!isDamaging&&!_isAttackPressed)
        {
            _animator.SetTrigger("hit");
            isDamaging = true;
        }
        
    }
}
