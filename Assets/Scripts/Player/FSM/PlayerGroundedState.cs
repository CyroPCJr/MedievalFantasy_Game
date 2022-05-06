using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MedievalFantasyGame.FSM
{
    public class PlayerGroundedState : PlayerBaseState, IRootState
    {

        public PlayerGroundedState(PlayerStateMachine currentContext, PlayerFactoryState playerFactoryState) : base(currentContext, playerFactoryState)
        {
            IsRootState = true;
        }

        public override void EnterState()
        {
            InitializeSubState();
            HandleGravity();
        }

        public override void UpdateState()
        {
            CheckSwitchState();
        }

        public override void ExitState() 
        { }

        public override void CheckSwitchState()
        {
            // if player is grounded and jump is pressed, switch to jump state
            if (Ctx.IsJumpingPressed)
            {
                SwitchState(Factory.Jump());
            }else if (!Ctx.CharacterController.isGrounded)
            {
                SwitchState(Factory.Fall());
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

        public void HandleGravity()
        {
            Ctx.CurrentMovementY = Ctx.Gravity;
            Ctx.AppliedMovementY = Ctx.Gravity;
        }
        
    }

}