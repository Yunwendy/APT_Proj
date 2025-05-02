using System.Collections.Generic;

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
public class QuizData
{
    public List<QuizItem> quizData;
}