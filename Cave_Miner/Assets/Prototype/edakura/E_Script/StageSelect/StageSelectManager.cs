using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    //ステージ名配列
    [SerializeField]
    private string[] caveScenes;

    //通知テキスト
    [SerializeField]
    private Text NotifyText;

    //通知用の変数
    private float notifyTimer = 0.0f;
    private const float DisplayDuration = 2.0f;

    //総額表示テキスト
    [SerializeField]
    private Text totalMoneyText;

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

        //ステージ選択
        //１キー
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            PlayerDataManager.Instance.currentStage = Stage.STAGE1;
            Debug.Log("Stage1選択中");
        }
        //２キー
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            if (ScoreManager.Instance.totalScore > 50000)
            {
                PlayerDataManager.Instance.currentStage = Stage.STAGE2;
                Debug.Log("Stage2選択中");
            }
            else
            {
                ShowNotify("お金が足りません");
            }
        }
        //３キー
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            if (ScoreManager.Instance.totalScore > 100000)
            {
                PlayerDataManager.Instance.currentStage = Stage.STAGE3;
                Debug.Log("Stage3選択中");
            }
            else
            {
                ShowNotify("お金が足りません");
            }
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

        //シーン移動
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            //現在のステージ番号
            int stageIndex = (int)PlayerDataManager.Instance.currentStage;

            //現在のステージ番号がステージ名配列の長さを超えていないか
            //見ているステージ配列の要素がNULLではないか
            if (stageIndex >= 0 && stageIndex < caveScenes.Length && !string.IsNullOrEmpty(caveScenes[stageIndex]))
            {
                //現在選ばれているステージ番号のステージに移動する
                SceneManager.LoadScene(caveScenes[stageIndex]);
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
}
