using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName ="Game/Datas/Player Data")]
    public class PlayerDataSO : ScriptableObject
    {
        [SerializeField] private float _jumpHeight = 3f;
        
        public float maxHealth = 100f;
        public float speedMovement = 10f;
        public float turnSmoothTime = 0.1f;
        public Vector3 velocityGravity = Vector3.zero;
        public LayerMask layerMask = 0;

        public float ApplyForceJump => Mathf.Sqrt(_jumpHeight * -2f * Physics.gravity.y);
    }

}