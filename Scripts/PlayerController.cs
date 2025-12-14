using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Cần Component Animator để điều khiển animation
    private Animator animator;

    // --- Biến Điều khiển Làn đường ---
    // desiredLane: 0 (Trái), 1 (Giữa), 2 (Phải)
    private int desiredLane = 1;
    public float laneDistance = 2.09f; // Khoảng cách giữa các làn đường 
    public float lateralSpeed = 15f; // Tốc độ di chuyển ngang mượt mà

    // Cần Component CharacterController
    private CharacterController controller;

    // Các biến Lực Nhảy
    public float jumpForce = 10f;
    private float verticalVelocity; // Vận tốc theo trục Y (nhảy/rơi)

    // Biến để kiểm soát trạng thái game
    private bool isDead = false;

    void Start()
    {
        // Lấy các Component cần thiết
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        // (DEBUG) Kiểm tra CharacterController đã được gán thành công
        if (controller == null)
        {
            Debug.LogError("LỖI: CharacterController không được tìm thấy trên đối tượng này!");
        }
        else
        {
            Debug.Log("CharacterController đã được gán thành công.");
        }

        // Đảm bảo nhân vật luôn ở vị trí 0 (trung tâm) ngay từ đầu trên trục X
        transform.position = new Vector3(0, transform.position.y, transform.position.z);

        // Kích hoạt animation chạy
        if (animator != null)
        {
            animator.SetBool("IsRunning", true);
        }
    }

    void Update()
    {
        // Nếu nhân vật chết hoặc thiếu Component thì dừng
        if (isDead || animator == null || controller == null) return;

        // 1. Xử lý Input (Nhảy, Chuyển làn)
        HandleMovementInput();

        // 2. Xử lý Lực Hút và Rơi
        HandleGravityAndFalling();

        // --- LOGIC DI CHUYỂN NGANG VÀ CHẠY TIẾN ---

        // Tính toán vị trí X đích (Target X) dựa trên desiredLane
        float targetX = (desiredLane - 1) * laneDistance;

        // Tính toán Vector di chuyển
        Vector3 moveVector = Vector3.zero;

        // Di chuyển ngang mượt mà (X)
        float deltaX = targetX - transform.position.x;
        moveVector.x = deltaX * lateralSpeed;

        // Vận tốc nhảy/rơi (Y)
        moveVector.y = verticalVelocity;

        // Trục Z: Giữ nguyên 0 vì đường sẽ cuộn (RoadManager xử lý Z)
        moveVector.z = 0;

        // Áp dụng di chuyển (Chỉ thay đổi vị trí X và Y)
        controller.Move(moveVector * Time.deltaTime);
    }

    void LateUpdate()
    {
        // Khóa góc xoay SAU CÙNG để ghi đè animation và ngăn chặn lỗi lộn ngược/xoay vòng
        if (!isDead)
        {
            // Khóa Rotation X và Z về 0, giữ Rotation Y là 180 độ (hướng chạy)
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
    }

    // =======================================================================
    // Định nghĩa các Hàm (Functions)
    // =======================================================================

    void HandleMovementInput()
    {
        // --- 1. Nhảy (Jump) ---
        if (IsGrounded() && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            animator.SetTrigger("Jump");
            verticalVelocity = jumpForce;
        }

        // --- 2. Lách Ngang (Strafe Left/Right) ---
        // Phím A/Mũi tên trái: Chuyển sang làn trái hơn
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane = Mathf.Max(0, desiredLane - 1);
        }

        // Phím D/Mũi tên phải: Chuyển sang làn phải hơn
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane = Mathf.Min(2, desiredLane + 1);
        }

        // --- 3. Kích hoạt Animation Lách Ngang ---
        animator.SetBool("StrafeLeft", Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow));
        animator.SetBool("StrafeRight", Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow));
    }

    void HandleGravityAndFalling()
    {
        if (controller.isGrounded)
        {
            // Nếu đang chạm đất, thiết lập vận tốc rơi về 0
            verticalVelocity = -0.5f;
        }
        else
        {
            // Áp dụng trọng lực (Gravity)
            verticalVelocity -= 20f * Time.deltaTime;
        }
    }

    bool IsGrounded()
    {
        return controller.isGrounded;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isDead) return;

        if (hit.gameObject.CompareTag("StumbleObstacle")) // Va chạm nhẹ (Vấp Té)
        {
            animator.SetTrigger("Stumble");
        }
        else if (hit.gameObject.CompareTag("GameOverObstacle")) // Va chạm mạnh (Game Over)
        {
            animator.SetTrigger("Die");
            isDead = true;
            // Thêm logic dừng game ở đây (ví dụ: Time.timeScale = 0;)
        }
    }
}