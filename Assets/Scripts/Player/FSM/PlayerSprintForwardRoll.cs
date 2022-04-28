using System.Collections;
using UnityEngine;

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
            //Ctx.StartCoroutine(LerpPosition(Ctx.AppliedMovement * 100.0f, 1.0f));
        }

        public override void ExitState()
        {
            Ctx.Animator.SetBool(Ctx.SprintForwardRollhash, false);
        }

        public override void InitializeSubState()
        { }

        public override void UpdateState()
        {
            Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x * 100.0f;
            Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * 100.0f;

            CheckSwitchState();
        }


        private IEnumerator LerpPosition(Vector3 targetPosition, float duration)
        {
            float time = 0.0f;
            Vector3 startPosition = Ctx.AppliedMovement;
            while (time < duration)
            {
                Ctx.AppliedMovement = Vector3.Lerp(startPosition, targetPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            Ctx.AppliedMovement = targetPosition;
        }

    }
}
