using UnityEngine;

namespace GameplayCore
{
    /// <summary>
    /// Keyboard input handler (WASD / Arrow keys).
    /// For mobile builds: replace MoveDirection and RotationDirection setters
    /// with values from your joystick library (e.g. joystick.Direction).
    /// </summary>
    public class GameplayInputHandler : MonoBehaviour, IGameplayInputHandler
    {
        public Vector2 MoveDirection { get; private set; }
        public Vector2 RotationDirection { get; private set; }

        private void Update()
        {
            MoveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            // Optional: right-stick / second axis for independent rotation.
            // By default no separate rotation input — player faces the move direction.
            RotationDirection = Vector2.zero;
        }
    }
}
