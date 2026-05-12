using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class E_TimeLimit : MonoBehaviour
{
    //public MonoBehaviour upgradeScript;

    public int TimeLimit;
    private float timeInterval;

    public Text timeText;

    public string nextSceneBuy;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeInterval = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = $"Žc‚èŽžŠÔ:{TimeLimit}•b";

        timeInterval += Time.deltaTime;

        if(timeInterval >= 1.0f)
        {
            TimeLimit--;
            timeInterval -= 1.0f;
        }

        if(TimeLimit < 0)
        {
            //upgradeScript.enabled = !upgradeScript.enabled;

            Debug.Log("UpgradeScript‚Ìó‘Ô‚ð‹t");

            SceneManager.LoadScene(nextSceneBuy);
        }


    }
}
