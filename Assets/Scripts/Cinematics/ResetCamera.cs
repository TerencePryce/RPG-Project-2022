using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RPG.Core
{
    public class ResetCamera : MonoBehaviour
    {
        public CinemachineVirtualCamera currentCamera;

        void Awake()
        {
            var dolly = currentCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
            dolly.m_PathPosition = 0f;
        }
    }
}
