using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class E_TitleSceneChanger : MonoBehaviour
{
    //シーン名を決める
    public string nextScene = "NextScene";

    // Update is called once per frame
    void Update()
    {
        //Enterキー押下時
        if(Keyboard.current.enterKey.wasPressedThisFrame)
        {
            //シーン移動
            SceneManager.LoadScene(nextScene);
        }

        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {

        }

        // Escapeキーが押されたらゲーム終了
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            QuitGame();
        }
    }

    void QuitGame()
    {
        // エディタ上での動作確認用（ビルド後には無視されます）
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 実際のゲーム（ビルド済みアプリ）を終了させる
        Application.Quit();
#endif
    }
}
