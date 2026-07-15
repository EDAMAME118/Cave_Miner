using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

public class E_PlayerController : MonoBehaviour
{
    //public Text BlockText;

    public InputAction MoveAction;
    public InputAction DigAction;
    Animator animator;
    bool isMoveing = false;
    int direction = 0;

    Vector2 PlayerVector;
    Rigidbody2D rbody;

    [SerializeField]
    float currentSpeed;

    public static Vector2 miningRange;
    public static Vector2 miningRangeOffset;

    //足音用
    AudioSource audioSource;
    [SerializeField] AudioClip walkClip;
    [SerializeField] private float walkAudioDelay = 0.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator= GetComponent<Animator>();

        MoveAction.Enable();
        DigAction.Enable();
        
        currentSpeed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //MoveActionの値を読み込んでPlayerVectorに入れる
        PlayerVector = MoveAction.ReadValue<Vector2>();

        currentSpeed = PlayerDataManager.Instance.playerSpeed;
        int dir=0;
        //向きの調整
        if (PlayerVector.x > 0.0f)
        {
            //Debug.Log("右移動");
            transform.rotation = Quaternion.Euler(0, 0, 90);
            //右
            dir = 3;
        }
        else if (PlayerVector.x < 0.0f)
        {
            //Debug.Log("左移動");
            transform.rotation = Quaternion.Euler(0, 0, -90);
            //左
            dir = 1;
        }
        else if (PlayerVector.y > 0.0f)
        {
            //Debug.Log("上移動");
            transform.rotation = Quaternion.Euler(0, 0, -180);
            //上
            dir = 2;
        }
        else if (PlayerVector.y < 0.0f)
        {
            //Debug.Log("下移動");
            transform.rotation = Quaternion.Euler(0, 0, 0);
            //下
            dir = 0;
        }
        if (dir != direction)
        {
            direction = dir;
            animator.SetInteger("Direction", direction);
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
        //Rigidbodyに速度入れる
        rbody.linearVelocity = PlayerVector * currentSpeed;
    }



}
