using UnityEngine;

public class Tire : MonoBehaviour, ISlotInfoProvider
{
    private bool isSnapped = false;
    private Transform snappedSlot;

    public bool IsSnapped => isSnapped;

    public string GetTargetSlotName() => "tire_snap";

    //[Header("Snap Sound Settings")]
    //public AudioClip snapSound;         // 효과음 클립
    //public AudioSource audioSource;     // 인스펙터에서 연결


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TireSlot") && !isSnapped)
        {
            SnapToSlot(other.transform);
            
        }
    }

    //private bool hasPlayedSnapSound = false;

    private void SnapToSlot(Transform slot)
    {
        //if (isSnapped)
        //{
        //    return;
        //}

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

        var rend = slot.GetComponentInChildren<MeshRenderer>();
        if (rend != null)
        {
            Color c = rend.material.color;
            c.a = 0f;
            rend.material.color = c;
        }


        //// 사운드는 단 한 번만 재생되게
        //if (!hasPlayedSnapSound && gameObject.name == "tire" && snapSound != null && audioSource != null)
        //{
        //    audioSource.PlayOneShot(snapSound);
        //    hasPlayedSnapSound = true;
        //}
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TireSlot") && isSnapped)
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









