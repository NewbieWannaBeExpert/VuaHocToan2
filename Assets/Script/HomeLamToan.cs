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
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(2).gameObject));
            ToSoSanh();
        });

        GameObject btnToHome = transform.GetChild(1).gameObject;
        btnToHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(1).gameObject));
            ToHome();
        });
        GameObject btnToCongTru = transform.GetChild(3).gameObject;
        btnToCongTru.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(3).gameObject));
            ToCongTru();
        });
    }

    void ToSoSanh()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/SoSanhDaiNgan"));
    }
    void ToHome()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HomeScene"));
    }
    void ToCongTru()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/LamToan_CongTru"));
    }
}
