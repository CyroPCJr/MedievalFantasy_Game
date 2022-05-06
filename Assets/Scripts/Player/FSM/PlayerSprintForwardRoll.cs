using System.Collections;
using UnityEngine;

namespace MedievalFantasyGame.FSM
{
    public class PlayerSprintForwardRoll : PlayerBaseState
    {
        private bool _isDodging = true;

        public PlayerSprintForwardRoll(PlayerStateMachine currentContext, PlayerFactoryState playerFactoryState) : base(currentContext, playerFactoryState)
        { }

        public override void CheckSwitchState()
        {
            if (!_isDodging)
            {
                SwitchState(Factory.Run());
                SwitchState(Factory.Walk());
            }
        }

        public override void EnterState()
        {
            Ctx.StartCoroutine(Dodge());
        }

        public override void ExitState()
        { }

        public override void InitializeSubState()
        { }

        public override void UpdateState()
        {
            CheckSwitchState();
        }

        private IEnumerator Dodge()
        {
            _isDodging = true;
            float timer = 0.0f;
            Ctx.Animator.SetTrigger(Ctx.DodgeHash);
            while (timer < Ctx.DodgeTimer)
            {
                float speed = Ctx.AnimationCurve.Evaluate(timer);
                timer += Time.deltaTime;
                yield return null;
            }
            Ctx.AppliedMovementX *= timer * 15.0f;
            Ctx.AppliedMovementZ *= timer * 15.0f;
            _isDodging = false;
        }

    }
}
