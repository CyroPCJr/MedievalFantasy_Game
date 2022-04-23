using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MedievalFantasyGame.FSM
{
    public class PlayerWalkState : PlayerBaseState
    {

        public PlayerWalkState(PlayerStateMachine currentContext, PlayerFactoryState playerFactoryState) : base(currentContext, playerFactoryState)
        { }

        public override void EnterState()
        {
            Ctx.Animator.SetBool(Ctx.WalkingHash, true);
            Ctx.Animator.SetBool(Ctx.RunningHash, false);
        }

        public override void UpdateState()
        {
            Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x;
            Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y;
            CheckSwitchState();
        }

        public override void ExitState() 
        { }

        public override void CheckSwitchState()
        {
            if (!Ctx.IsMovementPressed)
            {
                SwitchState(Factory.Idle());
            }
            else if (Ctx.IsMovementPressed && Ctx.IsRunPressed)
            {
                SwitchState(Factory.Run());
            }
        }

        public override void InitializeSubState() 
        { }
        
    }
}