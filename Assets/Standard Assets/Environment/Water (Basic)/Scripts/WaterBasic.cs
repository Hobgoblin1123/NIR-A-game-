using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
    [ExecuteInEditMode]
    public class WaterBasic : MonoBehaviour
    {
        void Update()
        {
            Renderer r = GetComponent<Renderer>();
            if (!r)
            {
                return;
            }
            Material mat = r.sharedMaterial;
            if (!mat)
            {
                return;
            }

            // Vector4 waveSpeed = (1f,1f,1f,1f);
            float t = Time.time / 20.0f;

            // Vector4 offset4 = waveSpeed * t;
            Vector4 offsetClamped = new Vector4(Mathf.Repeat(t, 1.0f), Mathf.Repeat(t, 1.0f),
                Mathf.Repeat(t, 1.0f), Mathf.Repeat(t, 1.0f));
            mat.SetVector("_WaveOffset", offsetClamped);
        }
    }
}