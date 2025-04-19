using System;
using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class EnergyBeam : MonoBehaviour
    {
        [SerializeField]
        private GameObject _lineObject;
        [SerializeField]
        private AnimationSequence _lineAnimationObject;
        [SerializeField]
        private AnimationSequence _endlineObject;
        [SerializeField]
        private GameObject _startlineObject;

        private Enemy _targetEnemy;
        public Enemy TargetEnemy { get { return _targetEnemy; } }
        private IRotation _rotation;
        private IReloadable _reload;

        private Action _requestNewEnemy;

        private BulletData _beamData;

        private IReadableModificator _damageModificator;
        private IReadableModificator _critModificator;
        private IReadableModificator _critChanceModificator;

        private ExplosionDamage _explosionDamage;

        public bool IsHaveTarget { get; private set; }
        private bool _isEvolved;

        public void Init(BulletData data,
            IReadableModificator damageModificator,
            IReadableModificator critModificator,
            IReadableModificator critChanceModificator)
        {
            _beamData = data;
            _damageModificator = damageModificator;
            _critModificator = critModificator;
            _critChanceModificator = critChanceModificator;

            DiscardEnemy();
        }

        public void SetReload(IReloadable reload) 
        {
            _reload = reload;
        }

        public void SetRotation(IRotation rotation) 
        {
            _rotation = rotation;
        }

        public void Tick()
        {
            if (_reload.CanAction)
            {
                Action();
            }
            _reload.Update();

            if (!IsHaveTarget)
                return;

            SetScale();


            SetRotation();
        }

        private float CalculateCriticalChance()
        {
            return _beamData.BasicCriticalChance + _critChanceModificator.Value;
        }

        private float CalculateCriticalMultiplier()
        {
            return _beamData.BasicCriticalMultiplier + _critModificator.Value;
        }

        private float CalculateDamage()
        {
            return _beamData.baseDamage * _damageModificator.Value;
        }

        private void HitEnemy()
        {
            _targetEnemy.TakeDamage(
                CalculateDamage(),
                CalculateCriticalChance(),
                CalculateCriticalMultiplier()
                );

            if (_isEvolved) 
            {
                _explosionDamage.ApplyExplosionDamage(_targetEnemy.transform.position, 4f, _beamData.baseDamage, CalculateCriticalChance(), CalculateCriticalMultiplier());
            }
        }

        private void CheckEnemyAlive() 
        {
            if (!_targetEnemy.IsActive)
            {
                DiscardEnemy();
            }
        }

        private void RequestEnemy() 
        {
            _requestNewEnemy?.Invoke();
        }

        public void DiscardEnemy() 
        {
            IsHaveTarget = false;
            _lineAnimationObject.Stop();
            _endlineObject.Stop();
            gameObject.SetActive(false);
            Vector3 objectScale = _lineObject.transform.localScale;
            objectScale.y = 0;
            _lineObject.transform.localScale = objectScale;
        }

        private void Action() 
        {
            if (!IsHaveTarget) 
            {
                _reload.StartReload();
                return;
            }

            HitEnemy();
            CheckEnemyAlive();
            _reload.StartReload();
        }

        private void SetRotation()
        {
            _rotation.SetRotation(_targetEnemy.transform.position);
        }

        public void SetEnemy(Enemy enemy) 
        {
            if (enemy == null)
                return;
            gameObject.SetActive(true);
            _lineAnimationObject.Play();
            _endlineObject.Play();
            _targetEnemy = enemy;
            IsHaveTarget = true;
        }

        private void SetEndLine() 
        {
            _endlineObject.transform.position = _targetEnemy.transform.position;
        }

        public void Evolve(BulletData data) 
        {
            _beamData = data;
            _lineAnimationObject.GetComponent<SpriteRenderer>().color = Color.red;
            _endlineObject.GetComponent<SpriteRenderer>().color = Color.red;
            _endlineObject.transform.localScale = new Vector3(2, 2, 2);
            _explosionDamage = new ExplosionDamage();
            _isEvolved = true;
        }

        private void SetScale()
        {
            SetEndLine();
            float distance = Vector2.Distance(_startlineObject.transform.position, _targetEnemy.transform.position);
            float scale = distance / (30f);

            Vector3 objectScale = _lineObject.transform.localScale;

            objectScale.y = scale;

            if (objectScale.y > 1.5f)
            {
                DiscardEnemy();
            }
            _lineObject.transform.localScale = objectScale;
        }
    }
}

