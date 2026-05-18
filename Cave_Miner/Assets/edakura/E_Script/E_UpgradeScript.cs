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

    public string nextSceneName;

    //アップグレード通知用の変数
    private float notifyTimer = 0.0f;
    private const float DisplayDuration = 2.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //その日得たスコアと採掘数を初期化
        ScoreManager.dayMiningCount = 0;
        ScoreManager.dayScore = 0;

        //強化時表示されるテキストを空にしておく
        NotifyText.text = $"";
    }

    // Update is called once per frame
    void Update()
    {
        //現在のスコアを表示
        ScoreText.text = $"現在のスコア:{ScoreManager.score}";

        //必要スコア表示、
        DiggingText.text = $"{PlayerDataManager.DiggingScore}スコア";
        SpeedText.text   = $"{PlayerDataManager.SpeedScore}スコア";
        RangeText.text   = $"{PlayerDataManager.RangeScore}スコア";
        //強化レベル表示
        DiggingLevelText.text = $"採掘速度 Lv{PlayerDataManager.DiggingLevel}";
        SpeedLevelText.text = $"移動速度 Lv{PlayerDataManager.SpeedLevel}";
        RangeLevelText.text = $"採掘範囲 Lv{PlayerDataManager.RangeLevel}";

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
            if(ScoreManager.score < PlayerDataManager.DiggingScore )
            {
                ShowNotify($"スコアが足りません");
            }
            //スコアが足りる場合
            else
            {
                //スコアを消費
                ScoreManager.score -= PlayerDataManager.DiggingScore;
                //必要スコアを上昇
                PlayerDataManager.DiggingScore += 100;
                //プレイヤーの採掘速度を上昇
                PlayerDataManager.playerDigSpeed += 0.5f;
                //レベル上昇
                PlayerDataManager.DiggingLevel += 1;

                ShowNotify($"採掘速度アップグレード完了");
            }
        }
        //2キー
        if(Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            //--移動速度強化
            //スコアが足りない場合
            if(ScoreManager.score < PlayerDataManager.SpeedScore)
            {
                ShowNotify($"スコアが足りません");
            }
            //スコアが足りる場合
            else
            {
                //スコアを消費
                ScoreManager.score -= PlayerDataManager.SpeedScore;
                //必要スコアを上昇
                PlayerDataManager.SpeedScore += 100;
                //プレイヤーの移動速度を上昇
                PlayerDataManager.playerSpeed += 0.5f;
                //レベル上昇
                PlayerDataManager.SpeedLevel += 1;

                ShowNotify($"移動速度アップグレード完了");
            }
        }
        //3キー
        if(Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            //--採掘範囲強化
            //スコアが足りない場合
            if (ScoreManager.score < PlayerDataManager.RangeScore)
            {
                ShowNotify($"スコアが足りません");
            }
            //スコアが足りる場合
            else
            {
                //スコアを消費
                ScoreManager.score -= PlayerDataManager.RangeScore;
                //必要スコアを上昇
                PlayerDataManager.RangeScore += 100;
                //採掘範囲上昇
                PlayerDataManager.miningRange += new Vector2(1, 1);
                PlayerDataManager.miningRangeOffset -= new Vector2(PlayerDataManager.miningRangeOffset.x, -0.5f);
                //レベル上昇
                PlayerDataManager.RangeLevel += 1;

                ShowNotify($"採掘範囲アップグレード完了");
            }
        }

        //Enterで次の画面へ移動
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            ////移動時自身をfalseにする
            //this.gameObject.SetActive(false);
            //シーン移動
            SceneManager.LoadScene(nextSceneName);
        }

    }

    //採掘範囲アップグレード
    void HandleRange()
    {

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
