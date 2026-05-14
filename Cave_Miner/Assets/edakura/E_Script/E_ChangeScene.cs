using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class E_ChangeScene : MonoBehaviour
{
    public string nextSceneName;

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
