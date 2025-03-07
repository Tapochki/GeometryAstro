using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TandC.GeometryAstro.ScriptableObjects;
using TandC.GeometryAstro.Settings;
using UnityEngine;
using VContainer;

namespace TandC.GeometryAstro.Services
{
    public class SoundService : ILoadUnit
    {
        private List<SoundSource> _soundSources;
        private List<SoundPlayQueue> _soundPlayQueue;

        private Transform _soundContainer;

        private LoadObjectsService _loadObjectsService;
        private DataService _dataService;

        public float MusicVolume { get; private set; }
        public float SoundVolume { get; private set; }

        public SoundData SoundData { get; private set; }

        public void SetMusicVolume(float value) => MusicVolume = value;

        public void SetSoundVolume(float value) => SoundVolume = value;

        [Inject]
        public void Construct(LoadObjectsService loadObjectsService, DataService dataService)
        {
            _loadObjectsService = loadObjectsService;
            _dataService = dataService;
        }

        public async UniTask Load()
        {
            Initialize();
            await UniTask.CompletedTask;
        }

        public void Initialize()
        {
            SoundData = _loadObjectsService.GetObjectByPath<SoundData>("Data/SoundData");

            _soundSources = new List<SoundSource>();
            _soundPlayQueue = new List<SoundPlayQueue>();

            _soundContainer = new GameObject("[Sound Container]").transform;
            _soundContainer.gameObject.AddComponent<AudioListener>();

            _dataService.OnCacheLoadedEvent += CachedDataLoadedEventHandler;
        }

        public void Update()
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
            PlaySound(Sounds.Click);
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
            SoundVolume = _dataService.AppSettingsData.soundVolume;
            MusicVolume = _dataService.AppSettingsData.musicVolume;
        }

        public void SaveData()
        {
            _dataService.AppSettingsData.soundVolume = SoundVolume;
            _dataService.AppSettingsData.musicVolume = MusicVolume;

            _dataService.SaveCache(CacheType.AppSettingsData);
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

        private SoundService _soundService;

        public SoundSource(Transform parent, AudioClip sound, Sounds soundType, SoundParameters parameters,
                           SoundService soundService)
        {
            Sound = sound;
            SoundType = soundType;
            SoundParameters = parameters;
            _soundService = soundService;

            SoundSourceObject = new GameObject($"[Sound] - {SoundType} - {Time.time}");
            SoundSourceObject.transform.SetParent(parent);
            AudioSource = SoundSourceObject.AddComponent<AudioSource>();
            AudioSource.clip = Sound;
            AudioSource.loop = SoundParameters.IsLoop;

            AudioSource.volume = SoundParameters.IsSFX ? _soundService.SoundVolume : _soundService.MusicVolume;

            AudioSource.Play();
        }

        public void Update()
        {
            float targetVolume = SoundParameters.Volume * (SoundParameters.IsSFX ? _soundService.SoundVolume : _soundService.MusicVolume);

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