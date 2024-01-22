using ChebDoorStudio.Gameplay.Items.Base;
using ChebDoorStudio.Settings;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChebDoorStudio.ScriptableObjects
{
    [CreateAssetMenu(fileName = "InitialGameData", menuName = "ChebDoorStudio/InitialGameData", order = 2)]
    public class InitialGameData : ScriptableObject
    {
        [SerializeField] public PlayerData playerData;
        [SerializeField] public EnemyData enemyData;
        [SerializeField] public List<ScoreMultiplierData> scoresMultipliers;
        [SerializeField] public List<LevelData> levels;
        [SerializeField] public ItemData itemData;
        [SerializeField] public List<PlayerSkins> playerSkins;

        public int coinsForAds = 5;
    }

    [Serializable]
    public class PlayerData
    {
        public Sprite skin;
        public float rotatingSpeed;
        public Vector3 initialRotate = Vector3.zero;
    }

    [Serializable]
    public class EnemyData
    {
        public float minRotatingSpeed;
        public float maxRotatingSpeed;

        public float minRotatingTime;
        public float maxRotatingTime;

        public Vector3 initialRotate = Vector3.zero;
    }

    [Serializable]
    public class ScoreMultiplierData
    {
        public int scoreMultiplier;
    }

    [Serializable]
    public class LevelData
    {
        public int level;
    }

    [Serializable]
    public class ItemData
    {
        public float timeToSpawnItem;

        public List<ItemBase> items;
    }

    [Serializable]
    public class PlayerSkins
    {
        public ShopItemType type;
        public Sprite skin;
    }
}