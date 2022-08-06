using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SharedData : MonoBehaviour
{
    public static AudioClip[] buttonClickSound;
    public static AudioClip[] numberSound;
    public static AudioClip[] victorySound;
    public static AudioClip[] rightBuzzSoundList;
    public static AudioClip[] wrongBuzzSoundList;
    public static Sprite[] listNumberDoVui;
    public static Sprite[] listBoyAnimated;
    public static Sprite[] listGirlAnimated;
    public static Sprite[] listNumberDoVui3;
    public static Sprite[] listNumberBg;
    public static Sprite[] listNumberBgDoVui3;
    public static Sprite[] listNumberBgLamToan;

    public static AudioClip[] wrongAlertClipList;
    public static AudioClip[] rightAlertClipList;
    public static Sprite[] listAnimalSprite;
    //public static AudioSource audioSource;
    public static void initSound()
    {
        if (buttonClickSound != null) { }
        else
        {
            buttonClickSound = Resources.LoadAll("Sound/ButtonClicked", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            numberSound = Resources.LoadAll("Sound/Number2", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            victorySound = Resources.LoadAll("Sound/AlertVictory", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            rightBuzzSoundList = Resources.LoadAll("Sound/RightBuzz", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            wrongBuzzSoundList = Resources.LoadAll("Sound/WrongBuzz", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            listNumberDoVui = Resources.LoadAll("Numbers2", typeof(Sprite)).Cast<Sprite>().ToArray();
            listNumberBg = Resources.LoadAll("Numbers2/NumberBg", typeof(Sprite)).Cast<Sprite>().ToArray();
            wrongAlertClipList = Resources.LoadAll("Sound/AlertRightWrong/Wrong", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            rightAlertClipList = Resources.LoadAll("Sound/AlertRightWrong/Right", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            listAnimalSprite = Resources.LoadAll("Animals", typeof(Sprite)).Cast<Sprite>().ToArray();
            listNumberBgDoVui3 = Resources.LoadAll("DoVui3_HocSo/BtnBg", typeof(Sprite)).Cast<Sprite>().ToArray();
            listNumberDoVui3 = Resources.LoadAll("DoVui3_HocSo/Numbers", typeof(Sprite)).Cast<Sprite>().ToArray();
            listNumberBgLamToan = Resources.LoadAll("LamToan/BtnBg", typeof(Sprite)).Cast<Sprite>().ToArray();        
            listBoyAnimated = Resources.LoadAll("BoyGirls/Boy", typeof(Sprite)).Cast<Sprite>().ToArray();
        }
    }
    public static void alertSoundCorrect(bool isCorrect, AudioSource audioSource)
    {
        int randAlertIndex;
        System.Random rand = new System.Random();
        int randBuzzSound = rand.Next(0, 3);
        if (isCorrect)
        {
            randAlertIndex = rand.Next(0, rightAlertClipList.Length);
            audioSource.PlayOneShot(rightAlertClipList[randAlertIndex]);
            audioSource.PlayOneShot(rightBuzzSoundList[randBuzzSound], 1f);

        }
        else
        {
            randAlertIndex = rand.Next(0, wrongAlertClipList.Length);
            audioSource.PlayOneShot(wrongAlertClipList[randAlertIndex]);
            audioSource.PlayOneShot(wrongBuzzSoundList[randBuzzSound], 1f);

        }
    }
    public static IEnumerator ToSceneAfterSomeTime(float time, string sceneName)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneName);
    }
    public static IEnumerator ZoomInAndOutButton(GameObject forGameObject)
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
