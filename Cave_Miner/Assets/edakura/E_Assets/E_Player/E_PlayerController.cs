using UnityEngine;
using UnityEngine.InputSystem;

public class E_PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    public InputAction DashAction;

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
    }

    private void FixedUpdate()
    {
        //Rigidbodyに速度入れる
        rbody.linearVelocity = PlayerVector * currentSpeed;
    }
}
