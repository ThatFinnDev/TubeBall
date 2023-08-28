using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static GameController controller;
    public Transform player;
    public Vector3 round;
    public Transform moving;
    public bool inGame;
    public Vector3 move;
    private bool wasInGame;
    public GameObject panel;
    public GameObject homeButton;

    public Transform dots;
    private List<Vector3> dotsPos;
    public Transform stripes;
    private List<Vector3> stripesPos;
    public Transform obsticales;
    private List<Vector3> obsticalsPos;
    
    public TextMeshProUGUI score;
    public TextMeshProUGUI highScore;
    public long scoreLong;
    public TextMeshProUGUI inGameScore;
    public LayerMask layerMask;
    public GameObject pipe;

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    private void Awake()
    {
        controller = this;
        score.SetText(long.Parse(PlayerPrefs.GetString("score","0")).ToString());
        highScore.SetText(long.Parse(PlayerPrefs.GetString("HighScore","0")).ToString());
    }

    private void Start()
    {
        dotsPos = new List<Vector3>();
        stripesPos = new List<Vector3>();
        obsticalsPos = new List<Vector3>();
        for (int i = 0; i < dots.childCount; i++)
        {
            dotsPos.Add(dots.GetChild(i).position);
        }
        for (int i = 0; i < stripes.childCount; i++)
        {
            stripesPos.Add(stripes.GetChild(i).position);
        }
        
        for (int i = 0; i < obsticales.childCount; i++)
        {
            obsticalsPos.Add(obsticales.GetChild(i).position);
        }
    }

    public void Play()
    {
        panel.SetActive(false);
        homeButton.SetActive(true);
        inGame = true;
        
        foreach (Transform transforme in obsticales)
        {
            transforme.transform.localRotation = Quaternion.Euler(
                0,
                Random.Range(0f,360f),
                0);
        }
    }

    public void Restart()
    {
        xRot = 0;
        pipe.transform.rotation = Quaternion.Euler(0,0,90);
        scoreLong = 0;
        Awake();
        panel.SetActive(true);
        homeButton.SetActive(false);
        inGame = false;

        moving.transform.position = Vector3.zero;

        
        for (int i = 0; i < obsticales.childCount; i++)
        {
            obsticales.GetChild(i).position = obsticalsPos[i];
            obsticales.GetChild(i).localRotation = Quaternion.identity;
        }
        for (int i = 0; i < dots.childCount; i++)
        {
            dots.GetChild(i).position = dotsPos[i];
        }
        for (int i = 0; i < stripes.childCount; i++)
        {
            stripes.GetChild(i).position = stripesPos[i];
        }
    }

    public float turnSpeed = 150;
    private float xRot = 0;
    void Update()
    {
        if (inGame)
        {
            #if UNITY_EDITOR
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
            #elif UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                if (Screen.width / 2 < Input.GetTouch(0).position.x) 
                    xRot += turnSpeed * Time.deltaTime;
                else
                    xRot -= turnSpeed * Time.deltaTime;
                
                pipe.transform.rotation = Quaternion.Euler(xRot,0,90);
            }
            #endif
            if (Physics.CheckSphere(player.position, 0.625f,layerMask))
            {
                if (long.Parse(PlayerPrefs.GetString("HighScore", "0")) < scoreLong)
                {
                    PlayerPrefs.SetString("HighScore", scoreLong.ToString());
                }

                Restart();
            }
            
            inGameScore.SetText(scoreLong.ToString());
            player.Rotate(round.x * Time.deltaTime, round.y * Time.deltaTime, round.z * Time.deltaTime);
            moving.transform.position += move * Time.deltaTime;
        }
    }


}
