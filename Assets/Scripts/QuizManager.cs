using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Drawing;
using System.Collections;


public class QuizManager : MonoBehaviour
{
    [Header("UI 텍스트")]
    public Text textQuestion;
    public Text textA;
    public Text textB;
    public Text textC;
    public Text textD;
    public Text textMsg;
    public Text textScore;

    [Header("선택 버튼")]
    public Button buttonA;
    public Button buttonB;
    public Button buttonC;
    public Button buttonD;

    [Header("컨트롤 버튼")]
    public Button buttonPlay;
    public Button buttonNext;
    public Button buttonReplay;
    public Button buttonExit;

    [Header("정답/오답 이미지")]
    public GameObject imageO_A;
    public GameObject imageO_B;
    public GameObject imageO_C;
    public GameObject imageO_D;
    public GameObject imageX_A;
    public GameObject imageX_B;
    public GameObject imageX_C;
    public GameObject imageX_D;

    [Header("선택지 텍스트")]
    public Text[] answerTexts; // A, B, C, D 순서
    public Material glowMaterial; // 글로우 효과용 머티리얼
    public float glowDuration = 0.4f; // 효과 지속 시간


    [Header("자동차 이동 제어")]
    public MoveImageLoop carMover; // ← 여기서 자동차 스크립트 참조

    [Header("퀴즈 설정")]
    public int maxQuestions = 5;

    private List<QuizItem> quizList;
    // 기존 quizList는 전체 문제를 로드한 후, 출제할 리스트는 별도로 구성
    private List<QuizItem> selectedQuizList;
    private int currentIndex = 0;
    private bool answered = false;

    public bool isPlaying { get; private set; } = false;

    [System.Serializable]
    public class QuizItem
    {
        public string ID;
        public string Question;
        public string A;
        public string B;
        public string C;
        public string D;
        public string Answer;
        public string Msg;
        public int Chapter;
    }

    [System.Serializable]
    public class QuizWrapper
    {
        public List<QuizItem> quizData;
    }

    private MoveImageLoop moveImageLoop;
    private int correctCount = 0; // 맞춘 문제 수
    private int score = 0;        // 총 점수

    string textMsgDefault = "정답 결정 후 A, B, C, D 부분을 클릭하세요";

    void Start()
    {

        moveImageLoop = FindObjectOfType<MoveImageLoop>();

        // 버튼 리스너 연결
        buttonA.onClick.AddListener(() => OnAnswerSelected("A"));
        buttonB.onClick.AddListener(() => OnAnswerSelected("B"));
        buttonC.onClick.AddListener(() => OnAnswerSelected("C"));
        buttonD.onClick.AddListener(() => OnAnswerSelected("D"));
        buttonNext.onClick.AddListener(NextQuestion);
        buttonPlay.onClick.AddListener(StartQuiz);
        buttonReplay.onClick.AddListener(ReplayQuiz);
        buttonExit.onClick.AddListener(ExitQuiz);

        LoadQuizData();

        textQuestion.text = "퀴즈에 도전하려면 '도전' 버튼을 클릭하세요";
        textMsg.text = textMsgDefault;

        EnableAnswerButtons(false);
        SetButtonStates(play: true, next: false, replay: false);
    }

