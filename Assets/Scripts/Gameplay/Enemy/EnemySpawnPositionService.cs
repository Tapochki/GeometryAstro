using System;
using System.Collections.Generic;
using System.Linq;
using TandC.Settings;
using UnityEngine;

namespace TandC.Gameplay
{

    public class EnemySpawnPositionService : MonoBehaviour
    {
        [SerializeField] private Transform _playerDirectionSpawnPoint;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _spawnOffsetFactor = 0.3f;

        private Dictionary<SpawnPositionType, Transform> _spawnPointObjects;

        private Dictionary<SpawnType, Func<List<Transform>>> _spawnTypes;

#region Initializing
        private void Start()
        {
            RegisterSpawnPositions();
            InitializeSpawnTypes();
        }

        private void RegisterSpawnPositions()
        {
            float screenX = _camera.orthographicSize * Screen.width / Screen.height;
            float screenY = _camera.orthographicSize;
            float offsetX = _spawnOffsetFactor * screenX;
            float offsetY = _spawnOffsetFactor * screenY;

            _spawnPointObjects = new Dictionary<SpawnPositionType, Transform>();

            RegisterSpawnPoint(SpawnPositionType.CornerTopLeft, -screenX - offsetX, screenY + offsetY);
            RegisterSpawnPoint(SpawnPositionType.CornerTopRight, screenX + offsetX, screenY + offsetY);
            RegisterSpawnPoint(SpawnPositionType.CornerBottomLeft, -screenX - offsetX, -screenY - offsetY);
            RegisterSpawnPoint(SpawnPositionType.CornerBottomRight, screenX + offsetX, -screenY - offsetY);

            RegisterSpawnPoint(SpawnPositionType.HorizontalTopLeft, -screenX + offsetX, screenY + offsetY);
            RegisterSpawnPoint(SpawnPositionType.HorizontalTopCenter, 0f, screenY + offsetY);
            RegisterSpawnPoint(SpawnPositionType.HorizontalTopRight, screenX - offsetX, screenY + offsetY);

            RegisterSpawnPoint(SpawnPositionType.HorizontalBottomLeft, -screenX + offsetX, -screenY - offsetY);
            RegisterSpawnPoint(SpawnPositionType.HorizontalBottomCenter, 0f, -screenY - offsetY);
            RegisterSpawnPoint(SpawnPositionType.HorizontalBottomRight, screenX - offsetX, -screenY - offsetY);

            RegisterSpawnPoint(SpawnPositionType.VerticalLeftTop, -screenX - offsetX, screenY - offsetY);
            RegisterSpawnPoint(SpawnPositionType.VerticalLeftCenter, -screenX - offsetX, 0f);
            RegisterSpawnPoint(SpawnPositionType.VerticalLeftBottom, -screenX - offsetX, -screenY + offsetY);

            RegisterSpawnPoint(SpawnPositionType.VerticalRightTop, screenX + offsetX, screenY - offsetY);
            RegisterSpawnPoint(SpawnPositionType.VerticalRightCenter, screenX + offsetX, 0f);
            RegisterSpawnPoint(SpawnPositionType.VerticalRightBottom, screenX + offsetX, -screenY + offsetY);

            _playerDirectionSpawnPoint.position = new Vector2(0, screenY + offsetY);
        }

        private void RegisterSpawnPoint(SpawnPositionType spawnType, float x, float y)
        {
            GameObject spawnPointObject = new GameObject(spawnType.ToString());
            spawnPointObject.transform.position = new Vector3(x, y, 0);
            spawnPointObject.transform.parent = _camera.transform;

            _spawnPointObjects.Add(spawnType, spawnPointObject.transform);
        }

        private void InitializeSpawnTypes()
        {
            _spawnTypes = new Dictionary<SpawnType, Func<List<Transform>>>
            {
                { SpawnType.RandomSpawn, GetRandomSpawnPosition },
                { SpawnType.CircleSpawn, GetCircleSpawnPositions },
                { SpawnType.AllTopHorizontalSpawn, GetAllTopHorizontalSpawnPositions },
                { SpawnType.AllBottomHorizontalSpawn, GetAllBottomHorizontalSpawnPositions },
                { SpawnType.AllLeftVerticalSpawn, GetAllLeftVerticalSpawnPositions },
                { SpawnType.AllRightVerticalSpawn, GetAllRightVerticalSpawnPositions }
            };
        }
#endregion

