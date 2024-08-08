using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public float maxDistance = 10f;
    public RaycastHit hit;
    public float radius = 10f;
    public LayerMask layerMask;
    Transform cameraTransform;

    PlayerInput _playerInput;
    bool _isInteractPressed=false;
    bool _isPinking = false;
    bool _isDisplayPressed = false;
    bool _isDisplaying = false;
    private Interactable interact;
    public Inventory inventory;
    public Display display;
    Coroutine _currentPickUpReset = null;
    Coroutine _currentDisplayReset = null;
    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.CharacterControls.Interact.started += OnInteract;
        _playerInput.CharacterControls.Interact.canceled += OnInteract;
        _playerInput.CharacterControls.Balo.started += OnDisplay;
        _playerInput.CharacterControls.Balo.canceled += OnDisplay;
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        _isInteractPressed = context.ReadValueAsButton();
        
    }
    void OnDisplay(InputAction.CallbackContext context)
    {
        _isDisplayPressed=context.ReadValueAsButton();
        
    }
    IEnumerator pickUpResetRoutine()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("can pick up");
        _isPinking = false ;
        
    }
    IEnumerator displayResetRoutine()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("end display");
        
        _isDisplaying = false;

    }
    // Start is called before the first frame update
    void Start()
    {
        inventory.list.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        cameraTransform = Camera.main.transform;
        Vector3 origin = cameraTransform.position;
        Vector3 direction = cameraTransform.forward;

        bool isHit = Physics.SphereCast(origin, radius, direction, out hit, maxDistance, layerMask);
        if (isHit)
        {
            if (hit.transform.TryGetComponent<Interactable>(out interact))
            {
                interact.showText();
                if (_isInteractPressed && !_isPinking)
                {
                    _isPinking = true;

                    inventory.list.Add(interact.img);
                    interact.Interact();
                    display.Load();
                    _currentPickUpReset = StartCoroutine(pickUpResetRoutine());
                }
            }

        }
        else if (interact != null)
        {
            interact.hideText();
        }
        if (_isDisplayPressed && !_isDisplaying)
        {
            _isDisplaying = true;
            display.animator.SetTrigger("isDisplaying");
            _currentDisplayReset = StartCoroutine(displayResetRoutine());
        }
    }
    private void OnDrawGizmos()
    {
        cameraTransform = Camera.main.transform;
        Vector3 origin = cameraTransform.position;
        Vector3 direction = cameraTransform.forward;

        bool isHit = Physics.SphereCast(origin, radius, direction, out hit, maxDistance, layerMask);
        if (isHit)
        {
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(origin, direction * hit.distance);
            Gizmos.DrawWireSphere(origin + direction * hit.distance, radius);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(origin, direction * maxDistance);
        }
    }
    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
    }
    private void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
    }

}
