using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class ScoreBoard : MonoBehaviour
{
    public Text[] scoresText_2Pairs;
    public Text[] dateText_2Pairs;

    public Text[] scoresText_6Pairs;
    public Text[] dateText_6Pairs;

    public Text[] scoresText_8Pairs;
    public Text[] dateText_8Pairs;

    public Text[] scoresText_10Pairs;
    public Text[] dateText_10Pairs;

    void Start()
    {
        UpdateScoreBoard();
    }

    public void UpdateScoreBoard()
    {
        Config.UpdateScoreList();

        DisplayPairsScoresData(Config.ScoreTimeList2Pairs, Config.PairNumberList2Pairs, scoresText_2Pairs, dateText_2Pairs);
        DisplayPairsScoresData(Config.ScoreTimeList6Pairs, Config.PairNumberList6Pairs, scoresText_6Pairs, dateText_6Pairs);
        DisplayPairsScoresData(Config.ScoreTimeList8Pairs, Config.PairNumberList8Pairs, scoresText_8Pairs, dateText_8Pairs);
        DisplayPairsScoresData(Config.ScoreTimeList10Pairs, Config.PairNumberList10Pairs, scoresText_10Pairs, dateText_10Pairs);
    }

    private void DisplayPairsScoresData(float[] scoreTimeList, string[] piarNumberList, Text[] scoreText, Text[] dataText)
    {
        for(var index = 0; index < 3; index++)
        {
            if (scoreTimeList[index] > 0)
            {
                var dataTime = Regex.Split(piarNumberList[index], "T");

                var minutes = Mathf.Floor(scoreTimeList[index] / 60);
                float seconds = Mathf.RoundToInt(scoreTimeList[index] % 60);

                scoreText[index].text = minutes.ToString("00") + ":" + seconds.ToString("00");
                dataText[index].text = dataTime[0] + " " + dataTime[1];
            }
            else
            {
                scoreText[index].text = " ";
                dataText[index].text = " ";
            }
        }
    }
}
