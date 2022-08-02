using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeLamToan : MonoBehaviour
{
    public static AudioSource audioSource;
    void Start()
    {

        GameObject btnSoSanh = transform.GetChild(2).gameObject;
        audioSource = btnSoSanh.AddComponent<AudioSource>();
        btnSoSanh.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToSoSanh();
        });

        GameObject btnToHome = transform.GetChild(1).gameObject;
        btnToHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToHome();
        });
    }

    void ToSoSanh()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.MyCoroutine(transform.GetChild(2).gameObject));
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/SoSanhDaiNgan"));
        //SceneManager.LoadScene("Scenes/SoSanhDaiNgan");
    }
    void ToHome()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.MyCoroutine(transform.GetChild(1).gameObject));
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HomeScene"));
       // SceneManager.LoadScene("Scenes/HomeScene");
    }
}
