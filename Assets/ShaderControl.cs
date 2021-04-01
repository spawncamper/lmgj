using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderControl : MonoBehaviour
{
    [SerializeField] bool triggerShader = false;
    [SerializeField] float fadeTime;
    [SerializeField] float fadeOutTargetAlpha = 1f;
    [SerializeField] Renderer shaderRenderer;
    int nFrames;
    int remainingFrames;
    float deltaAlpha;
    float currentDissolveValue = 0f;

    void Update()
    {
        if (triggerShader == true)
        {
            StartCoroutine(ShaderDissolve(fadeTime));
        }
    }

    IEnumerator ShaderDissolve(float fadeTime)
    {
        nFrames = Mathf.RoundToInt(fadeTime / Time.deltaTime);
        remainingFrames = nFrames;
        deltaAlpha = fadeOutTargetAlpha / nFrames;
        while (remainingFrames > 0)
        {
            shaderRenderer.material.SetFloat("_alpha", currentDissolveValue);
            currentDissolveValue += deltaAlpha;
            remainingFrames--;
            yield return null;
        }
    }
}