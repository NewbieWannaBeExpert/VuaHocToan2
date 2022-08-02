using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class HomeSoScript : MonoBehaviour
{
    public static AudioSource audioSource;
    //public static AudioClip buttonClickSound;
    // Start is called before the first frame update
    void Start()
    {
       // buttonClickSound = Resources.Load("Sound/aButtonClicked111.mp3") as AudioClip;
        GameObject btnToHocSo = transform.GetChild(2).gameObject;
        btnToHocSo.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToHocSo();
        });

        GameObject btnToHome = transform.GetChild(1).gameObject;
        audioSource = btnToHome.AddComponent<AudioSource>();
        //audioSource.PlayOneShot(buttonClickSound, 100f);
        btnToHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToHome();
        });
        GameObject btnToDoVui = transform.GetChild(3).gameObject;
        btnToDoVui.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToDoVui();
        });


    }
    public static IEnumerator MyCoroutine(GameObject forGameObject)
    {
        float speed = 0.2f;
        scale(forGameObject);
        yield return new WaitForSeconds(speed);
        normalFlash(forGameObject);
    }
    public static void scale(GameObject forGameObject)
    {
        forGameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.0f);
        Debug.Log("Scale bigger");
    }
    public static void normalFlash(GameObject forGameObject)
    {
        forGameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Debug.Log("Scale smaller");
    }
    void ToHocSo()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.MyCoroutine(transform.GetChild(1).gameObject));
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/ListScene"));
    }
    void ToDoVui()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.MyCoroutine(transform.GetChild(2).gameObject));
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/DoVuiSo1"));
    }
    void ToHome()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.MyCoroutine(transform.GetChild(1).gameObject));
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HomeScene")); 
    }
}
