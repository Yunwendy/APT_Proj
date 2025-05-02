using UnityEngine;

public class MoveImageLoop : MonoBehaviour
{
    public RectTransform imageDecoration; // 자동차 이미지
    public RectTransform imageChoices;    // 퀴즈 선택 영역
    public float speed = 100f;

    public GameObject carTrailPrefab; // 프리팹으로 변경!
    private ParticleSystem carTrailInstance;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool isMoving = false;

    void Start()
    {
        SetStartAndEndPositions();

        // 파티클 인스턴스 생성 및 자식으로 설정
        if (carTrailPrefab != null)
        {
            GameObject trailObj = Instantiate(carTrailPrefab, imageDecoration);
            trailObj.transform.localPosition = new Vector3(-imageDecoration.rect.width / 2, 0f, 0f);
            carTrailInstance = trailObj.GetComponent<ParticleSystem>();
            carTrailInstance.Stop();
        }
    }

    void SetStartAndEndPositions()
    {
        float imageWidth = imageDecoration.rect.width;
        float choicesWidth = imageChoices.rect.width;
        float axisYpos = -284.0f;

        startPos = new Vector3(choicesWidth / 2 - imageWidth / 2, axisYpos, 0f);
        endPos = new Vector3(-choicesWidth / 2 - imageWidth, axisYpos, 0f);
        imageDecoration.localPosition = startPos;
    }

    void Update()
    {
        if (!isMoving) return;

        imageDecoration.localPosition += Vector3.left * speed * Time.deltaTime;

        if (imageDecoration.localPosition.x <= endPos.x)
        {
            imageDecoration.localPosition = startPos;
        }
    }

    public void StartCar()
    {
        isMoving = true;
        imageDecoration.localPosition = startPos;

        if (carTrailInstance != null)
            carTrailInstance.Play();
    }

    public void StopCar()
    {
        isMoving = false;

        if (carTrailInstance != null)
            carTrailInstance.Stop();
    }
}
