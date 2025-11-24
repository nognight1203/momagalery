using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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
        if (mainCamera == null)
            mainCamera = Camera.main.transform;

        pitch = mainCamera.localEulerAngles.x;
        yaw = playerBody.eulerAngles.y;
    }

    void Update()
    {
        HandleMouseRotation();
        HandleMovement();
    }

    // 🔹 只阻擋 Scrollbar 的判斷
    bool IsPointerOverScrollbar()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var r in results)
        {
            // ⭐ 只有 Tag = "Scrollbar" 才阻擋旋轉
            if (r.gameObject.CompareTag("Scrollbar"))
                return true;
        }
        return false;
    }

    // 🔹 移動（角色前方向）
    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // UI Button 模擬方向
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

    // 🔹 滑鼠/觸控旋轉（排除 Scrollbar）
    void HandleMouseRotation()
    {
        // ⭐ 如果鼠標在 Scrollbar 上 → 直接不處理旋轉
        if (IsPointerOverScrollbar())
        {
            isDragging = false;
            return;
        }

        // 滑鼠拖曳開始
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
            isDragging = false;

        // 滑鼠拖曳旋轉
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

        // 觸控支援 -------------------------------------------------
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            // ⭐ 手機觸控版本一樣需要排除 Scrollbar
            if (IsPointerOverScrollbar())
                return;

            if (t.phase == TouchPhase.Moved)
            {
                yaw += t.deltaPosition.x * rotateSpeed * 0.1f;
                pitch -= t.deltaPosition.y * pitchSpeed * 0.1f;
                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

                playerBody.rotation = Quaternion.Euler(0f, yaw, 0f);

                Vector3 camAngles = mainCamera.localEulerAngles;
                camAngles.x = pitch;
                mainCamera.localEulerAngles = camAngles;
            }
        }
    }

    // 🔹 UI Button 事件
    public void OnMoveForwardDown() => moveForward = true;
    public void OnMoveForwardUp() => moveForward = false;
    public void OnMoveBackDown() => moveBack = true;
    public void OnMoveBackUp() => moveBack = false;
    public void OnMoveLeftDown() => moveLeft = true;
    public void OnMoveLeftUp() => moveLeft = false;
    public void OnMoveRightDown() => moveRight = true;
    public void OnMoveRightUp() => moveRight = false;
}