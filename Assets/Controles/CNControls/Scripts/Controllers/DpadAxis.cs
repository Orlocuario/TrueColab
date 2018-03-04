﻿using UnityEngine;
using UnityEngine.UI;

namespace CnControls
{
    public class DpadAxis : MonoBehaviour
    {
        public string AxisName;
        public float AxisMultiplier;

        public RectTransform RectTransform { get; private set; }
        public int LastFingerId { get; set; }
        private VirtualAxis _virtualAxis;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _virtualAxis = _virtualAxis ?? new VirtualAxis(AxisName);
            LastFingerId = -1;

            CnInputManager.RegisterVirtualAxis(_virtualAxis);
        }

        private void OnDisable()
        {
            CnInputManager.UnregisterVirtualAxis(_virtualAxis);
        }

        public void Press(Vector2 screenPoint, Camera eventCamera, int pointerId)
        {
            gameObject.GetComponent<Image>().color = new Color32(144, 144, 144, 255);
            _virtualAxis.Value = Mathf.Clamp(AxisMultiplier, -1f, 1f);
            LastFingerId = pointerId;
        }

        public void TryRelease(int pointerId)
        {
            if (LastFingerId == pointerId)
            {
                gameObject.GetComponent<Image>().color = new Color32(231, 231, 231, 255);
                _virtualAxis.Value = 0f;
                LastFingerId = -1;
            }
        }
    }
}