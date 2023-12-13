using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
public class Config
{
    // Dosya dizini için deðiþkenin platforma göre atanmasý
#if UNITY_EDITOR
    static readonly string Dir = Directory.GetCurrentDirectory();
#elif UNITY_ANDROID
    static readonly string Dir = Application.persistentDataPath;
#else
    static readonly string Dir = Directory.GetCurrentDirectory();
#endif
    static readonly string File = @"\PairMathing.ini"; // Puan kayýtlarýnýn tutulacaðý dosyanýn adý
    static readonly string Path = Dir + File; // Dosyanýn tam yolu

    private const int NumberOfScoreRecords = 4; // Kaydedilecek puan kayýtlarýnýn sayýsý

    // Farklý sayýda eþleþtirme için puan kayýtlarýný ve eþleþtirme sayýlarýný tutacak diziler
    public static float[] ScoreTimeList2Pairs = new float[NumberOfScoreRecords]; 
    public static string[] PairNumberList2Pairs = new string[NumberOfScoreRecords]; 

    public static float[] ScoreTimeList6Pairs = new float[NumberOfScoreRecords];
    public static string[] PairNumberList6Pairs = new string[NumberOfScoreRecords];

    public static float[] ScoreTimeList8Pairs = new float[NumberOfScoreRecords];
    public static string[] PairNumberList8Pairs = new string[NumberOfScoreRecords];

    public static float[] ScoreTimeList10Pairs = new float[NumberOfScoreRecords];
    public static string[] PairNumberList10Pairs = new string[NumberOfScoreRecords];

    private static bool _bestScore=false; // Yeni bir puan kaydýnýn en iyi skor olup olmadýðýný belirlemek için kullanýlacak deðiþken

    public static void CreateScoreFile() //puan dosyalarýný oluþturma
    {
        if (System.IO.File.Exists(Path) == false)
        {
            CreateFile();
        }
        UpdateScoreList();
    }

    public static void UpdateScoreList() //puan kayýtlarýný  güncelleme
    {
        // Ýlgili dizilere puan kayýtlarýný ve eþleþtirme sayýlarýný güncelleme
        var file = new StreamReader(Path);
        UpdateScoreList(file, ScoreTimeList2Pairs, PairNumberList2Pairs);
        UpdateScoreList(file, ScoreTimeList6Pairs, PairNumberList6Pairs);
        UpdateScoreList(file, ScoreTimeList8Pairs, PairNumberList8Pairs);
        UpdateScoreList(file, ScoreTimeList10Pairs, PairNumberList10Pairs);
        file.Close();
    }

    public static void UpdateScoreList(StreamReader file, float[] scoreTimeList,string[] pairNumberList)
    {
        if (file == null) return;

        var line = file.ReadLine();

        // Dosyadan puan kayýtlarýný okuma
        while (line != null && line[0] == '(' )
        {
            line = file.ReadLine();
        }
        for(int i = 1; i < NumberOfScoreRecords; i++)
        {
            var word = line.Split("#");

            if (word[0] == i.ToString())
            {
                string[] substring = Regex.Split(word[1], "D");

                // Puan kaydýný ve eþleþtirme sayýsýný ilgili dizilere atama
                if (float.TryParse(substring[0], out var scoreOnPosition))
                {
                    scoreTimeList[i - 1] = scoreOnPosition;
                    if (scoreTimeList[i - 1] > 0)
                    {
                        var dataTime = Regex.Split(substring[1], "T");
                        pairNumberList[i - 1] = dataTime[0] + "T" + dataTime[1];
                    }
                    else
                    {
                        pairNumberList[i - 1] = " ";
                    }
                }
                else
                {
                    scoreTimeList[i - 1] = 0;
                    pairNumberList[i - 1] = " ";
                }
            }
            line = file.ReadLine();
        }
    }

    public static void PlaceScoreOnBoard(float time) //Puan sýralamasýný belirler
    {
        UpdateScoreList();
        _bestScore = false;

        switch (PairsManager.Instance.GetPairNumber())
        {
            case PairsManager.EPairNumber.E2Pairs:
                PlaceScoreOnBoard(time, ScoreTimeList2Pairs, PairNumberList2Pairs);
                break;
            case PairsManager.EPairNumber.E6Pairs:
                PlaceScoreOnBoard(time, ScoreTimeList6Pairs, PairNumberList6Pairs);
                break;
            case PairsManager.EPairNumber.E8Pairs:
                PlaceScoreOnBoard(time, ScoreTimeList8Pairs, PairNumberList8Pairs);
                break;
            case PairsManager.EPairNumber.E10Pairs:
                PlaceScoreOnBoard(time, ScoreTimeList10Pairs, PairNumberList10Pairs);
                break;
        }

        SaveScoreList();
    }

    private static void PlaceScoreOnBoard(float time, float[] scoreTimeList, string[] pairNumberList)
    {
        var theTime = System.DateTime.Now.ToString("hh:mm");
        var theData = System.DateTime.Now.ToString("MM/dd/yyyy");
        var currentDate = theData + "T" + theTime;

        for(int i =0; i < NumberOfScoreRecords; i++)
        {
            if (scoreTimeList[i] > time || scoreTimeList[i] == 0.0f)
            {
                if (i == 0)
                    _bestScore = true;

                // Puaný ve eþleþtirme sayýsýný sýralamaya ekler
                for (var moveDownFrom=(NumberOfScoreRecords-1); moveDownFrom > i; moveDownFrom--)
                {
                    scoreTimeList[moveDownFrom] = scoreTimeList[moveDownFrom - 1];
                    pairNumberList[moveDownFrom] = pairNumberList[moveDownFrom - 1];
                }
                scoreTimeList[i] = time;
                pairNumberList[i] = currentDate;
                break;
            }
        }
    }

    public static bool IsBestScore()
    {
        return _bestScore;
    }

    public static void CreateFile()
    {
        SaveScoreList();
    }

    public static void SaveScoreList()
    {
        System.IO.File.WriteAllText(Path, string.Empty);

        var writer = new StreamWriter(Path, false);

        // Puan kayýtlarýný dosyaya kaydetme
        writer.WriteLine("(2PAIRS)");
        for(var i = 1; i <= NumberOfScoreRecords; i++)
        {
            var x = ScoreTimeList2Pairs[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + PairNumberList2Pairs[i - 1]);
        }
        writer.WriteLine("(6PAIRS)");
        for (var i = 1; i <= NumberOfScoreRecords; i++)
        {
            var x = ScoreTimeList6Pairs[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + PairNumberList6Pairs[i - 1]);
        }
        writer.WriteLine("(8PAIRS)");
        for (var i = 1; i <= NumberOfScoreRecords; i++)
        {
            var x = ScoreTimeList8Pairs[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + PairNumberList8Pairs[i - 1]);
        }
        writer.WriteLine("(10PAIRS)");
        for (var i = 1; i <= NumberOfScoreRecords; i++)
        {
            var x = ScoreTimeList10Pairs[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + PairNumberList10Pairs[i - 1]);
        }
        writer.Close();
    }
}
