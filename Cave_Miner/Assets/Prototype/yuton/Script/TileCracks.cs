using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileCracks : MonoBehaviour
{
    [SerializeField] private TileRangeDestroyer tileDestroyer;

    [SerializeField] private Tilemap crackTilemap;

    [SerializeField] private TileBase crack1Tile;
    [SerializeField] private TileBase crack2Tile;
    [SerializeField] private TileBase crack3Tile;

    void Update()
    {
        // àÍíUëSïîè¡Ç∑
        crackTilemap.ClearAllTiles();

        foreach (KeyValuePair<Vector3Int, float> pair in tileDestroyer.DigProgress)
        {
            Vector3Int tilePos = pair.Key;
            float currentTime = pair.Value;

            TileBase tile = tileDestroyer.targetTilemap.GetTile(tilePos);

            if (tile == null)
                continue;

            float requiredTime = 3f;

            if (tile is ScoreTile scoreTile)
            {
                requiredTime = scoreTile.mining_soeed;
            }

            float progress = currentTime / requiredTime;

            if (progress >= 0.75f)
            {
                crackTilemap.SetTile(tilePos, crack3Tile);
            }
            else if (progress >= 0.5f)
            {
                crackTilemap.SetTile(tilePos, crack2Tile);
            }
            else if (progress >= 0.25f)
            {
                crackTilemap.SetTile(tilePos, crack1Tile);
            }
        }
    }
}