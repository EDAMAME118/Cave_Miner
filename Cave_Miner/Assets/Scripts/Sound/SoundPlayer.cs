using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer Instance;

    private AudioSource audioSource;

    void Awake()
    {
        //シングルトン
        //自身が存在しない場合
        if(Instance == null)
        {
            //自身をInstanceに登録
            Instance = this;
            //シーンが切り替わっても破棄されないようにする
            DontDestroyOnLoad(this.gameObject);
        }
        //すでに存在する場合
        else
        {
            //自身を破棄
            Destroy(this.gameObject);
        }


    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 引数のSEを流す関数
    /// </summary>
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
