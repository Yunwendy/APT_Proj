using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float lookSpeed = 2f;
    public float zoomSpeed = 5f; // 줌 속도를 더 미세하게 조정
    public float minZoomDistance = 1f;
    public float maxZoomDistance = 100f;

    private Vector3 lastMousePos;

    void Update()
    {
        // === 카메라 이동 (우클릭) ===
        if (Input.GetMouseButton(1)) // 우클릭
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            Vector3 move = (-transform.right * delta.x + -transform.up * delta.y) * Time.deltaTime * moveSpeed;
            transform.position += move;
        }

        // === 카메라 회전 (휠 클릭) ===
        if (Input.GetMouseButton(2)) // 휠 클릭
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            transform.eulerAngles += new Vector3(-mouseY, mouseX, 0);
        }

        // === 줌 인/아웃 (휠 스크롤) ===
        float scroll = Input.GetAxis("Mouse ScrollWheel") * 0.5f; // 감도를 낮춰 줌 이동을 더 미세하게
        if (scroll != 0)
        {
            Vector3 zoom = transform.forward * scroll * zoomSpeed;
            Vector3 newPosition = transform.position + zoom;

            float distance = Vector3.Distance(newPosition, Vector3.zero); // 기준점에서 거리 측정
            if (distance > minZoomDistance && distance < maxZoomDistance)
            {
                transform.position = newPosition;
            }
        }

        lastMousePos = Input.mousePosition;
    }
}


