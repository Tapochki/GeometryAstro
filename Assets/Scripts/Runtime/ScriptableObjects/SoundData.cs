using System;
using System.Collections.Generic;
using TandC.GeometryAstro.Settings;
using UnityEngine;

namespace TandC.GeometryAstro.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "TandC/SoundData", order = 1)]
    public class SoundData : ScriptableObject
    {
        [SerializeField] public List<SoundInfo> sounds;

        [Serializable]
        public class SoundInfo
        {
            public Sounds soundType;
            public AudioClip audioClip;

            [Range(0, 1f)]
            public float volume = 1.0f;

            public bool isLoop;
            public bool isSfx;
        }
    }
}