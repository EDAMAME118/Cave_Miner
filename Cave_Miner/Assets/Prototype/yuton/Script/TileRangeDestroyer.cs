using System.Collections.Generic; // Dictionaryを使うために追加
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileRangeDestroyer : MonoBehaviour
{
    //鉱石の値段表示用
    [SerializeField] private Text oreScoreText;
    //鉱石の値段テキストの表示制御用
    private float oreTextInterval = 0.0f;
    private float displayDuration = 2.0f;

    //SE
    AudioSource audiosource;
    [SerializeField] private AudioClip[] digClip;//採掘音 
    [SerializeField] AudioClip breakClip;//破壊音
    [SerializeField] private float miningaudioDelay = 0.0f;

    [SerializeField] private Image gaugeFillImage;
    [SerializeField] private GameObject breakParticlePrefab;

    public Tilemap targetTilemap;
    
    private Transform destroyRange;
    public Vector3Int currentMiningTile;
    // 【変更】Updateでも使うため、一番進捗のあるタイルの情報を保持する変数
    private float currentdigtime = 0.0f;
    private float requiredTime = 3.0f;
    public float crackstime=0.0f;

    //これを打てばほかのスクリプトで取得できるらしいよ、やったね
    public float CurrentDigTime => currentdigtime;
    public float RequiredTime => requiredTime;
    // [タイルの座標, 掘り続けた時間] をセットで保存します
    private Dictionary<Vector3Int, float> digProgress = new Dictionary<Vector3Int, float>();

    public Dictionary<Vector3Int, float> DigProgress => digProgress;

    private void Awake()
    {
        destroyRange = GetComponent<Transform>();
        destroyRange.localScale = PlayerDataManager.Instance.miningRange;
        destroyRange.localPosition = PlayerDataManager.Instance.miningRangeOffset;
    }
    void Start()
    { 
        
        audiosource = GetComponent<AudioSource>();

        // 最初はゲージを空にしておく
        if (gaugeFillImage != null) gaugeFillImage.fillAmount = 0f;

        //値段表示テキストをからにしておく（掘るまで表示しない）
        oreScoreText.text = "";

        if (this.gameObject.activeSelf == false)
        {
            this.gameObject.SetActive(true);
        }
    }

    public void DestroyTilesInBounds()
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        Vector3Int minCell = targetTilemap.WorldToCell(bounds.min);
        Vector3Int maxCell = targetTilemap.WorldToCell(bounds.max);

        List<Vector3Int> removeList = new List<Vector3Int>();

        foreach (var pair in digProgress)
        {
            Vector3Int pos = pair.Key;

            bool isOutside =
                pos.x < minCell.x || pos.x > maxCell.x ||
                pos.y < minCell.y || pos.y > maxCell.y;

            if (isOutside)
            {
                removeList.Add(pos);
            }
        }

        foreach (var pos in removeList)
        {
            digProgress.Remove(pos);
        }

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
                    
                   
                    if (miningaudioDelay > 0.5f)
                    {
                        //SE
                        int index = Random.Range(0, digClip.Length);

                        //2種類のSEをランダム再生
                        audiosource.PlayOneShot(digClip[index]);

                        miningaudioDelay = 0.0f;
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
                        requiredTime = tileRequiredTime;

                        currentMiningTile = targetPos;
                    }

                    // 4. 採掘時間が目標に達したかチェック
                    if (digProgress[targetPos] >= tileRequiredTime)
                    {
                        // タイルを破壊
                        targetTilemap.SetTile(targetPos, null);

                        if (tile is ScoreTile Clip)
                        {
                            //値段を表示
                            ShowNotify($"{Clip.scoreValue}円獲得");
                            
                            //タイル破壊SE
                            audiosource.PlayOneShot(Clip.breakClip);
                        }
                        

                        // タイルの中央座標を取得
                        Vector3 worldPos = targetTilemap.GetCellCenterWorld(targetPos);

                        if (tile is ScoreTile perticle)
                        {
                            // パーティクル生成
                            Instantiate(
                                perticle.breakPerticle,
                                worldPos,
                                Quaternion.identity);
                        }

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
            if (gaugeFillImage != null || requiredTime > 0f)
            {
                gaugeFillImage.fillAmount = Mathf.Clamp01(currentdigtime / requiredTime);

                //se間隔を測る変数
                miningaudioDelay += Time.deltaTime;

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

        //通知テキストのカウントダウン処理
        if (oreTextInterval > 0)
        {
            oreTextInterval -= Time.deltaTime;
            if (oreTextInterval <= 0)
            {
                oreScoreText.text = ""; // 時間が来たら消す
            }
        }


        // デバッグ用隠しコマンド
        if (Keyboard.current.f1Key.isPressed && Keyboard.current.f2Key.isPressed && Keyboard.current.enterKey.isPressed)
        {
            ScoreManager.Instance.score = 777777777;
        }


    }

    /// <summary>
    /// 鉱石値段表示用
    /// </summary>
    /// <param name="NText"></param>
    void ShowNotify(string NText)
    {
        oreScoreText.text = NText;
        oreTextInterval = displayDuration;
    }
}
