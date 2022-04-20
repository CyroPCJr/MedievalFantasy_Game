using System.Collections.Generic;

namespace MedievalFantasyGame.FSM
{
    public enum PlayerStates
    {
        idle,
        walk,
        run,
        grounded,
        jump,
        fall
    }

    public class PlayerFactoryState
    {
        private PlayerStateMachine _context;
        private Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>(6);
        public PlayerFactoryState(PlayerStateMachine currentContext)
        {
            _context = currentContext;
            _states[PlayerStates.idle] = new PlayerIdleState(_context, this);
            _states[PlayerStates.walk] = new PlayerWalkState(_context, this);
            _states[PlayerStates.grounded] = new PlayerGroundedState(_context, this);
            _states[PlayerStates.run] = new PlayerRunState(_context, this);
            _states[PlayerStates.jump] = new PlayerJumpState(_context, this);
            _states[PlayerStates.fall] = new PlayerFallState(_context, this);
        }

        public PlayerBaseState Idle()
        {
            return _states[PlayerStates.idle];
        }

        public PlayerBaseState Walk()
        {
            return _states[PlayerStates.walk];
        }

        public PlayerBaseState Grounded()
        {
            return _states[PlayerStates.grounded];
        }

        public PlayerBaseState Run()
        {
            return _states[PlayerStates.run];
        }

        public PlayerBaseState Jump()
        {
            return _states[PlayerStates.jump];
        }

        public PlayerBaseState Fall()
        {
            return _states[PlayerStates.fall];
        }

    }
}
