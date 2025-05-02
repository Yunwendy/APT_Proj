using System.Collections.Generic;
using UnityEngine;

public class MouseCrosshairLine : MonoBehaviour
{
    public Camera mainCamera;
    public float crosshairSize = 0.05f;

    public float circleRadius = 0.25f;
    public int circleSegmentCount = 20;

    private LineRenderer horizontalLine;
    private LineRenderer verticalLine;
    private LineRenderer circleLine;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // 가로선 생성
        horizontalLine = CreateLineRenderer("HorizontalLine");

        // 세로선 생성
        verticalLine = CreateLineRenderer("VerticalLine");

        // 원 생성
        circleLine = CreateLineRenderer("CircleLine");
        circleLine.loop = true;
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            // Raycast hit 지점
            Vector3 center = hitInfo.point;

            Vector3 liftedCenter = center + hitInfo.normal * 0.3f; // 띄우는 거리

            Vector3 right = mainCamera.transform.right;
            Vector3 up = mainCamera.transform.up;

            horizontalLine.enabled = true;
            verticalLine.enabled = true;
            circleLine.enabled = true;

            // 가로선 설정
            horizontalLine.SetPosition(0, liftedCenter - right * crosshairSize);
            horizontalLine.SetPosition(1, liftedCenter + right * crosshairSize);

            // 세로선 설정
            verticalLine.SetPosition(0, liftedCenter - up * crosshairSize);
            verticalLine.SetPosition(1, liftedCenter + up * crosshairSize);

            // 원 위치 설정
            DrawCircle(liftedCenter);

            var validTags = new HashSet<string> { "DC", "Gear", "Motor", "Tire", "Tire2", "Tire3", "Tire4", "Cool", "VCU" };

            if (validTags.Contains(hitInfo.collider.gameObject.tag))
            {
                SetLineColor(Color.green);
            }
            else
            {
                horizontalLine.enabled = false;
                verticalLine.enabled = false;
                circleLine.enabled = false;
            }
        }
        else
        {
            horizontalLine.enabled = false;
            verticalLine.enabled = false;
            circleLine.enabled = false;
        }
    }

    LineRenderer CreateLineRenderer(string name)
    {
        GameObject obj = new GameObject(name);
        LineRenderer lr = obj.AddComponent<LineRenderer>();
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        return lr;
    }

    void SetLineColor(Color color)
    {
        horizontalLine.startColor = color;
        horizontalLine.endColor = color;
        verticalLine.startColor = color;
        verticalLine.endColor = color;
        circleLine.startColor = color;
        circleLine.endColor = color;
    }

    void DrawCircle(Vector3 center)
    {
        float distance = Vector3.Distance(mainCamera.transform.position, center);
        float actualRadius = circleRadius * distance * 0.3f;
        // (0.1은 조절용: 필요에 따라 크거나 작게 조정)

        circleLine.positionCount = circleSegmentCount + 1; // 시작점 = 끝점
        float angleStep = 360f / circleSegmentCount;

        for (int i = 0; i <= circleSegmentCount; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep);
            //Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * circleRadius;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * actualRadius;

            // 카메라 방향을 기준으로 XY 평면을 결정
            Vector3 right = mainCamera.transform.right;
            Vector3 up = mainCamera.transform.up;

            Vector3 worldOffset = right * offset.x + up * offset.y;
            circleLine.SetPosition(i, center + worldOffset);
        }
    }
}














