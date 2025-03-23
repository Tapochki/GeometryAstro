using System;
using System.Collections.Generic;
using TandC.GeometryAstro.ConfigUtilities;
using TandC.GeometryAstro.Settings;
using UnityEditor;
using UnityEngine;

namespace TandC.GeometryAstro.Data 
{
    [CreateAssetMenu(fileName = "ModificatorUpgradeConfig", menuName = "TandC/Menu/ModificatorUpgradeConfig", order = 9)]
    public class ModificatorUpgradeConfig : ScriptableObject, IJsonSerializable
    {
        [SerializeField]
        private List<ModificatorData> _startModificatorsData;


        public List<ModificatorData> StartModificatorsData { get => _startModificatorsData; }

#if UNITY_EDITOR
        private void OnEnable()
        {
            InitializeModificatorsIfEmpty();
        }

        [ContextMenu("Initialize Default Modificators")]
        private void InitializeModificatorsIfEmpty()
        {
            if (_startModificatorsData == null || _startModificatorsData.Count == 0)
            {
                InitializeModificators();
                EditorUtility.SetDirty(this);
            }
        }
#endif
        public void InitializeModificators()
        {
            _startModificatorsData = new List<ModificatorData>
            {
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.Damage,
                        IncrementValue = 10f,
                        IsPercentageValue = true
                    },
                    MaxLevel = 10
                },
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.MaxHealth,
                        IncrementValue = 25f,
                        IsPercentageValue = false
                    },
                    MaxLevel = 10
                },
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.Armor,
                        IncrementValue = 3f,
                        IsPercentageValue = false
                    },
                    MaxLevel = 10
                },
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.SpeedMoving,
                        IncrementValue = 10f,
                        IsPercentageValue = true
                    },
                    MaxLevel = 10
                },
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.ReviveCount,
                        IncrementValue = 1f,
                        IsPercentageValue = false
                    },
                    MaxLevel = 1
                },
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.CriticalDamageMultiplier,
                        IncrementValue = 0.25f,
                        IsPercentageValue = false
                    },
                    MaxLevel = 10
                },
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.CriticalChance,
                        IncrementValue = 2f,
                        IsPercentageValue = false
                    },
                    MaxLevel = 10
                },
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.CurseStrength,
                        IncrementValue = 0.1f,
                        IsPercentageValue = false
                    },
                    MaxLevel = 10
                },
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.CurseSpeed,
                        IncrementValue = 0.1f,
                        IsPercentageValue = false
                    },
                    MaxLevel = 10
                },
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.BulletsSize,
                        IncrementValue = 0.1f,
                        IsPercentageValue = false
                    },
                    MaxLevel = 10
                },
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.Duplicator,
                        IncrementValue = 1f,
                        IsPercentageValue = false
                    },
                    MaxLevel = 1
                },
                new ModificatorData
                {
                     IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.HealtRestoreCount,
                        IncrementValue = 0.1f,
                        IsPercentageValue = false
                    },
                    MaxLevel = 10
                },
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.ReloadTimer,
                        IncrementValue = -3f,
                        IsPercentageValue = true
                    },
                    MaxLevel = 10
                },
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.PickUpRadius,
                        IncrementValue = 20f,
                        IsPercentageValue = true
                    },
                    MaxLevel = 10
                },
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.ReceivedExperience,
                        IncrementValue = 0.25f,
                        IsPercentageValue = true
                    },
                    MaxLevel = 10
                },
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.ReceivingCoins,
                        IncrementValue = 0.25f,
                        IsPercentageValue = true
                    },
                    MaxLevel = 10
                },
                new ModificatorData
                {
                    IncreamentData = new IncreamentModificatorData
                    {
                        Type = ModificatorType.Luck,
                        IncrementValue = 20f,
                        IsPercentageValue = true
                    },
                    MaxLevel = 5
                }
            };
        }
    }

    [Serializable]
    public class ModificatorData
    {
        public Sprite Icon;
        public string ModificatorName;
        public string ModificatorDescription;

        public int MaxLevel;

        public IncreamentModificatorData IncreamentData;
    }
}
