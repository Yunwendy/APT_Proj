using UnityEngine;

public interface ISlotInfoProvider
{
    string GetTargetSlotName(); // 예: "tire_snap"
}

public class Tire3 : MonoBehaviour, ISlotInfoProvider
{
    private bool isSnapped = false;
    private Transform snappedSlot;

    public bool IsSnapped => isSnapped;

    public string GetTargetSlotName() => "tire_snap3";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TireSlot3") && !isSnapped)
        {
            SnapToSlot(other.transform);
        }
    }

    private void SnapToSlot(Transform slot)
    {
        transform.position = slot.position;
        isSnapped = true;
        snappedSlot = slot;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // 알파값 0으로 (완전히 숨김)
        var rend = slot.GetComponentInChildren<MeshRenderer>();
        if (rend != null)
        {
            Color c = rend.material.color;
            c.a = 0f;
            rend.material.color = c;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TireSlot3") && isSnapped)
        {
            isSnapped = false;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = false;
        }
    }

    private void Update()
    {
        PreventSinkingBelowGround();
    }

    private void PreventSinkingBelowGround()
    {
        float groundY = 0f;

        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null)
        {
            float bottomY = meshRenderer.bounds.min.y;

            if (bottomY < groundY)
            {
                float offset = groundY - bottomY;
                transform.position += new Vector3(0f, offset, 0f);

                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                }
            }
        }
    }
}



