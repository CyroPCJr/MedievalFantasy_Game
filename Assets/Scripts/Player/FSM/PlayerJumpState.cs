using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine currentContext, PlayerFactoryState playerFactoryState) : base(currentContext, playerFactoryState)
        {
            IsRootState = true;
            InitializeSubState();
        }

        public override void EnterState()
        {
            HandleJump();
        }

        public override void UpdateState()
        {
            CheckSwitchState();
            HandleGravity();
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

        private void HandleGravity()
        {
            bool isFalling = Ctx.CurrentMovementY <= 0.0f || !Ctx.IsJumpingPressed;
            float fallingMultiplier = 2.0f;
            if (isFalling)
            {
                float previousYVelocity = Ctx.CurrentMovementY;
                Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.Gravity * fallingMultiplier * Time.deltaTime);
                Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * 0.5f, -20.0f);
            }
            else
            {
                float previousYVelocity = Ctx.CurrentMovementY;
                Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.Gravity * Time.deltaTime);
                Ctx.AppliedMovementY = (previousYVelocity + Ctx.CurrentMovementY) * 0.5f;
            }
        }
    }
}