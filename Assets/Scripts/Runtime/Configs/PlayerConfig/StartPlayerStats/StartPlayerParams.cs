using System.Collections.Generic;
using TandC.GeometryAstro.Settings;
using UnityEditor;
using UnityEngine;

namespace TandC.GeometryAstro.Data
{
    [CreateAssetMenu(fileName = "StartPlayerParams", menuName = "TandC/Player Params/Start Player Params", order = 1)]
    public class StartPlayerParams : ScriptableObject
    {
        [Tooltip("We store the player's starting parameters.")]
        public List<PlayerParam> StartParams = new List<PlayerParam>();
        [Tooltip("Player's starting active/passive skills")]
        public List<SkillType> StartSkills = new List<SkillType>();

#if UNITY_EDITOR
        [ContextMenu("Initialize Default Params")]
        private void InitializeDefaultParams()
        {
            StartParams.Clear();

            foreach (ModificatorType type in System.Enum.GetValues(typeof(ModificatorType)))
            {
                if (type == ModificatorType.None) continue;

                StartParams.Add(new PlayerParam
                {
                    Type = type,
                    StartValue = 0f,
                    IsPercentageValue = false
                });
            }

            EditorUtility.SetDirty(this);
        }

        protected void OnEnable()
        {
            if (StartParams.Count == 0)
            {
                InitializeDefaultParams();
            }
        }
#endif
    }

    [System.Serializable]
    public class PlayerParam
    {
        [Tooltip("Parameter type for modification")]
        public ModificatorType Type;
        [Tooltip("Start value of the parameter")]
        public float StartValue;
        [Tooltip("True if the paratransit is used as a percentage. For example, the percentage of increase in crit damage.")]
        public bool IsPercentageValue;

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
