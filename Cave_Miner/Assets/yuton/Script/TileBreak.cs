//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.Tilemaps;

//public class TileRangeDestroyer : MonoBehaviour
//{
//    public Tilemap targetTilemap;

//    private float diginterval=0.0f;
//    //この関数を呼び出すと、オブジェクトのColliderの範囲内のタイルを消去します
//    public void DestroyTilesInBounds()
//    {
//        // 1. オブジェクトに付いているColliderの範囲(Bounds)を取得
//        Bounds bounds = GetComponent<Collider2D>().bounds;

//        // 2. Boundsの最小値と最大値をタイルマップの格子座標(Vector3Int)に変換
//        Vector3Int minCell = targetTilemap.WorldToCell(bounds.min);
//        Vector3Int maxCell = targetTilemap.WorldToCell(bounds.max);

//        // 3. X軸とY軸でループを回して、範囲内の全座標をチェック
//        bool hasDestroyeAny = false;
//        for (int x = minCell.x; x <= maxCell.x; x++)
//        {
//            for (int y = minCell.y; y <= maxCell.y; y++)
//            {
//                Vector3Int targetPos = new Vector3Int(x, y, 0);

//                // --- 修正・追加部分 ---
//                // その場所のタイルデータを取得
//                TileBase tile = targetTilemap.GetTile(targetPos);

//                if (tile != null)
//                {
//                    // もし取得したタイルが「ScoreTile」なら、設定されたスコアを加算
//                    if (tile is ScoreTile scoreTile)
//                    {
//                        diginterval += Time.deltaTime;
//                        if(scoreTile.mining_soeed<diginterval)
//                        {
//                        targetTilemap.SetTile(targetPos, null);
//                        ScoreManager.score += scoreTile.scoreValue;
//                        ScoreManager.totalScore+=scoreTile.scoreValue;
//                        ScoreManager.dayScore += scoreTile.scoreValue;
//                        ScoreManager.miningCount++;
//                        ScoreManager.totalMiningCount++;
//                        Debug.Log($"{scoreTile.typeName} を破壊！ スコア +{scoreTile.scoreValue} (合計: {ScoreManager.score})");
//                            hasDestroyeAny = true; 
//                        }
//                    }
//                    //鉱石以外の時
//                    //else
//                    //{
//                    //    diginterval += Time.deltaTime;
//                    //    if (diginterval > 3.0f)
//                    //    {
//                    //        targetTilemap.SetTile(targetPos, null);

//                    //        // ScoreTile以外の普通のタイルだった場合のデフォルトスコア（必要なければ0でもOK）
//                    //        ScoreManager.score += 10;
//                    //        ScoreManager.totalScore += 10;
//                    //        ScoreManager.dayScore += 10;
//                    //        ScoreManager.miningCount++;
//                    //        ScoreManager.totalMiningCount++;
//                    //        hasDestroyeAny = true;
//                    //    }

//                    //}
//                    if(hasDestroyeAny)
//                {
//                    diginterval = 0;
//                }
//                }

//                // ----------------------
//            }
//        }
//    }

//    void Update()
//    {
//        if (Keyboard.current.zKey.isPressed)
//        {
//            DestroyTilesInBounds();
//        }
//        else
//            diginterval = 0f;
//    }
//}

////HasTile　指定した座標にタイルが存在するか確認する関数

////2
////colは触れている相手が自動で渡される
////↓入っている情報
////col.gameObject → 相手のオブジェクト
////col.transform → 相手の位置
////col.tag → タグ（"Player" など）
////col.GetComponent<>() → 相手のスクリプト取得

////col.ClosestPoint(調べたい位置)でコライダー上で一番近い点を指す
using System.Collections.Generic; // Dictionaryを使うために追加
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileRangeDestroyer : MonoBehaviour
{
    public Tilemap targetTilemap;

    // --- 変更点：ブロックごとの採掘時間を記憶する辞書 ---
    // [タイルの座標, 掘り続けた時間] をセットで保存します
    private Dictionary<Vector3Int, float> digProgress = new Dictionary<Vector3Int, float>();

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