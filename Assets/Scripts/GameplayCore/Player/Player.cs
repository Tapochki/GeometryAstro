using System.Collections.Generic;
using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Main player ship controller. Handles movement and rotation.
    /// Weapons are registered via RegisterWeapon() and ticked automatically.
    ///
    /// Setup:
    ///   1. Add Rigidbody2D to this GameObject (set Gravity Scale = 0, Freeze Z rotation).
    ///   2. Add a GameplayInputHandler (or your own IGameplayInputHandler) to the same GameObject.
    ///   3. Optionally assign a separate body Transform for rotation visuals.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;

        [Header("References")]
        [Tooltip("The visual body to rotate (defaults to this transform if left empty).")]
        [SerializeField] private Transform _bodyTransform;

        [Tooltip("Parent transform where weapon patterns are spawned.")]
        [SerializeField] private Transform _skillTransform;

        private IGameplayInputHandler _inputHandler;
        private IMove _moveComponent;
        private IRotation _rotateComponent;

        private readonly List<IActiveSkill> _weapons = new();

        public Transform SkillTransform => _skillTransform != null ? _skillTransform : transform;

        private void Awake()
        {
            _inputHandler = GetComponent<IGameplayInputHandler>();
            _moveComponent = new MoveComponent(GetComponent<Rigidbody2D>());
            _rotateComponent = new PlayerRotateComponent(_bodyTransform != null ? _bodyTransform : transform);
        }

        private void FixedUpdate()
        {
            _moveComponent.Move(_inputHandler.MoveDirection, _moveSpeed);

            Vector2 rotDir = _inputHandler.RotationDirection != Vector2.zero
                ? _inputHandler.RotationDirection
                : _inputHandler.MoveDirection;

            if (rotDir != Vector2.zero)
                _rotateComponent.SetRotation(rotDir);

            _rotateComponent.Update();

            for (int i = 0; i < _weapons.Count; i++)
                _weapons[i].Tick();
        }

        /// <summary>
        /// Register a weapon so it is ticked every frame. Call Initialization() before or here.
        /// </summary>
        public void RegisterWeapon(IActiveSkill weapon)
        {
            weapon.Initialization();
            _weapons.Add(weapon);
        }
    }
}
