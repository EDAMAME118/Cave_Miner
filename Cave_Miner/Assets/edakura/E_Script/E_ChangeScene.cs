using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class E_ChangeScene : MonoBehaviour
{
    public string nextSceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
