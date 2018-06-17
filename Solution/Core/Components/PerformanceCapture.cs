using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Components
{
    public class PerformanceCapture : MonoBehaviour
    {
        public static int FrameRate { get; private set; }

        private List<IUpdate> _subRoutines = new List<IUpdate>()
        {
            new FramerateCounter(),
        };
        
        private void Update()
        {
            foreach (IUpdate routine in _subRoutines)
            {
                routine.Update();
            }
        }

        private class FramerateCounter : IUpdate
        {
            private const float UPDATE_INTERVAL = 0.5f;

            private float _totalDelta;
            private float _deltaCount;

            public void Update()
            {
                _totalDelta += Time.deltaTime;
                _deltaCount++;
                
                if(_totalDelta > UPDATE_INTERVAL)
                {
                    FrameRate = Mathf.RoundToInt(_deltaCount / _totalDelta);

                    _totalDelta = 0;
                    _deltaCount = 0;
                }
            }
        }
        private interface IUpdate
        {
            void Update();
        }
    }
}
