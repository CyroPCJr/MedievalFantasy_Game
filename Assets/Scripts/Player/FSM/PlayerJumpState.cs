using UnityEngine;

namespace MedievalFantasyGame.FSM
{
    public class PlayerJumpState : PlayerBaseState, IRootState
    {
        public PlayerJumpState(PlayerStateMachine currentContext, PlayerFactoryState playerFactoryState) : base(currentContext, playerFactoryState)
        {
            IsRootState = true;
        }

        public override void EnterState()
        {
            InitializeSubState();
            HandleJump();
        }

        public override void UpdateState()
        {
            HandleGravity();
            CheckSwitchState();
        }

        public override void ExitState()
        {
            Ctx.Animator.SetBool(Ctx.JumpingHash, false);
        }

        public override void CheckSwitchState()
        {
            if (Ctx.CharacterController.isGrounded)
            {
                SwitchState(Factory.Grounded());
            }
        }

        public override void InitializeSubState()
        {
            if (!Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            {
                SetSubStates(Factory.Idle());
            }
            else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            {
                SetSubStates(Factory.Walk());
            }
            else
            {
                SetSubStates(Factory.Run());
            }
        }

        private void HandleJump()
        {
            Ctx.Animator.SetBool(Ctx.JumpingHash, true);
            Ctx.CurrentMovementY = Ctx.InitialJumpVelocity;
            Ctx.AppliedMovementY = Ctx.InitialJumpVelocity;
        }

        public void HandleGravity()
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            if (Ctx.CurrentMovementY <= 0.0f || !Ctx.IsJumpingPressed)
            {
                float fallingMultiplier = 2.0f;
                Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.Gravity * fallingMultiplier * Time.deltaTime);
                Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * 0.5f, -20.0f);
            }
            else
            {
                Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.Gravity * Time.deltaTime);
                Ctx.AppliedMovementY = (previousYVelocity + Ctx.CurrentMovementY) * 0.5f;
            }
        }
    }
}