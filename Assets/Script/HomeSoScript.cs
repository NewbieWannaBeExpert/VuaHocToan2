using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class HomeSoScript : MonoBehaviour
{
    public static AudioSource audioSource;
    public static AudioClip buttonClickSound;
    // Start is called before the first frame update
    void Start()
    {
        buttonClickSound = Resources.Load("Sound/aButtonClicked111.mp3") as AudioClip;
        GameObject btnToHocSo = transform.GetChild(2).gameObject;
        btnToHocSo.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToHocSo();
        });

        GameObject btnToHome = transform.GetChild(1).gameObject;
        audioSource = btnToHome.AddComponent<AudioSource>();
        audioSource.PlayOneShot(buttonClickSound, 100f);
        btnToHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToHome();
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
        audioSource.PlayOneShot(HomeSceneScript.buttonClickSound[0], 1f);
        StartCoroutine(HomeSoScript.MyCoroutine(transform.GetChild(2).gameObject));
        StartCoroutine(ToHocSoAfterSomeTime(0.9f));
    }
    void ToHome()
    {
        Debug.Log("To home click in hoc so home");
        StartCoroutine(HomeSoScript.MyCoroutine(transform.GetChild(1).gameObject));
        audioSource.PlayOneShot(HomeSceneScript.buttonClickSound[0], 1f);
        StartCoroutine(ToHomeAfterSomeTime(0.9f));
        //SceneManager.LoadScene("Scenes/HomeScene");
    }
    IEnumerator ToHomeAfterSomeTime(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Scenes/HomeScene");
        // Code to execute after the delay
    }
    IEnumerator ToHocSoAfterSomeTime(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Scenes/ListScene");
       
    }

}
