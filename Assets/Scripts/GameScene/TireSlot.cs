using UnityEngine;

public class TireSlot : MonoBehaviour
{
    public Transform snapPoint; // 오브젝트가 스냅될 위치
    private bool isOccupied = false; // 슬롯에 오브젝트가 있는지 확인

    // 'Tire' 오브젝트가 이 슬롯에 스냅되도록 처리
    public void SnapObject(GameObject tire)
    {
        if (isOccupied) return; // 이미 오브젝트가 있으면 무시

        Rigidbody rb = tire.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 먼저 속도 초기화
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // 그 다음 kinematic 처리
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // 위치 및 회전 맞추기
        tire.transform.position = snapPoint.position;
        tire.transform.rotation = snapPoint.rotation;

        isOccupied = true;
    }

    // 슬롯에서 오브젝트를 해제할 때 호출되는 메서드
    public void ReleaseObject(GameObject tire)
    {
        if (!isOccupied) return;

        Rigidbody rb = tire.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        isOccupied = false;
    }
}



