// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.InputSystem;

// [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
// public class InputReader : ScriptableObject, InputControl.IGameplayActions
// {
//     public event UnityAction<Vector2> MoveEvent;
//     public event UnityAction InteractEvent;
//     public event UnityAction JumpEvent;
//     public event UnityAction AttackEvent;
//     public event UnityAction Skill1Event;
//     public event UnityAction Skill2Event;
//     public event UnityAction<Vector2, bool> CameraMoveEvent;
//     public event UnityAction EnableMouseControlCameraEvent;
//     public event UnityAction DisableMouseControlCameraEvent;

//     private InputControl _inputControl = null;

//     private void OnEnable()
//     {
//         if (_inputControl == null)
//         {
//             _inputControl = new InputControl();
//             _inputControl.Gameplay.SetCallbacks(this);
//         }
//         _inputControl.Gameplay.Enable();
//     }

//     private void OnDisable()
//     {
//         _inputControl.Gameplay.Disable();
//     }


//     // Gameplay Input Actions 

//     private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

//     public void OnMove(InputAction.CallbackContext context)
//     {
//         MoveEvent?.Invoke(context.ReadValue<Vector2>());
//     }

//     public void OnJump(InputAction.CallbackContext context)
//     {
//         if (context.phase == InputActionPhase.Performed)
//         {
//             JumpEvent?.Invoke();
//         }
//     }

//     public void OnAttack(InputAction.CallbackContext context)
//     {
//         if (context.phase == InputActionPhase.Performed)
//         {
//             AttackEvent?.Invoke();
//         }
//     }

//     public void OnPause(InputAction.CallbackContext context)
//     {
//     }

//     public void OnSkill_1(InputAction.CallbackContext context)
//     {
//         if (context.phase == InputActionPhase.Performed)
//         {
//             Skill1Event?.Invoke();
//         }
//     }

//     public void OnSkill_2(InputAction.CallbackContext context)
//     {
//         if (context.phase == InputActionPhase.Performed)
//         {
//             Skill2Event?.Invoke();
//         }
//     }

//     public void OnRotateCamera(InputAction.CallbackContext context)
//     {
//         CameraMoveEvent?.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
//     }

//     public void OnMouseControlCamera(InputAction.CallbackContext context)
//     {
//         if (context.phase == InputActionPhase.Performed)
//         {
//             EnableMouseControlCameraEvent?.Invoke();
//         }

//         if (context.phase == InputActionPhase.Canceled)
//         {
//             DisableMouseControlCameraEvent?.Invoke();
//         }
//     }

//     public void OnInteract(InputAction.CallbackContext context)
//     {
//         if (context.phase == InputActionPhase.Performed)
//         {
//             InteractEvent?.Invoke();
//         }
//     }

// }
