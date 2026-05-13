using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileRangeDestroyer : MonoBehaviour
{
    public Tilemap targetTilemap;

    // この関数を呼び出すと、オブジェクトのColliderの範囲内のタイルを消去します
    public void DestroyTilesInBounds()
    {
        // 1. オブジェクトに付いているColliderの範囲(Bounds)を取得
        Bounds bounds = GetComponent<Collider2D>().bounds;

        // 2. Boundsの最小値と最大値をタイルマップの格子座標(Vector3Int)に変換
        Vector3Int minCell = targetTilemap.WorldToCell(bounds.min);
        Vector3Int maxCell = targetTilemap.WorldToCell(bounds.max);

        // 3. X軸とY軸でループを回して、範囲内の全座標をチェック
        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                Vector3Int targetPos = new Vector3Int(x, y, 0);

                // --- 修正・追加部分 ---
                // その場所のタイルデータを取得
                TileBase tile = targetTilemap.GetTile(targetPos);

                if (tile != null)
                {
                    // もし取得したタイルが「ScoreTile」なら、設定されたスコアを加算
                    if (tile is ScoreTile scoreTile)
                    {
                        ScoreManager.score += scoreTile.scoreValue;
                        ScoreManager.totalScore+=scoreTile.scoreValue;
                        ScoreManager.miningCount++;
                        ScoreManager.totalMiningCount++;
                        Debug.Log($"{scoreTile.typeName} を破壊！ スコア +{scoreTile.scoreValue} (合計: {ScoreManager.score})");
                    }
                    else
                    {
                        // ScoreTile以外の普通のタイルだった場合のデフォルトスコア（必要なければ0でもOK）
                        ScoreManager.score += 10;
                        ScoreManager.totalScore += 10;
                        ScoreManager.miningCount++;
                        ScoreManager.totalMiningCount++;
                    }

                    // タイルを削除
                    targetTilemap.SetTile(targetPos, null);
                }
                // ----------------------
            }
        }
    }

    void Update()
    {
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            DestroyTilesInBounds();
        }
    }
}

//HasTile　指定した座標にタイルが存在するか確認する関数

//2
//colは触れている相手が自動で渡される
//↓入っている情報
//col.gameObject → 相手のオブジェクト
//col.transform → 相手の位置
//col.tag → タグ（"Player" など）
//col.GetComponent<>() → 相手のスクリプト取得

//col.ClosestPoint(調べたい位置)でコライダー上で一番近い点を指す
