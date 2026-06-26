using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class E_UpgradeScript : MonoBehaviour
{
    //各種テキスト表示用変数
    public Text DiggingText;
    public Text SpeedText;
    public Text RangeText;
    public Text NotifyText;

    public Text DiggingLevelText;
    public Text SpeedLevelText;
    public Text RangeLevelText;

    public Text ScoreText;


    private float holdTimerZKey = 0f;           // 押し続けている時間を測るタイマー
    private float continuousUpgradeTimer = 0f;  // 連続強化の間隔を測るタイマー
    private float holdThreshold = 1.0f;         // 長押し判定になるまでの時間（1秒）
    private float upgradeInterval = 0.1f;       // 長押し中の強化間隔（0.2秒ごとに強化）
    //今どの強化項目を選択しているか
    private int upgradeSelectIndex = (int)Upgrade.MINING_SPEED;


    public string nextSceneName;

    //アップグレード通知用の変数
    private float notifyTimer = 0.0f;
    private const float DisplayDuration = 2.0f;

    //サウンド用
    AudioSource upgradeAudioSource; 
    [SerializeField] private AudioClip upgradeFailureClip;
    [SerializeField] private AudioClip upgradeSuccessClip;
    //[SerializeField] private float upgradeInterval = 0.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //その日得たスコアと採掘数を初期化
        ScoreManager.Instance.dayMiningCount = 0;
        ScoreManager.Instance.dayScore = 0;

        //強化時表示されるテキストを空にしておく
        NotifyText.text = $"";

        upgradeAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //現在のスコアを表示
        ScoreText.text = $"現在の金額:{ScoreManager.Instance.score}円";

        //必要スコア表示、
        DiggingText.text = $"{PlayerDataManager.Instance.DiggingScore}円";

        //強化レベル表示
        DiggingLevelText.text = $"採掘速度 Lv{PlayerDataManager.Instance.DiggingLevel}";

        //アップグレード上限を設ける
        SetUpgradeMaxLevel();

        //左右キーによる強化項目選択
        if(Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            upgradeSelectIndex++;
        }
        else if(Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            upgradeSelectIndex--;
        }

        //強化項目を超えて選択値が移動した場合の処理
        //右行きすぎたら
        if(upgradeSelectIndex > 2)
        {
            //選択を左端に戻す
            upgradeSelectIndex = 0;
        }
        //左行きすぎたら
        else if(upgradeSelectIndex < 0)
        {
            //選択を右端に戻す
            upgradeSelectIndex = 2;
        }

        //選択中のアップグレード項目のテキスト色変え
        UpgradeTextColorChanger();


        //通知テキストのカウントダウン処理
        if (notifyTimer > 0)
        {
            notifyTimer -= Time.deltaTime;
            if (notifyTimer <= 0)
            {
                NotifyText.text = ""; // 時間が来たら消す
            }
        }

        //各種ステータスをアップグレード
        UpgradeStatus();
        
        //Enterで次の画面へ移動
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            //シーン移動
            SceneManager.LoadScene(nextSceneName);
        }

    }

    /// <summary>
    /// 採掘速度強化
    /// </summary>
    void MiningSpeedUpgrade()
    {
        //--採掘速度強化
        //スコアが足りない場合
        if (ScoreManager.Instance.score < PlayerDataManager.Instance.DiggingScore)
        {
            ShowNotify($"お金が足りません");
            //アップグレード失敗SE
            upgradeAudioSource.PlayOneShot(upgradeFailureClip);
        }
        //スコアが足りる場合
        else
        {
            //スコアを消費
            ScoreManager.Instance.score -= PlayerDataManager.Instance.DiggingScore;
            //必要スコアを上昇
            PlayerDataManager.Instance.DiggingScore += (int)System.Math.Ceiling(PlayerDataManager.Instance.DiggingScore * 0.1);
            //プレイヤーの採掘速度を上昇
            PlayerDataManager.Instance.playerDigSpeed += 0.08f;
            //レベル上昇
            PlayerDataManager.Instance.DiggingLevel += 1;

            //アップグレード成功SE
            upgradeAudioSource.PlayOneShot(upgradeSuccessClip);
            ShowNotify($"採掘速度アップグレード完了");
        }
    }

    /// <summary>
    /// 移動速度強化
    /// </summary>
    void MoveSpeedUpgrade()
    {
        //移動速度レベルがすでにMAXに達している場合
        if (PlayerDataManager.Instance.SpeedLevel >= 50)
        {
            ShowNotify("移動速度はすでにMAXです");
        }
        else
        {
            //--移動速度強化
            //スコアが足りない場合
            if (ScoreManager.Instance.score < PlayerDataManager.Instance.SpeedScore)
            {
                ShowNotify($"お金が足りません");
                //アップグレード失敗SE
                upgradeAudioSource.PlayOneShot(upgradeFailureClip);

            }
            //スコアが足りる場合
            else
            {
                //スコアを消費
                ScoreManager.Instance.score -= PlayerDataManager.Instance.SpeedScore;
                //必要スコアを上昇
                PlayerDataManager.Instance.SpeedScore += 500;
                //プレイヤーの移動速度を上昇
                PlayerDataManager.Instance.playerSpeed += 0.1f;
                //レベル上昇
                PlayerDataManager.Instance.SpeedLevel += 1;

                //アップグレード成功SE
                upgradeAudioSource.PlayOneShot(upgradeSuccessClip);
                ShowNotify($"移動速度アップグレード完了");
            }
        }
    }

    /// <summary>
    /// 採掘範囲強化
    /// </summary>
    void MiningRangeUpgrade()
    {
        //採掘範囲レベルがすでに上限に達している場合
        if (PlayerDataManager.Instance.RangeLevel >= 15)
        {
            ShowNotify($"採掘範囲はすでにMAXです");

        }
        else
        {
            //--採掘範囲強化
            //スコアが足りない場合
            if (ScoreManager.Instance.score < PlayerDataManager.Instance.RangeScore)
            {
                ShowNotify($"お金が足りません");
                //アップグレード失敗SE
                upgradeAudioSource.PlayOneShot(upgradeFailureClip);

            }
            //スコアが足りる場合
            else
            {
                //スコアを消費
                ScoreManager.Instance.score -= PlayerDataManager.Instance.RangeScore;
                //必要スコアを上昇
                PlayerDataManager.Instance.RangeScore += (int)System.Math.Ceiling(PlayerDataManager.Instance.RangeScore * 0.5);
                //採掘範囲上昇
                PlayerDataManager.Instance.miningRange += new Vector2(0.25f, 0.25f);
                PlayerDataManager.Instance.miningRangeOffset -= new Vector2(PlayerDataManager.Instance.miningRangeOffset.x, 0.125f);
                //レベル上昇
                PlayerDataManager.Instance.RangeLevel += 1;

                //アップグレード成功SE
                upgradeAudioSource.PlayOneShot(upgradeSuccessClip);
                ShowNotify($"採掘範囲アップグレード完了");
            }
        }
    }

    /// <summary>
    /// 強化時テキスト表示用関数
    /// </summary>
    /// <param name="NText"></param>
    void ShowNotify(string NText)
    {
        NotifyText.text = NText;
        notifyTimer = DisplayDuration;
    }

    /// <summary>
    /// 選択項目のテキストを色変え
    /// </summary>
    void UpgradeTextColorChanger()
    {
        //強化項目テキストの色変え
        if (upgradeSelectIndex == (int)Upgrade.MINING_SPEED)
        {
            DiggingText.color = new Color32(255, 0, 0, 255);
        }
        else
        {
            DiggingText.color = new Color32(50, 50, 50, 255);
        }

        if (upgradeSelectIndex == (int)Upgrade.MOVE_SPEED)
        {
            SpeedText.color = new Color32(255, 0, 0, 255);
        }
        else
        {
            SpeedText.color = new Color32(50, 50, 50, 255);
        }

        if (upgradeSelectIndex == (int)Upgrade.MINING_RANGE)
        {
            RangeText.color = new Color32(255, 0, 0, 255);
        }
        else
        {
            RangeText.color = new Color32(50, 50, 50, 255);
        }
    }

    /// <summary>
    /// アップグレード上限を設ける
    /// </summary>
    void SetUpgradeMaxLevel()
    {
        //移動速度の上限を設けておく
        if (PlayerDataManager.Instance.SpeedLevel >= 50)
        {
            SpeedText.text = "LevelMAX";
            SpeedLevelText.color = new Color32(255, 255, 0, 255);
            SpeedLevelText.text = "移動速度 LvMAX";
        }
        else
        {
            SpeedText.text = $"{PlayerDataManager.Instance.SpeedScore}円";
            SpeedLevelText.color = new Color32(255, 255, 255, 255);
            SpeedLevelText.text = $"移動速度 Lv{PlayerDataManager.Instance.SpeedLevel}";
        }

        //採掘範囲の上限を設けておく
        if (PlayerDataManager.Instance.RangeLevel >= 15)
        {

            RangeText.text  = "LevelMAX";
            RangeLevelText.color = new Color32(255, 255, 0, 255);
            RangeLevelText.text = "採掘範囲 LvMAX";
        }
        else
        {
            RangeText.text = $"{PlayerDataManager.Instance.RangeScore}円";
            RangeLevelText.color = new Color32(255, 255, 255,255);
            RangeLevelText.text = $"採掘範囲 Lv{PlayerDataManager.Instance.RangeLevel}";
        }
    }

    /// <summary>
    /// 各種ステータスをアップグレード
    /// </summary>
    void UpgradeStatus() 
    {
        //長押し/アップグレード
        if (Keyboard.current.zKey.isPressed)
        {
            bool doUpgrade = false; // 強化処理を実行するかどうかのフラグ

            // 単押しの瞬間
            if (Keyboard.current.zKey.wasPressedThisFrame)
            {
                doUpgrade = true;
            }
            else // 押しっ放しの場合
            {
                // Zキータイマー加算
                holdTimerZKey += Time.deltaTime;

                // 長押しの閾値（1秒）を超えたら連続強化モードへ
                if (holdTimerZKey > holdThreshold)
                {
                    continuousUpgradeTimer += Time.deltaTime;

                    // 強化間隔（0.2秒）を超えたら強化フラグを立てる
                    if (continuousUpgradeTimer >= upgradeInterval)
                    {
                        doUpgrade = true;
                        // インターバル分を引いて、次の強化タイミングに備える
                        continuousUpgradeTimer -= upgradeInterval;
                    }
                }
            }

            // 強化実行フラグがONなら実際の処理を行う
            if (doUpgrade)
            {
                // 採掘速度
                if (upgradeSelectIndex == (int)Upgrade.MINING_SPEED)
                {
                    MiningSpeedUpgrade();
                    Debug.Log("1");
                }
                // 移動速度
                else if (upgradeSelectIndex == (int)Upgrade.MOVE_SPEED)
                {
                    MoveSpeedUpgrade();
                    Debug.Log("2");
                }
                // 採掘範囲
                else if (upgradeSelectIndex == (int)Upgrade.MINING_RANGE)
                {
                    MiningRangeUpgrade();
                    Debug.Log("3");
                }
            }
        }
        else
        {
            // キーを離したら、タイマーを両方とも0にリセットする
            holdTimerZKey = 0f;
            continuousUpgradeTimer = 0f;
        }
    }
}
