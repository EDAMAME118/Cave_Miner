using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class E_UpgradeScript : MonoBehaviour
{
    //各種テキスト
    public Text DiggingText;
    public Text SpeedText;
    public Text RangeText;
    public Text NotifyText;

    public Text DiggingLevelText;
    public Text SpeedLevelText;
    public Text RangeLevelText;

    public Text ScoreText;

    public string nextSceneName;

    //アップグレードに必要なスコア変数(強化毎に上昇)
    private int DiggingScore;
    private int SpeedScore;
    private int RangeScore;
    //各種強化レベル
    private int DiggingLevel;
    private int SpeedLevel;
    private int RangeLevel;

    //アップグレード通知用の変数
    private float notifyTimer = 0.0f;
    private const float DisplayDuration = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Start関数を一度しか動かしたくない
        //必然的にgameObjectをTrue、Falseにする方法で
        //シーン移動時アップグレードが作動しないようにする
        //そのためシーン移動時Destroyしないようにする
        DontDestroyOnLoad(this.gameObject);

        //必要スコア初期化
        DiggingScore = 100;
        SpeedScore   = 100;
        RangeScore   = 100;

        //強化レベル初期化
        DiggingLevel = 1;
        SpeedLevel   = 1;
        RangeLevel   = 1;

        //強化時表示されるテキストを空にしておく
        NotifyText.text = $"";
    }

    private void OnEnable()
    {
        NotifyText.text = $"";
    }

    // Update is called once per frame
    void Update()
    {
        //現在のスコアを表示
        ScoreText.text = $"現在のスコア:{ScoreManager.score}";

        //必要スコア表示、
        DiggingText.text = $"{DiggingScore}スコア";
        SpeedText.text   = $"{SpeedScore}スコア";
        RangeText.text   = $"{RangeScore}スコア";
        //強化レベル表示
        DiggingLevelText.text = $"採掘速度 Lv{DiggingLevel}";
        SpeedLevelText.text = $"移動速度 Lv{SpeedLevel}";
        RangeLevelText.text = $"採掘範囲 Lv{RangeLevel}";

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
        //1キー
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            //--採掘速度強化
            //スコアが足りない場合
            if(ScoreManager.score < DiggingScore )
            {
                ShowNotify($"スコアが足りません");
            }
            //スコアが足りる場合
            else
            {
                //スコアを消費
                ScoreManager.score -= DiggingScore;
                //必要スコアを上昇
                DiggingScore += 100;
                //プレイヤーの採掘速度を上昇
                E_PlayerController.digSpeed += 0.5f;
                //レベル上昇
                DiggingLevel += 1;

                ShowNotify($"採掘速度アップグレード完了");
            }
        }
        //2キー
        if(Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            //--移動速度強化
            //スコアが足りない場合
            if(ScoreManager.score < SpeedScore)
            {
                ShowNotify($"スコアが足りません");
            }
            //スコアが足りる場合
            else
            {
                //スコアを消費
                ScoreManager.score -= SpeedScore;
                //必要スコアを上昇
                SpeedScore += 100;
                //プレイヤーの移動速度を上昇
                E_PlayerController.MoveSpeed += 0.5f;
                //レベル上昇
                SpeedLevel += 1;

                ShowNotify($"移動速度アップグレード完了");
            }
        }
        //3キー
        if(Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            //--採掘範囲強化
            //スコアが足りない場合
            if (ScoreManager.score < RangeScore)
            {
                ShowNotify($"スコアが足りません");
            }
            //スコアが足りる場合
            else
            {
                //スコアを消費
                ScoreManager.score -= RangeScore;
                //必要スコアを上昇
                RangeScore += 100;
                //プレイヤーの移動速度を上昇
                E_PlayerController.digRange += 0.5f;
                //レベル上昇
                RangeLevel += 1;

                ShowNotify($"採掘範囲アップグレード完了");
            }
        }

        //Enterで次の画面へ移動
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            //移動時自身をfalseにする
            this.gameObject.SetActive(false);
            //シーン移動
            SceneManager.LoadScene(nextSceneName);
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
