using System;
using UnityEngine;

namespace TandC.Data
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "TandC/Game/PlayerConfig", order = 1)]
    public class PlayerConfig : ScriptableObject
    {
        public PlayerData PlayerData;

        public void SetDefaultPlayerData() 
        {
            PlayerData = new PlayerData()
            {
                StartHealth = 100f,
                StartSpeed = 500f,
                ModelId = 0
            };
        }
    }

    [Serializable]
    public class PlayerData
    {
        public float StartHealth;
        public float StartSpeed;
        public int ModelId;
    }
}