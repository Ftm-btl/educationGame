using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureManager : MonoBehaviour
{

    public Picture PicturePrefab;
    public Transform PicSpawnPosition;
    public Vector2 StartPosition = new Vector2(-2f, 3f);

    [Space] 
    [Header("End Game Screen")]
    public GameObject EndGamePanel;
    public GameObject NewBestScoreText;
    public GameObject YourScoreText;
    public GameObject EndTimeText;
    
    public enum GameState 
    {
        NoAction,
        MovingOnPositions,
        DeletingPuzzels,
        FlipBack,
        Checking,
        GameEnd
    };

    public enum PuzzelState
    {
        PuzzelRotating,
        CanRotate
    };

    public enum RevealedState
    {
        NoRevealed,
        OneRevealed,
        TwoRevealed
    };

    [HideInInspector]
    public GameState CurrentGameState;
    [HideInInspector]
    public PuzzelState CurrentPuzzelState;
    [HideInInspector]
    public RevealedState PuzzelRevealedNumber;

    [HideInInspector]
    public List<Picture> PictureList;

    private Vector2 _offset = new Vector2(1.2f, 1.5f);
    private Vector2 _offsetFor6Pairs = new Vector2(1.2f, 1.2f);
    private Vector2 _offsetFor8Pairs = new Vector2(1.2f, 1.2f);
    private Vector2 _offsetFor10Pairs = new Vector2(1.2f, 1.2f);
    private Vector3 _newScaleDown = new Vector3(0.9f, 0.9f,0.001f);

    private List<Material> _materialList = new List<Material>();
    private List<string> _texturePathList = new List<string>();
    private Material _firstMaterial;
    private string _firstTexturePath;

    private int _firstRevealedPic;
    private int _secondRevealedPic;
    private int _revealedPicNumber=0;
    private int _picToDestroy1;
    private int _picToDestroy2;

    private bool _corutineStarted = false;

    private int _pairNumbers;
    private int _removedPairs;
    private Timer _gameTimer;

    void Start()
    {
        CurrentGameState = GameState.NoAction;
        CurrentPuzzelState = PuzzelState.CanRotate;
        PuzzelRevealedNumber = RevealedState.NoRevealed;
        _revealedPicNumber = 0;
        _firstRevealedPic = -1;
        _secondRevealedPic = -1;

        _removedPairs = 0;
        _pairNumbers = (int)PairsManager.Instance.GetPairNumber();

        _gameTimer = GameObject.Find("Main Camera").GetComponent<Timer>();


        LoadMaterials();

        if (PairsManager.Instance.GetPairNumber() == PairsManager.EPairNumber.E2Pairs)
        {
            CurrentGameState = GameState.MovingOnPositions;
            SpawnPictureMesh(2, 2, StartPosition, _offset, false);
            MovePicture(2, 2, StartPosition, _offset);
        }
        else if (PairsManager.Instance.GetPairNumber() == PairsManager.EPairNumber.E6Pairs)
        {
            CurrentGameState = GameState.MovingOnPositions;
            SpawnPictureMesh(3, 4, StartPosition, _offset, false);
            MovePicture(3, 4, StartPosition, _offsetFor6Pairs);
        }
        else if (PairsManager.Instance.GetPairNumber() == PairsManager.EPairNumber.E8Pairs)
        {
            CurrentGameState = GameState.MovingOnPositions;
            SpawnPictureMesh(4, 4, StartPosition, _offset, false);
            MovePicture(4, 4, StartPosition, _offsetFor8Pairs);
        }
        else if (PairsManager.Instance.GetPairNumber() == PairsManager.EPairNumber.E10Pairs)
        {
            CurrentGameState = GameState.MovingOnPositions;
            SpawnPictureMesh(4, 5, StartPosition, _offset, true);
            MovePicture(4, 5, StartPosition, _offsetFor10Pairs);
        }

    }

    public void CheckPicture() //kullanýcnýn resimleri kontrol etmek için fonk.
    {
        CurrentGameState = GameState.Checking;
        _revealedPicNumber = 0;

        for(int id=0; id < PictureList.Count; id++)
        {
            if(PictureList[id].Revealed && _revealedPicNumber < 2)
            {
                if (_revealedPicNumber == 0)
                {
                    _firstRevealedPic = id;
                    _revealedPicNumber++;
                }
                else if (_revealedPicNumber == 1)
                {
                    _secondRevealedPic = id;
                    _revealedPicNumber++;
                }
            }
        }

        if (_revealedPicNumber == 2)
        {
            if(PictureList[_firstRevealedPic].GetIndex()==PictureList[_secondRevealedPic].GetIndex()&&_firstRevealedPic != _secondRevealedPic)
            {
                CurrentGameState = GameState.DeletingPuzzels;
                _picToDestroy1 = _firstRevealedPic;
                _picToDestroy2 = _secondRevealedPic;
            }
            else
            {
                CurrentGameState = GameState.FlipBack;
            }            
        }

        CurrentPuzzelState = PictureManager.PuzzelState.CanRotate;

        if (CurrentGameState == GameState.Checking)
        {
            CurrentGameState = GameState.NoAction;
        }
    }

    private void DestroyPicture() //eþleþen resimleri yok eder
    {
        PuzzelRevealedNumber = RevealedState.NoRevealed;
        PictureList[_picToDestroy1].Deactivate();
        PictureList[_secondRevealedPic].Deactivate();
        _revealedPicNumber = 0;
        _removedPairs++;
        CurrentGameState = GameState.NoAction;
        CurrentPuzzelState = PuzzelState.CanRotate;
    }

    private IEnumerator FlipBack() //eþleþmeyenleri kapatýr
    {
        _corutineStarted = true;

        yield return new WaitForSeconds(0.5f);


        PictureList[_firstRevealedPic].FlipBack();
        PictureList[_secondRevealedPic].FlipBack();

        PictureList[_firstRevealedPic].Revealed=false;
        PictureList[_secondRevealedPic].Revealed=false;

        PuzzelRevealedNumber = RevealedState.NoRevealed;
        CurrentGameState = GameState.NoAction;

        _corutineStarted = false;
    }

    private void LoadMaterials()
    {
        var materialFilePath = PairsManager.Instance.GetMaterialDirectoryName();
        var textureFilePath = PairsManager.Instance.GetPuzzelCatagoryTextureDirectoryName();
        var pairNumber = (int)PairsManager.Instance.GetPairNumber();
        const string matBaseName = "Pic";
        var firstMaterialName = "Back";

        for (var index = 1; index <= pairNumber; index++)
        {
            var currentFilePath = materialFilePath + matBaseName + index;
            Material mat = Resources.Load(currentFilePath, typeof(Material)) as Material;
            _materialList.Add(mat);

            var currentTextureFilePath = textureFilePath + matBaseName + index;
            _texturePathList.Add(currentTextureFilePath);
        }

        _firstTexturePath = textureFilePath + firstMaterialName;
        _firstMaterial = Resources.Load(materialFilePath + firstMaterialName, typeof(Material)) as Material;

    }
    void Update()
    {
        if (CurrentGameState==GameState.DeletingPuzzels)
        {
            if (CurrentPuzzelState==PuzzelState.CanRotate)
            {
                DestroyPicture();
                CheckEndGame();
            }
        }
        if (CurrentGameState == GameState.FlipBack)
        {
            if (CurrentPuzzelState == PuzzelState.CanRotate && _corutineStarted == false)
            {
                StartCoroutine(FlipBack());
            }
        }
        if (CurrentGameState==GameState.GameEnd)
        {
            if(PictureList[_firstRevealedPic].gameObject.activeSelf==false&&
                PictureList[_secondRevealedPic].gameObject.activeSelf==false&&
               EndGamePanel.activeSelf == false)
            {
                ShowEndGameInformation();
            }
        }
    }

    private bool CheckEndGame()
    {
        if (_removedPairs == _pairNumbers && CurrentGameState != GameState.GameEnd)
        {
            CurrentGameState = GameState.GameEnd;
            _gameTimer.StopTimer();
            Config.PlaceScoreOnBoard(_gameTimer.GetCurrentTime());
        }
        return (CurrentGameState == GameState.GameEnd);
    }

    private void ShowEndGameInformation()
    {
        EndGamePanel.SetActive(true);

        if (Config.IsBestScore())
        {
            NewBestScoreText.SetActive(true);
            YourScoreText.SetActive(false);
        }
        else
        {
            NewBestScoreText.SetActive(false);
            YourScoreText.SetActive(true);
        }

        var timer = _gameTimer.GetCurrentTime();
        var minuts = Mathf.Floor(timer / 60);
        var seconds = Mathf.RoundToInt(timer % 60);
        var newText = minuts.ToString("00") + ":" + seconds.ToString("00");
        EndTimeText.GetComponent<Text>().text = newText;
    }

    private void SpawnPictureMesh(int rows, int columns, Vector2 Pos, Vector2 offset, bool scaleDown) //resimleri yaratýr ve yerleþtirir.
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var tempPicture = (Picture)Instantiate(PicturePrefab, PicSpawnPosition.position, PicturePrefab.transform.rotation);

                if (scaleDown)
                {
                    tempPicture.transform.localScale = _newScaleDown;
                }

                tempPicture.name = tempPicture.name + 'c' + col + 'r' + row;
                PictureList.Add(tempPicture);
            }
        }
        ApplyTextures();
    }

    private void ApplyTextures() //resimlere rastgele malzemeleri ve dokularý uygular.
    {
        var rndMatIndex = Random.Range(0, _materialList.Count);
        var AppliedTimes = new int[_materialList.Count];
        for (int i = 0; i < _materialList.Count; i++)
        {
            AppliedTimes[i] = 0;
        }
        foreach (var o in PictureList)
        {
            var randPrevious = rndMatIndex;
            var counter = 0;
            var forceMat = false;

            while (AppliedTimes[rndMatIndex] >= 2 || ((randPrevious == rndMatIndex) && !forceMat))
            {
                rndMatIndex = Random.Range(0, _materialList.Count);
                counter++;
                if (counter > 100)
                {
                    for (var j = 0; j < _materialList.Count; j++)
                    {
                        if (AppliedTimes[j] < 2)
                        {
                            forceMat = true;
                        }
                        if (forceMat == false)
                            return;
                    }
                }
            }
            o.SetFirstMaterial(_firstMaterial, _firstTexturePath);
            o.ApplyFirstMaterial();
            o.SetSecondMaterial(_materialList[rndMatIndex], _texturePathList[rndMatIndex]);
            o.SetIndex(rndMatIndex);
            o.Revealed = false;
            AppliedTimes[rndMatIndex] += 1;
            forceMat = false;
        }
    }

    private void MovePicture(int rows, int columns, Vector2 pos, Vector2 offset) //resimleri hedef konumlara hareket ettirir.
    {
        var index = 0;
        for (var col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                    var targetPosition = new Vector3((pos.x + (offset.x * row)), (pos.y - (offset.y * col)), 0.0f);
                    StartCoroutine(MoveToPosition(targetPosition, PictureList[index]));
                    index++;
            }
        }
    }
    private IEnumerator MoveToPosition(Vector3 target, Picture obj)
    {
        var randomDis = 7;
        while (obj.transform.position != target)
        {
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomDis * Time.deltaTime);
                yield return 0;
        }
    } 
}
