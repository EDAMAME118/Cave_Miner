using System.Collections.Generic; // Dictionaryを使うために追加
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileRangeDestroyer : MonoBehaviour
{
    [SerializeField] private Image gaugeFillImage;
    public Tilemap targetTilemap;
    
    private Transform destroyRange;

    // 【変更】Updateでも使うため、一番進捗のあるタイルの情報を保持する変数
    private float currentdigtime = 0.0f;
    private float requiredTime = 3.0f;
    public float crackstime=0.0f;

    // [タイルの座標, 掘り続けた時間] をセットで保存します
    private Dictionary<Vector3Int, float> digProgress = new Dictionary<Vector3Int, float>();

    void Start()
    {
        destroyRange = GetComponent<Transform>();
        destroyRange.localScale = PlayerDataManager.Instance.miningRange;
        destroyRange.localPosition = PlayerDataManager.Instance.miningRangeOffset;

        // 最初はゲージを空にしておく
        if (gaugeFillImage != null) gaugeFillImage.fillAmount = 0f;
    }

    public void DestroyTilesInBounds()
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        Vector3Int minCell = targetTilemap.WorldToCell(bounds.min);
        Vector3Int maxCell = targetTilemap.WorldToCell(bounds.max);

        // ゲージ計算用に、このフレームでの「最大進捗率」を一時的に記録する変数
        float maxProgressRatio = 0f;

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
                    digProgress[targetPos] += Time.deltaTime * PlayerDataManager.Instance.playerDigSpeed;

                    // 3. このブロックを破壊するのに必要な時間を判定（デフォルトは3.0秒）
                    float tileRequiredTime = 3.0f; // ローカル変数にして個別に計算
                    if (tile is ScoreTile scoreTile)
                    {
                        tileRequiredTime = scoreTile.mining_soeed;
                    }

                    // 【追加】このタイルの進捗率（0.0 〜 1.0）を計算
                    float currentRatio = digProgress[targetPos] / tileRequiredTime;

                    // 範囲内で「一番壊れそうなタイル」の情報を全体の変数にキープする
                    if (currentRatio > maxProgressRatio)
                    {
                        maxProgressRatio = currentRatio;
                        currentdigtime = digProgress[targetPos];
                        crackstime = currentdigtime;
                        requiredTime = tileRequiredTime;
                    }

                    // 4. 採掘時間が目標に達したかチェック
                    if (digProgress[targetPos] >= tileRequiredTime)
                    {
                        // タイルを破壊
                        targetTilemap.SetTile(targetPos, null);

                        // --- スコア加算処理 ---
                        if (tile is ScoreTile destroyedScoreTile)
                        {
                            ScoreManager.Instance.score += destroyedScoreTile.scoreValue;
                            ScoreManager.Instance.totalScore += destroyedScoreTile.scoreValue;
                            ScoreManager.Instance.dayScore += destroyedScoreTile.scoreValue;
                            Debug.Log($"{destroyedScoreTile.typeName} を破壊！ スコア +{destroyedScoreTile.scoreValue} (合計: {ScoreManager.Instance.score})");
                        }
                        else
                        {
                            ScoreManager.Instance.score += 10;
                            ScoreManager.Instance.totalScore += 10;
                            ScoreManager.Instance.dayScore += 10;
                        }

                        // 共通のカウントアップ
                        ScoreManager.Instance.miningCount++;
                        ScoreManager.Instance.totalMiningCount++;
                        ScoreManager.Instance.dayMiningCount++;

                        // 5. 破壊が終わったので、この座標のタイマー記憶を削除する
                        digProgress.Remove(targetPos);
                    }
                }
                else
                {
                    if (digProgress.ContainsKey(targetPos))
                    {
                        digProgress.Remove(targetPos);
                    }
                }
            }
        }

        // もし範囲内に一つもタイルがなかったら数値をリセット
        if (maxProgressRatio == 0f)
        {
            currentdigtime = 0f;
            crackstime += currentdigtime;
            requiredTime = 3.0f;
        }
    }

    void Update()
    {
        if (Keyboard.current.zKey.isPressed)
        {
            // キーが押されている間は、毎フレーム範囲内のタイルの時間を進める
            DestroyTilesInBounds();

            // 【修正】「現在の秒数 / 必要時間」で0.0〜1.0の割合にしてゲージに代入
            if (gaugeFillImage != null && requiredTime > 0f)
            {
                gaugeFillImage.fillAmount = Mathf.Clamp01(currentdigtime / requiredTime);
            }
        }
        else
        {
            // キーを離したら、すべてのブロックの「掘り途中の時間」をリセットする
            if (digProgress.Count > 0)
            {
                digProgress.Clear();
            }

            // 【追加】キーを離したらゲージもゼロにする
            currentdigtime = 0f;
            if (gaugeFillImage != null)
            {
                gaugeFillImage.fillAmount = 0f;
            }
        }

        // デバッグ用隠しコマンド
        if (Keyboard.current.f1Key.isPressed && Keyboard.current.f2Key.isPressed && Keyboard.current.enterKey.isPressed)
        {
            ScoreManager.Instance.score = 777777777;
        }
    }
}
