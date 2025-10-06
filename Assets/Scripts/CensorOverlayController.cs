using UnityEngine;

public class CensorOverlayController : MonoBehaviour
{
    // Drag your player's Sprite Renderer here
    public SpriteRenderer playerSpriteRenderer;

    // Drag the child Quad's Mesh Renderer here
    public MeshRenderer censorQuadRenderer;

    private void Start()
    {
        playerSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        censorQuadRenderer = GetComponent<MeshRenderer>();
        censorQuadRenderer.enabled = true;
        playerSpriteRenderer.enabled = false;
    }
    void Update()
    {
        // Ensure the Quad's material is always using the player's current sprite texture
        censorQuadRenderer.material.SetTexture("_MainTex", playerSpriteRenderer.sprite.texture);
    }

    // You can call these functions to turn the effect on and off
    public void ShowCensor(bool isVisible)
    {
        censorQuadRenderer.enabled = isVisible;
    }
}