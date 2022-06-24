using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MedievalFantasyGame.FSM
{
    public class PlayerFallState : PlayerBaseState, IRootState
    {
        public PlayerFallState(PlayerStateMachine currentContext, PlayerFactoryState playerFactoryState) : base(currentContext, playerFactoryState)
        {
            IsRootState = true;

        }

        public override void CheckSwitchState()
        {
            if (Ctx.CharacterController.isGrounded)
            {
                SwitchState(Factory.Grounded());
            }
        }

        public override void EnterState()
        {
            InitializeSubState();
            Ctx.Animator.SetBool(Ctx.FallingHash, true);
        }

        public override void ExitState()
        {
            Ctx.Animator.SetBool(Ctx.FallingHash, false);
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

        public override void UpdateState()
        {
            HandleGravity();
            CheckSwitchState();
        }

        public void HandleGravity()
        {
            float previousVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + Ctx.Gravity * Time.deltaTime;
            Ctx.AppliedMovementY = Mathf.Max((previousVelocity + Ctx.CurrentMovementY) * 0.5f, -20.0f);
        }
    }
}
