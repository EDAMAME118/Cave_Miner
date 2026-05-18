using System.Collections.Generic; // Dictionaryを使うために追加
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileRangeDestroyer : MonoBehaviour
{
    public Tilemap targetTilemap;
    private BoxCollider2D destroyRange;

    // --- 変更点：ブロックごとの採掘時間を記憶する辞書 ---
    // [タイルの座標, 掘り続けた時間] をセットで保存します
    private Dictionary<Vector3Int, float> digProgress = new Dictionary<Vector3Int, float>();
    void Start()
    {
        destroyRange = GetComponent<BoxCollider2D>();

        destroyRange.size = PlayerDataManager.miningRange;
        destroyRange.offset = PlayerDataManager.miningRangeOffset;
        
    }
    public void DestroyTilesInBounds()
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        Vector3Int minCell = targetTilemap.WorldToCell(bounds.min);
        Vector3Int maxCell = targetTilemap.WorldToCell(bounds.max);

        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                Vector3Int targetPos = new Vector3Int(x, y, 0);
                TileBase tile = targetTilemap.GetTile(targetPos);

                if (tile != null)
                {
                    // 1. まだこの座標を掘り始めていなければ、0秒からスタートとして登録
                    if (!digProgress.ContainsKey(targetPos))
                    {
                        digProgress[targetPos] = 0f;
                    }
                    // 2. この特定のブロックの採掘時間だけを進める
                    digProgress[targetPos] += Time.deltaTime+PlayerDataManager.playerDigSpeed;

                    // 3. このブロックを破壊するのに必要な時間を判定（デフォルトは3.0秒）
                    float requiredTime = 3.0f;
                    if (tile is ScoreTile scoreTile)
                    {
                        // ScoreTileの場合は設定された時間を適用
                        requiredTime = scoreTile.mining_soeed;
                    }

                    // 4. 採掘時間が目標に達したかチェック
                    if (digProgress[targetPos] >= requiredTime)
                    {
                        // タイルを破壊
                        targetTilemap.SetTile(targetPos, null);

                        // --- スコア加算処理 ---
                        if (tile is ScoreTile destroyedScoreTile)
                        {
                            ScoreManager.score += destroyedScoreTile.scoreValue;
                            ScoreManager.totalScore += destroyedScoreTile.scoreValue;
                            ScoreManager.dayScore += destroyedScoreTile.scoreValue;
                            Debug.Log($"{destroyedScoreTile.typeName} を破壊！ スコア +{destroyedScoreTile.scoreValue} (合計: {ScoreManager.score})");
                        }
                        else
                        {
                            // 鉱石以外の普通のタイルの場合
                            ScoreManager.score += 10;
                            ScoreManager.totalScore += 10;
                            ScoreManager.dayScore += 10;
                        }

                        // 共通のカウントアップ
                        ScoreManager.miningCount++;
                        ScoreManager.totalMiningCount++;
                        ScoreManager.dayMiningCount++;

                        // 5. 破壊が終わったので、この座標のタイマー記憶を削除する
                        digProgress.Remove(targetPos);
                    }
                }
                else
                {
                    // もしタイルが無い（既に壊れた、あるいは範囲がずれて対象から外れた）場合、
                    // 無駄な記憶を消去してメモリを節約する
                    if (digProgress.ContainsKey(targetPos))
                    {
                        digProgress.Remove(targetPos);
                    }
                }
            }
        }
    }

    void Update()
    {


        if (Keyboard.current.zKey.isPressed)
        {
            // キーが押されている間は、毎フレーム範囲内のタイルの時間を進める
            DestroyTilesInBounds();
        }
        else
        {
            // キーを離したら、すべてのブロックの「掘り途中の時間」をリセットする
            if (digProgress.Count > 0)
            {
                digProgress.Clear();
            }
        }
    }
}