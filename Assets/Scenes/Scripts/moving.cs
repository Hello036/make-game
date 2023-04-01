using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float jumpForce = 7f;
    public float gravityScale = 2.5f;
    public float turnSmoothTime = 0.3f;
    public float speedSmoothTime = 0.3f;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private CharacterController controller;
    private CameraController cameraController;
    private Animator animator;
    private Vector3 velocity;
    private float turnSmoothVelocity;
    private float speedSmoothVelocity;
    private bool isGrounded;
    private bool isJumping;

    void Start()
    {
        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // 수평축과 수직축 입력값
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 이동 방향 벡터 계산
        Vector3 direction = cameraController.TargetDirection * vertical + cameraController.transform.right * horizontal;

        // 캐릭터를 회전시킴
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // 이동 속도를 부드럽게 변화시킴
        float targetSpeed = moveSpeed * direction.magnitude;
        if (direction.magnitude < 0.1f)
        {
            targetSpeed = 0f;
        }
        float smoothedSpeed = Mathf.SmoothDamp(animator.GetFloat("Speed"), targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        animator.SetFloat("Speed", smoothedSpeed / moveSpeed);

        // 캐릭터를 이동시킴
        Vector3 move = direction * smoothedSpeed * Time.deltaTime;
        controller.Move(move);

        // 점프
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            animator.SetTrigger("Jump");
            isJumping = true;
            isGrounded = false; // 현재 땅에서 떨어졌음을 표시
        }

        // 중력 적용
        velocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;

        // 이동 및 충돌 처리
        controller.Move(velocity * Time.deltaTime);

        // 바닥 체크
        isGrounded = controller.isGrounded; // controller.isGrounded를 사용하여 현재 캐릭터가 땅에 있는지 검사
        if (isGrounded && isJumping)
        {
            animator.SetTrigger("Land");
            isJumping = false;
        }
    }
}