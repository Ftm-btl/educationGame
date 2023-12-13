using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizData", menuName = "QuizData", order = 1)]
public class GameQuizDataScriptable : ScriptableObject
{
    public string categoryName;
    public List<Question> questions;
}
