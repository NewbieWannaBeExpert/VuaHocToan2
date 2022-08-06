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
        SharedData.initSound();
        GameObject btnToHocSo = transform.GetChild(1).gameObject;
       // GameObject gToHocSo = Instantiate(btnToHocSo, transform);
        //Debug.Log("button click sound:" + buttonClickSound.GetInstanceID());
        audioSource = btnToHocSo.AddComponent<AudioSource>();
        btnToHocSo.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(1).gameObject));
            ToHocSo();
        });
        GameObject btnLamToan = transform.GetChild(2).gameObject;
        btnLamToan.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(2).gameObject));
            ToLamToan();
        });
        GameObject btnSticker = transform.GetChild(3).gameObject;
        btnSticker.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(3).gameObject));
            ToSticker();
        });
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
