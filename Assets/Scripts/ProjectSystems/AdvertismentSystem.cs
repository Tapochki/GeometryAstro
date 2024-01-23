using System;
using TandC.Settings;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

namespace TandC.ProjectSystems
{
    public class AdvertismentSystem : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public event Action OnAdvertismentCompleteEvent;

        public event Action OnAdvertismentFailedEvent;

        [SerializeField] private string _androidGameId;
        [SerializeField] private string _iosGameId;
        [SerializeField] private bool _testMode;

        private string _video;
        private string _rewardedVideo;

        private string _gameId;
        public bool IsLoadRewardVideo { get; private set; }
        public bool IsLoadVideo { get; private set; }

        [Inject]
        public void Construct()
        {
            Utilities.Logger.Log("AdvertismentSystem Construct", LogTypes.Info);
        }

        public void Initialize()
        {
#if UNITY_ANDROID
            _gameId = _androidGameId;
            _video = "Interstitial_Android";
            _rewardedVideo = "Rewarded_Android";
#elif UNITY_IOS

            _gameId = _iosGameId;
            _video = "Interstitial_iOS";
            _rewardedVideo = "Rewarded_iOS";
#endif

            Advertisement.Initialize(_gameId, _testMode, this);
        }

        public void OnInitializationComplete()
        {
            LoadAd();
            LoadRewardVideo();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Utilities.Logger.Log($"Error Ads Initialize: ({error} - {message})", LogTypes.Error);
        }

        public void LoadRewardVideo()
        {
            Advertisement.Load(_rewardedVideo, this);
        }

        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(_rewardedVideo) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                Utilities.Logger.Log("Unity Ads completed", LogTypes.Info);
                OnAdvertismentCompleteEvent?.Invoke();
            }
            else
            {
                Utilities.Logger.Log("Unity Ads failed", LogTypes.Warning);
                OnAdvertismentFailedEvent?.Invoke();
                return;
            }

            if (adUnitId == _video)
            {
                LoadAd();
            }
            if (adUnitId == _rewardedVideo)
            {
                LoadRewardVideo();
            }
        }

        public void LoadAd()
        {
            Advertisement.Load(_video, this);
        }

        public void ShowAdsVideo(Action CompleteEvent, Action FailedEvent = null)
        {
            if (IsLoadRewardVideo)
            {
                OnAdvertismentCompleteEvent = CompleteEvent;

                if (FailedEvent != null)
                {
                    OnAdvertismentFailedEvent = FailedEvent;
                }

                IsLoadRewardVideo = false;
                Advertisement.Show(_rewardedVideo, this);
            }
        }

        public void ShowAd()
        {
            if (IsLoadVideo)
            {
                IsLoadVideo = false;
                Advertisement.Show(_video, this);
            }
        }

        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            if (adUnitId == _rewardedVideo)
            {
                IsLoadRewardVideo = true;
            }
            else
            {
                IsLoadVideo = true;
            }
        }

        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Utilities.Logger.Log($"Error on Ads load: {adUnitId} - {error} - {message}", LogTypes.Error);
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Utilities.Logger.Log($"Error on Ads show: {adUnitId} - {error} - {message}", LogTypes.Error);
        }

        public void OnUnityAdsShowStart(string adUnitId)
        {
        }

        public void OnUnityAdsShowClick(string adUnitId)
        {
        }
    }
}