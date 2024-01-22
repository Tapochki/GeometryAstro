using ChebDoorStudio.Settings;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChebDoorStudio.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "ChebDoorStudio/SoundData", order = 1)]
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