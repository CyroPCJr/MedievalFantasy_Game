using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSM
{
    public class PlayerIdleState : PlayerBaseState
    {
        //TODO: arruamar esses substates aqui
        public PlayerIdleState(PlayerStateMachine currentContext, PlayerFactoryState playerFactoryState) : base(currentContext, playerFactoryState)
        { }


        public override void EnterState()
        {
            Ctx.Animator.SetBool(Ctx.WalkingHash, false);
            Ctx.Animator.SetBool(Ctx.RunningHash, false);
            Ctx.AppliedMovementX = 0;
            Ctx.AppliedMovementZ = 0;
        }

        public override void UpdateState()
        {
            CheckSwitchState();
        }

        public override void ExitState() { }

        public override void CheckSwitchState()
        {
            if (Ctx.IsMovementPressed)
            {
                if (Ctx.IsRunPressed)
                {
                    SwitchState(Factory.Run());
                }
                else
                {
                    SwitchState(Factory.Walk());
                }
            }
        }

        public override void InitializeSubState() { }
    }

}