        public List<Transform> GetSpawnPointsForType(SpawnType spawnType) 
        {
            if (_spawnTypes.TryGetValue(spawnType, out var spawnMethod))
            {
                return spawnMethod.Invoke();
            }

            throw new ArgumentException($"Unknown spawn type: {spawnType}");
        }

        public Vector2 GetOppositePosition(Vector2 spawnPosition)
        {
            Vector2 oppositeSpawnPoint = -spawnPosition;
            return oppositeSpawnPoint;
        }

#region DifferentSpawnTypes

        private List<Transform> GetCircleSpawnPositions()
        {
            return new List<Transform>(_spawnPointObjects.Values);
        }

        private List<Transform> GetRandomSpawnPosition()
        {
            List<Transform> spawnPositions = new List<Transform>();

            int randomValue = UnityEngine.Random.Range(0, 2);

            if (randomValue == 0)
            {
                spawnPositions.Add(GetPlayerMoveDirectionPosition());
            }
            else
            {
                spawnPositions.Add(GetRandomPositionFromRegister());
            }

            return spawnPositions;
        }

        private Transform GetRandomPositionFromRegister()
        {
            List<Transform> spawnPointList = _spawnPointObjects.Values.ToList();
            return (spawnPointList.Count > 0) ? spawnPointList[UnityEngine.Random.Range(0, spawnPointList.Count)] : null;
        }

        private Transform GetPlayerMoveDirectionPosition()
        {
            return _playerDirectionSpawnPoint;
        }

        private List<Transform> GetAllTopHorizontalSpawnPositions()
        {
            return new List<Transform>
            {
                GetSpawnRegisterPosition(SpawnPositionType.CornerTopLeft),
                GetSpawnRegisterPosition(SpawnPositionType.HorizontalBottomLeft),
                GetSpawnRegisterPosition(SpawnPositionType.HorizontalBottomCenter),
                GetSpawnRegisterPosition(SpawnPositionType.HorizontalBottomRight),
                GetSpawnRegisterPosition(SpawnPositionType.CornerTopRight)
            };
        }

        private List<Transform> GetAllBottomHorizontalSpawnPositions()
        {
            return new List<Transform>
            {
                GetSpawnRegisterPosition(SpawnPositionType.CornerBottomLeft),
                GetSpawnRegisterPosition(SpawnPositionType.HorizontalTopLeft),
                GetSpawnRegisterPosition(SpawnPositionType.HorizontalTopCenter),
                GetSpawnRegisterPosition(SpawnPositionType.HorizontalTopRight),
                GetSpawnRegisterPosition(SpawnPositionType.CornerBottomRight)
            };
        }

        private List<Transform> GetAllRightVerticalSpawnPositions()
        {
            return new List<Transform>
            {
                GetSpawnRegisterPosition(SpawnPositionType.CornerTopRight),
                GetSpawnRegisterPosition(SpawnPositionType.VerticalRightTop),
                GetSpawnRegisterPosition(SpawnPositionType.VerticalRightCenter),
                GetSpawnRegisterPosition(SpawnPositionType.VerticalRightBottom),
                GetSpawnRegisterPosition(SpawnPositionType.CornerBottomRight)
            };
        }

        private List<Transform> GetAllLeftVerticalSpawnPositions()
        {
            return new List<Transform>
            {
                GetSpawnRegisterPosition(SpawnPositionType.CornerTopLeft),
                GetSpawnRegisterPosition(SpawnPositionType.VerticalLeftTop),
                GetSpawnRegisterPosition(SpawnPositionType.VerticalLeftCenter),
                GetSpawnRegisterPosition(SpawnPositionType.VerticalLeftBottom),
                GetSpawnRegisterPosition(SpawnPositionType.CornerBottomLeft)
            };
        }

        private Transform GetSpawnRegisterPosition(SpawnPositionType positionType)
        {
            if (_spawnPointObjects.TryGetValue(positionType, out Transform spawnPoint))
            {
                return spawnPoint;
            }

            throw new ArgumentException($"Unknown spawn position: {positionType}");
        }
        #endregion

    }

}


