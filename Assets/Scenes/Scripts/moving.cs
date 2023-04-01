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
        // ������� ������ �Է°�
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // �̵� ���� ���� ���
        Vector3 direction = cameraController.TargetDirection * vertical + cameraController.transform.right * horizontal;

        // ĳ���͸� ȸ����Ŵ
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // �̵� �ӵ��� �ε巴�� ��ȭ��Ŵ
        float targetSpeed = moveSpeed * direction.magnitude;
        if (direction.magnitude < 0.1f)
        {
            targetSpeed = 0f;
        }
        float smoothedSpeed = Mathf.SmoothDamp(animator.GetFloat("Speed"), targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        animator.SetFloat("Speed", smoothedSpeed / moveSpeed);

        // ĳ���͸� �̵���Ŵ
        Vector3 move = direction * smoothedSpeed * Time.deltaTime;
        controller.Move(move);

        // ����
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            animator.SetTrigger("Jump");
            isJumping = true;
            isGrounded = false; // ���� ������ ���������� ǥ��
        }

        // �߷� ����
        velocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;

        // �̵� �� �浹 ó��
        controller.Move(velocity * Time.deltaTime);

        // �ٴ� üũ
        isGrounded = controller.isGrounded; // controller.isGrounded�� ����Ͽ� ���� ĳ���Ͱ� ���� �ִ��� �˻�
        if (isGrounded && isJumping)
        {
            animator.SetTrigger("Land");
            isJumping = false;
        }
    }
}