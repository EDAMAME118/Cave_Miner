using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class E_PlayerController : MonoBehaviour
{
    public Text BlockText;

    public InputAction MoveAction;
    public InputAction DashAction;
    public InputAction DigAction;

    Vector2 PlayerVector;
    Rigidbody2D rbody;
    public float MoveSpeed = 5.0f;
    public float DashMultiplier = 1.5f;

    [SerializeField]
    float currentSpeed;
    float digSpeed = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        MoveAction.Enable();
        DashAction.Enable();
        DigAction.Enable();

        currentSpeed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //MoveActionの値を読み込んでPlayerVectorに入れる
        PlayerVector = MoveAction.ReadValue<Vector2>();

        //ダッシュボタン押されてるなら移動速度上昇
        if(DashAction.IsPressed())
        {
            currentSpeed = MoveSpeed * DashMultiplier;
        }
        else
        {
            currentSpeed = MoveSpeed;
        }

        

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
    }

    private void FixedUpdate()
    {
        //Rigidbodyに速度入れる
        rbody.linearVelocity = PlayerVector * currentSpeed;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Block"))
        {
            BlockText.text =
                "Blockを破壊する：Zボタン";
            Debug.Log("Blockに接触中");
            if (DigAction.IsPressed())
            {
                Debug.Log("破壊中");
                digSpeed -= 0.1f;
                if(digSpeed < 0.0f)
                {
                    Destroy(collision.gameObject);
                    Debug.Log("破壊完了");
                    digSpeed = 5.0f;
                }
            }
            else
            {
                digSpeed = 5.0f;
            }
        }
        else
        {
            BlockText.text = "";
        }
    }
}
