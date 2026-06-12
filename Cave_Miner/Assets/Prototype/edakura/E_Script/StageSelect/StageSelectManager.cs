using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    //ステージ名配列
    [SerializeField]
    private string[] caveScenes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //ステージ選択
        //１キー
        if(Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            PlayerDataManager.Instance.currentStage = Stage.STAGE1;
            Debug.Log("Stage1選択中");
        }
        //２キー
        else if(Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            PlayerDataManager.Instance.currentStage = Stage.STAGE2;
            Debug.Log("Stage2選択中");
        }
        //３キー
        else if(Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            PlayerDataManager.Instance.currentStage = Stage.STAGE3;
            Debug.Log("Stage3選択中");
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
}
