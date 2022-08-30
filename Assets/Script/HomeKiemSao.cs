using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class HomeKiemSao : MonoBehaviour
{
    public static AudioSource audioSource;
    void AnimateGameObject()
    {
        GameObject g = transform.GetChild(2).gameObject;
        LeanTween.cancel(g);
        var ltseq = LeanTween.sequence();
        var oldXPos = g.transform.localPosition.x;
        ltseq.append(LeanTween.moveX(g, oldXPos + 0.2f, 0.2f));
        ltseq.append(LeanTween.moveX(g, oldXPos, 0.2f));
    }
    //public static AudioClip[] buttonClickSound;
    void Start()
    {
        SharedData.initAllResources();
        GameObject btnHome = transform.GetChild(3).gameObject;
        audioSource = btnHome.AddComponent<AudioSource>();
        audioSource.clip = SharedData.bgSoundList[3];
        audioSource.loop = true;
        audioSource.volume = 0.1f;
        audioSource.Play();
        btnHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(btnHome));
            ToHome();
        });
        GameObject btnPlay = transform.GetChild(2).gameObject;
        btnPlay.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(btnPlay));
            ToPlay();
        });
        InvokeRepeating("AnimateGameObject", 0f, 3f);
        ShowRanDino();
    }
    void ShowRanDino()
    {
        GameObject someAni = transform.GetChild(1).gameObject;
        int totalDino = 5;
        GameObject[] dinoList = new GameObject[totalDino];
        for(int i = 0; i < 5; i ++ )
        {
            dinoList[i] = someAni.transform.GetChild(i).gameObject;
        }
        System.Random g = new System.Random();
        int randNum = g.Next(0, totalDino);
        for(int i = 0; i < totalDino; i++)
        {
            if(i == randNum)
            {
                dinoList[i].SetActive(true);
            } else
            {
                dinoList[i].SetActive(false);
            }
        }
    }
    void ToHome()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HomeScene"));
    }
    void ToPlay()
    {
        SharedData.isFindingStarMode = true;
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        System.Random g = new System.Random();
        int randNum = g.Next(0, 4);
        if (randNum == 0)
        {
            StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/TestDienSo"));
        }
        else if (randNum == 1)
        {
            StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/TestDoVui1"));
        }
        else if (randNum == 2)
        {
            StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/TestDoVui2"));
        }
        else
        {
            StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/TestCongTru"));
        }
    }

}
