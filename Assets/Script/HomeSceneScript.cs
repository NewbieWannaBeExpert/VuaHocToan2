using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class HomeSceneScript : MonoBehaviour
{
    public static AudioSource audioSource;
    public static AudioClip[] buttonClickSound;
    void Start()
    {
        buttonClickSound = Resources.LoadAll("Sound/ButtonClicked", typeof(AudioClip)).Cast<AudioClip>().ToArray();
        GameObject btnToHocSo = transform.GetChild(1).gameObject;
        GameObject gToHocSo = Instantiate(btnToHocSo, transform);
        //Debug.Log("button click sound:" + buttonClickSound.GetInstanceID());
        audioSource = gToHocSo.AddComponent<AudioSource>();
        gToHocSo.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToHocSo();
        });
        Destroy(btnToHocSo);
        GameObject btnLamToan = transform.GetChild(2).gameObject;
        btnLamToan.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToLamToan();
        });
    }

    void ToHocSo()
    {
        audioSource.PlayOneShot(buttonClickSound[0], 1f);
        SceneManager.LoadScene("Scenes/HocSoHomeScene");
    }
    void ToLamToan()
    {
        audioSource.PlayOneShot(buttonClickSound[0], 1f);
        SceneManager.LoadScene("Scenes/LamToanHomeScene");
    }

}
