using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // UIを操作するために必要

public class GaugeController : MonoBehaviour
{
    // インスペクターから GaugeFill（中身の画像）をドラッグ＆ドロップする
    [SerializeField] private Image gaugeFillImage;

    private float maxHp = 100f;
    private float currentHp;

    void Start()
    {
        currentHp = maxHp;
        UpdateGauge();
    }

    // テスト用にキー入力でHPを増減させる
    // テスト用にキー入力でHPを増減させる
    void Update()
    {
        // 【修正】KeyCode.Damage ➔ KeyCode.D に変更
        if (Keyboard.current.dKey.isPressed) // ダメージを受ける（Dキー）
        {
            TakeDamage(0.1f);
        }
        if (Keyboard.current.hKey.isPressed)// 回復する（Hキー）
        {
            Heal(0.1f);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        currentHp = Mathf.Clamp(currentHp, 0f, maxHp); // 0以下にならないように制限
        UpdateGauge();
    }

    public void Heal(float amount)
    {
        currentHp += amount;
        currentHp = Mathf.Clamp(currentHp, 0f, maxHp); // maxHp以上にならないように制限
        UpdateGauge();
    }

    // ゲージの表示を更新するメソッド
    private void UpdateGauge()
    {
        //現在の値 / 最大値 で 0.0 〜 1.0 の割合を計算してFill Amountに代入
        gaugeFillImage.fillAmount = currentHp / maxHp;
    }
}