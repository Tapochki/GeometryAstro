using Studio.ScriptableObjects;
using Studio.Settings;
using Studio.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Studio.ProjectSystems
{
    public class SoundSystem : IInitializable
    {
        private List<SoundSource> _soundSources;
        private List<SoundPlayQueue> _soundPlayQueue;

        private Transform _soundContainer;

        private LoadObjectsSystem _loadObjectsSystem;
        private DataSystem _dataSystem;

        public float MusicVolume { get; private set; }
        public float SoundVolume { get; private set; }

        public SoundData SoundData { get; private set; }

        public void SetMusicVolume(float value) => MusicVolume = value;

        public void SetSoundVolume(float value) => SoundVolume = value;

        [Inject]
        public void Construct(LoadObjectsSystem loadObjectsSystem, DataSystem dataSystem)
        {
            Utilities.Logger.Log("SoundSystem Construct", LogTypes.Info);

            _loadObjectsSystem = loadObjectsSystem;
            _dataSystem = dataSystem;
        }

        public void Initialize()
        {
            SoundData = _loadObjectsSystem.GetObjectByPath<SoundData>("Data/SoundData");

            _soundSources = new List<SoundSource>();
            _soundPlayQueue = new List<SoundPlayQueue>();

            _soundContainer = new GameObject("[Sound Container]").transform;
            _soundContainer.gameObject.AddComponent<AudioListener>();
            MonoBehaviour.DontDestroyOnLoad(_soundContainer);

            _dataSystem.OnCacheLoadedEvent += CachedDataLoadedEventHandler;
        }

        public void OnUpdateEventHandler()
        {
            if (_soundSources == null)
            {
                return;
            }

            if (_soundPlayQueue == null)
            {
                return;
            }

            for (int i = 0; i < _soundSources.Count; i++)
            {
                _soundSources[i].Update();

                if (_soundSources[i].IsSoundEnded())
                {
                    _soundSources[i].Dispose();
                    _soundSources.RemoveAt(i--);
                }
            }

            for (int i = 0; i < _soundPlayQueue.Count; i++)
            {
                _soundPlayQueue[i].time -= Time.deltaTime;

                if (_soundPlayQueue[i].time <= 0f)
                {
                    _soundPlayQueue[i].action?.Invoke();
                    _soundPlayQueue.RemoveAt(i--);
                }
            }
        }

        public void PlayClickSound()
        {

        }

        public void PlaySound(Sounds soundType)
        {
            if (soundType == Sounds.Unknown)
            {
                return;
            }

            var soundInfo = SoundData.sounds.Find(item => item.soundType == soundType);

            if (soundInfo == null)
            {
                return;
            }

            SoundSource foundSameSource = _soundSources.Find(soundSource => soundSource.SoundType == soundType);

            if (foundSameSource != null)
            {
                if (!soundInfo.isSfx)
                {
                    return;
                }
            }

            AudioClip sound = soundInfo.audioClip;
            SoundParameters parameters = new SoundParameters()
            {
                IsLoop = soundInfo.isLoop,
                IsSFX = soundInfo.isSfx,
                Volume = soundInfo.volume,
            };

            _soundSources.Add(new SoundSource(_soundContainer, sound, soundType, parameters, this));
        }

        private void CachedDataLoadedEventHandler()
        {
            SoundVolume = _dataSystem.AppSettingsData.soundVolume;
            MusicVolume = _dataSystem.AppSettingsData.musicVolume;
        }

        public void SaveData()
        {
            _dataSystem.AppSettingsData.soundVolume = SoundVolume;
            _dataSystem.AppSettingsData.musicVolume = MusicVolume;

            _dataSystem.SaveCache(CacheType.AppSettingsData);
        }

        public void StopSound(Sounds soundType)
        {
            for (int i = 0; i < _soundSources.Count; i++)
            {
                if (_soundSources[i].SoundType == soundType)
                {
                    _soundSources[i].StopPlaying();
                }
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _soundSources.Count; i++)
            {
                _soundSources[i].Dispose();
            }

            _soundSources.Clear();
            _soundPlayQueue.Clear();
            MonoBehaviour.Destroy(_soundContainer.gameObject);
        }

        public void PlaySoundDelayed(Sounds soundType, float delay)
        {
            if (soundType == Sounds.Unknown)
            {
                return;
            }

            delay = Mathf.Clamp(delay, 0f, 999f);

            if (delay == 0f)
            {
                PlaySound(soundType);
            }
            else
            {
                _soundPlayQueue.Add(new SoundPlayQueue()
                {
                    time = delay,
                    action = () => PlaySound(soundType)
                });
            }
        }

        public void StopAllSounds()
        {
            for (int i = 0; i < _soundSources.Count; i++)
            {
                _soundSources[i].StopPlaying();
            }
        }
    }

    internal class SoundPlayQueue
    {
        public float time;
        public Action action;
    }

    internal class SoundSource
    {
        public GameObject SoundSourceObject { get; }
        public AudioClip Sound { get; }
        public AudioSource AudioSource { get; }
        public Sounds SoundType { get; }
        public SoundParameters SoundParameters { get; }

        private SoundSystem _soundSystem;

        public SoundSource(Transform parent, AudioClip sound, Sounds soundType, SoundParameters parameters,
                           SoundSystem soundSystem)
        {
            Sound = sound;
            SoundType = soundType;
            SoundParameters = parameters;
            _soundSystem = soundSystem;

            SoundSourceObject = new GameObject($"[Sound] - {SoundType} - {Time.time}");
            SoundSourceObject.transform.SetParent(parent);
            AudioSource = SoundSourceObject.AddComponent<AudioSource>();
            AudioSource.clip = Sound;
            AudioSource.loop = SoundParameters.IsLoop;

            AudioSource.volume = SoundParameters.IsSFX ? _soundSystem.SoundVolume : _soundSystem.MusicVolume;

            AudioSource.Play();
        }

        public void Update()
        {
            float targetVolume = SoundParameters.Volume * (SoundParameters.IsSFX ? _soundSystem.SoundVolume : _soundSystem.MusicVolume);

            AudioSource.volume = targetVolume;
        }

        public bool IsSoundEnded()
        {
            return !AudioSource.loop && !AudioSource.isPlaying;
        }

        public void Dispose()
        {
            AudioSource.Stop();
            MonoBehaviour.Destroy(SoundSourceObject);
        }

        public void StopPlaying()
        {
            AudioSource.loop = false;

            AudioSource.Stop();
        }
    }

    public class SoundParameters
    {
        public bool IsLoop { get; set; } = false;
        public bool IsSFX { get; set; } = true;
        public float Volume { get; set; } = 1f;
    }
}