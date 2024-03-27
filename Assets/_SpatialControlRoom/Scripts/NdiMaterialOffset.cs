using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NdiMaterialOffset : MonoBehaviour
{
    public enum RenderTextureOffset
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        None
    }

    [SerializeField] private RenderTextureOffset textureOffset;

    private Material material;
    private Vector2 tiling;
    private Vector2 offset;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeMaterial();
    }

    void Start()
    {
        ApplyNdiMaterialOffset();
    }

    private void InitializeMaterial()
    {
        if (material == null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                material = renderer.material;
            }
        }
    }

    public void ApplyNdiMaterialOffset()
    {
        InitializeMaterial();

        if (material == null) return;

        switch (textureOffset)
        {
            case RenderTextureOffset.TopLeft:
                offset = new Vector2(0f, 0.5f);
                tiling = new Vector2(0.5f, 0.5f);
                break;

            case RenderTextureOffset.TopRight:
                offset = new Vector2(0.5f, 0.5f);
                tiling = new Vector2(0.5f, 0.5f);
                break;

            case RenderTextureOffset.BottomLeft:
                offset = new Vector2(0f, 0f);
                tiling = new Vector2(0.5f, 0.5f);
                break;

            case RenderTextureOffset.BottomRight:
                offset = new Vector2(0.5f, 0f);
                tiling = new Vector2(0.5f, 0.5f);
                break;

            case RenderTextureOffset.None:
                offset = new Vector2(0f, 0f);
                tiling = new Vector2(1f, 1f);

                break;
        }

        material.mainTextureOffset = offset;
        material.mainTextureScale = tiling;
    }

    private void OnValidate()
    {
        if (material != null)
        {
            ApplyNdiMaterialOffset();
        }
    }
}
