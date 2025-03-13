using System;
using System.Collections;
using System.Collections.Generic;
using TandC.GeometryAstro.Services;
using TandC.GeometryAstro.Utilities;
using UnityEngine;

namespace TandC.GeometryAstro.UI
{
    public class GamePageModel : MonoBehaviour
    {
        private UIService _uiService;
        private SoundService _soundService;

        private GameObject _selfObject;
        public GameObject SelfObject
        {
            get
            {
                if (_selfObject == null)
                {
                    _selfObject = _uiService.Canvas.transform.Find("View - GamePage").gameObject; // TODO fix name of go in hierarchy
                }

                return _selfObject;
            }
        }

        public GamePageModel(
            SoundService soundService,
            UIService uiService)
        {
            _uiService = uiService;
            _soundService = soundService;
        }

        public void OpenPause()
        {
            // TODO - play click sound
            _uiService.OpenPage<PausePageView>();
        }
    }
}