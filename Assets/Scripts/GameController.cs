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
        { if (long.TryParse(PlayerPrefs.GetString("coins", "0"), out long _coins)) return _coins; return 0; }
        set
        {
            instance.moneyText.SetText(value.ToString());
            PlayerPrefs.SetString("coins", value.ToString());
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
    public Transform moving;
    public GameObject panel;
    public GameObject inGameUtilites;
    public GameObject difficultyButton;
    public GameObject coinPrefab;
    public static GameController instance;

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

    
    private void OnApplicationQuit() { PlayerPrefs.Save(); }

    private void Awake()
    {
        instance = this;
        GetComponent<Volume>().enabled = true;
        selectedDifficulty = Mathf.Clamp(PlayerPrefs.GetInt("difficulty",0),0, difficulties.Length - 1);
        score.SetText(long.Parse(PlayerPrefs.GetString("score"+selectedDifficulty.ToString().Replace("0",""),"0")).ToString());
        highScore.SetText(long.Parse(PlayerPrefs.GetString("HighScore"+selectedDifficulty.ToString().Replace("0",""),"0")).ToString());
        difficultyButton.transform.GetChild(selectedDifficulty).gameObject.SetActive(true);
        instance.moneyText.SetText(coins.ToString());
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

        foreach (Transform coin in coinsHolder) Destroy(coin.gameObject);
        
        moving.transform.position = movigPosition;
        foreach (Transform child in moving)if(child.name!="Coins") foreach (Transform thing in child) thing.GetComponent<MoveBack>().ResetInfo();
        
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
    void Update()
    {
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.O))
            ScreenCapture.CaptureScreenshot(Application.persistentDataPath+"/"+System.DateTime.Now.ToString("yyyyMMddHHmmssffff")+".png");
        #endif
        if(inGame) if(Input.GetKeyDown(KeyCode.Escape)) if(paused) UnPause(); else Pause();
        if (inGame && !paused)
        {
            if (scoreLong == nextCoinSpawn)
            {
                nextCoinSpawn += Random.Range(10, 15);
                int coinCount = Random.Range(10, 25);
                float newRotation = Random.Range(0, 360);
                for (int i = 0; i < coinCount; i++)
                {
                    GameObject coin = Instantiate(coinPrefab, coinsHolder);
                    coin.transform.localPosition = new Vector3(0, moving.transform.position.x+(15 + (i*2)), 0);
                    coin.transform.Rotate(new Vector3(0,newRotation,0));
                    newRotation += Random.Range(-35, 35);
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
    public bool boughBuyDefault = false;
}