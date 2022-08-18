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
    public AudioClip[] alertFindBigList;
    public AudioClip[] alertFindSmallList;
    public AudioClip[] alertFindLongList;
    public AudioClip[] alertFindShortList;
    public AudioClip[] alertFindTallList;
    public AudioClip[] alertFindNotTallList;
    public static int userChoice = -1;//-1 no anser, 0 wrong, 1 correct
    public static Sprite[] listBigSprite;
    public static Sprite[] listSmallSprite;
    public static Sprite[] listLongSprite;
    public static Sprite[] listShortSprite;
    public static Sprite[] listTallSprite;
    public static Sprite[] listNotTallSprite;
    public static float localScale = 0.6f;
    public int compareType = 0;//0: big-small, 1: long-short, 2: tall-nottall
    //This variable is to determine if the big image should store the small image and vice versa
    //if randBigShuffle is 0 then the bigImage still displays the big image
    //if randBigShuffle is 1 then the bigImage displays the small image
    public static int isFindBig = 0;
    void SetupSprites()
    {
        listBigSprite = Resources.LoadAll("Items/Compare/Big_Small/Big", typeof(Sprite)).Cast<Sprite>().ToArray();
        listSmallSprite = Resources.LoadAll("Items/Compare/Big_Small/Small", typeof(Sprite)).Cast<Sprite>().ToArray();
        listLongSprite = Resources.LoadAll("Items/Compare/Long_Short/Long",typeof(Sprite)).Cast<Sprite>().ToArray();
        listShortSprite = Resources.LoadAll("Items/Compare/Long_Short/Short", typeof(Sprite)).Cast<Sprite>().ToArray();
        listTallSprite = Resources.LoadAll("Items/Compare/Tall_NNot/Tall", typeof(Sprite)).Cast<Sprite>().ToArray();
        listNotTallSprite = Resources.LoadAll("Items/Compare/Tall_NNot/NotTall", typeof(Sprite)).Cast<Sprite>().ToArray();
    }
    void LoadSoundClip()
    {
        alertFindBigList = Resources.LoadAll("Sound/FindBigSmall/Big",typeof(AudioClip)).Cast<AudioClip>().ToArray();
        alertFindSmallList = Resources.LoadAll("Sound/FindBigSmall/Small", typeof(AudioClip)).Cast<AudioClip>().ToArray();
        alertFindLongList = Resources.LoadAll("Sound/AlertFindLongShort/Long", typeof(AudioClip)).Cast<AudioClip>().ToArray();
        alertFindShortList = Resources.LoadAll("Sound/AlertFindLongShort/Short", typeof(AudioClip)).Cast<AudioClip>().ToArray();
        alertFindTallList = Resources.LoadAll("Sound/AlertFindTall/Tall", typeof(AudioClip)).Cast<AudioClip>().ToArray();
        alertFindNotTallList = Resources.LoadAll("Sound/AlertFindLongShort/Short", typeof(AudioClip)).Cast<AudioClip>().ToArray();
    }
    void Start()
    {
        GameObject btnHome = transform.GetChild(1).gameObject;
        btnHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
            StartCoroutine(SharedData.ZoomInAndOutButton(btnHome));
            ToHome();
        });
        GameObject btnReplay = transform.GetChild(5).gameObject;
        btnReplay.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
            StartCoroutine(SharedData.ZoomInAndOutButton(btnReplay));
            LoadRandImage();
        });
        audioSource = btnHome.AddComponent<AudioSource>();  
        LoadSoundClip();
        SetupSprites();
        LoadRandImage();
       
    }
    public void LoadRandImage()
    {
        
        int randomIndex = 0;
        System.Random myObject = new System.Random();
        randomIndex = myObject.Next(0, 9);
        compareType = myObject.Next(0, 3);
        System.Random randBigShuffleSeed = new System.Random();
        isFindBig = randBigShuffleSeed.Next(0, 2);
        Debug.Log("Rand big shuffle is:" + isFindBig + " and compare type is:" + compareType);
       
        GameObject bigImage = transform.GetChild(3).gameObject;
        GameObject smallImage = transform.GetChild(4).gameObject;
        imageBig = bigImage;
        imageSmall = smallImage;
        if(compareType ==  0) //big-small
        {
            if (isFindBig == 1)
            {
                audioSource.PlayOneShot(alertFindBigList[0]);
            }
            else
            {
                audioSource.PlayOneShot(alertFindSmallList[0]);
            }
            bigImage.transform.GetComponent<Image>().sprite = listBigSprite[randomIndex];
            smallImage.transform.GetComponent<Image>().sprite = listSmallSprite[randomIndex];
        } else if(compareType == 1)
        {
            if (isFindBig == 1)
            {
                audioSource.PlayOneShot(alertFindLongList[0]);
            }
            else
            {
                audioSource.PlayOneShot(alertFindShortList[0]);
            }
            bigImage.transform.GetComponent<Image>().sprite = listLongSprite[randomIndex];
            smallImage.transform.GetComponent<Image>().sprite = listShortSprite[randomIndex];
        } else
        {
            if (isFindBig == 1)
            {
                audioSource.PlayOneShot(alertFindTallList[0]);
            }
            else
            {
                audioSource.PlayOneShot(alertFindNotTallList[0]);
            }
            bigImage.transform.GetComponent<Image>().sprite = listTallSprite[randomIndex];
            smallImage.transform.GetComponent<Image>().sprite = listNotTallSprite[randomIndex];
        }
        
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
    
    void ToHome()
    {
       Debug.Log("You click on home button");
       SceneManager.LoadScene("Scenes/LamToanHomeScene");
    }
    public void showCorrect(bool isCorrect)
    {
        if (isCorrect)
        {
            SharedData.alertSoundCorrect(true, audioSource);
            StartCoroutine(ReloadGame());
        } else
        {
            SharedData.alertSoundCorrect(false, audioSource);
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
    public IEnumerator ReloadGame()
    {
        yield return new WaitForSeconds(2.5f);
        LoadRandImage();
    }
    public static IEnumerator ScaleUpNDown(GameObject forGameObject)
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
            StartCoroutine(SoSanhDaiNgan.ScaleUpNDown(SoSanhDaiNgan.imageBig));
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
            StartCoroutine(SoSanhDaiNgan.ScaleUpNDown(SoSanhDaiNgan.imageSmall));
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
