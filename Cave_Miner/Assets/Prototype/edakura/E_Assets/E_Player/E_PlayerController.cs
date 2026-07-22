using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

public class E_PlayerController : MonoBehaviour
{
    //public Text BlockText;

    public InputAction MoveAction;
    public InputAction DigAction;
    bool isMoveing = false;
    int direction = 0;

    Vector2 PlayerVector;
    Rigidbody2D rbody;

    [SerializeField]
    float currentSpeed;
    [SerializeField] private Transform MiningRange;

    public static Vector2 miningRange;
    public static Vector2 miningRangeOffset;

    //足音用
    AudioSource audioSource;
    [SerializeField] AudioClip walkClip;
    [SerializeField] private float walkAudioDelay = 0.0f;

    //アニメーション用
    Animator animator;
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerWalk";
    public string nowAnime = "";
    public string oldAnime = "";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator= GetComponent<Animator>();

        MoveAction.Enable();
        DigAction.Enable();
        
        currentSpeed = 0.0f;

        animator = this.GetComponent<Animator>();
        nowAnime = stopAnime;       //停止から開始
        oldAnime = stopAnime;       //停止から開始
    }

    // Update is called once per frame
    void Update()
    {
    
        //MoveActionの値を読み込んでPlayerVectorに入れる
        PlayerVector = MoveAction.ReadValue<Vector2>();

        currentSpeed = PlayerDataManager.Instance.playerSpeed;
        
        //向きの調整
        if (PlayerVector.x < 0.0f)
        {
            //Debug.Log("右移動");
            transform.rotation = Quaternion.Euler(0, 0,-90);
            
        }
        else if (PlayerVector.x > 0.0f)
        {
            //Debug.Log("左移動");
            transform.rotation = Quaternion.Euler(0, 0, 90);

        }
        else if (PlayerVector.y > 0.0f)
        {
            //Debug.Log("上移動");
            transform.rotation = Quaternion.Euler(0, 0, 180);
           
        }
        else if (PlayerVector.y < 0.0f)
        {
            //Debug.Log("下移動");
            transform.rotation = Quaternion.Euler(0, 0, 0);
          
        }
        //プレイヤーが移動している場合
        if (PlayerVector != Vector2.zero)
        {
            isMoveing = true;
            //足音用変数加算
            walkAudioDelay += Time.deltaTime;
            if (walkAudioDelay > 0.5f)
            {
                Debug.Log("足音再生");
                //効果音再生
                //audioSource.PlayOneShot(walkClip);
                walkAudioDelay = 0.0f;
            }
        }
        //立ち止まっている場合
        else
        {
            walkAudioDelay = 0.0f;
            isMoveing = false;
        }
    }

    private void FixedUpdate()
    {
        //アニメーション更新
        if (PlayerVector.x == 0.0f && PlayerVector.y == 0.0f)
        {
            nowAnime = stopAnime;
        }
        else
        {
            nowAnime = moveAnime;
        }

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);
        }

        //Rigidbodyに速度入れる
        rbody.linearVelocity = PlayerVector * currentSpeed;
    }



}
