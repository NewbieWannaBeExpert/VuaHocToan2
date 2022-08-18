using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeLamToan : MonoBehaviour
{
    public static AudioSource audioSource;
    public static int CongTruNum = 3;
    void Start()
    {

        GameObject btnSoSanh = transform.GetChild(2).gameObject;
        audioSource = btnSoSanh.AddComponent<AudioSource>();
        btnSoSanh.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(btnSoSanh));
            ToSoSanh();
        });

        GameObject btnToHome = transform.GetChild(1).gameObject;
        btnToHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(btnToHome));
            ToHome();
        });
        GameObject btnToCongTru = transform.GetChild(3).gameObject;
        btnToCongTru.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            CongTruNum = 2;
            StartCoroutine(SharedData.ZoomInAndOutButton(btnToCongTru));
            ToCongTru();
            
        });
        GameObject btnToCongTru2 = transform.GetChild(4).gameObject;
        btnToCongTru2.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            CongTruNum = 3;
            StartCoroutine(SharedData.ZoomInAndOutButton(btnToCongTru2));
            ToCongTru();
            
        });
        GameObject btnToDoVui = transform.GetChild(5).gameObject;
        btnToDoVui.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            
            StartCoroutine(SharedData.ZoomInAndOutButton(btnToDoVui));
            ToDoVui();

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
    void ToDoVui()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/DoVuiSo3"));
    }

}
