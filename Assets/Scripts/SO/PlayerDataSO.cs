using UnityEngine;

namespace MedievalFantasyGame.PlayerSO
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Game/Datas/Player Data")]
    public class PlayerDataSO : ScriptableObject
    {
        public const float MaxHealth = 100.0f;
        [field: SerializeField, Range(1.0f, 10.0f)] public float MaxJumpHeight { get; private set; } = 1.0f;
        [field: SerializeField, Range(5.0f, 100.0f)] public float RunSpeed { get; private set; } = 15.0f;
        [field: SerializeField, Range(5.0f, 100.0f)] public float WalkSpeed { get; private set; } = 4.0f;

    }

}