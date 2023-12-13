using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
public class Config
{
    // Dosya dizini i�in de�i�kenin platforma g�re atanmas�
#if UNITY_EDITOR
    static readonly string Dir = Directory.GetCurrentDirectory();
#elif UNITY_ANDROID
    static readonly string Dir = Application.persistentDataPath;
#else
    static readonly string Dir = Directory.GetCurrentDirectory();
#endif
    static readonly string File = @"\PairMathing.ini"; // Puan kay�tlar�n�n tutulaca�� dosyan�n ad�
    static readonly string Path = Dir + File; // Dosyan�n tam yolu

    private const int NumberOfScoreRecords = 4; // Kaydedilecek puan kay�tlar�n�n say�s�

    // Farkl� say�da e�le�tirme i�in puan kay�tlar�n� ve e�le�tirme say�lar�n� tutacak diziler
    public static float[] ScoreTimeList2Pairs = new float[NumberOfScoreRecords]; 
    public static string[] PairNumberList2Pairs = new string[NumberOfScoreRecords]; 

    public static float[] ScoreTimeList6Pairs = new float[NumberOfScoreRecords];
    public static string[] PairNumberList6Pairs = new string[NumberOfScoreRecords];

    public static float[] ScoreTimeList8Pairs = new float[NumberOfScoreRecords];
    public static string[] PairNumberList8Pairs = new string[NumberOfScoreRecords];

    public static float[] ScoreTimeList10Pairs = new float[NumberOfScoreRecords];
    public static string[] PairNumberList10Pairs = new string[NumberOfScoreRecords];

    private static bool _bestScore=false; // Yeni bir puan kayd�n�n en iyi skor olup olmad���n� belirlemek i�in kullan�lacak de�i�ken

    public static void CreateScoreFile() //puan dosyalar�n� olu�turma
    {
        if (System.IO.File.Exists(Path) == false)
        {
            CreateFile();
        }
        UpdateScoreList();
    }

    public static void UpdateScoreList() //puan kay�tlar�n�  g�ncelleme
    {
        // �lgili dizilere puan kay�tlar�n� ve e�le�tirme say�lar�n� g�ncelleme
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

        // Dosyadan puan kay�tlar�n� okuma
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

                // Puan kayd�n� ve e�le�tirme say�s�n� ilgili dizilere atama
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

    public static void PlaceScoreOnBoard(float time) //Puan s�ralamas�n� belirler
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

                // Puan� ve e�le�tirme say�s�n� s�ralamaya ekler
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

        // Puan kay�tlar�n� dosyaya kaydetme
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
