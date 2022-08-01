using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeLamToan : MonoBehaviour
{
    void Start()
    {

        GameObject btnSoSanh = transform.GetChild(2).gameObject;
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
        SceneManager.LoadScene("Scenes/SoSanhDaiNgan");
    }
    void ToHome()
    {
        SceneManager.LoadScene("Scenes/HomeScene");
    }
}
