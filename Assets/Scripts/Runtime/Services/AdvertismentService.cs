#if UNITY_ANDROID || UNITY_IOS
using System;
using TandC.GeometryAstro.Utilities.Logging;
using UnityEngine;
using UnityEngine.Advertisements;

namespace TandC.GeometryAstro.Services
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

        public void Construct()
        {
            Log.Default.D("AdvertismentSystem Construct");
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
            Log.Default.D($"Error Ads Initialize: ({error} - {message})");
        }

        public void LoadRewardVideo()
        {
            Advertisement.Load(_rewardedVideo, this);
        }

        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(_rewardedVideo) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                Log.Default.D("Unity Ads completed");
                OnAdvertismentCompleteEvent?.Invoke();
            }
            else
            {
                Log.Default.ThrowException("Unity Ads failed");
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
            Log.Default.ThrowException($"Error on Ads load: {adUnitId} - {error} - {message}");
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Log.Default.ThrowException($"Error on Ads show: {adUnitId} - {error} - {message}");
        }

        public void OnUnityAdsShowStart(string adUnitId)
        {
        }

        public void OnUnityAdsShowClick(string adUnitId)
        {
        }
    }

}
#endif