using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuizShuffleList
{
    public static List<E> ShuffleListItems<E>(List<E> inputList)
    {
        List<E> originalList = new List<E>();
        originalList.AddRange(inputList);
        List<E> randomList = new List<E>();

        System.Random r = new System.Random();
        int randomIndex = 0;
        while (originalList.Count > 0)
        {
            randomIndex = r.Next(0, originalList.Count); // Listedeki rastgele bir öğeyi seçin
            randomList.Add(originalList[randomIndex]); // Yeni rastgele listeye ekleyin
            originalList.RemoveAt(randomIndex); // Yinelemeleri önlemek için kaldırın
        }

        return randomList; // Yeni rastgele listeyi döndürün
    }
}
