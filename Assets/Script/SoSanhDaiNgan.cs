using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SoSanhDaiNgan : MonoBehaviour
{
    public AudioSource audioSource;
    public static GameObject imageBig;
    public static GameObject imageSmall;
    public static AudioClip[] wrongAlertClipList;
    public static AudioClip[] rightAlertClipList;
    public AudioClip[] alertFindBigList;
    public AudioClip[] alertFindSmallList;
    public static int userChoice = -1;//-1 no anser, 0 wrong, 1 correct
    public static Sprite[] listBigSprite;
    public static Sprite[] listSmallSprite;
    public static Sprite[] listLongSprite;
    public static Sprite[] listShortSprite;
    public static Sprite[] listTallSprite;
    public static Sprite[] listNotTallSprite;
    public static float localScale = 0.6f;
    //This variable is to determine if the big image should store the small image and vice versa
    //if randBigShuffle is 0 then the bigImage still displays the big image
    //if randBigShuffle is 1 then the bigImage displays the small image
    public static int isFindBig = 0;
    void LoadSoundClip()
    {
        wrongAlertClipList = Resources.LoadAll("Sound/AlertRightWrong/Wrong", typeof(AudioClip)).Cast<AudioClip>().ToArray();
        rightAlertClipList = Resources.LoadAll("Sound/AlertRightWrong/Right",typeof(AudioClip)).Cast<AudioClip>().ToArray();
        alertFindBigList = Resources.LoadAll("Sound/FindBigSmall/Big",typeof(AudioClip)).Cast<AudioClip>().ToArray();
        alertFindSmallList = Resources.LoadAll("Sound/FindBigSmall/Small", typeof(AudioClip)).Cast<AudioClip>().ToArray();
    }
    void Start()
    {
        GameObject btnHome = transform.GetChild(1).gameObject;
        btnHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToHome();
        });
        GameObject btnReplay = transform.GetChild(5).gameObject;
        btnReplay.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            LoadRandImage();
        });
        audioSource = btnHome.AddComponent<AudioSource>();  
        LoadSoundClip();
        SetupSprites();
        LoadRandImage();
       
    }
    void LoadRandImage()
    {
        int randomIndex = 0;
        System.Random myObject = new System.Random();
        randomIndex = myObject.Next(0, 9);
        System.Random randBigShuffleSeed = new System.Random();
        isFindBig = randBigShuffleSeed.Next(0, 2);
        Debug.Log("Rand big shuffle is:" + isFindBig);
        if(isFindBig == 1)
        {
            audioSource.PlayOneShot(alertFindBigList[0]);
        } else
        {
            audioSource.PlayOneShot(alertFindSmallList[0]);
        }
        GameObject bigImage = transform.GetChild(3).gameObject;
        GameObject smallImage = transform.GetChild(4).gameObject;
        imageBig = bigImage;
        imageSmall = smallImage;
        bigImage.transform.GetComponent<Image>().sprite = listBigSprite[randomIndex];
        smallImage.transform.GetComponent<Image>().sprite = listSmallSprite[randomIndex];
        Vector3 smallPosition = smallImage.transform.position;
        Vector3 bigPosition = bigImage.transform.position;
        int shuffleBigSmallPos = randBigShuffleSeed.Next(9, 11);
        if(shuffleBigSmallPos == 10)
        {
            smallImage.transform.position = bigPosition;
            bigImage.transform.position = smallPosition;
        }
        smallImage.transform.localScale = new Vector3(localScale, localScale, 1);
        bigImage.transform.localScale = new Vector3(localScale, localScale, 1);
        smallImage.SetActive(true);
        bigImage.SetActive(true);
        bigImage.AddComponent<ClickAction>();
        smallImage.AddComponent<ClickAction>();
    }
    void SetupSprites()
    {
        listBigSprite = Resources.LoadAll("Items/Compare/Big_Small/Big", typeof(Sprite)).Cast<Sprite>().ToArray();
        listSmallSprite = Resources.LoadAll("Items/Compare/Big_Small/Small", typeof(Sprite)).Cast<Sprite>().ToArray();
    }
    void ToHome()
    {
        Debug.Log("You click on home button");
       SceneManager.LoadScene("Scenes/HomeScene");
    }
    public void showCorrect(bool isCorrect)
    {
        int randAlertIndex = 0;
        System.Random rand = new System.Random();
        if (isCorrect)
        {
            randAlertIndex = rand.Next(0, rightAlertClipList.Length);
            audioSource.PlayOneShot(rightAlertClipList[randAlertIndex]);
        } else
        {
            randAlertIndex = rand.Next(0, wrongAlertClipList.Length);
            audioSource.PlayOneShot(wrongAlertClipList[randAlertIndex]); 
        }
    }
    public void Update()
    {
        if(userChoice == 1)
        {
            showCorrect(true);
            userChoice = -1;
        } else if (userChoice == 0)
        {
            showCorrect(false);
            userChoice = -1;
        }
    }


    /*public void Blink(GameObject forGameObject)
    {
        var endTime = Time.time + waitTime;
        while (Time.time < waitTime)
        {
            forGameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new  WaitForSeconds(0.2f);
            forGameObject.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }*/
    public static IEnumerator MyCoroutine(GameObject forGameObject)
    {
        float speed = 0.2f;
        scale(forGameObject);
        yield return new WaitForSeconds(speed);
        normalFlash(forGameObject);
    }
    public static void scale(GameObject forGameObject)
    {
        forGameObject.transform.localScale = new Vector3(0.725f, 0.725f, 1.0f);
        Debug.Log("Scale bigger");
    }
   public static void normalFlash(GameObject forGameObject)
    {
        forGameObject.transform.localScale = new Vector3(localScale, localScale, 1.0f);
        Debug.Log("Scale smaller");
    }
}
public class ClickAction : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        
        Debug.Log(name  + " Image clicked");
        if(name.Contains("Big"))
        {
            StartCoroutine(SoSanhDaiNgan.MyCoroutine(SoSanhDaiNgan.imageBig));
            Debug.Log("You click on big image");
            if (SoSanhDaiNgan.isFindBig == 1)
            {
                SoSanhDaiNgan.userChoice = 1;
            } else
            {
                SoSanhDaiNgan.userChoice = 0;
            }
        } else
        {
            StartCoroutine(SoSanhDaiNgan.MyCoroutine(SoSanhDaiNgan.imageSmall));
            if (SoSanhDaiNgan.isFindBig == 1)
            {
                SoSanhDaiNgan.userChoice = 0;
            }
            else
            {
                SoSanhDaiNgan.userChoice = 1;
            }
            Debug.Log("You click on small image");
        }
        // OnClick code goes here ...
    }
}
