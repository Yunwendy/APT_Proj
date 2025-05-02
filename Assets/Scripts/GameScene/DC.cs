using UnityEngine;

public class DC : MonoBehaviour, ISlotInfoProvider
{
    private bool isSnapped = false;
    private Transform snappedSlot;

    public bool IsSnapped => isSnapped;

    public string GetTargetSlotName() => "DC_snap";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DCSlot") && !isSnapped)
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

        // ¾ËÆÄ°ª 0À¸·Î (¿ÏÀüÈ÷ ¼û±è)
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
        if (other.CompareTag("DCSlot") && isSnapped)
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

