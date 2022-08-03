using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SharedData : MonoBehaviour
{
    public static AudioClip[] buttonClickSound;
    public static AudioClip[] numberSound;
    public static Sprite[] listNumberDoVui;
    public static Sprite[] listNumberBg;
    public static AudioClip[] wrongAlertClipList;
    public static AudioClip[] rightAlertClipList;
    //public static AudioSource audioSource;
    public static void initSound()
    {
        if (buttonClickSound != null) { }
        else
        {
       //     audioSource = forGameObject.AddComponent<AudioSource>();
            buttonClickSound = Resources.LoadAll("Sound/ButtonClicked", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            numberSound = Resources.LoadAll("Sound/Number2", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            listNumberDoVui = Resources.LoadAll("Numbers2", typeof(Sprite)).Cast<Sprite>().ToArray();
            listNumberBg = Resources.LoadAll("Numbers2/NumberBg", typeof(Sprite)).Cast<Sprite>().ToArray();
            wrongAlertClipList = Resources.LoadAll("Sound/AlertRightWrong/Wrong", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            rightAlertClipList = Resources.LoadAll("Sound/AlertRightWrong/Right", typeof(AudioClip)).Cast<AudioClip>().ToArray();
        }
    }
    public static void alertSoundCorrect(bool isCorrect, AudioSource audioSource)
    {
        int randAlertIndex;
        System.Random rand = new System.Random();
        if (isCorrect)
        {
            randAlertIndex = rand.Next(0, SharedData.rightAlertClipList.Length);
            audioSource.PlayOneShot(SharedData.rightAlertClipList[randAlertIndex]);
        }
        else
        {
            randAlertIndex = rand.Next(0, SharedData.wrongAlertClipList.Length);
            audioSource.PlayOneShot(SharedData.wrongAlertClipList[randAlertIndex]);
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
       
    }
    public static void normalFlash(GameObject forGameObject)
    {
        forGameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
       
    }
}
