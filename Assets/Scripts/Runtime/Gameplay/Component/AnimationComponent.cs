using System.Collections.Generic;
using TandC.GeometryAstro.Utilities;
using UnityEngine;
namespace TandC.GeometryAstro.Gameplay
{
    public class AnimationComponent
    {
        private AnimationSequence _animationSequence;
        private int _framePerSecond;

        public AnimationComponent(GameObject animatedObject, List<Sprite> spriteAnimation, int framePerSecond = 3) 
        {
            _animationSequence = animatedObject.GetComponentInChildren<AnimationSequence>();
            _animationSequence.LoadFrames(spriteAnimation.ToArray());
            _animationSequence.playAtStart = true;
            _animationSequence.loop = true;
            _framePerSecond = framePerSecond;
            _animationSequence.ResetAnimation();
            Play();
        }

        public void Play() 
        {
            _animationSequence.Play(_framePerSecond, true, 0);
        }

        public void Stop() 
        {
            _animationSequence.Stop();
        }
    }
}

