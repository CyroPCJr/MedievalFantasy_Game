namespace MedievalFantasyGame.FSM
{

    public abstract class PlayerBaseState
    {
        private bool _isRootState = false;
        private PlayerStateMachine _ctx;
        private PlayerFactoryState _factory;
        private PlayerBaseState _currentSuperState;
        private PlayerBaseState _currentSubState;

        protected bool IsRootState { set => _isRootState = value; }
        protected PlayerStateMachine Ctx => _ctx;
        protected PlayerFactoryState Factory => _factory;

        public PlayerBaseState(PlayerStateMachine currentContext, PlayerFactoryState playerFactoryState)
        {
            _ctx = currentContext;
            _factory = playerFactoryState;
        }

        public abstract void EnterState();

        public abstract void UpdateState();

        public abstract void ExitState();

        public abstract void CheckSwitchState();

        public abstract void InitializeSubState();

        public void UpdateStates()
        {
            UpdateState();
            if (_currentSubState != null)
            {
                _currentSubState.UpdateStates(); ;
            }
        }

        protected void SwitchState(PlayerBaseState newState)
        {
            // current state exit safe
            ExitState();
            // new state enter safe
            newState.EnterState();

            if (_isRootState)
            {
                // switch current state of context
                Ctx.CurrentState = newState;
            }
            else if (_currentSuperState != null)
            {
                _currentSuperState.SetSubStates(newState);
            }
        }

        protected void SetSuperStates(PlayerBaseState newSuperState)
        {
            _currentSuperState = newSuperState;
        }

        protected void SetSubStates(PlayerBaseState newSubState)
        {
            _currentSubState = newSubState;
            newSubState.SetSuperStates(this);
        }
    }

}