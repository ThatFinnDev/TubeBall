using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static float obstacleTPPosX = 3.15f;
    public static float decoTPPosX = 7;
    public static long coins
    {
        get
        { if (long.TryParse(PlayerPrefs.GetString("coins", "0"), out long _out)) return _out; return 0; }
        set
        {
            if (value < 0) value = 0;
            instance.moneyText.SetText(value.ToString());
            if (DebugMenu.instance != null) DebugMenu.instance.coins.value.text = value.ToString();
            PlayerPrefs.SetString("coins", value.ToString());
        }
    }
    
    public static long totalCoins
    {
        get
        { if (long.TryParse(PlayerPrefs.GetString("totalCoins", "0"), out long _out)) return _out; return 0; }
        set
        {
            if (value < 0) value = 0;
            if (DebugMenu.instance != null) DebugMenu.instance.totalCoins.value.text = value.ToString();
            PlayerPrefs.SetString("totalCoins", value.ToString());
        }
    }
    public static long totalGoldRush
    {
        get
        { if (long.TryParse(PlayerPrefs.GetString("totalGoldRush", "0"), out long _out)) return _out; return 0; }
        set
        {
            if (value < 0) value = 0;
            if (DebugMenu.instance != null) DebugMenu.instance.totalGoldRush.value.text = value.ToString();
            PlayerPrefs.SetString("totalGoldRush", value.ToString());
        }
    }
    public static long scoreLong;
    
    
    public Difficulty[] difficulties;
    public int selectedDifficulty = 0;
    
    public float turnSpeed = 150;
    public bool inGame;
    private bool wasInGame;
    public LayerMask layerMask;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI score;
    public TextMeshProUGUI highScore;
    public TextMeshProUGUI inGameScore;
    public GameObject pauseMenu;
    public GameObject pipe;
    public Transform player;
    public Camera camera;
    public Transform moving;
    public GameObject panel;
    public GameObject inGameUtilites;
    public GameObject difficultyButton;
    public static GameController instance;
    public GameObject goldRushCollectorPrefab;
    public Transform obstacles;
    public Transform coinsHolder;
    public void SetDifficultyIndex(int index)
    {       
        selectedDifficulty = index;
        PlayerPrefs.SetInt("difficulty",index);
        score.SetText(long.Parse(PlayerPrefs.GetString("score"+selectedDifficulty.ToString().Replace("0",""),"0")).ToString());
        highScore.SetText(long.Parse(PlayerPrefs.GetString("HighScore"+selectedDifficulty.ToString().Replace("0",""),"0")).ToString());
    }
    public static Difficulty difficulty => instance.difficulties[instance.selectedDifficulty];
    Vector3 movigPosition { get { if (inGame) return new Vector3(moving.transform.position.x,0,0) + (new Vector3(difficulty.speed,0,0) * Time.deltaTime); return Vector3.zero; } }

    
    private void OnApplicationQuit() { if(PreferenceManager.shouldSave) PlayerPrefs.Save(); }

    private bool firstTime = true;
    private void Awake()
    {
        instance = this;
        if (PlayerPrefs.GetInt("firstTimeLaunchCompleted", 0) == 0)
        {
            PlayerPrefs.SetInt("firstTimeLaunchCompleted", 1);
            if (PlayerPrefs.HasKey("coins"))
                if(!PlayerPrefs.HasKey("totalCoins"))
                {
                    totalCoins = coins;
                    totalCoins += GetComponent<CosmeticsManager>().GetSpendCoins();
                }
        }
        PlayerPrefs.Save();
        
        if(firstTime) GetComponent<Volume>().enabled = true;
        firstTime = false;
        selectedDifficulty = Mathf.Clamp(PlayerPrefs.GetInt("difficulty",0),0, difficulties.Length - 1);
        score.SetText(long.Parse(PlayerPrefs.GetString("score"+selectedDifficulty.ToString().Replace("0",""),"0")).ToString());
        highScore.SetText(long.Parse(PlayerPrefs.GetString("HighScore"+selectedDifficulty.ToString().Replace("0",""),"0")).ToString());
        difficultyButton.transform.GetChild(selectedDifficulty).gameObject.SetActive(true);
        instance.moneyText.SetText(coins.ToString());
        
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToPortrait = true;
        if(Debug.isDebugBuild)
            if (DebugMenu.instance == null)
            {
                GameObject debugMenu = Resources.Load<GameObject>("DebugMenu");
                GameObject obj = GameObject.Instantiate(debugMenu, Vector3.zero, Quaternion.identity, null);
                GameObject.DontDestroyOnLoad(obj);
            }
    }

    public void Play()
    {
        panel.SetActive(false);
        inGameUtilites.SetActive(true);
        pauseMenu.SetActive(false);
        nextCoinSpawn = difficulty.firstCoinSpawn;
        inGame = true;
        foreach (Transform obstacle in obstacles)
        {
            obstacle.transform.localRotation = Quaternion.Euler(0, Random.Range(difficulty.minRotation,difficulty.maxRotation), 0);
            obstacle.transform.localPosition += new Vector3(0,0, ((System.Convert.ToBoolean(Random.Range(0, 2))?-1:1)*Random.Range(difficulty.minOffset,difficulty.maxOffset)));
        }
    }

    public void Restart()
    {
        UnPause();
        xRot = 0;
        pipe.transform.rotation = Quaternion.Euler(0,0,90);
        scoreLong = 0;
        Awake();
        panel.SetActive(true);
        inGameUtilites.SetActive(false);
        pauseMenu.SetActive(false);
        inGame = false;

        foreach (Transform coin in coinsHolder) if(coin.CompareTag("Coin")) CoinPooler.instance.AddToPool(coin.gameObject);
        foreach (Transform coin in coinsHolder) Destroy(coin.gameObject);
        
        moving.transform.position = movigPosition;
        foreach (Transform child in moving)if(child.name!="Coins") foreach (Transform thing in child) thing.GetComponent<MoveBack>().ResetInfo();
        
    }

    [SerializeField] private Transform entries;
    private void Start()
    {
        foreach (Transform child in entries)
        {
            if (child.GetComponent<CheckEntry>() != null)
                child.GetComponent<CheckEntry>().Start();
            else if (child.GetComponent<SliderEntry>() != null)
                child.GetComponent<SliderEntry>().Start();
        }
    }

    private float xRot = 0;

    private bool paused = false;
    public long nextCoinSpawn = 0;

    public void Pause()
    {
        if (!inGame) return;
        paused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        inGameUtilites.SetActive(false);
    }

    public void UnPause()
    {
        paused = false;
        Time.timeScale = 1f;
        if (!inGame) return;
        pauseMenu.SetActive(false);
        inGameUtilites.SetActive(true);
        
    }

    public bool goldRush = false;

    private bool autoStopGoldRush = false;
    private float stopGoldRushAt = 0f;
    public void StartGoldRush(float howLong)
    {
        goldRush = true;
        autoStopGoldRush = true;
        stopGoldRushAt = scoreLong + howLong;
        nextCoinSpawn = scoreLong += 2;
        totalGoldRush++;
        foreach (Transform obstacle in obstacles)
        {
            obstacle.GetComponent<BoxCollider>().enabled = false;
            obstacle.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    ScreenOrientation lastOrientation;
    void Update()
    {
        if(lastOrientation!=Screen.orientation)
        {
            lastOrientation = Screen.orientation;
            PreferenceManager.OnOrientationChange(lastOrientation);
        }
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.O))
            ScreenCapture.CaptureScreenshot(Application.persistentDataPath+"/"+System.DateTime.Now.ToString("yyyyMMddHHmmssffff")+".png");
        if (Input.GetKeyDown(KeyCode.P))
            StartGoldRush(20f);
        #endif
        if(inGame) if(Input.GetKeyDown(KeyCode.Escape)) if(paused) UnPause(); else Pause();
        if (inGame && !paused)
        {
            if (autoStopGoldRush)
            {
                if (scoreLong >= stopGoldRushAt)
                {
                    goldRush = false;
                    autoStopGoldRush = false;
                    nextCoinSpawn=scoreLong+Random.Range(10, 15);
                }
            }
            if (scoreLong >= nextCoinSpawn)
            {
                if (goldRush) nextCoinSpawn += 2;
                else nextCoinSpawn += Random.Range(10, 15);
                int coinCount = Random.Range(10, 25);
                float newRotation = Random.Range(0, 360);
                int last=0;
                for (int i = 0; i < coinCount; i++)
                {
                    GameObject coin = CoinPooler.instance.RemoveFromPool(coinsHolder);
                    coin.transform.SetParent(coinsHolder);
                    coin.transform.localPosition = new Vector3(0, moving.transform.position.x+(15 + (i*2)), 0);
                    coin.transform.Rotate(new Vector3(0,newRotation,0));
                    newRotation += Random.Range(-35, 35);
                    last = i;
                }

                if (!goldRush)
                    if(Random.Range(0, 7)==1)
                    {
                        last += 1; 
                        GameObject goldRushCollector = Instantiate(goldRushCollectorPrefab, coinsHolder); 
                        goldRushCollector.transform.localPosition = new Vector3(0, moving.transform.position.x+(15 + (last*2)), 0);
                        goldRushCollector.transform.Rotate(new Vector3(0,newRotation,0));
                    }
            }
            if (Input.GetKey(KeyCode.D))
            {
                xRot += turnSpeed * Time.deltaTime;
                pipe.transform.rotation = Quaternion.Euler(xRot,0,90);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                xRot -= turnSpeed * Time.deltaTime;
                pipe.transform.rotation = Quaternion.Euler(xRot,0,90);
            }
            else if (Input.touchCount > 0)
            {
                if (Screen.width / 2 < Input.GetTouch(0).position.x) xRot += turnSpeed * Time.deltaTime;
                else xRot -= turnSpeed * Time.deltaTime;
                pipe.transform.rotation = Quaternion.Euler(xRot,0,90);
            }
            if (Physics.CheckSphere(player.position, 0.625f,layerMask))
            {
                if (long.Parse(PlayerPrefs.GetString("HighScore"+selectedDifficulty.ToString().Replace("0",""), "0")) < scoreLong)
                {
                    PlayerPrefs.SetString("HighScore"+selectedDifficulty.ToString().Replace("0",""), scoreLong.ToString());
                }

                Restart();
                return;
            }
            
            inGameScore.SetText(scoreLong.ToString());
            player.Rotate(new Vector3(0,0,difficulty.speed*20)*Time.deltaTime);
        }
        moving.transform.position = movigPosition;
    }


}
[System.Serializable]
public class Difficulty
{
    public string name = "";
    public float speed = 7f;
    public float minOffset = 0f;
    public float maxOffset = 0f;
    public float minRotation = 0f;
    public float maxRotation = 360f;
    public long firstCoinSpawn = 10;
}

[System.Serializable]
public class BuyableMaterial
{
    public Material material;
    public long cost = 1000;
    public bool notBuyable = false;
    public bool boughBuyDefault = false;
}