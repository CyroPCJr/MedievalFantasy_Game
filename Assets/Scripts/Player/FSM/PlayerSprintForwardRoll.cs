namespace MedievalFantasyGame.FSM
{
    public class PlayerSprintForwardRoll : PlayerBaseState
    {
        public PlayerSprintForwardRoll(PlayerStateMachine currentContext, PlayerFactoryState playerFactoryState) : base(currentContext, playerFactoryState)
        { }

        public override void CheckSwitchState()
        {
            if (Ctx.IsRunPressed)
            {
                SwitchState(Factory.Run());
            }
        }

        public override void EnterState()
        {
            Ctx.Animator.SetBool(Ctx.SprintForwardRollhash, true);
        }

        public override void ExitState()
        {
            Ctx.Animator.SetBool(Ctx.SprintForwardRollhash, false);
        }

        public override void InitializeSubState() 
        { }

        public override void UpdateState()
        {
            CheckSwitchState();
        }
    }
}
