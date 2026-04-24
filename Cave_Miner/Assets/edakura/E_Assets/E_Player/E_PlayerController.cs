using UnityEngine;
using UnityEngine.InputSystem;

public class E_PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    public InputAction DashAction;
    public InputAction DigAction;

    Vector2 PlayerVector;
    Rigidbody2D rbody;
    public float MoveSpeed = 5.0f;
    public float DashMultiplier = 1.5f;

    [SerializeField]
    float currentSpeed;

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

        if(DigAction.IsPressed())
        {
            Debug.Log("採掘ボタン押下");
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
}
