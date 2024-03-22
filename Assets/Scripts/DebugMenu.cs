using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.UI;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    public static DebugMenu instance;
    [Space]
    [Header("Stats")]
    public GameObject typeButton;
    public DecrecmentIncrementValueEntrySuper coins;
    public DecrecmentIncrementValueEntrySuper totalCoins;
    public DecrecmentIncrementValueEntrySuper totalGoldRush;
    [Space]
    [Header("Info")]
    public DebugTwoValueInfo easyScore;
    public DebugTwoValueInfo normalScore;
    public DebugTwoValueInfo hardScore;
    public DebugTwoValueInfo lolScore;
    [Space]
    [Header("Cheats")]
    public DebugToggle freeShopping;
    public DebugButton correctTotalCoins;
    public DebugButton exportSave;
    public DebugButton loadSave;

    void PanelUpdate()
    {
        easyScore.setValues(long.Parse(PlayerPrefs.GetString("score","0")),long.Parse(PlayerPrefs.GetString("HighScore","0")));
        normalScore.setValues(long.Parse(PlayerPrefs.GetString("score1","0")),long.Parse(PlayerPrefs.GetString("HighScore1","0")));
        hardScore.setValues(long.Parse(PlayerPrefs.GetString("score2","0")),long.Parse(PlayerPrefs.GetString("HighScore2","0")));
        lolScore.setValues(long.Parse(PlayerPrefs.GetString("score3","0")),long.Parse(PlayerPrefs.GetString("HighScore3","0")));
        /*
        if (Player.instance != null)
            playerPosition.setValues(Player.instance.transform.position);
        else
            playerPosition.setValues(Vector3.zero);*/
    }
    void InitializeDebugMenu()
    {
        easyScore.title.text = "Easy Score";
        normalScore.title.text = "Normal Score";
        hardScore.title.text = "Hard Score";
        lolScore.title.text = "lol Score";
        coins.setFunctionality("Coins", delegate { GameController.coins++; }, delegate { GameController.coins--;},delegate { GameController.coins+=100; }, delegate { GameController.coins-=100; }, GameController.coins.ToString());
        totalCoins.setFunctionality("Total Coins", delegate { GameController.totalCoins++; }, delegate { GameController.totalCoins--; },delegate { GameController.totalCoins+=100; }, delegate { GameController.totalCoins-=100; },GameController.totalCoins.ToString());
        totalGoldRush.setFunctionality("Total GoldRush", delegate { GameController.totalGoldRush++; }, delegate { GameController.totalGoldRush--; },delegate { GameController.totalGoldRush+=100; }, delegate { GameController.totalGoldRush-=100; },GameController.totalGoldRush.ToString());
        
        freeShopping.setFunctionality("Free Shopping", false, delegate { PreferenceManager.freeShopping = freeShopping.isOn; });
        exportSave.setFunctionality("Export Save", delegate {PreferenceManager.ExportSaveData();});
        loadSave.setFunctionality("Load Save & Quit", delegate {PreferenceManager.LoadSaveData();});
        correctTotalCoins.setFunctionality("Correct Total Coins", delegate { PreferenceManager.CorrectTotalCoins();});
        /*
        playerPosition.title.text = "PlayerPos";
        coins.setFunctionality("Coins", delegate { PlayerInfo.coins++; }, delegate { PlayerInfo.coins--; });
        metals.setFunctionality("Metals", delegate { PlayerInfo.metals++; }, delegate { PlayerInfo.metals--; });
        gems.setFunctionality("Gems", delegate { PlayerInfo.gems++; }, delegate { PlayerInfo.gems--; });
        playerGravity.setFunctionality("Use Player Gravity", true, delegate { usePlayerGravity = playerGravity.isOn; });
        freezePlayerRot.setFunctionality("Freeze Playerrotation", false, delegate { freezePlayerRotX = freezePlayerRot.isOn1; }, false, delegate { freezePlayerRotY = freezePlayerRot.isOn2; });
        PlayerInfo.coins += 0;
        PlayerInfo.metals += 0;
        PlayerInfo.gems += 0;*/
    }






    //Functionality DONT CHANGE
    public void NextMenu()
    {
        if (transform.childCount < 3)
            return;
        GameObject[] menus = new GameObject[transform.childCount - 1];
        int activeMenu = 0;
        int wasTypeButton = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!child.name.Contains("TypeButton(Clone)"))
            {
                menus[i + wasTypeButton] = child.gameObject;
                if (child.gameObject.activeSelf)
                    activeMenu = i + wasTypeButton;
            }
            else
                wasTypeButton++;
        }

        int newActiveMenu = activeMenu + 1;
        if (newActiveMenu >= menus.Length)
            newActiveMenu = 0;
        menus[activeMenu].SetActive(false);
        menus[newActiveMenu].SetActive(true);

    }

    public void PreviousMenu()
    {
        if (transform.childCount < 3)
            return;
        GameObject[] menus = new GameObject[transform.childCount - 1];
        int activeMenu = 0;
        int wasTypeButton = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!child.name.Contains("TypeButton(Clone)"))
            {
                menus[i + wasTypeButton] = child.gameObject;
                if (child.gameObject.activeSelf)
                    activeMenu = i + wasTypeButton;
            }
            else
                wasTypeButton++;
        }

        int newActiveMenu = activeMenu - 1;
        if (newActiveMenu == -1)
            newActiveMenu = menus.Length - 1;
        menus[activeMenu].SetActive(false);
        menus[newActiveMenu].SetActive(true);
    }

    private void Awake()
    {

        for (int i = 0; i < transform.childCount; i++) { transform.GetChild(i).gameObject.SetActive(false); }
        DebugManager.instance.enableRuntimeUI = true;
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        InitializeDebugMenu();
    }

    private GameObject typeButtonInstance;

    public void TypeButton()
    {
        bool needToChangeBack = false;
        for (int i = 0; i < transform.childCount; i++)
        {

            if (transform.GetChild(i).gameObject.activeSelf &&
                !transform.GetChild(i).name.Contains("TypeButton(Clone)"))
            {
                needToChangeBack = true;
                //transform.GetChild(i).gameObject.SetActive(false);
                break;
            }
        }

        if (!needToChangeBack)
        {
            open = true;
            transform.GetChild(0).gameObject.SetActive(true);
            if (FindObjectOfType<DebugUIHandlerCanvas>() != null)
                Destroy(FindObjectOfType<DebugUIHandlerCanvas>().gameObject);
        }
        else
        {
            open = false;
            DebugManager.instance.displayRuntimeUI = true;
        }
    }

    private bool open = false;

    public void Close()
    {
        DebugManager.instance.displayRuntimeUI = false;
        open = false;
        Destroy(typeButtonInstance);
    }

    /*DEACTIVATED CUZ IT ALLOWS CHEATS IN NON DEVELOPMENT BUILD
private float tab = 0;
private bool tapping = false;
private int lastTouchCount = 0;
*/
    private void LateUpdate()
    {
        DebugUIHandlerCanvas debugUIHandlerCanvas = FindObjectOfType<DebugUIHandlerCanvas>();

        if (debugUIHandlerCanvas != null)
        {
            debugUIHandlerCanvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            debugUIHandlerCanvas.GetComponent<Canvas>().sortingOrder = 30000;
            foreach (Transform child in debugUIHandlerCanvas.transform)
            {
                child.GetComponent<RectTransform>().sizeDelta = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
                child.GetComponent<RectTransform>().transform.position = transform.GetChild(0).GetComponent<RectTransform>().position;
            }

            for (int i = 0; i < transform.childCount; i++)
            {

                if (transform.GetChild(i).gameObject.activeSelf &&
                    !transform.GetChild(i).name.Contains("TypeButton(Clone)"))
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                    open = false;
                    break;
                }
            }
            if (typeButtonInstance == null)
            {
                typeButtonInstance = Instantiate(typeButton, transform);
                typeButtonInstance.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Button>()
                    .onClick.AddListener(TypeButton);
            }
        }
        else if (!open)
        {
            if (debugUIHandlerCanvas == null)
            {
                Destroy(typeButtonInstance);
            }
        }
    }
    private void Update()
    {
        /*
        DEACTIVATED CUZ IT ALLOWS CHEATS IN NON DEVELOPMENT BUILD
        if (!Debug.isDebugBuild && Flags.debugMode)
        {
            if (Input.touchCount == 3&&lastTouchCount!=3)
            {
                if (!tapping)
                {
                    tapping = true;
                }
                else
                {
                    tapping = false;
                    if (tab < 1f)
                    {
                        if (open)
                        {
                            TypeButton();
                            Close();
                        }
                        else
                            DebugManager.instance.displayRuntimeUI = !DebugManager.instance.displayRuntimeUI;
                    }

                    tab = 0;
                }
            }

            if (tapping)
                tab += Time.deltaTime;
            lastTouchCount = Input.touchCount;
        }
        */

        PanelUpdate();
    }

}