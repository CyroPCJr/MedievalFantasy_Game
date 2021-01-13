using System;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader = null;
        [SerializeField] private PlayerDataSO _playerDataSO = null;
        
        public Transform gameplayCamera = null;

        [HideInInspector] public Vector3 movementInput;
        [HideInInspector] public Vector3 movementVector;
        [HideInInspector] public ControllerColliderHit lastHit;

        private Vector2 _previousMovementInput = Vector2.zero;
        private CharacterController _characterController = null;
        private Transform _transform = null;
        
        private float _turnSmoothSpeed;
       

        private void Awake()
        {
            if (!TryGetComponent(out _characterController))
            {
                Debug.LogWarning("[WARNING] -- Failed to load CharacterController component.");
            }

            if (!TryGetComponent(out _transform))
            {
                Debug.LogWarning("[WARNING] -- Failed to load Transform component.");
            }

        }

        private void OnEnable()
        {
            _inputReader.MoveEvent += OnMove;
            _inputReader.JumpEvent += OnJump;
            _inputReader.AttackEvent += OnAttack;
            _inputReader.Skill1Event += OnSkill_1;
            _inputReader.Skill2Event += OnSkill_2;
        }



        private void OnDisable()
        {
            _inputReader.MoveEvent -= OnMove;
            _inputReader.JumpEvent -= OnJump;
            _inputReader.AttackEvent -= OnAttack;
            _inputReader.Skill1Event -= OnSkill_1;
            _inputReader.Skill2Event -= OnSkill_2;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            lastHit = hit;
        }

        private void Update()
        {
            RecalculateMovement();
        }

        private void RecalculateMovement()
        {
            //Get the two axes from the camera and flatten them on the XZ plane
            Vector3 cameraForward = gameplayCamera.forward;
            cameraForward.y = 0f;
            Vector3 cameraRight = gameplayCamera.right;
            cameraRight.y = 0f;

            //Use the two axes, modulated by the corresponding inputs, and construct the final vector
            Vector3 adjustedMovement = (cameraRight.normalized * _previousMovementInput.x) + (cameraForward.normalized * _previousMovementInput.y);

            movementInput = Vector3.ClampMagnitude(adjustedMovement, 1f);

            if (_characterController.isGrounded && _playerDataSO.velocityGravity.y < 0f)
            {
                _playerDataSO.velocityGravity.y = -2f;
            }

            if (movementInput.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(movementInput.x, movementInput.z) * Mathf.Rad2Deg;
                _transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetAngle, ref _turnSmoothSpeed, _playerDataSO.turnSmoothTime);

                _characterController.Move(movementInput * _playerDataSO.speedMovement * Time.deltaTime);
            }
            _playerDataSO.velocityGravity.y += Physics.gravity.y * Time.deltaTime;
            _characterController.Move(_playerDataSO.velocityGravity * Time.deltaTime);
            
        }

        // Actions
        private void OnMove(Vector2 movement)
        {
            _previousMovementInput = movement;
        }

        private void OnJump()
        {
            if (_characterController.isGrounded)
            {
                _playerDataSO.velocityGravity.y = _playerDataSO.ApplyForceJump;
            }
        }

        private void OnAttack()
        {
            Debug.Log("Attack Event");
        }

        private void OnSkill_1()
        {
            Debug.Log("Skill 1 Event");
        }
        private void OnSkill_2()
        {
            Debug.Log("Skill 2 Event");
        }

    }

}