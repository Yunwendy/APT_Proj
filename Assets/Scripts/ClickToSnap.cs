using UnityEngine;
using System.Collections;

public class ClickToSnap : MonoBehaviour
{
    public Transform targetAnchor;           // Snap 위치
    public CanvasGroup finishMessageUI;      // 페이드용 메시지 UI
    private Rigidbody rb;
    private bool isSnapped = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        isSnapped = false;

        if (finishMessageUI != null)
            finishMessageUI.alpha = 0f;
    }

    void Update()
    {
        if (!isSnapped && transform.position.y < -5f)
        {
            ResetPosition();
        }
    }

    void OnMouseDown()
    {
        if (isSnapped) return;

        if (targetAnchor != null)
        {
            transform.position = targetAnchor.position;
            transform.rotation = initialRotation;

            if (rb != null)
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            isSnapped = true;
            Debug.Log($"{gameObject.name} 위치만 스냅됨, 회전 유지!");

            // 조립 카운트 증가
            AssemblyCounter.screwCount++;

            if (AssemblyCounter.screwCount >= 4)
            {
                StartCoroutine(ShowMessageOnly());
            }
        }
    }

    void ResetPosition()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    IEnumerator ShowMessageOnly()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            finishMessageUI.alpha = Mathf.Lerp(0f, 1f, t / 1f);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            finishMessageUI.alpha = Mathf.Lerp(1f, 0f, t / 1f);
            yield return null;
        }
    }
}



