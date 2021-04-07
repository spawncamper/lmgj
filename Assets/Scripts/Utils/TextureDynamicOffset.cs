using UnityEngine;

public class TextureDynamicOffset : MonoBehaviour
{
    // Scroll main texture based on time

    [SerializeField] float scrollSpeed = 0.5f;
    Renderer myRenderer;

    void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        myRenderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}