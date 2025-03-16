using System.Collections.Generic;
using System.Linq;
using TandC.GeometryAstro.Settings;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Gameplay
{

    public class EnemySpawnPositionService : IEnemySpawnPositionService
    {
        [Inject] private GameplayCamera _gameplayCamera;

        [SerializeField] private float _spawnOffsetFactor = 0.4f;

        private Dictionary<SpawnPositionType, Transform> _spawnPoints;
        private ScreenBoundsCalculator _screenBounds;
        private Transform _spawnPointsParent;

        #region Initialization

        public void Init()
        {
            _spawnPointsParent = new GameObject("[SpawnPoints]").transform;
            _spawnPointsParent.SetParent(_gameplayCamera.transform);

            _screenBounds = new ScreenBoundsCalculator(_gameplayCamera.Camera);

            _spawnPoints = new Dictionary<SpawnPositionType, Transform>();

            var offset = _screenBounds.GetOffset(_spawnOffsetFactor);

            CreateCornerPoints(offset);

            GenerateHorizontalTopPoints(offset);
            GenerateHorizontalBottomPoints(offset);
            GenerateVerticalLeftPoints(offset);
            GenerateVerticalRightPoints(offset);


            CacheSpawnGroups();
        }
        #endregion
        #region Generate Points

        private void GenerateHorizontalTopPoints(Vector2 offset) 
        {
            GenerateHorizontalLine(
                SpawnPositionType.HorizontalTopLeft,
                SpawnPositionType.HorizontalTopCenter,
                SpawnPositionType.HorizontalTopRight,
                _screenBounds.VerticalMax + offset.y
                );
        }

        private void GenerateHorizontalBottomPoints(Vector2 offset)
        {
            GenerateHorizontalLine(
                SpawnPositionType.HorizontalBottomLeft,
                SpawnPositionType.HorizontalBottomCenter,
                SpawnPositionType.HorizontalBottomRight,
                -_screenBounds.VerticalMax - offset.y
                );
        }

        private void GenerateVerticalLeftPoints(Vector2 offset)
        {
            GenerateVerticalLine(
                SpawnPositionType.VerticalLeftTop,
                SpawnPositionType.VerticalLeftCenter,
                SpawnPositionType.VerticalLeftBottom,
                -_screenBounds.HorizontalMax - offset.x 
                );
        }

        private void GenerateVerticalRightPoints(Vector2 offset)
        {
            GenerateVerticalLine(
                SpawnPositionType.VerticalRightTop,
                SpawnPositionType.VerticalRightCenter,
                SpawnPositionType.VerticalRightBottom,
                _screenBounds.HorizontalMax + offset.x
            );
        }

        private void CreateCornerTopLeftPoint(Vector2 offset) 
        {
            CreateSpawnPoint(
                SpawnPositionType.CornerTopLeft,
                -_screenBounds.HorizontalMax - offset.x,
                _screenBounds.VerticalMax + offset.y
            );
        }

        private void CreateCornerTopRightPoint(Vector2 offset)
        {
            CreateSpawnPoint(
                SpawnPositionType.CornerTopRight,
                _screenBounds.HorizontalMax + offset.x,
                _screenBounds.VerticalMax + offset.y
            );
        }

        private void CreateCornerBottomLeftPoint(Vector2 offset)
        {
            CreateSpawnPoint(
                SpawnPositionType.CornerBottomLeft,
                -_screenBounds.HorizontalMax - offset.x,
                -_screenBounds.VerticalMax - offset.y
            );
        }

        private void CreateCornerBottomRightPoint(Vector2 offset)
        {
            CreateSpawnPoint(
                SpawnPositionType.CornerBottomRight,
                _screenBounds.HorizontalMax + offset.x,
                -_screenBounds.VerticalMax - offset.y
            );
        }

        private void CreateCornerPoints(Vector2 offset)
        {
            CreateCornerTopLeftPoint(offset);
            CreateCornerTopRightPoint(offset);
            CreateCornerBottomLeftPoint(offset);
            CreateCornerBottomRightPoint(offset);
        }

        private void GenerateHorizontalLine(SpawnPositionType leftType, SpawnPositionType centerType, SpawnPositionType rightType, float yPosition)
        {
            float horizontalOffset = _screenBounds.HorizontalMax * 0.25f;

            CreateSpawnPoint(
                leftType,
                -_screenBounds.HorizontalMax + horizontalOffset,
                yPosition
            );

            CreateSpawnPoint(
                centerType,
                0,
                yPosition
            );

            CreateSpawnPoint(
                rightType,
                _screenBounds.HorizontalMax - horizontalOffset,
                yPosition
            );
        }

        private void GenerateVerticalLine(SpawnPositionType topType, SpawnPositionType centerType, SpawnPositionType bottomType, float xPosition)
        {
            float verticalOffset = _screenBounds.VerticalMax * 0.25f;

            CreateSpawnPoint(
                topType,
                xPosition,
                _screenBounds.VerticalMax - verticalOffset
            );

            CreateSpawnPoint(
                centerType,
                xPosition,
                0
            );

            CreateSpawnPoint(
                bottomType,
                xPosition,
                -_screenBounds.VerticalMax + verticalOffset
            );
        }
        #endregion

        #region Public Interface

        public Vector2 GetOppositePosition(Vector2 spawnPosition)
        {
            return _screenBounds.GetMirroredPosition(spawnPosition);
        }

        public Transform GetRandomPositionFromRegister()
        {
            List<Transform> spawnPointList = _spawnPoints.Values.ToList();
            return spawnPointList[UnityEngine.Random.Range(0, spawnPointList.Count)];
        }

        #endregion

        #region Helper Classes
        private class ScreenBoundsCalculator
        {
            private readonly Camera _camera;
            public float HorizontalMax { get; }
            public float VerticalMax { get; }

            public ScreenBoundsCalculator(Camera camera)
            {
                _camera = camera;
                VerticalMax = _camera.orthographicSize;
                HorizontalMax = VerticalMax * _camera.aspect;
            }

            public Vector2 GetOffset(float factor) =>
                new Vector2(HorizontalMax * factor, VerticalMax * factor);

            public Vector2 GetMirroredPosition(Vector2 position) =>
                _camera.transform.position - (Vector3)position;
        }
        #endregion

        #region Spawn Groups
        private void CacheSpawnGroups()
        {
            //_cachedSpawnGroups = new Dictionary<SpawnType, Transform>
            //{
            //    [SpawnType.RandomSpawn] = new { GetRandomPositionFromRegister() },
            //};

        }

        private IReadOnlyList<Transform> GetAllPoints() =>
            _spawnPoints.Values.ToArray();

        private IReadOnlyList<Transform> GetHorizontalGroup(bool isTop)
        {
            var typePrefix = isTop ?
                SpawnPositionType.HorizontalTop :
                SpawnPositionType.HorizontalBottom;

            return Enumerable.Range(0, 3)
                .Select(i => _spawnPoints[typePrefix + i])
                .ToArray();
        }
        #endregion

        #region Factory
        private void CreateSpawnPoint(SpawnPositionType type, float x, float y)
        {
            var point = new GameObject(type.ToString())
            {
                transform =
            {
                position = new Vector3(x, y, 0),
                parent = _spawnPointsParent
            }
            };

            _spawnPoints.Add(type, point.transform);
        }
        #endregion
    }
}


