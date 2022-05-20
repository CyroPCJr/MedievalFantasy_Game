using UnityEngine;
using UnityEngine.InputSystem;

namespace MedievalFantasyGame.FSM
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [field: SerializeField] public AnimationCurve AnimationCurve { get; private set; }

        private InputControl _playerInputActions;

        private Vector2 _currentMovementInput = Vector2.zero;
        private Vector3 _currentMovement = Vector3.zero;
        private Vector3 _currentRunMovement = Vector3.zero;
        private Vector3 _appliedMovement = Vector3.zero;

        private const float _rotationPerFrame = 15.0f;
        private const float _runMultiplier = 4.0f;

        //Jump variable
        private float _initialJumpVelocity = 0.0f;
        private float _maxJumpHeight = 1.0f;
        private float _maxJumpTime = 0.75f;

        #region set variables
        PlayerFactoryState _states;

        #endregion

        #region getters and setters

        public CharacterController CharacterController { get; private set; }
        public Animator Animator { get; private set; }
        public PlayerBaseState CurrentState { get; set; }

        // string hashes
        public int JumpingHash { get; private set; } = 0;
        public int RunningHash { get; private set; } = 0;
        public int WalkingHash { get; private set; } = 0;
        public int FallingHash { get; private set; } = 0;
        public int DodgeHash { get; private set; } = 0;

        // actions inputs pressed
        public bool IsJumpingPressed { get; private set; } = false;
        public bool IsMovementPressed { get; private set; } = false;
        public bool IsRunPressed { get; private set; } = false;
        public bool IsSprintForwardRollPressed { get; private set; } = false;

        public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
        public Vector3 AppliedMovement { get => _appliedMovement; set => _appliedMovement = value; }
        public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
        public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
        public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }
        public float RunMultiplier { get { return _runMultiplier; } }
        public float InitialJumpVelocity { get { return _initialJumpVelocity; } }
        public float Gravity { get; private set; } = Physics.gravity.y;
        public Vector2 CurrentMovementInput { get { return _currentMovementInput; } }
        public float DodgeTimer { get; private set; } = 0.0f;

        #endregion
        private void Awake()
        {
            _playerInputActions = new InputControl();
            CharacterController = GetComponent<CharacterController>();
            Animator = GetComponentInChildren<Animator>();

            // setup states
            _states = new PlayerFactoryState(this);
            CurrentState = _states.Grounded();
            CurrentState.EnterState();

            // cache animation parameters
            WalkingHash = Animator.StringToHash("isWalking");
            RunningHash = Animator.StringToHash("isRunning");
            JumpingHash = Animator.StringToHash("isJumping");
            FallingHash = Animator.StringToHash("isFalling");
            DodgeHash = Animator.StringToHash("isDodge");
            // Input Actions
            _playerInputActions.Player.Movement.started += OnMovement;
            _playerInputActions.Player.Movement.performed += OnMovement;
            _playerInputActions.Player.Movement.canceled += OnMovement;

            _playerInputActions.Player.Run.started += OnRun;
            _playerInputActions.Player.Run.canceled += OnRun;

            _playerInputActions.Player.Jump.started += OnJump;
            _playerInputActions.Player.Jump.canceled += OnJump;

            _playerInputActions.Player.SprintForwardRoll.started += OnForwardRoll;
            _playerInputActions.Player.SprintForwardRoll.canceled += OnForwardRoll;

            setJumpVariables();
        }

        private void Start()
        {
            setDodgeVariables();
            UpdateMovement(AppliedMovement);
        }

        private void OnEnable()
        {
            _playerInputActions.Player.Enable();
        }

        private void OnDisable()
        {
            _playerInputActions.Player.Disable();
        }

        private void Update()
        {
            HandleRotation();
            CurrentState.UpdateStates();
            UpdateMovement(AppliedMovement);
        }

        #region callback handler function to set the player input

        private void OnJump(InputAction.CallbackContext ctx)
        {
            IsJumpingPressed = ctx.ReadValueAsButton();
        }

        private void OnRun(InputAction.CallbackContext ctx)
        {
            IsRunPressed = ctx.ReadValueAsButton();
        }

        private void OnMovement(InputAction.CallbackContext ctx)
        {
            _currentMovementInput = ctx.ReadValue<Vector2>();
            _currentMovement.x = _currentMovementInput.x;
            _currentMovement.z = _currentMovementInput.y;
            _currentRunMovement.x = _currentMovementInput.x * _runMultiplier;
            _currentRunMovement.z = _currentMovementInput.y * _runMultiplier;
            IsMovementPressed = _currentMovementInput.x != 0.0f || _currentMovementInput.y != 0.0f;
        }

        private void OnForwardRoll(InputAction.CallbackContext ctx)
        {
            IsSprintForwardRollPressed = ctx.ReadValueAsButton();
        }

        #endregion

        private void setJumpVariables()
        {
            float timeToApex = _maxJumpTime / 2.0f;
            Gravity = (-2.0f * _maxJumpHeight) / Mathf.Pow(timeToApex, 2.0f);
            _initialJumpVelocity = (2.0f * _maxJumpHeight) / timeToApex;
        }

        private void setDodgeVariables()
        {
            Keyframe dodgeLastFrame = AnimationCurve[AnimationCurve.length - 1];
            DodgeTimer = dodgeLastFrame.time;
        }

        private void HandleRotation()
        {
            if (IsMovementPressed)
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

        public void UpdateMovement(Vector3 direction) => CharacterController.Move(Time.deltaTime * direction);

    }
}