using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    //ステージ名配列
    [SerializeField]
    private string[] caveSceneNames;


    private int stageSelectIndex;

    //通知テキスト
    [SerializeField]
    private Text NotifyText;

    //通知用の変数
    private float notifyTimer = 0.0f;
    private const float DisplayDuration = 2.0f;

    //総額表示テキスト
    [SerializeField]
    private Text totalMoneyText;

    //ステージ画像
    [SerializeField] private Image stage1Image;
    [SerializeField] private Image stage2Image;
    [SerializeField] private Image stage3Image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //強化時表示されるテキストを空にしておく
        NotifyText.text = $"";
    }

    // Update is called once per frame
    void Update()
    {
        //総額表示
        totalMoneyText.text  = $"稼いだ総額:{ScoreManager.Instance.totalScore}円";


        //ステージを左右キーで選択
        if(Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            stageSelectIndex--;
        }
        else if(Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            stageSelectIndex++;
        }

        //ステージ数を超えて右に行ったら
        if(stageSelectIndex > (int)Stage.STAGE3)
        {
            //ステージを左に戻す
            stageSelectIndex = (int)Stage.STAGE1;
        }
        //左に行ったら
        else if(stageSelectIndex < (int)Stage.STAGE1)
        {
            stageSelectIndex = (int)Stage.STAGE3;
        }

        //Enterキーでステージを決定し、移動
        if(Keyboard.current.enterKey.wasPressedThisFrame)
        {
            //選択しているのがステージ１ならば
            if(stageSelectIndex == (int)Stage.STAGE1)
            {
                //ステージ設定
                PlayerDataManager.Instance.currentStage = Stage.STAGE1;
                //シーン移動
                StageChanger();
            }
            //選択しているのがステージ２ならば
            else if (stageSelectIndex == (int)Stage.STAGE2)
            {
                //総スコアチェック
                if (ScoreManager.Instance.totalScore > 50000)
                {
                    //ステージ設定
                    PlayerDataManager.Instance.currentStage = Stage.STAGE2;
                    //シーン移動
                    StageChanger();
                }
                else
                {
                    ShowNotify($"お金が足りません\n\n" +
                               $"必要な残り金額：{50000 - ScoreManager.Instance.totalScore}円");
                }
            }
            //選択しているのがステージ３ならば
            else if (stageSelectIndex == (int)Stage.STAGE3)
            {
                //総スコアチェック
                if (ScoreManager.Instance.totalScore > 100000)
                {
                    //ステージ設定
                    PlayerDataManager.Instance.currentStage = Stage.STAGE3;
                    //シーン移動
                    StageChanger();
                }
                else
                {
                    ShowNotify($"お金が足りません\n\n" + 
                               $"必要な残り金額：{100000 - ScoreManager.Instance.totalScore}円");
                }
            }
        }

        //ステージ画像の色変更
        StageImageColorChanger();

        //通知テキストのカウントダウン処理
        if (notifyTimer > 0)
        {
            notifyTimer -= Time.deltaTime;
            if (notifyTimer <= 0)
            {
                NotifyText.text = ""; // 時間が来たら消す
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
    /// ステージ画像選択時の色変更
    /// </summary>
    void StageImageColorChanger()
    {
        //ステージ画像の色変更
        //ステージ１
        if (stageSelectIndex == (int)Stage.STAGE1)
        {
            stage1Image.color = new Color32(140, 140, 255, 255);
        }
        else
        {
            stage1Image.color = new Color32(255, 255, 255, 255);
        }

        //ステージ２
        if (stageSelectIndex == (int)Stage.STAGE2)
        {
            stage2Image.color = new Color32(140, 140, 255, 255);
        }
        else
        {
            stage2Image.color = new Color32(255, 255, 255, 255);
        }
        //ステージ３
        if (stageSelectIndex == (int)Stage.STAGE3)
        {
            stage3Image.color = new Color32(140, 140, 255, 255);
        }
        else
        {
            stage3Image.color = new Color32(255, 255, 255, 255);
        }
    }

    /// <summary>
    /// ステージ移動
    /// </summary>
    void StageChanger()
    {
        //現在のステージ番号
        int stageIndex = (int)PlayerDataManager.Instance.currentStage;

        //現在のステージ番号がステージ名配列の長さを超えていないか
        //見ているステージ配列の要素がNULLではないか
        if (stageIndex >= 0 && stageIndex < caveSceneNames.Length && !string.IsNullOrEmpty(caveSceneNames[stageIndex]))
        {
            //現在選ばれているステージ番号のステージに移動する
            SceneManager.LoadScene(caveSceneNames[stageIndex]);
        }
    }
}
