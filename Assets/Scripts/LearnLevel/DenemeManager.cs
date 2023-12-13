using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LearnData", menuName = "LearnData", order = 1)]
public class DenemeManager : ScriptableObject
{
    public List<LearnData> learns;
}
