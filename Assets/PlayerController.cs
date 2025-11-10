using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform playerBody;
    public Transform mainCamera;

    [Header("Settings")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 2f;    // 滑鼠水平旋轉速度
    public float pitchSpeed = 1.5f;   // 相機上下旋轉速度
    public float minPitch = -30f;
    public float maxPitch = 60f;
    public bool invertMouseX = false; // 左右反向
    public bool invertMouseY = false; // 上下反向

    private float yaw = 0f;
    private float pitch = 20f;

    private Vector2 lastMousePos;
    private bool isDragging = false;

    // UI 按鈕長按控制
    private bool moveForward, moveBack, moveLeft, moveRight;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main.transform;
        pitch = mainCamera.localEulerAngles.x;
        yaw = playerBody.eulerAngles.y;
    }

    void Update()
    {
        HandleMouseRotation();
        HandleMovement();
    }

    // 🔹 移動（角色前方向）
    void HandleMovement()
    {
        // 鍵盤輸入
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // UI 按鈕輸入
        if (moveForward) v += 1f;
        if (moveBack) v -= 1f;
        if (moveLeft) h -= 1f;
        if (moveRight) h += 1f;

        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            Vector3 moveDir = playerBody.rotation * inputDir;
            playerBody.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }

    // 🔹 滑鼠拖曳控制角色旋轉 + 相機俯仰
    void HandleMouseRotation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0)) isDragging = false;

        if (isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;

            float deltaX = invertMouseX ? -delta.x : delta.x;
            float deltaY = invertMouseY ? -delta.y : delta.y;

            yaw += deltaX * rotateSpeed;
            pitch -= deltaY * pitchSpeed;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            playerBody.rotation = Quaternion.Euler(0f, yaw, 0f);

            Vector3 camAngles = mainCamera.localEulerAngles;
            camAngles.x = pitch;
            mainCamera.localEulerAngles = camAngles;
        }

        // 觸控支援
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                float deltaX = invertMouseX ? -t.deltaPosition.x : t.deltaPosition.x;
                float deltaY = invertMouseY ? -t.deltaPosition.y : t.deltaPosition.y;

                yaw += deltaX * rotateSpeed * 0.1f;
                pitch -= deltaY * pitchSpeed * 0.1f;
                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

                playerBody.rotation = Quaternion.Euler(0f, yaw, 0f);

                Vector3 camAngles = mainCamera.localEulerAngles;
                camAngles.x = pitch;
                mainCamera.localEulerAngles = camAngles;
            }
        }
    }

    // 🔹 UI 按鈕事件綁定
    public void OnMoveForwardDown() => moveForward = true;
    public void OnMoveForwardUp() => moveForward = false;
    public void OnMoveBackDown() => moveBack = true;
    public void OnMoveBackUp() => moveBack = false;
    public void OnMoveLeftDown() => moveLeft = true;
    public void OnMoveLeftUp() => moveLeft = false;
    public void OnMoveRightDown() => moveRight = true;
    public void OnMoveRightUp() => moveRight = false;
}