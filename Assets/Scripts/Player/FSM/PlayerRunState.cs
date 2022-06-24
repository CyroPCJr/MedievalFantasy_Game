
namespace MedievalFantasyGame.FSM
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
            Ctx.UpdateAppliedMovement(Ctx.PlayerSo.RunSpeed);
            CheckSwitchState();
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
            else if (Ctx.IsSprintForwardRollPressed)
            {
                SwitchState(Factory.SprintForwardRoll());
            }
        }

        public override void InitializeSubState() { }
    }
}