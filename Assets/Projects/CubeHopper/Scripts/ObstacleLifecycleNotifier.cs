using System;
using UnityEngine;

namespace Projects.CubeHopper.Scripts
{
    public class ObstacleLifecycleNotifier : MonoBehaviour
    {
        public event Action Destroyed;

        private void OnDestroy()
        {
            Destroyed?.Invoke();
        }
    }
}
