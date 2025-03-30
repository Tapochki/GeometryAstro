using TandC.GeometryAstro.Data;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.Gameplay 
{
    public class ShieldSkill : IActiveSkill, IShield
    {
        public bool IsWeapon { get => false; }

        private IReloadable _reloader;
        private ActiveSkillData _data;
        private GameObject _shieldObject;
        private SpriteRenderer _shieldSprite;
        private Animator _animator;

        private ShieldColorConfig _colorConfig;

        private bool _isShieldActive;

        private int _currentShieldHealth;
        private int _maxShieldHealth;

        public ActiveSkillType SkillType { get; } = ActiveSkillType.Shield;


        private ShieldKnockbackEffect _knockbackEffect;

        private bool _isEvolve;

        public void SetData(ActiveSkillData data)
        {
            _data = data;
        }

        public void SetShieldColorConfig(ShieldColorConfig shieldColorConfig) 
        {
            _colorConfig = shieldColorConfig;
        }

        public void SetReloader(IReloadable reloader)
        {
            _reloader = reloader;
        }

        public void InitShieldObject(Transform owner) 
        {
            _shieldObject = GameObject.Instantiate(_data._skillPrefab, owner);
            _animator = _shieldObject.GetComponent<Animator>();
            _shieldSprite = _shieldObject.GetComponent<SpriteRenderer>();
        }

        public void SetPlayerShield(Player player) 
        {
            player.SetShield(this);
        }

        public void Initialization()
        {
            SetStartShieldHealth();
            ActivateShield();
        }

        private void SetStartShieldHealth() 
        {
            _currentShieldHealth = _maxShieldHealth = 1;
            UpdateShieldColor();
        }

        private void UpdateShieldColor() 
        {
            _shieldSprite.color = _colorConfig.shieldColors[_currentShieldHealth - 1];
        }

        private void InitKnockBack() 
        {
            _knockbackEffect = new ShieldKnockbackEffect(_data.detectorRadius, _data.bulletData.baseDamage, LayerMask.GetMask("Enemy"));
        }

        private void ActivateShield()
        {
            _animator.Play("Appear", -1, 0);
        }

        public void RestoringShield() 
        {
            if (_isEvolve)
            {
                FullRestoreShield();
                return;
            }

            if (_currentShieldHealth == 0) 
            {
                RestoreFromDeactivateShield();
                return;
            }

            RegularRestore();
            StartReload();
        }

        private void StartReload() 
        {
            if (_currentShieldHealth < _maxShieldHealth)
            {
                _reloader.StartReload();
            }
        }

        private void RegularRestore() 
        {
            _currentShieldHealth++;
            UpdateShieldColor();
            _animator.Play("RestoringShield", -1, 0);
        }

        private void RestoreFromDeactivateShield() 
        {
            _currentShieldHealth++;
            UpdateShieldColor();
            ActivateShield();
            StartReload();
        }

        private void FullRestoreShield() 
        {
            _currentShieldHealth = _maxShieldHealth;
            UpdateShieldColor();
            ActivateShield();
        }

        public bool TryAbsorbDamage() 
        {
            if(_currentShieldHealth <= 0) 
            {
                return false;
            }
            TakeDamage();
            return true;
        }

        private void TakeDamage()
        {
            _currentShieldHealth--;
            _reloader.StartReload();

            if (_currentShieldHealth <= 0)
            {
                DeactivateShield();
                return;
            }
            UpdateShieldColor();
            _animator.Play("Damage", -1, 0);
        }

        private void DeactivateShield()
        {
            _animator.Play("Break", -1, 0);
            if (_isEvolve)
            {
                KnockbackEffect();
            }
        }

        private void KnockbackEffect()
        {
            _knockbackEffect.TriggerEffect(_shieldObject.transform.position);
        }

        public void Tick()
        {
            _reloader.Update();

            if (_reloader.CanAction && _currentShieldHealth < _maxShieldHealth)
            {
                RestoringShield();
            }
        }

        public void Evolve()
        {
            _isEvolve = true;
            InitKnockBack();
        }

        public void Upgrade(float value = 0)
        {
            _maxShieldHealth++;
            _reloader.StartReload();
        }
    }

}

