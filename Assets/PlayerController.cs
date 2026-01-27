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
    public float rotateSpeed = 2f;
    public float pitchSpeed = 1.5f;
    public float minPitch = -30f;
    public float maxPitch = 60f;
    public bool invertX = false;
    public bool invertY = false;

    float yaw;
    float pitch;

    bool moveForward, moveBack, moveLeft, moveRight;

    // PC 滑鼠控制
    bool mouseDragging = false;
    Vector2 lastMousePos;
    bool isFirstDragFrame = false;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main.transform;
        pitch = mainCamera.localEulerAngles.x;
        yaw = playerBody.eulerAngles.y;
    }

    void Update()
    {
        HandleMovement();      // 按鈕 + 鍵盤
        HandleRotationPC();    // PC 滑鼠旋轉
        HandleRotationMobile();// 手機旋轉
    }

    // -------------------------------
    // 判斷是否點到 UI（ScrollBar / Button / InputField）
    bool IsPointerOverUI(Vector2 pos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = pos;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var r in results)
        {
            if (r.gameObject.GetComponent<UnityEngine.UI.Selectable>())
                return true;
        }
        return false;
    }

    // -------------------------------
    // 鍵盤 + UI 按鈕移動
    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (moveForward) v += 1f;
        if (moveBack) v -= 1f;
        if (moveLeft) h -= 1f;
        if (moveRight) h += 1f;

        Vector3 dir = new Vector3(h, 0, v).normalized;

        if (dir.sqrMagnitude > 0.01f)
        {
            Vector3 moveDir = playerBody.rotation * dir;
            playerBody.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }

    // -------------------------------
    // PC 滑鼠旋轉（同時可移動）
    void HandleRotationPC()
    {
        if (Application.isMobilePlatform) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI(Input.mousePosition)) return; // 點 UI 不旋轉
            mouseDragging = true;
            isFirstDragFrame = true;
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0)) mouseDragging = false;
        if (!mouseDragging) return;

        Vector2 currentPos = Input.mousePosition;

        if (isFirstDragFrame)
        {
            lastMousePos = currentPos;
            isFirstDragFrame = false;
            return;
        }

        Vector2 delta = currentPos - lastMousePos;
        lastMousePos = currentPos;

        float deltaX = invertX ? -delta.x : delta.x;
        float deltaY = invertY ? -delta.y : delta.y;

        yaw += deltaX * rotateSpeed;
        pitch -= deltaY * pitchSpeed;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        ApplyRotation();
    }

    // -------------------------------
    // 手機旋轉（多點觸控 + UI 遮擋，同時可移動）
    void HandleRotationMobile()
    {
        if (!Application.isMobilePlatform) return;

        // 🔥 只允許單指旋轉
        if (Input.touchCount != 1) return;

        Touch t = Input.GetTouch(0);

        // 點到 UI 不旋轉
        if (IsPointerOverUI(t.position)) return;

        if (t.phase == TouchPhase.Moved)
        {
            float deltaX = invertX ? -t.deltaPosition.x : t.deltaPosition.x;
            float deltaY = invertY ? -t.deltaPosition.y : t.deltaPosition.y;

            yaw += deltaX * rotateSpeed * 0.1f;
            pitch -= deltaY * pitchSpeed * 0.1f;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            ApplyRotation();
        }
    }

    // -------------------------------
    void ApplyRotation()
    {
        playerBody.rotation = Quaternion.Euler(0, yaw, 0);
        Vector3 camAngles = mainCamera.localEulerAngles;
        camAngles.x = pitch;
        mainCamera.localEulerAngles = camAngles;
    }

    // -------------------------------
    // UI 按鈕事件
    public void OnMoveForwardDown() => moveForward = true;
    public void OnMoveForwardUp() => moveForward = false;
    public void OnMoveBackDown() => moveBack = true;
    public void OnMoveBackUp() => moveBack = false;
    public void OnMoveLeftDown() => moveLeft = true;
    public void OnMoveLeftUp() => moveLeft = false;
    public void OnMoveRightDown() => moveRight = true;
    public void OnMoveRightUp() => moveRight = false;
}