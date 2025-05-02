using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;   // UI.Text 사용 시
// using TMPro;        // TextMeshPro 사용 시

public class ObjectPicker : MonoBehaviour
{
    private GameObject currentGhostSlot;
    public Transform holdPoint;
    public float pickUpRange = 3f; // Raycast 거리
    private GameObject heldObject;

    private readonly string[] targetTags = {
       "Tire", "Tire2", "Cool", "DC", "Motor", "Gear", "VCU", "Tire3", "Tire4"
    };

    private Collider[] playerColliders;

    public float minDistanceFromPlayer = 0.5f; // 플레이어와 최소 거리

    [Header("UI")]
    public Text nameText;        // (또는 TMP_Text nameText;)
    public float showNameRange = 2f;  // 부품 이름을 보여줄 범위

    void Start()
    {
        // 플레이어 콜라이더들 캐싱
        playerColliders = GetComponentsInChildren<Collider>();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (heldObject == null)
                TryPickUpObject();
            else
                DropObject();
        }

        if (heldObject != null)
        {
            Vector3 targetPosition = holdPoint.position;
            targetPosition = PreventOverlap(targetPosition);
            heldObject.transform.position = targetPosition;
            ShowObjectName(heldObject);
        }
        if (heldObject != null)
        {
            Vector3 targetPosition = holdPoint.position;
            targetPosition = PreventOverlap(targetPosition);
            heldObject.transform.position = targetPosition;

            ShowObjectName(heldObject);
            HighlightGhostSlot(heldObject);
        }
        IgnorePlayerObjectCollision();
    }

    void TryPickUpObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            GameObject target = hit.collider.gameObject;

            foreach (string tag in targetTags)
            {
                if (target.CompareTag(tag))
                {
                    if (IsAlreadySnapped(target)) return;

                    heldObject = target;

                    Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.useGravity = false;
                    }

                    Collider heldCol = heldObject.GetComponent<Collider>();
                    if (heldCol != null)
                    {
                        foreach (Collider playerCol in playerColliders)
                        {
                            Physics.IgnoreCollision(playerCol, heldCol, true);
                        }
                        heldCol.enabled = false;
                    }

                    return;
                }
            }
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            Collider heldCol = heldObject.GetComponent<Collider>();
            if (heldCol != null)
            {
                heldCol.enabled = true;

                foreach (Collider playerCol in playerColliders)
                {
                    Physics.IgnoreCollision(playerCol, heldCol, false);
                }
            }

            heldObject = null;
            nameText.text = ""; // 이름 초기화
        }
        if (currentGhostSlot != null)
        {
            var rend = currentGhostSlot.GetComponent<MeshRenderer>();
            if (rend != null)
            {
                Color c = rend.material.color;
                c.a = 0f;
                rend.material.color = c;
            }
            currentGhostSlot = null;
        }

    }

    Vector3 PreventOverlap(Vector3 targetPosition)
    {
        Vector3 playerPosition = transform.position;
        float distanceToPlayer = Vector3.Distance(targetPosition, playerPosition);

        if (distanceToPlayer < minDistanceFromPlayer)
        {
            Vector3 directionAwayFromPlayer = (targetPosition - playerPosition).normalized;
            targetPosition = playerPosition + directionAwayFromPlayer * minDistanceFromPlayer;
        }

        return targetPosition;
    }

    private void ShowObjectName(GameObject obj)
    {
        if (nameText != null)
        {
            nameText.text = obj.name;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickUpRange);
    }

    private void IgnorePlayerObjectCollision()
    {
        foreach (string tag in targetTags)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objs)
            {
                Collider objCol = obj.GetComponent<Collider>();
                if (objCol == null) continue;

                foreach (Collider playerCol in playerColliders)
                {
                    if (!Physics.GetIgnoreCollision(playerCol, objCol))
                    {
                        Physics.IgnoreCollision(playerCol, objCol, true);
                    }
                }
            }
        }
    }

    private bool IsAlreadySnapped(GameObject target)
    {
        return (target.GetComponent<Tire>()?.IsSnapped ?? false)
            || (target.GetComponent<Tire2>()?.IsSnapped ?? false)
            || (target.GetComponent<Tire3>()?.IsSnapped ?? false)
            || (target.GetComponent<Tire4>()?.IsSnapped ?? false)
            || (target.GetComponent<Motor>()?.IsSnapped ?? false)
            || (target.GetComponent<Gear>()?.IsSnapped ?? false)
            || (target.GetComponent<DC>()?.IsSnapped ?? false)
            || (target.GetComponent<VCU>()?.IsSnapped ?? false)
            || (target.GetComponent<Cool>()?.IsSnapped ?? false);
    }

    private List<GameObject> allGhostSlots = new List<GameObject>();
    private Color transparentGreen = new Color(0f, 1f, 0f, 0.4f);
    private Color transparentRed = new Color(1f, 0f, 0f, 0.4f);
    private Color transparentDefault = new Color(1f, 1f, 1f, 0.2f); // 원래 색상

    void HighlightGhostSlot(GameObject obj)
    {
        // 현재 집은 오브젝트의 슬롯 이름을 가져옴
        if (!(obj.GetComponent<ISlotInfoProvider>() is ISlotInfoProvider infoProvider))
            return;

        string mySlotName = infoProvider.GetTargetSlotName();
        GameObject mySlot = GameObject.Find(mySlotName);
        if (mySlot == null) return;

        // 모든 슬롯을 한 번 수집
        if (allGhostSlots.Count == 0)
            allGhostSlots = GameObject.FindGameObjectsWithTag("GhostSlot").ToList(); // 슬롯 태그 지정 필요

        // 일단 모두 기본색으로 되돌림
        foreach (var slot in allGhostSlots)
        {
            var rend = slot.GetComponent<MeshRenderer>();
            if (rend != null)
                rend.material.color = transparentDefault;
        }

        // 1. 자신의 슬롯은 초록색으로 유지
        var myRenderer = mySlot.GetComponent<MeshRenderer>();
        if (myRenderer != null)
            myRenderer.material.color = transparentGreen;

        // 2. 집은 부품이 다른 슬롯에 가까이 접근한 경우, 해당 슬롯은 빨간색으로 바뀜
        foreach (var slot in allGhostSlots)
        {
            if (slot == mySlot) continue;

            float dist = Vector3.Distance(obj.transform.position, slot.transform.position);
            if (dist < 0.5f) // 임계 거리
            {
                var rend = slot.GetComponent<MeshRenderer>();
                if (rend != null)
                    rend.material.color = transparentRed;
            }
        }
    }


}




















