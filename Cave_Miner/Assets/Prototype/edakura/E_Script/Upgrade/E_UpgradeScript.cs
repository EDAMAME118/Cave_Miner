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


    //長押し用タイマー
    private float holdTimer1Key = 0.0f;
    private float holdTimer2Key = 0.0f;
    private float holdTimer3Key = 0.0f;

    public string nextSceneName;

    //アップグレード通知用の変数
    private float notifyTimer = 0.0f;
    private const float DisplayDuration = 2.0f;

    //サウンド用
    AudioSource upgradeAudioSource;
    [SerializeField] private AudioClip upgradeFailureClip;
    [SerializeField] private AudioClip upgradeSuccessClip;


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

        //移動速度の上限を設けておく
        if (PlayerDataManager.Instance.SpeedLevel >= 50)
        {
            SpeedText.text = "LevelMAX";
            SpeedLevelText.text = "移動速度 LvMAX";
        }
        else
        {
            SpeedText.text = $"{PlayerDataManager.Instance.SpeedScore}円";
            SpeedLevelText.text = $"移動速度 Lv{PlayerDataManager.Instance.SpeedLevel}";
        }

        //採掘範囲の上限を設けておく
        if (PlayerDataManager.Instance.RangeLevel >= 5)
        {
            RangeText.text = "LevelMAX";
            RangeLevelText.text = "採掘範囲 LvMAX";
        }
        else
        {
            RangeText.text   = $"{PlayerDataManager.Instance.RangeScore}円";
            RangeLevelText.text = $"採掘範囲 Lv{PlayerDataManager.Instance.RangeLevel}";
        }
        

        //通知テキストのカウントダウン処理
        if (notifyTimer > 0)
        {
            notifyTimer -= Time.deltaTime;
            if (notifyTimer <= 0)
            {
                NotifyText.text = ""; // 時間が来たら消す
            }
        }


        //--強化ボタン押下時の動作
        MiningSpeedUpgrade();

        MoveSpeedUpdate();

        MiningRangeUpgrade();
        

        //Enterで次の画面へ移動
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            ////移動時自身をfalseにする
            //this.gameObject.SetActive(false);
            //シーン移動
            SceneManager.LoadScene(nextSceneName);
        }

    }

    /// <summary>
    /// 採掘速度強化
    /// </summary>
    void MiningSpeedUpgrade()
    {
        //1キー
        //長押しされてるか、単押しか
        if (Keyboard.current.digit1Key.isPressed)
        {
            //タイマー加算
            holdTimer1Key += Time.deltaTime;

            //強化キー押された瞬間か、タイマーが閾値超えてるか
            if (Keyboard.current.digit1Key.wasPressedThisFrame || holdTimer1Key > 1.0f)
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
                    PlayerDataManager.Instance.DiggingScore += 500;
                    //プレイヤーの採掘速度を上昇
                    PlayerDataManager.Instance.playerDigSpeed += 0.2f;
                    //レベル上昇
                    PlayerDataManager.Instance.DiggingLevel += 1;

                    //アップグレード成功SE
                    upgradeAudioSource.PlayOneShot(upgradeSuccessClip);
                    ShowNotify($"採掘速度アップグレード完了");
                }
            }
        }
        else
        {
            //長押しされてないならタイマーを0に
            holdTimer1Key = 0.0f;
        }
    }

    /// <summary>
    /// 移動速度強化
    /// </summary>
    void MoveSpeedUpdate()
    {
        //2キー
        //長押しされてるか、単押しか
        if (Keyboard.current.digit2Key.isPressed)
        {
            //タイマー加算
            holdTimer2Key += Time.deltaTime;

            //強化キー押された瞬間か、タイマーが閾値超えてるか
            if (Keyboard.current.digit2Key.wasPressedThisFrame || holdTimer2Key > 1.0f)
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
                        PlayerDataManager.Instance.SpeedScore += 300;
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
        }
        else
        {
            //長押しされてないならタイマーを0に
            holdTimer2Key = 0.0f;
        }
    }

    /// <summary>
    /// 採掘範囲強化
    /// </summary>
    void MiningRangeUpgrade()
    {
        //3キー
        //長押しされてるか、単押しか
        if (Keyboard.current.digit3Key.isPressed)
        {
            //タイマー加算
            holdTimer3Key += Time.deltaTime;
            
            //強化キー押された瞬間か、タイマーが閾値超えてるか
            if (Keyboard.current.digit3Key.wasPressedThisFrame || holdTimer3Key > 1.0f)
            {
                //採掘範囲レベルがすでに上限に達している場合
                if (PlayerDataManager.Instance.RangeLevel >= 5)
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
                        PlayerDataManager.Instance.RangeScore += 20000;
                        //採掘範囲上昇
                        PlayerDataManager.Instance.miningRange += new Vector2(0.5f, 0.5f);
                        PlayerDataManager.Instance.miningRangeOffset -= new Vector2(PlayerDataManager.Instance.miningRangeOffset.x, 0.25f);
                        //レベル上昇
                        PlayerDataManager.Instance.RangeLevel += 1;

                        //アップグレード成功SE
                        upgradeAudioSource.PlayOneShot(upgradeSuccessClip);
                        ShowNotify($"採掘範囲アップグレード完了");
                    }
                }
            }
        }
        else
        { 
            //長押しされてないならタイマーを0に
            holdTimer3Key = 0.0f;
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

}
