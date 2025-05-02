using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 전환을 위한 네임스페이스
using UnityEngine.UI;  // UI.Text 사용
using System.Collections;


public class CameraRotator : MonoBehaviour
{
    public Transform target;           // 카메라가 따라갈 대상 (차)
    public float rotationSpeed = 10f;  // 회전 속도
    public float distance = 10f;       // 카메라와 대상 간의 거리

    public Text completionText;        // "조립 완료" 텍스트 UI
    public float fadeInDuration = 2f;  // 텍스트가 페이드 인 되는 시간

    public Text instructionText;       // "처음으로 돌아가려면 R 키를 눌러주세요" 텍스트 UI
    private CanvasGroup instructionCanvasGroup; // instructionText의 CanvasGroup

    private float currentAngle = 0f;   // 카메라의 현재 각도
    private CanvasGroup canvasGroup;   // UI 텍스트의 CanvasGroup

    void Start()
    {
        // 텍스트의 CanvasGroup을 가져옴
        canvasGroup = completionText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = completionText.gameObject.AddComponent<CanvasGroup>();
        }

        // 처음에는 텍스트가 보이지 않도록 설정
        canvasGroup.alpha = 0;

        // "처음으로 돌아가려면 R 키를 눌러주세요" 텍스트 CanvasGroup 가져오기
        instructionCanvasGroup = instructionText.GetComponent<CanvasGroup>();
        if (instructionCanvasGroup == null)
        {
            instructionCanvasGroup = instructionText.gameObject.AddComponent<CanvasGroup>();
        }

        // 처음에는 텍스트가 보이지 않도록 설정
        instructionCanvasGroup.alpha = 0;

        // 텍스트가 페이드 인 되도록 설정
        StartCoroutine(FadeInText(instructionCanvasGroup, fadeInDuration));
    }

    void Update()
    {
        // 카메라 회전
        currentAngle += rotationSpeed * Time.deltaTime;
        if (currentAngle >= 360f) currentAngle = 0f;

        float xPos = target.position.x + Mathf.Sin(currentAngle * Mathf.Deg2Rad) * distance;
        float zPos = target.position.z + Mathf.Cos(currentAngle * Mathf.Deg2Rad) * distance;
        float yPos = target.position.y + 3f; // 숫자가 클수록 더 위에서 내려다봄


        // 카메라의 새로운 위치 설정
        transform.position = new Vector3(xPos, yPos, zPos);

        // 카메라가 항상 대상(차)을 바라보도록
        transform.LookAt(target);

        // 일정 시간이 지나면 텍스트 페이드 인
        if (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / fadeInDuration;
        }

        // R 키를 눌렀을 때 Start 씬으로 이동
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnStartButtonClicked();  // R키를 누르면 Start 씬으로 이동
        }
    }

    // 페이드 인 효과를 적용하는 코루틴
    private IEnumerator FadeInText(CanvasGroup canvasGroup, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1;
    }

    // Start 씬으로 이동하는 함수
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Start Scene");  // Start 씬 이름을 적어주세요.
    }
}



