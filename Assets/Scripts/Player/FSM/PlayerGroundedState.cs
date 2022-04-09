using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class PlayerGroundedState : PlayerBaseState
    {

        public PlayerGroundedState(PlayerStateMachine currentContext, PlayerFactoryState playerFactoryState) : base(currentContext, playerFactoryState)
        {
            IsRootState = true;
            InitializeSubState();
        }


        public override void EnterState()
        {
            Ctx.CurrentMovementY = Ctx.GroundGravity;
            Ctx.AppliedMovementY = Ctx.GroundGravity;
        }

        public override void UpdateState()
        {
            CheckSwitchState();
        }

        public override void ExitState() { }

        public override void CheckSwitchState()
        {
            // if player is grounded and jump is pressed, switch to jump state
            if (Ctx.IsJumpingPressed)
            {
                SwitchState(Factory.Jump());
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
    }

}