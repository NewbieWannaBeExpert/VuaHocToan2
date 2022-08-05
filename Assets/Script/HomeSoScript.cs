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
        GameObject btnToHocSo = transform.GetChild(2).gameObject;
        btnToHocSo.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(2).gameObject));
            ToHocSo();
        });

        GameObject btnToHome = transform.GetChild(1).gameObject;
        audioSource = btnToHome.AddComponent<AudioSource>();
        btnToHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(1).gameObject));
            ToHome();
        });
        GameObject btnToDoVui1 = transform.GetChild(3).gameObject;
        btnToDoVui1.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(3).gameObject));
            ToDoVui(1);
        });
        GameObject btnToDoVui2 = transform.GetChild(4).gameObject;
        btnToDoVui2.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(4).gameObject));
            ToDoVui(2);
        });
        GameObject btnToDoVui3 = transform.GetChild(5).gameObject;
        btnToDoVui3.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(5).gameObject));
            ToDoVui(3);
        });
        GameObject btnToDoVui4 = transform.GetChild(6).gameObject;
        btnToDoVui4.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(6).gameObject));
            ToDoVui(4);
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
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/ListScene"));
    }
    void ToDoVui(int dovuiIndex)
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/DoVuiSo" +dovuiIndex));
    }
    void ToHome()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HomeScene")); 
    }
}
