using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    

    //移動先のシーンをUnityから設定するw
    [SerializeField]
    private string sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        //エンターキーで設定したシーンに移動
        if(Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
