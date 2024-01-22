using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Studio.Utilities
{
    public class AnimationSequence : MonoBehaviour
    {
        private WaitForSeconds _frameDelay;

        private Image _uiImage;

        private SpriteRenderer _spriteRenderer;

        public Sprite[] frames;

        public float framesPerSecond;

        public bool loop;

        public int currentFrame;

        public bool wasPaused;

        public bool isPlaying;

        public bool playAtStart;

        private bool _breakOfCoroutine;

        private void Awake()
        {
            _uiImage = GetComponent<Image>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            if (playAtStart)
            {
                Play(framesPerSecond, loop, currentFrame);
            }
        }

        private void OnDisable()
        {
            if (playAtStart)
            {
                Stop();
                _breakOfCoroutine = false;
            }
        }

        public void LoadFrames(Sprite[] frames, bool reverse = false)
        {
            if (reverse)
            {
                this.frames = frames.Reverse().ToArray();
            }
            else
            {
                this.frames = frames;
            }
            currentFrame = -1;
        }

        public void Play(float framesPerSecond = 60, bool loop = true, int frame = 0)
        {
            if (frames == null || frames.Length == 0)
            {
                return;
            }

            if (isPlaying)
            {
                Stop();
                _breakOfCoroutine = false;
            }

            this.framesPerSecond = framesPerSecond;
            this.loop = loop;

            _frameDelay = new WaitForSeconds(1f / this.framesPerSecond);

            wasPaused = false;

            if (gameObject.activeSelf)
            {
                StartCoroutine(AnimationSequenceRoutine());
                isPlaying = true;
            }
        }

        public void Stop()
        {
            if (!isPlaying)
            {
                return;
            }

            wasPaused = false;
            _breakOfCoroutine = true;

            StopCoroutine(AnimationSequenceRoutine());

            isPlaying = false;
        }

        public void Pause()
        {
            if (!isPlaying)
            {
                return;
            }

            wasPaused = true;

            StopCoroutine(AnimationSequenceRoutine());

            isPlaying = false;
        }

        public void ResetAnimation()
        {
            if (frames == null || frames.Length == 0)
            {
                return;
            }

            currentFrame = 0;

            if (_uiImage != null)
            {
                _uiImage.sprite = frames[currentFrame];
            }

            if (_spriteRenderer != null)
            {
                _spriteRenderer.sprite = frames[currentFrame];
            }
        }

        private IEnumerator AnimationSequenceRoutine()
        {
            do
            {
                while (currentFrame < frames.Length)
                {
                    if (_breakOfCoroutine)
                    {
                        _breakOfCoroutine = false;
                        yield break;
                    }

                    if (_uiImage != null)
                    {
                        _uiImage.sprite = frames[currentFrame];
                    }

                    if (_spriteRenderer != null)
                    {
                        _spriteRenderer.sprite = frames[currentFrame];
                    }

                    currentFrame++;

                    yield return _frameDelay;
                }

                currentFrame = 0;
            } while (loop);
        }
    }
}