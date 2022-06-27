using System;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : NetworkBehaviour
{
    
    private Rigidbody _rigidbody;
    private CharacterActions _characterActions;
    private InputAction _move;
    private InputAction _attack;
    private Vector2 _moveDirection = Vector2.zero;
    public float moveSpeed = 5f;
    private string _clientLabel;

    public override void OnStartClient()
    {
        base.OnStartClient();
        _clientLabel = "Client: " + DateTime.UtcNow.Millisecond;
    }

    private void Awake()
    {
        _characterActions = new CharacterActions();
    }

    void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _move = _characterActions.Player.Move;
        _attack = _characterActions.Player.Fire;
        _move.Enable();
        _attack.Enable();
        _attack.performed += Fire;
    }

    private void OnDestroy()
    {
        _move.Disable();
        _attack.performed -= Fire;
        _attack.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        _moveDirection = _move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        _rigidbody.velocity = new Vector2(_moveDirection.x * moveSpeed * Time.deltaTime, _moveDirection.y * moveSpeed * Time.deltaTime);
    }

    private void Fire(InputAction.CallbackContext context)
    {
        
        Debug.Log(_clientLabel + " Just Fired!"); 
            
    }
}
