using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Networking;

namespace Metrics.Components
{
    public class NetworkPerformanceRenderer : MonoBehaviour
    {
        [SerializeField]
        private float Interval = 1;

        private float _timeSinceLastUpdate = float.MaxValue;

        private void Update()
        {
            if (!Client.IsConnected)
                return;

            if(_timeSinceLastUpdate > Interval)
            {
                NetworkPerformance.Statistics stats = NetworkPerformance.Update();
                Debug.Log(stats.Ping);

                _timeSinceLastUpdate = 0;
            }
            else
            {
                _timeSinceLastUpdate += Time.deltaTime;
            }
        }
    }
}
