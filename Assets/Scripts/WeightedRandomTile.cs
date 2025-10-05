using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "WeightedRandomTile", menuName = "Tiles/Weighted Random Tile")]
public class WeightedRandomTile : TileBase
{
    [System.Serializable]
    public class WeightedSprite
    {
        public Sprite sprite;
        [Range(0.01f, 1f)] public float weight = 1f;
    }

    public WeightedSprite[] sprites;

    public Color color = Color.white;
    public Tile.ColliderType colliderType = Tile.ColliderType.None;

    // Used for randomization per tile position, so the result is deterministic
    private static System.Random seededRandom = new System.Random();

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if (sprites == null || sprites.Length == 0)
            return;

        tileData.sprite = GetWeightedSprite(position);
        tileData.color = color;
        tileData.colliderType = colliderType;
    }

    private Sprite GetWeightedSprite(Vector3Int position)
    {
        // Use position to seed for deterministic randomness (so the tilemap stays consistent)
        int seed = position.x * 73856093 ^ position.y * 19349663;
        seededRandom = new System.Random(seed);

        float totalWeight = 0f;
        foreach (var ws in sprites)
            totalWeight += ws.weight;

        float randomValue = (float)(seededRandom.NextDouble() * totalWeight);
        float cumulative = 0f;

        foreach (var ws in sprites)
        {
            cumulative += ws.weight;
            if (randomValue <= cumulative)
                return ws.sprite;
        }

        // fallback
        return sprites[sprites.Length - 1].sprite;
    }
}