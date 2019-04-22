﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace M8.UI.Texts {
    /// <summary>
    /// Use this to display numbers that gradually counts to value
    /// </summary>
    [AddComponentMenu("M8/UI/Texts/Counter")]
    public class TextCounter : MonoBehaviour {
        public Text target;
        public string format = "";
        public float delay; //delay to count
                
        public int count {
            get { return mCount; }
            set {
                if(mCount != value) {
                    var prevCount = mCount;
                    mCount = value;

                    if(mRout == null)
                        mRout = StartCoroutine(DoCount(prevCount));
                }
            }
        }

        public bool isPlaying { get { return mRout != null; } }
        
        private int mCount;
        private Coroutine mRout;

        public void SetCountImmediate(int aCount) {
            mCount = aCount;
            Stop();
        }

        public void Stop() {
            if(mRout != null) {
                StopCoroutine(mRout);
                mRout = null;
            }

            ApplyNumber(mCount);
        }

        void OnDisable() {
            Stop();
        }
        
        IEnumerator DoCount(int prevCount) {
            float startCount = prevCount;
            int curCount;

            var curTime = 0f;
            while(curTime < delay) {
                yield return null;

                curTime += Time.deltaTime;

                var t = Mathf.Clamp01(curTime / delay);

                curCount = Mathf.RoundToInt(Mathf.Lerp(startCount, mCount, t));

                ApplyNumber(curCount);
            }

            ApplyNumber(mCount);

            mRout = null;
        }

        void ApplyNumber(int num) {
            if(!target)
                return;

            if(!string.IsNullOrEmpty(format))
                target.text = num.ToString(format);
            else
                target.text = num.ToString();
        }
    }
}