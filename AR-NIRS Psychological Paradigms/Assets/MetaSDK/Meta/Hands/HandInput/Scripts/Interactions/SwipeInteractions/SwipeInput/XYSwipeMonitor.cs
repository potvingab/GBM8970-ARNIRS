// Copyright © 2018, Meta Company.  All rights reserved.
// 
// Redistribution and use of this software (the "Software") in binary form, without modification, is 
// permitted provided that the following conditions are met:
// 
// 1.      Redistributions of the unmodified Software in binary form must reproduce the above 
//         copyright notice, this list of conditions and the following disclaimer in the 
//         documentation and/or other materials provided with the distribution.
// 2.      The name of Meta Company (“Meta”) may not be used to endorse or promote products derived 
//         from this Software without specific prior written permission from Meta.
// 3.      LIMITATION TO META PLATFORM: Use of the Software is limited to use on or in connection 
//         with Meta-branded devices or Meta-branded software development kits.  For example, a bona 
//         fide recipient of the Software may incorporate an unmodified binary version of the 
//         Software into an application limited to use on or in connection with a Meta-branded 
//         device, while he or she may not incorporate an unmodified binary version of the Software 
//         into an application designed or offered for use on a non-Meta-branded device.
// 
// For the sake of clarity, the Software may not be redistributed under any circumstances in source 
// code form, or in the form of modified binary code – and nothing in this License shall be construed 
// to permit such redistribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDER "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL META COMPANY BE LIABLE FOR ANY DIRECT, 
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, 
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS 
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using UnityEngine;

namespace Meta.HandInput.SwipeInteraction
{
    /// <summary>
    /// Tracks the position of hand features in the swipe volume along a vertical and  a horizontal axes
    /// </summary>
    public class XYSwipeMonitor : SwipeHandMonitor
    {
        [Tooltip("Motion threshold to switch axis of motion")]
        [SerializeField]
        private float _switchThresh = 0.6f;
        private float _switchTime = 0f;
        private const float SWITCH_TIMEOUT = 0.1f;

        private const float MAX_TRAVEL = 0.5f;

        private const float SMOOTHING = 0.75f;

        private bool _horizontalSwipe = true;

        /// <summary>
        /// The horizontal position of swiping hand along the swipe plane
        /// </summary>
        public float SwipeX { get; private set; }

        /// <summary>
        /// The vertical position of swiping hand along the swipe plane
        /// </summary>
        public float SwipeY { get; private set; }

        /// <summary>
        /// The change in the horizontal position of swiping hand along the swipe plane
        /// </summary>
        public float DeltaX { get; private set; }

        /// <summary>
        /// The change in the vertical position of swiping hand along the swipe plane
        /// </summary>
        public float DeltaY { get; private set; }

        /// <summary>
        /// True when the hand is swiping in the horizontal direction
        /// </summary>
        public bool HorizontalSwipe
        {
            get { return _horizontalSwipe; }
        }

        private void Awake()
        {
            _handTrigger.FirstHandFeatureEnterEvent.AddListener(SwipeOn);
            _handTrigger.LastHandFeatureExitEvent.AddListener(SwipeOff);
        }

        private void Update()
        {
            if (_engaged)
            {
                float newSwipeX = GetHandsXval(_handTrigger.HandFeatureList[0]);
                float priorSwipeX = SwipeX;
                SwipeX = SMOOTHING * SwipeX + (1f - SMOOTHING) * newSwipeX;
                DeltaX = (SwipeX - priorSwipeX);

                float newSwipeY = GetHandsYval(_handTrigger.HandFeatureList[0]);
                float priorSwipeY = SwipeY;
                SwipeY = SMOOTHING * SwipeY + (1f - SMOOTHING) * newSwipeY;
                DeltaY = (SwipeY - priorSwipeY);

                float velX = DeltaX / Time.deltaTime;
                float velY = DeltaY / Time.deltaTime;
                if (Mathf.Abs(velX) > _switchThresh || Mathf.Abs(velY) > _switchThresh)
                {
                    bool moveHorizontal = (Mathf.Abs(velX) > Mathf.Abs(velY));
                    if (moveHorizontal != _horizontalSwipe && (Time.time - _switchTime) > SWITCH_TIMEOUT)
                    {
                        _horizontalSwipe = moveHorizontal;
                        _switchTime = Time.time;
                    }
                }
            }
        }

        private float GetHandsXval(HandFeature hf)
        {
            return Mathf.Clamp(_swipePlaneTransform.InverseTransformPoint(hf.Position).x, -MAX_TRAVEL, MAX_TRAVEL);
        }

        private float GetHandsYval(HandFeature hf)
        {
            return Mathf.Clamp(_swipePlaneTransform.InverseTransformPoint(hf.Position).y, -MAX_TRAVEL, MAX_TRAVEL);
        }

        private void SwipeOn(HandFeature hf)
        {
            _engaged = true;
            SwipeX = GetHandsXval(hf);
            SwipeY = GetHandsYval(hf);
            DeltaX = 0f;
            DeltaY = 0f;
            _swipePlaneEngaged.Invoke();
        }

        private void SwipeOff(HandFeature hf)
        {
            _engaged = false;
            _swipePlaneDisengaged.Invoke();
        }
    }
}
