using UnityEngine;
using UnityEngine.InputSystem;

namespace FSM
{

    public class PlayerInputs : MonoBehaviour
    {
        private InputControl _playerInputActions;
        private CharacterController _characterController;
        private Animator _animatorController;

        private Vector2 _currentMovementInput = Vector2.zero;
        private Vector3 _currentMovement = Vector3.zero;
        private Vector3 _currentRunMovement = Vector3.zero;
        private Vector3 _appliedMovement = Vector3.zero;
        private bool _isMovementPressed = false;
        private bool _isRunPressed = false;
        private bool _isJumpPressed = false;

        private int _walkingAniHash = 0;
        private int _runningAniHash = 0;
        private int _jumpingAniHash = 0;

        private const float _rotationPerFrame = 15.0f;
        private const float _runMultiplier = 4.0f;

        // Gravity variables
        private float _gravity = -9.8f;
        private float _groundGravity = -0.05f;

        //Jump variable
        private float _initialJumpVelocity = 0.0f;
        private float _maxJumpHeight = 4.0f;
        private float _maxJumpTime = 0.75f;
        private bool _isJumping = false;
        private bool isJumpingAnimate = false;
        private void Awake()
        {
            _playerInputActions = new InputControl();
            _characterController = GetComponent<CharacterController>();
            _animatorController = GetComponentInChildren<Animator>();
            // cache animation parameters
            _walkingAniHash = Animator.StringToHash("isWalking");
            _runningAniHash = Animator.StringToHash("isRunning");
            _jumpingAniHash = Animator.StringToHash("isJumping");
            //Input Actions
            _playerInputActions.Player.Movement.started += OnMovement;
            _playerInputActions.Player.Movement.performed += OnMovement;
            _playerInputActions.Player.Movement.canceled += OnMovement;

            _playerInputActions.Player.Run.started += OnRun;
            _playerInputActions.Player.Run.canceled += OnRun;

            _playerInputActions.Player.Jump.started += OnJump;
            _playerInputActions.Player.Jump.canceled += OnJump;

            setJumpVariables();
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            _isJumpPressed = ctx.ReadValueAsButton();
        }

        private void OnRun(InputAction.CallbackContext ctx)
        {
            _isRunPressed = ctx.ReadValueAsButton();
        }

        private void OnMovement(InputAction.CallbackContext ctx)
        {
            _currentMovementInput = ctx.ReadValue<Vector2>();
            _currentMovement.x = _currentMovementInput.x;
            _currentMovement.z = _currentMovementInput.y;
            _currentRunMovement.x = _currentMovementInput.x * _runMultiplier;
            _currentRunMovement.z = _currentMovementInput.y * _runMultiplier;
            _isMovementPressed = _currentMovementInput.x != 0.0f || _currentMovementInput.y != 0.0f;
        }

        private void OnEnable()
        {
            _playerInputActions.Player.Enable();
        }

        private void OnDisable()
        {
            _playerInputActions.Player.Disable();
        }

        private void FixedUpdate()
        {
            HandleRotation();
            HandleAnimation();
            if (_isRunPressed)
            {
                _appliedMovement.x = _currentRunMovement.x;
                _appliedMovement.z = _currentRunMovement.z;
            }
            else
            {
                _appliedMovement.x = _currentMovement.x;
                _appliedMovement.z = _currentMovement.z;
            }
            _characterController.Move(Time.fixedDeltaTime * _appliedMovement);
            HandleGravity();
            HandleJump();
        }

        private void HandleRotation()
        {
            if (_isMovementPressed)
            {
                Vector3 positionToLookAt;
                positionToLookAt.x = _currentMovement.x;
                positionToLookAt.y = 0.0f;
                positionToLookAt.z = _currentMovement.z;
                Quaternion currentRotation = transform.rotation;
                Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationPerFrame * Time.deltaTime);
            }
        }

        private void HandleAnimation()
        {
            bool isWalking = _animatorController.GetBool(_walkingAniHash);
            bool isRunning = _animatorController.GetBool(_runningAniHash);

            if (_isMovementPressed && !isWalking)
            {
                _animatorController.SetBool(_walkingAniHash, true);

            }
            else if (!_isMovementPressed && isWalking)
            {
                _animatorController.SetBool(_walkingAniHash, false);
            }

            if ((_isMovementPressed && _isRunPressed) && !isRunning)
            {
                _animatorController.SetBool(_runningAniHash, true);
            }
            else if ((!_isMovementPressed || !_isRunPressed) && isRunning)
            {
                _animatorController.SetBool(_runningAniHash, false);
            }
        }


        private void HandleGravity()
        {
            bool isFalling = _currentMovement.y <= 0.0f || !_isJumpPressed;
            float fallingMultiplier = 2.0f;
            if (_characterController.isGrounded)
            {
                if (isJumpingAnimate)
                {
                    _animatorController.SetBool(_jumpingAniHash, false);
                    isJumpingAnimate = false;
                }
                _currentMovement.y = _groundGravity;
                _appliedMovement.y = _groundGravity;
            }
            else if (isFalling)
            {
                float previousYVelocity = _currentMovement.y;
                _currentMovement.y = _currentMovement.y + (_gravity * fallingMultiplier * Time.deltaTime);
                _appliedMovement.y = Mathf.Max((previousYVelocity + _currentMovement.y) * 0.5f, -20.0f);
            }
            else
            {
                float previousYVelocity = _currentMovement.y;
                _currentMovement.y = _currentMovement.y + (_gravity * Time.deltaTime);
                _appliedMovement.y = (previousYVelocity + _currentMovement.y) * 0.5f;
            }
        }

        private void setJumpVariables()
        {
            float timeToApex = _maxJumpTime / 2.0f;
            _gravity = (-2.0f * _maxJumpHeight) / Mathf.Pow(timeToApex, 2.0f);
            _initialJumpVelocity = (2.0f * _maxJumpHeight) / timeToApex;

        }

        private void HandleJump()
        {
            if (!_isJumping && _characterController.isGrounded && _isJumpPressed)
            {
                _animatorController.SetBool(_jumpingAniHash, true);
                isJumpingAnimate = true;
                _isJumping = true;
                _currentMovement.y = _initialJumpVelocity;
                _appliedMovement.y = _initialJumpVelocity;
            }
            else if (!_isJumpPressed && _characterController.isGrounded && _isJumping)
            {
                _isJumping = false;
            }
        }
    }
}