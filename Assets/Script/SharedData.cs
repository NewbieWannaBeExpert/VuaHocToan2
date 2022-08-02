using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SharedData : MonoBehaviour
{
    public static AudioClip[] buttonClickSound;
    //public static AudioSource audioSource;
    public static void initSound()
    {
        if (buttonClickSound != null) { }
        else
        {
       //     audioSource = forGameObject.AddComponent<AudioSource>();
            buttonClickSound = Resources.LoadAll("Sound/ButtonClicked", typeof(AudioClip)).Cast<AudioClip>().ToArray();
        }
    }
    public static IEnumerator ToSceneAfterSomeTime(float time, string sceneName)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneName);
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
}
