using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SharedData : MonoBehaviour
{
    public static AudioClip[] buttonClickSound;
    public static AudioClip[] numberSound;
    public static AudioClip[] numberOnlySound;
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
    public static bool isGameOn = true;
    public static int currentTestSentence = 1;
    public static int totalTestSentence = 2;

    public static AudioClip[] wrongAlertClipList;
    public static AudioClip[] rightAlertClipList;
    public static Sprite[] listAnimalSprite;
    public static int InitGivenStar = 300;// for openning 6 images.
    public static int currentStickerDetailIndex = 0;
    public static string PrefNumOfStar = "VuaHocToan_PrefNumOfStar";
    //This is to save the current state of the list box that cover the sticker
    //This will take value like this: 0_1_0_0_1_0 -> Then the box with index 1,4 will be open
    //Box with index 0,2,3,5 will close.
    public static string PrefStickerOpenBoxListString = "VuaHocToan_StickerOpenBoxString";
    public static string PrefStickerCurrentPage = "VuaHocToan_PrefStickerCurrentPage";
    public static IEnumerator DestroyGameObjectAfterDelay(GameObject g, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(g);
    }
    public static void SetCurrentStickerPage(int page)
    {
        PlayerPrefs.SetInt(PrefStickerCurrentPage, page);
    }
    public static int GetCurrentStickerPage()
    {
        if (PlayerPrefs.HasKey(PrefStickerCurrentPage))
        {
            return PlayerPrefs.GetInt(PrefStickerCurrentPage);
        } else
        {
            return 0;
        }
    }
    public static void SetStickerOpenBoxString(string value)
    {
        PlayerPrefs.SetString(PrefStickerOpenBoxListString, value);
    }
    public static string GetStickerOpenBoxString()
    {
        if(PlayerPrefs.HasKey(PrefStickerOpenBoxListString))
        {
            Debug.Log("Has key of Open boxes");
            string ret = PlayerPrefs.GetString(PrefStickerOpenBoxListString);
            Debug.Log("Open box string is: " + ret);
            return ret;
        } else
        {
            Debug.Log("No key of Open Boxes");
            PlayerPrefs.SetString(PrefStickerOpenBoxListString, "0_0_0_0_0_0");
            //Default all the sticker is not open
            return "0_0_0_0_0_0";
        }
    }
    public static void ResetStickerOpenBoxString()
    {
        SetStickerOpenBoxString("0_0_0_0_0_0");
    }
    public static void SetNumberOfStar(int numOfStar)
    {
        PlayerPrefs.SetInt(PrefNumOfStar, numOfStar);
    }
    public static int GetNumberOfStar()
    {
        if(PlayerPrefs.HasKey(PrefNumOfStar))
        {
            return PlayerPrefs.GetInt(PrefNumOfStar);
        } else
        {
            PlayerPrefs.SetInt(PrefNumOfStar, InitGivenStar);
            return InitGivenStar;
        }
    }
    public static IEnumerator DestroyGameObjectAfterSomeTime(GameObject g, float afterDelay)
    {
        yield return new WaitForSeconds(afterDelay);
        Destroy(g);
    }
   
    //public static AudioSource audioSource;
    public static void initAllResources()
    {
        if (PlayerPrefs.HasKey(PrefNumOfStar))
        { } else { SetNumberOfStar(InitGivenStar); }
            
        if (buttonClickSound != null) { }
        else
        {
            buttonClickSound = Resources.LoadAll("Sound/ButtonClicked", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            numberSound = Resources.LoadAll("Sound/Number2", typeof(AudioClip)).Cast<AudioClip>().ToArray();
            numberOnlySound = Resources.LoadAll("Sound/NumberOnly", typeof(AudioClip)).Cast<AudioClip>().ToArray();
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
