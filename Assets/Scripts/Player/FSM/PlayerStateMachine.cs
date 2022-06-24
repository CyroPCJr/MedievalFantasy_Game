using UnityEngine;
using UnityEngine.InputSystem;

namespace MedievalFantasyGame.FSM
{
    public class PlayerStateMachine : MonoBehaviour
    {

        [field: SerializeField] public PlayerSO.PlayerDataSO PlayerSo;
        [field: SerializeField] public AnimationCurve AnimationCurve { get; private set; }
        [field: System.NonSerialized] public bool IsDodging = false;
        private InputControl _playerInputActions;

        private Vector2 _currentMovementInput = Vector2.zero;
        private Vector3 _currentMovement = Vector3.zero;
        //private Vector3 _currentRunMovement = Vector3.zero;
        private Vector3 _appliedMovement = Vector3.zero;

        private const float _rotationPerFrame = 15.0f;

        //Jump variable
        private const float _maxJumpTime = 0.75f;

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

        public float CurrentMovementY { get => _currentMovement.y; set => _currentMovement.y = value; }
        public Vector3 AppliedMovement { get => _appliedMovement; set => _appliedMovement = value; }
        public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
        public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
        public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }

        public float InitialJumpVelocity { get; private set; } = 0.0f;
        public float Gravity { get; private set; } = Physics.gravity.y;

        public float DodgeTimer { get; private set; } = 0.0f;

        #endregion
        private void Awake()
        {
            _playerInputActions = new InputControl();
            CharacterController = GetComponent<CharacterController>();
            Animator = GetComponentInChildren<Animator>();

            // setup states
            PlayerFactoryState _states = new PlayerFactoryState(this);
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
            setDodgeVariables();
        }

        private void Start()
        {
            UpdateCharacterControllerMovement(AppliedMovement);
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
            UpdateCharacterControllerMovement(AppliedMovement);
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
            Gravity = (-2.0f * PlayerSo.MaxJumpHeight) / Mathf.Pow(timeToApex, 2.0f);
            InitialJumpVelocity = (2.0f * PlayerSo.MaxJumpHeight) / timeToApex;
        }

        private void setDodgeVariables()
        {
            Keyframe dodgeLastFrame = AnimationCurve[AnimationCurve.length - 1];
            DodgeTimer = dodgeLastFrame.time;
        }

        private void HandleRotation()
        {
            if (IsMovementPressed && !IsDodging)
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

        public void UpdateCharacterControllerMovement(Vector3 direction) => CharacterController.Move(Time.deltaTime * direction);

        public void UpdateAppliedMovement(float speed)
        {
            AppliedMovementX = _currentMovementInput.x * speed;
            AppliedMovementZ = _currentMovementInput.y * speed;
        }

    }
}