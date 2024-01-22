using ChebDoorStudio.ProjectSystems;
using ChebDoorStudio.ScriptableObjects;
using ChebDoorStudio.UI.Views.Base;
using ChebDoorStudio.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ChebDoorStudio.UI.Views
{
    public class ViewShopPage : View
    {
        public event Action<Sprite> OnPlayerSkinChangeEvent;

        private Button _backButton;
        private Button _buyButton;
        private Button _selectButton;
        private Button _advertisementButton;

        private ShadowedTextMexhProUGUI _coinsCountText;
        private ShadowedTextMexhProUGUI _adButtonText;

        private ScrollRect _scrollRect;

        private Transform _shopItemsParent;

        private AdvertismentSystem _advertisingSystem;
        private LoadObjectsSystem _loadObjectsSystem;
        private LocalisationSystem _localisationSystem;
        private GameStateSystem _gameStateSystem;
        private DataSystem _dataSystem;
        private VaultSystem _vaultSystem;
        private PurchasingSystem _purchasingSystem;

        [Inject]
        public void Construct(AdvertismentSystem advertismentSystem, LoadObjectsSystem loadObjectsSystem,
                                LocalisationSystem localisationSystem, GameStateSystem gameStateSystem,
                                DataSystem dataSystem, VaultSystem vaultSystem, PurchasingSystem purchasingSystem)
        {
            _advertisingSystem = advertismentSystem;
            _loadObjectsSystem = loadObjectsSystem;
            _localisationSystem = localisationSystem;
            _gameStateSystem = gameStateSystem;
            _dataSystem = dataSystem;
            _vaultSystem = vaultSystem;
            _purchasingSystem = purchasingSystem;

            _vaultSystem.OnCoinsAmountChangedEvent += OnCoinsAmountChangedEventHandler;
        }

        public override void Initialize()
        {
            _backButton = transform.Find("Container_SafeArea/Button_Back").GetComponent<Button>();
            _buyButton = transform.Find("Button_Buy").GetComponent<Button>();
            _selectButton = transform.Find("Button_Select").GetComponent<Button>();
            _advertisementButton = transform.Find("Button_Ad").GetComponent<Button>();

            _coinsCountText = transform.Find("Container_SafeArea/Container_Coins/ShadowedText_Value").GetComponent<ShadowedTextMexhProUGUI>();
            _adButtonText = transform.Find("Button_Ad/ShadowedText_Title").GetComponent<ShadowedTextMexhProUGUI>();

            _scrollRect = transform.Find("Scroll View").GetComponent<ScrollRect>();

            _shopItemsParent = _scrollRect.transform.Find("Viewport/Content").transform;

            base.Initialize();

            _coinsCountText.UpdateTextAndShadowValue(_vaultSystem.Coins.Get().ToString());

            CreateShopItems();

            HideShopButton();

            //OnPlayerSkinChangeEvent?.Invoke(_shopItems.Find(x => x.Type == Settings.ShopItemType.RedPlayer).Data.itemPreview);

            _backButton.onClick.AddListener(BackButtonOnClickHandler);
            _buyButton.onClick.AddListener(BuyButtonOnClickHandler);
            _selectButton.onClick.AddListener(SelectButtonOnClickHandler);
            _advertisementButton.onClick.AddListener(AdvertisementButtonOnClickHandler);

            //_adButtonText.UpdateTextAndShadowValue(string.Format(_localisationSystem.GetString($"title_button_get_money"), _initialGameData.coinsForAds));
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();

            _scrollRect.horizontalNormalizedPosition = 0.0f;

            HideShopButton();

            ResetItemsView();
        }

        public override void Dispose()
        {
            base.Dispose();

            _backButton.onClick.AddListener(BackButtonOnClickHandler);
            _buyButton.onClick.AddListener(BuyButtonOnClickHandler);
            _selectButton.onClick.AddListener(SelectButtonOnClickHandler);
            _advertisementButton.onClick.AddListener(AdvertisementButtonOnClickHandler);

            _backButton = null;
            _buyButton = null;
            _selectButton = null;
            _advertisementButton = null;

            _shopItemsParent = null;
        }

        private void HideShopButton()
        {
            _buyButton.gameObject.SetActive(false);
            _selectButton.gameObject.SetActive(false);
            _advertisementButton.gameObject.SetActive(false);
        }

        private void CreateShopItems()
        {
            //for (int i = 0; i < _shopData.items.Count; i++)
            //{
            //    ShopItem shopItem = MonoBehaviour.Instantiate(_shopItemPrefab, _shopItemsParent);

            //    shopItem.Initialize(_shopData.items[i],
            //                        _dataSystem.ShopPurchasedItemData.shopItemData.Exists(x => x == _shopData.items[i].type));

            //    shopItem.OnItemSelectedEvent += OnItemSelectedEventHandler;

            //    _shopItems.Add(shopItem);
            //}
        }

        private void ResetItemsView()
        {
            //for (int i = 0; i < _shopItems.Count; i++)
            //{
            //    _shopItems[i].SetSelected(false);
            //}
        }

        //private void RemoveAdItemOnSelectHandler(ShopItem item)
        //{
        //    // TODO - add product id and init it in PurchasingSystem script

        //    //_purchasingSystem.BuyProduct();
        //}

        //private void OnItemSelectedEventHandler(ShopItem item)
        //{
        //    for (int i = 0; i < _shopItems.Count; i++)
        //    {
        //        _shopItems[i].SetSelected(false);
        //    }
        //    item.SetSelected(true);

        //    _selectedShopItem = item;

        //    HideShopButton();

        //    if (_vaultSystem.Coins.Get() >= item.Price)
        //    {
        //        if (!item.IsBoughted)
        //        {
        //            _buyButton.gameObject.SetActive(true);
        //        }
        //        else
        //        {
        //            _selectButton.gameObject.SetActive(true);
        //        }
        //    }
        //    else
        //    {
        //        _advertisementButton.gameObject.SetActive(true);
        //    }
        //}

        private void OnCoinsAmountChangedEventHandler(int amount)
        {
            _coinsCountText.UpdateTextAndShadowValue(amount.ToString());
        }

        private void BackButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();
            _sceneView.HideView();
        }

        private void BuyButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _buyButton.gameObject.SetActive(false);
            _selectButton.gameObject.SetActive(true);

            //_vaultSystem.Coins.Substruct(_selectedShopItem.Price);

            //_dataSystem.ShopPurchasedItemData.shopItemData.Add(_selectedShopItem.Type);
            //_dataSystem.SaveCache(Settings.CacheType.ShopPurchasedItemData);
        }

        private void SelectButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            //OnPlayerSkinChangeEvent?.Invoke(_selectedShopItem.Data.itemPreview);

            //_dataSystem.SelectedPlayerSkinData.skin = _selectedShopItem.Type;
            //_dataSystem.SaveCache(Settings.CacheType.SelectedPlayerSkinData);
        }

        private void AdvertisementButtonOnClickHandler()
        {
            _soundSystem.PlayClickSound();

            _advertisingSystem.ShowAdsVideo(OnAdsComplete);
        }

        private void OnAdsComplete()
        {
            //_vaultSystem.Coins.Add(_initialGameData.coinsForAds);
        }
    }
}