    void LoadQuizData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "quiz.json");

        if (File.Exists(path))
        {
            try
            {
                string jsonString = File.ReadAllText(path);
                var wrapper = JsonConvert.DeserializeObject<QuizWrapper>(jsonString);

                if (wrapper == null || wrapper.quizData == null || wrapper.quizData.Count == 0)
                {
                    Debug.LogError("퀴즈 데이터를 불러올 수 없습니다: 내용이 비어있거나 잘못되었습니다.");
                    textQuestion.text = "퀴즈 데이터를 불러오지 못했습니다.";
                    textMsg.text = "문제가 발생했어요.\n담당자에게 문의해 주세요.";
                    quizList = new List<QuizItem>();
                    selectedQuizList = new List<QuizItem>();
                    return;
                }

                quizList = wrapper.quizData;

                // 문제 섞기
                ShuffleList(quizList);

                // 출제할 문제 수 제한
                selectedQuizList = quizList.GetRange(0, Mathf.Min(maxQuestions, quizList.Count));
            }
            catch (JsonException ex)
            {
                Debug.LogError($"퀴즈 JSON 파싱 오류: {ex.Message}");
                textQuestion.text = "퀴즈 파일에 문제가 있습니다.";
                textMsg.text = "퀴즈 형식이 올바르지 않습니다.\n파일을 확인해 주세요.";
                quizList = new List<QuizItem>();
                selectedQuizList = new List<QuizItem>();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"알 수 없는 오류 발생: {ex.Message}");
                textQuestion.text = "퀴즈 데이터를 불러오는 중 문제가 발생했습니다.";
                textMsg.text = "예기치 못한 오류입니다.\n담당자에게 문의해 주세요.";
                quizList = new List<QuizItem>();
                selectedQuizList = new List<QuizItem>();
            }
        }
        else
        {
            Debug.LogError("퀴즈 파일을 찾을 수 없습니다: " + path);
            textQuestion.text = "퀴즈 파일이 존재하지 않습니다.";
            textMsg.text = "파일을 StreamingAssets 폴더에 넣었는지 확인해 주세요.";
            quizList = new List<QuizItem>();
            selectedQuizList = new List<QuizItem>();
        }
    }


    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }


    void StartQuiz()
    {
        isPlaying = true;
        currentIndex = 0;
        correctCount = 0;
        score = 0;
        UpdateScoreUI(); // 점수 UI 초기화
        DisplayQuestion(currentIndex);
        SetButtonStates(play: false, next: false, replay: false);

        if (moveImageLoop != null)
        {
            moveImageLoop.StartCar();
        }
    }

    void ReplayQuiz()
    {
        isPlaying = true;
        currentIndex = 0;
        correctCount = 0;
        score = 0;

        // 문제 새로 섞기 및 다시 선택
        if (quizList != null && quizList.Count > 0)
        {
            ShuffleList(quizList);
            selectedQuizList = quizList.GetRange(0, Mathf.Min(maxQuestions, quizList.Count));
        }

        UpdateScoreUI(); // 점수 UI 초기화
        DisplayQuestion(currentIndex);
        SetButtonStates(play: false, next: false, replay: false);

        if (carMover != null) carMover.StartCar(); // 자동차도 다시 출발
    }


    void UpdateScoreUI()
    {
        if (textScore != null)
        {
            textScore.text = $"{score}점";
        }
    }


    void ExitQuiz()
    {
        isPlaying = false;

        textQuestion.text = "퀴즈 도전이 종료되었습니다.";
        textA.text = textB.text = textC.text = textD.text = "";
        textMsg.text = "";

        EnableAnswerButtons(false);
        SetButtonStates(play: true, next: false, replay: false);

        if (carMover != null) carMover.StopCar(); // 자동차 멈춤

        // ★ 여기 추가!!
        SceneSelector.CarButtonUnlock = true;

        // 씬 존재 여부 확인 후 로드
        string sceneName = "SelectScene";

        if (IsSceneExists(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' does not exist!");
        }
    }



    // 씬 존재 여부 체크 함수
    bool IsSceneExists(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            Debug.Log("scenePat = " + scenePath);
            Debug.Log("sceneNameInBuild = " + sceneNameInBuild);

            if (sceneNameInBuild == sceneName)
            {
                return true;
            }
        }

        return false;
    }

    void DisplayQuestion(int index)
    {
        if (selectedQuizList != null && index < selectedQuizList.Count)
        {
            var q = selectedQuizList[index];
            textQuestion.text = q.Question;
            textA.text = q.A;
            textB.text = q.B;
            textC.text = q.C;
            textD.text = q.D;
            textMsg.text = textMsgDefault;

            answered = false;
            EnableAnswerButtons(true);
            buttonNext.interactable = false;

            HideAllOX(); // OX 이미지 초기화
        }
        else
        {
            textQuestion.text =
                "퀴즈 도전 완료!" + $"  {score}점입니다.";
            textA.text = textB.text = textC.text = textD.text = "";

            string scoreComment = string.Empty;

            if (score < 60)
            {
                scoreComment = "전기차에 대한 공부가 많이 필요해요";
            }
            else if (score >= 60 && score <= 99)
            {
                scoreComment = "조금 더 전기차를 사랑해 주세요";
            }

            else if (score == 100)
            {
                scoreComment = "당신이 진정 EV 마스터!!!";
            }
            else
            {
                scoreComment = "점수가 이상해요. 담당자에게 문의하세요";
            }

            textMsg.text =
                    //$"{selectedQuizList.Count}개의 퀴즈 문제 중 정답은 {correctCount}개를 성공했어요\n" +
                    $" {scoreComment}";

            isPlaying = false;
            EnableAnswerButtons(false);
            HideAllOX();
            SetButtonStates(play: false, next: false, replay: true);

            if (carMover != null) carMover.StopCar();
        }
    }


    void OnAnswerSelected(string selected)
    {
        if (answered || selectedQuizList == null || currentIndex >= selectedQuizList.Count) return;

        var q = selectedQuizList[currentIndex];
        string correct = q.Answer;

        int selectedIndex = AnswerToIndex(selected);
        if (selectedIndex >= 0 && selectedIndex < answerTexts.Length)
        {
            StartCoroutine(FlashGlow(answerTexts[selectedIndex]));
        }

        ShowOX(selected, correct); // OX 이미지 표시

        if (selected == correct)
        {
            textMsg.text = q.Msg;
            correctCount++;
            score += 20;
            UpdateScoreUI();
        }
        else
        {
            textMsg.text = $"오답! 정답은 {correct}입니다.\n다음 문제에 도전하세요.";
        }

        answered = true;
        EnableAnswerButtons(false);
        buttonNext.interactable = true;

        if (moveImageLoop != null)
        {
            moveImageLoop.StopCar();
        }
    }




    void NextQuestion()
    {
        currentIndex++;
        DisplayQuestion(currentIndex);

        if (carMover != null && isPlaying)
        {
            carMover.StartCar(); // 다음 문제 시작 시 자동차 다시 출발
        }
    }

    void EnableAnswerButtons(bool enabled)
    {
        buttonA.interactable = enabled;
        buttonB.interactable = enabled;
        buttonC.interactable = enabled;
        buttonD.interactable = enabled;
    }

    void SetButtonStates(bool play, bool next, bool replay)
    {
        buttonPlay.interactable = play;
        buttonNext.interactable = next;
        buttonReplay.interactable = replay;
    }

    void ShowOX(string selected, string correct)
    {
        // 먼저 모든 OX 숨기기
        HideAllOX();

        // 정답 이미지 항상 표시
        ShowO(correct);

        // 오답 선택 시 X 표시
        if (selected != correct)
        {
            ShowX(selected);
        }
    }

    void ShowO(string choice)
    {
        switch (choice)
        {
            case "A": imageO_A?.SetActive(true); break;
            case "B": imageO_B?.SetActive(true); break;
            case "C": imageO_C?.SetActive(true); break;
            case "D": imageO_D?.SetActive(true); break;
        }
    }

    void ShowX(string choice)
    {
        switch (choice)
        {
            case "A": imageX_A?.SetActive(true); break;
            case "B": imageX_B?.SetActive(true); break;
            case "C": imageX_C?.SetActive(true); break;
            case "D": imageX_D?.SetActive(true); break;
        }
    }

    void HideAllOX()
    {
        imageO_A?.SetActive(false);
        imageO_B?.SetActive(false);
        imageO_C?.SetActive(false);
        imageO_D?.SetActive(false);
        imageX_A?.SetActive(false);
        imageX_B?.SetActive(false);
        imageX_C?.SetActive(false);
        imageX_D?.SetActive(false);
    }

    IEnumerator FlashGlow(Text target)
    {

        if (target == null || glowMaterial == null) yield break;

        //Material originalMaterial = target.fontMaterial;
        //target.fontMaterial = glowMaterial;
        //yield return new WaitForSeconds(glowDuration);
        //target.fontMaterial = originalMaterial;
    }

    int AnswerToIndex(string answer)
    {
        switch (answer)
        {
            case "A": return 0;
            case "B": return 1;
            case "C": return 2;
            case "D": return 3;
            default: return -1;
        }
    }



}
