using System.Collections;
using UnityEngine;

namespace MedievalFantasyGame.FSM
{
    public class PlayerSprintForwardRoll : PlayerBaseState
    {

        private Coroutine _DodgeCoroutine;
        public PlayerSprintForwardRoll(PlayerStateMachine currentContext, PlayerFactoryState playerFactoryState) : base(currentContext, playerFactoryState)
        { }

        public override void CheckSwitchState()
        {
            if (!Ctx.IsDodging)
            {
                SwitchState(Factory.Run());
                SwitchState(Factory.Walk());
            }
        }

        public override void EnterState()
        {
            _DodgeCoroutine = Ctx.StartCoroutine(Dodge());
        }

        public override void ExitState()
        {
            Ctx.StopCoroutine(_DodgeCoroutine);
        }

        public override void InitializeSubState()
        { }

        public override void UpdateState()
        {
            CheckSwitchState();
        }

        private IEnumerator Dodge()
        {
            Ctx.IsDodging = true;
            float timer = 0.0f;
            Ctx.Animator.SetTrigger(Ctx.DodgeHash);
            while (timer < Ctx.DodgeTimer)
            {
                float speed = Ctx.AnimationCurve.Evaluate(timer);
                Ctx.UpdateCharacterControllerMovement(speed * Ctx.AppliedMovement);
                timer += Time.deltaTime;
                yield return null;
            }

            Ctx.IsDodging = false;
        }

    }
}
