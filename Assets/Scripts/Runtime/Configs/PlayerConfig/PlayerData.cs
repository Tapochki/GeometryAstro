using System;
using UnityEngine;

namespace TandC.GeometryAstro.Data
{
    [CreateAssetMenu(fileName = "UserData", menuName = "TandC/Game/UserData", order = 1)]
    public class UserData : ScriptableObject
    {
        public PlayerData PlayerData;

        public UserData() 
        {
            SetDefaultPlayerData();

        }

        private void SetDefaultPlayerData() 
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