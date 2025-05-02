using UnityEngine;
using UnityEngine.UI;  // UI.Text 사용
using UnityEngine.SceneManagement;  // 씬 전환을 위한
using System.Collections;

public class FrameDropper : MonoBehaviour
{
    [Header("Drop Settings")]
    public Transform targetPoint;     // 차 프레임이 내려갈 목표 지점
    public float dropSpeed = 2f;      // 내리는 속도

    [Header("Parts (스냅 상태 체크)")]
    public Tire tire;
    public Tire2 tire2;
    public Tire3 tire3;
    public Tire4 tire4;
    public Motor motor;
    public Gear gear;
    public DC dcdc;
    public VCU vcu;
    public Cool cool;

    [Header("UI")]
    public Text messageText;          // 안내 문구를 띄울 UI Text
    public Image fadeImage;          // 페이드 아웃에 사용할 이미지

    private bool hasShownMessage = false;
    private bool shouldDrop = false;

    void Start()
    {
        // 처음엔 메시지 숨기기
        if (messageText != null) messageText.text = "";

        // 페이드 아웃 이미지 초기화
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0);  // 초기에는 완전 투명
        }
    }

    void Update()
    {
        // 1) 모든 파츠가 스냅되고, 아직 메시지를 안 보여줬으면
        if (!hasShownMessage && AllSnapped())
        {
            ShowReadyMessage();
        }

        // 2) 안내 메시지를 본 상태에서 X키를 누르면 프레임 내리기 시작
        if (hasShownMessage && Input.GetKeyDown(KeyCode.X))
        {
            shouldDrop = true;
            messageText.text = "";  // 메시지 지우기
        }

        // 3) 내리기 동작
        if (shouldDrop)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPoint.position,
                dropSpeed * Time.deltaTime
            );
        }

        // 목표 지점에 도달한 후 씬 전환
        if (shouldDrop && transform.position == targetPoint.position)
        {
            StartCoroutine(FadeOutAndLoad());
        }
    }

    // 모든 파츠가 IsSnapped == true 인지 확인
    private bool AllSnapped()
    {
        return tire != null && tire.IsSnapped
            && tire2 != null && tire2.IsSnapped
            && tire3 != null && tire3.IsSnapped
            && tire4 != null && tire4.IsSnapped
            && motor != null && motor.IsSnapped
            && gear != null && gear.IsSnapped
            && dcdc != null && dcdc.IsSnapped
            && vcu != null && vcu.IsSnapped
            && cool != null && cool.IsSnapped;
    }

    // 안내 메시지 띄우기
    private void ShowReadyMessage()
    {
        hasShownMessage = true;
        if (messageText != null)
        {
            messageText.text = "모든 부품 장착 완료! X키를 눌러 프레임을 내려주세요.";
        }
    }

    // 페이드 아웃 후 씬 전환 코루틴
    private IEnumerator FadeOutAndLoad()
    {
        // 페이드 아웃 시작 (1초 동안 알파값을 1로 변경)
        float fadeDuration = 1f;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            if (fadeImage != null)
            {
                fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, timeElapsed / fadeDuration));
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // 씬 전환
        SceneManager.LoadScene("Example");  // "Example" 씬으로 로드
    }
}









