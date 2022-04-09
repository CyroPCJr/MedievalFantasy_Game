using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class PlayerRunState : PlayerBaseState
    {

        public PlayerRunState(PlayerStateMachine currentContext, PlayerFactoryState playerFactoryState) : base(currentContext, playerFactoryState)
        { }


        public override void EnterState()
        {
            Ctx.Animator.SetBool(Ctx.WalkingHash, true);
            Ctx.Animator.SetBool(Ctx.RunningHash, true);
        }

        public override void UpdateState()
        {
            CheckSwitchState();
            Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x * Ctx.RunMultiplier;
            Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * Ctx.RunMultiplier;
        }

        public override void ExitState() { }

        public override void CheckSwitchState()
        {
            if (!Ctx.IsMovementPressed)
            {
                SwitchState(Factory.Idle());
            }
            else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            {
                SwitchState(Factory.Walk());
            }
        }

        public override void InitializeSubState() { }
    }
}