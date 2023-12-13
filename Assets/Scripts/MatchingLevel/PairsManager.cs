using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PairsManager : MonoBehaviour
{
    private readonly Dictionary<EPuzzleCatagories, string> _puzzelCatDirectory = new Dictionary<EPuzzleCatagories, string>();
    private int _settings;
    private const int SettingsNumber = 2;


    public enum EPairNumber
    {
        NotSet = 0,
        E2Pairs = 2,
        E6Pairs = 6,
        E8Pairs  = 8,
        E10Pairs = 10,
    }

    public enum EPuzzleCatagories
    {
        NotSet,
        Animals,
        Fruits,
        Toys
    }

    public struct Settings
    {
        public EPairNumber PairsNumber;
        public EPuzzleCatagories PuzzelCatagory;
    };

    private Settings _gameSettings;

    public static PairsManager Instance;

    void Awake()
    {
        if (Instance==null)
        {
            DontDestroyOnLoad(target: this);
            Instance = this;
        }
        else
        {
            Destroy(obj: this);
        }
    }
    void Start()
    {
        SetPuzzelCatDirectory();
        _gameSettings = new Settings();
        ResetGameSettings();
    }

    private void SetPuzzelCatDirectory()
    {
        _puzzelCatDirectory.Add(EPuzzleCatagories.Animals, "Animals");
        _puzzelCatDirectory.Add(EPuzzleCatagories.Fruits, "Friuts");
        _puzzelCatDirectory.Add(EPuzzleCatagories.Toys, "Toys");
    }

    public void SetPairNumber(EPairNumber Number)
    {
        if (_gameSettings.PairsNumber == EPairNumber.NotSet)
            _settings++;

        _gameSettings.PairsNumber = Number;
    }

    public void SetPuzzelCatagories(EPuzzleCatagories cat)
    {
        if (_gameSettings.PuzzelCatagory == EPuzzleCatagories.NotSet)
            _settings++;

        _gameSettings.PuzzelCatagory = cat;
    }

    public EPairNumber GetPairNumber()
    {
        return _gameSettings.PairsNumber;
    }

    public EPuzzleCatagories GetPuzzleCatagories()
    {
        return _gameSettings.PuzzelCatagory;
    }

    public void ResetGameSettings()
    {
        _settings = 0;
        _gameSettings.PuzzelCatagory = EPuzzleCatagories.NotSet;
        _gameSettings.PairsNumber = EPairNumber.NotSet;
    }

    public bool AllSettingsReady()
    {
        return _settings == SettingsNumber;
    }

    public string GetMaterialDirectoryName()
    {
        return "Materials/";
    }
    public string GetPuzzelCatagoryTextureDirectoryName()
    {
        if (_puzzelCatDirectory.ContainsKey(_gameSettings.PuzzelCatagory))
        {
            return "Graphics/PuzzelCat/" + _puzzelCatDirectory[_gameSettings.PuzzelCatagory]+"/";
        }
        else
        {
            Debug.LogError("EROR: CONNAT GET DÝRECTORY NAME");
            return "";
        }
    }
}
