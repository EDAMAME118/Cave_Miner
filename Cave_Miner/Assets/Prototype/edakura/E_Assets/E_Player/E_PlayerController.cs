using UnityEngine;
using UnityEngine.InputSystem;

public class E_PlayerController : MonoBehaviour
{
    //public Text BlockText;

    public InputAction MoveAction;
    public InputAction DigAction;

    Vector2 PlayerVector;
    Rigidbody2D rbody;

    [SerializeField]
    float currentSpeed;

    public static Vector2 miningRange;
    public static Vector2 miningRangeOffset;

    //足音用
    AudioSource audioSource;
    [SerializeField] AudioClip walkClip;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        MoveAction.Enable();
        DigAction.Enable();

        currentSpeed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //MoveActionの値を読み込んでPlayerVectorに入れる
        PlayerVector = MoveAction.ReadValue<Vector2>();

        currentSpeed = PlayerDataManager.playerSpeed;
        
        //向きの調整
        if (PlayerVector.x > 0.0f)
        {
            //Debug.Log("右移動");
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (PlayerVector.x < 0.0f)
        {
            //Debug.Log("左移動");
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (PlayerVector.y > 0.0f)
        {
            //Debug.Log("上移動");
            transform.rotation = Quaternion.Euler(0, 0, -180);
        }
        else if (PlayerVector.y < 0.0f)
        {
            //Debug.Log("下移動");
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if(PlayerVector != Vector2.zero)
        {

        }
    }

    private void FixedUpdate()
    {
        //Rigidbodyに速度入れる
        rbody.linearVelocity = PlayerVector * currentSpeed;
    }
}
