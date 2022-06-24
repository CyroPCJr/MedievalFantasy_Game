// using Managers.Cameras;
// using System;
// using System.Linq;
// using UnityEngine;

// namespace Managers.Spawn
// {
//     public class SpawnSystem : MonoBehaviour
//     {
//         [Header("Settings")]
//         [SerializeField] private int _defaultSpawnIndex = 0;

//         [Header("Project References")]
//         [SerializeField] private Player.Player _playerPrefab = null;

//         [Header("Scene References")]
//         [SerializeField] private CameraManager _cameraManager;
//         [SerializeField] private Transform[] _spawnLocations;

//         void Start()
//         {
//             try
//             {
//                 Spawn(_defaultSpawnIndex);
//             }
//             catch (Exception e)
//             {
//                 Debug.LogError($"[SpawnSystem] Failed to spawn player. {e.Message}");
//             }
//         }

//         void Reset()
//         {
//             AutoFill();
//         }

//         /// <summary>
//         /// This function tries to autofill some of the parameters of the component, so it's easy to drop in a new scene
//         /// </summary>
//         [ContextMenu("Attempt Auto Fill")]
//         private void AutoFill()
//         {
//             if (_cameraManager == null)
//                 _cameraManager = FindObjectOfType<CameraManager>();

//             if (_spawnLocations == null || _spawnLocations.Length == 0)
//                 _spawnLocations = transform.GetComponentsInChildren<Transform>(true)
//                                     .Where(t => t != this.transform)
//                                     .ToArray();
//         }

//         private void Spawn(int spawnIndex)
//         {
//             Transform spawnLocation = GetSpawnLocation(spawnIndex, _spawnLocations);
//             Player.Player playerInstance = InstantiatePlayer(_playerPrefab, spawnLocation, _cameraManager);
//             SetupCameras(playerInstance);
//         }

//         private Transform GetSpawnLocation(int index, Transform[] spawnLocations)
//         {
//             if (spawnLocations == null || spawnLocations.Length == 0)
//                 throw new Exception("No spawn locations set.");

//             index = Mathf.Clamp(index, 0, spawnLocations.Length - 1);
//             return spawnLocations[index];
//         }

//         private Player.Player InstantiatePlayer(Player.Player playerPrefab, Transform spawnLocation, CameraManager _cameraManager)
//         {
//             if (playerPrefab == null)
//                 throw new Exception("Player Prefab can't be null.");

//             Player.Player playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);

//             return playerInstance;
//         }

//         private void SetupCameras(Player.Player player)
//         {
//             player.gameplayCamera = _cameraManager.mainCamera.transform;
//             _cameraManager.SetupProtagonistVirtualCamera(player.transform);
//         }

//     }

// }
