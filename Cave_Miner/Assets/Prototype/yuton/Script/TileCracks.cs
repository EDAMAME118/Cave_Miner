using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCracks : MonoBehaviour
{
    [SerializeField] private TileRangeDestroyer tileDestroyer;

    [Header("ひび割れ画像")]
    [SerializeField] private Sprite crack1;
    [SerializeField] private Sprite crack2;
    [SerializeField] private Sprite crack3;

    [SerializeField] private SpriteRenderer crackRenderer;

    void Update()
    {
        float requiredTime = tileDestroyer.RequiredTime;
        float currentTime = tileDestroyer.crackstime;

        // 掘っているタイル座標
        Vector3Int tilePos = tileDestroyer.currentMiningTile;

        // タイル中心へ移動
        crackRenderer.transform.position =
            tileDestroyer.targetTilemap.GetCellCenterWorld(tilePos);

        float progress = currentTime / requiredTime;

        if (progress >= 0.75f)
        {
            crackRenderer.sprite = crack3;
            crackRenderer.enabled = true;
        }
        else if (progress >= 0.5f)
        {
            crackRenderer.sprite = crack2;
            crackRenderer.enabled = true;
        }
        else if (progress >= 0.25f)
        {
            crackRenderer.sprite = crack1;
            crackRenderer.enabled = true;
        }
        else
        {
            crackRenderer.enabled = false;
        }
    }
}