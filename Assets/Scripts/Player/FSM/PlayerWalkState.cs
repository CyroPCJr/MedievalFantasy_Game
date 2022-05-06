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
            Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x * 4.0f;
            Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * 4.0f;
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
            else if (Ctx.IsSprintForwardRollPressed)
            {
                SwitchState(Factory.SprintForwardRoll());
            }
        }

        public override void InitializeSubState() 
        { }
        
    }
}