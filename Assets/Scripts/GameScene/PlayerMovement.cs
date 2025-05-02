using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    private CharacterController controller;
    private Transform cameraTransform;

    private float verticalRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        Debug.Log("transform position = " + transform.position);
    }

    void Update()
    {
        MovePlayer();
        LookAround();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move *= moveSpeed;

        // Y축 이동을 제거해 떨림 방지
        controller.Move(new Vector3(move.x, 0f, move.z) * Time.deltaTime);
    }

    void LateUpdate()
    {
        // Y 좌표를 항상 0으로 고정 (떴을 경우 보정)
        Vector3 pos = transform.position;
        pos.y = 0.5f;
        transform.position = pos;
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -20f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}





