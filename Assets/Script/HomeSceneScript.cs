using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class HomeSceneScript : MonoBehaviour
{
    public static AudioSource audioSource;
    //public static AudioClip[] buttonClickSound;
    void Start()
    {
        SharedData.initAllResources();
        GameObject btnToHocSo = transform.GetChild(1).gameObject;
       // GameObject gToHocSo = Instantiate(btnToHocSo, transform);
        //Debug.Log("button click sound:" + buttonClickSound.GetInstanceID());
        audioSource = btnToHocSo.AddComponent<AudioSource>();
        btnToHocSo.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(btnToHocSo));
            ToHocSo();
        });
        GameObject btnLamToan = transform.GetChild(2).gameObject;
        btnLamToan.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(btnLamToan));
            ToLamToan();
        });
        GameObject btnSticker = transform.GetChild(3).gameObject;
        btnSticker.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(btnSticker));
            ToSticker();
        });
        GameObject btnKiemSao = transform.GetChild(4).gameObject;
        //btnKiemSao.SetActive(false);
        btnKiemSao.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(btnKiemSao));
            ToKiemSao();
        });
    }
    void ToKiemSao()
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
        else if(randNum == 2) 
        {
            StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/TestDoVui2"));
        } else
        {
            StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/TestCongTru"));
        }
    }
    void ToSticker()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/StickerList"));
    }
    void ToHocSo()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HocSoHomeScene"));
    }
    void ToLamToan()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/LamToanHomeScene"));
       // SceneManager.LoadScene("Scenes/LamToanHomeScene");
    }
   

}
