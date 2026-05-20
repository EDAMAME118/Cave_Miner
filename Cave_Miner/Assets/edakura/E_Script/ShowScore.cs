using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    SpriteRenderer boxRenderer;

    Transform boxTransform;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"ÉXÉRÉA:{ScoreManager.dayScore}";

        boxTransform.localScale = new Vector3(2, 2, 2);
    }
}
