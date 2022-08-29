using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class TestDoVui2 : MonoBehaviour
{
    public AudioSource audioSource;
    public static Sprite[] listAnimalSprite;
    public List<GameObject> listNumberButton;
    public AudioClip[] alertFindMoreList;
    public AudioClip[] alertFindLessList;
    public static Sprite[] listBtnBg;
    private int correctIndex = 0;
    private int correctNumberIndexReal = 0;
   
    private int isFindBig = 0;
    void UpdateNumberOfSentence()
    {
        GameObject txtSen = transform.GetChild(6).transform.GetChild(1).gameObject;
        txtSen.gameObject.GetComponent<Text>().text = SharedData.currentTestSentence.ToString() + "/" + SharedData.totalTestSentence.ToString();
    }
    void UpdateNumberOfStar()
    {
        GameObject g = transform.GetChild(5).gameObject;
        g.transform.GetChild(1).GetComponent<Text>().text = SharedData.GetNumberOfStar().ToString();
    }
    void LoadSoundClip()
    {
        alertFindLessList = new AudioClip[2];
        alertFindMoreList = new AudioClip[2];
        AudioClip findLessAlert1 = Resources.Load<AudioClip>("Sound/Alerts/alertLessAnimal");
        AudioClip findLessAlert2 = Resources.Load<AudioClip>("Sound/Alerts/alertLessAnimal2");
        alertFindLessList[0] = findLessAlert1;
        alertFindLessList[1] = findLessAlert2;
        AudioClip findMoreAlert1 = Resources.Load<AudioClip>("Sound/Alerts/alertMoreAnimal");
        AudioClip findMoreAlert2 = Resources.Load<AudioClip>("Sound/Alerts/alertMoreAnimal2");
        alertFindMoreList[0] = findMoreAlert1;
        alertFindMoreList[1] = findMoreAlert2;
    }
    void SetupGraphics()
    {
        listBtnBg = Resources.LoadAll("DoVui2_HocSo/BtnBg", typeof(Sprite)).Cast<Sprite>().ToArray();
    }
    void initAll()
    {
        SetupGraphics();
        LoadSoundClip();
        UpdateNumberOfStar();
        UpdateNumberOfSentence();
    }
    void Start()
    {
        initAll();
        //RescaleBgImage();
        listNumberButton = new List<GameObject>();
        GameObject btnHome = transform.GetChild(2).gameObject;
        audioSource = btnHome.AddComponent<AudioSource>();
        btnHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToHome();
        });
        
        GameObject btnSound = transform.GetChild(3).gameObject;
        btnSound.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ReplaySound();
        });
        LoadAnimalBoardList();

    }
    IEnumerator ReplayAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Replay(0.5f);
    }
    void BtnNumberClicked(int itemIndex)
    {
        if (SharedData.isGameOn == false)
        {
            return;
        }
        Debug.Log("You click on index:" + itemIndex);
        GameObject currentClickedNumber = transform.GetChild(7 + itemIndex).gameObject;
        if (itemIndex == correctIndex)
        {
            SharedData.isGameOn = false;
            // Debug.Log("CORRECT!");
            currentClickedNumber.transform.GetChild(2).GetComponent<Image>().sprite = listBtnBg[1];
            currentClickedNumber.transform.GetChild(3).GetComponent<Image>().sprite = listBtnBg[1];
            SharedData.alertSoundCorrect(true, audioSource);
            StartCoroutine(ReplayAfterDelay(2.5f));
            int totalStar = SharedData.GetNumberOfStar();
            SharedData.SetNumberOfStar(totalStar + SharedData.starPlus);
            UpdateNumberOfStar();
            SharedData.currentTestSentence++;
            ShowFlyingStar(currentClickedNumber,true);
        } else
        {
            //Debug.Log("IN_CORRECT");
            int totalStar = SharedData.GetNumberOfStar();
            if (totalStar > 0)
            {
                if (totalStar - SharedData.starMinus > 0)
                {
                    SharedData.SetNumberOfStar(totalStar - SharedData.starMinus);
                }
                else
                {
                    SharedData.SetNumberOfStar(0);
                }

                UpdateNumberOfStar();
            }
            SharedData.alertSoundCorrect(false, audioSource);
            currentClickedNumber.transform.GetChild(2).GetComponent<Image>().sprite = listBtnBg[2];
            ShowFlyingStar(currentClickedNumber, false);
        }

        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(currentClickedNumber));
    }
    void ShowFlyingStar(GameObject g, bool isCorrect)
    {
        //Show Flying star
        GameObject coinStatus = transform.GetChild(5).gameObject;
        GameObject moneyImage = g.transform.GetChild(4).gameObject;
        int totalEmotion = SharedData.listEmotionSprite.Length - 1;
        if (isCorrect == false) {
            System.Random r = new System.Random();
            int randIndex = r.Next(0, totalEmotion);
            //Sprite[] sadSpriteList = Resources.LoadAll<Sprite>("Others/SadIcon").ToArray();
            moneyImage.GetComponent<Image>().sprite = SharedData.listEmotionSprite[randIndex];//sadSpriteList[randIndex];
        }
        moneyImage.SetActive(true);
        GameObject moneyDuplicated = Instantiate(moneyImage, transform);
        moneyDuplicated.transform.position = moneyImage.transform.position;
        LeanTween.cancel(moneyDuplicated);
        moneyDuplicated.transform.localScale = new Vector3(0.7f, 0.7f, 1.0f);
        LeanTween.scale(moneyDuplicated, new Vector3(2.0f, 2.0f, 2.0f), 0.3f).setEase(LeanTweenType.easeOutQuint);
        if (isCorrect == false)
        {
            LeanTween.move(moneyDuplicated, new Vector3(0f, -5.0f, 1.0f), 0.5f).setEaseInBack();
        }
        else
        {
            LeanTween.move(moneyDuplicated, coinStatus.transform.position, 0.5f).setEaseInBack();
        }
        StartCoroutine(SharedData.DestroyGameObjectAfterDelay(moneyDuplicated, 0.7f));
        moneyImage.SetActive(false);
    }
    public void SoundForCorrectNumber(int numberIndex)
    {
        audioSource.PlayOneShot(SharedData.numberSound[numberIndex]);
    }
    void AlertFindSound(int isFindBig)
    {
        System.Random randBigShuffleSeed = new System.Random();
        int randIndex = randBigShuffleSeed.Next(0, 2);
        if (isFindBig == 1)
        {
            if (randIndex == 0)
            {
                audioSource.PlayOneShot(alertFindMoreList[1], 1.0f);
            }
            else
            {
                audioSource.PlayOneShot(alertFindMoreList[0], 1.0f);
            }
        }
        else
        {
            if (randIndex == 0)
            {
                audioSource.PlayOneShot(alertFindLessList[1], 1.0f);
            }
            else
            {
                audioSource.PlayOneShot(alertFindLessList[0], 1.0f);
            }
        }
    }
    void LoadAnimalBoardList()
    {
        System.Random randBigShuffleSeed = new System.Random();
        isFindBig = randBigShuffleSeed.Next(0, 2);
        AlertFindSound(isFindBig);
        int totalItem = 2;
        int numRows = 2;
        int numCols = 1;
        float initX = 0f;
        float initY = 1.5f;
        float paddingX = 2.0f;
        float paddingY = 3.87f;
        float initImageX = -1.0f;
        float initImageY = 2f;
        float paddingImageX = 1.0f;
        float paddingImageY = 1.5f;
        float imageScale = 0.7f;
        GameObject btnNumberPattern = transform.GetChild(4).gameObject;
        btnNumberPattern.SetActive(true);
        float scaleWidth=1.0f, scaleHeigth=1.0f;
        float buttonWidth = btnNumberPattern.GetComponent<RectTransform>().rect.width;
        float buttonHeight = btnNumberPattern.GetComponent<RectTransform>().rect.height;
        if (Screen.height > 1.5f * Screen.width)
        {
            numCols = 1;
            numRows = 3;
            Debug.Log("Screen to long");
        } else
        {
            initY = 1.3f;
            paddingY = 4.0f;
            scaleWidth = 1.5f;
            scaleHeigth = 1.2f;
        }
        int num1,num2;
        System.Random myObject = new System.Random();
        num1 = myObject.Next(1, 5);
        num2 = myObject.Next(6, 9);
        while(num2 == num1)
        {
            num2 = myObject.Next(1, 9);
        }
        if(isFindBig == 1)
        {
            if(num1>num2)
            {
                correctIndex = 0;
            } else
            {
                correctIndex = 1;
            }
        } else
        {
            if(num1> num2)
            {
                correctIndex = 1;
            } else
            {
                correctIndex = 0;
            }
        }
        GameObject btnNumberClone;
        int randAnimalIndex = myObject.Next(0, SharedData.listAnimalSprite.Length);
        for (int counter = 0; counter < 2; counter++)
        {
            btnNumberClone = Instantiate(btnNumberPattern, transform);
            btnNumberClone.transform.GetChild(2).GetComponent<Image>().sprite = listBtnBg[0];//bg normal
            btnNumberClone.transform.GetChild(0).GetComponent<Image>().sprite = listBtnBg[1];//bg right
            btnNumberClone.transform.GetChild(1).GetComponent<Image>().sprite = listBtnBg[2];//bg wrong
            int numImageRows = 3;
            int numImageCols = 3;
            GameObject moneyImage = btnNumberClone.transform.GetChild(4).gameObject;
            moneyImage.SetActive(false);
            int realIndex = 0;
            if(counter == 0)
            {
                realIndex = num1;
            } else if(counter == 1)
            {
                realIndex = num2;
            }
            GameObject spritePatter = btnNumberPattern.transform.GetChild(3).gameObject;
            spritePatter.SetActive(true);
            if (realIndex > 6)
            {
                numImageRows = 3;
                numImageCols = 3;
                imageScale = 0.6f;
                paddingImageY = 1.0f;
                initImageX = -1.5f;
                initImageY = 1.82f;
                paddingImageX = 1.5f;
            }
            else if (realIndex > 3)
            {
                numImageRows = 2;
                numImageCols = 3;
                imageScale = 0.7f;
                initImageX = -1.5f;
                paddingImageX = 1.42f;
                paddingImageY = 1.4f;
                initImageY = 1.42f;
            }
            else if (realIndex == 3)
            {
                imageScale = 0.9f;
                initImageY = 1.5f;
                paddingImageX = 2.0f;
                numImageRows = 2;
                numImageCols = 2;
            } else if(realIndex == 2)
            {
                numImageRows = 1;
                numImageCols = 2;
                paddingImageX = 2.0f;
                imageScale = 1.0f;
                initImageY = 1.0f;
            } else if(realIndex == 1)
            {
                imageScale = 1.2f;
                initImageX = 0f;
                initImageY = 1f;
            }
            for (int k = 0; k < numImageRows; k++)
            {
                for (int m = 0; m < numImageCols; m++)
                {
                    if (k * numImageCols + m >= realIndex)
                    {
                        break;
                    }
                    GameObject imageClone = Instantiate(spritePatter, btnNumberClone.transform);
                    imageClone.transform.GetComponent<Image>().sprite = SharedData.listAnimalSprite[randAnimalIndex];
                    imageClone.transform.position = new Vector3(initImageX + m * paddingImageX, initImageY - k * paddingImageY, 90);
                    imageClone.transform.localScale = new Vector3(imageScale, imageScale, 1);
                }
            }
            spritePatter.SetActive(false);
            listNumberButton.Add(btnNumberClone);
            btnNumberClone.transform.position = new Vector3(initX , initY  - counter * paddingY);
            btnNumberClone.transform.GetChild(2).GetComponent<Image>().transform.localScale = new Vector3(scaleWidth, scaleHeigth, 1);
            btnNumberClone.GetComponent<Button>().AddEventListener(counter, BtnNumberClicked);
        }
        btnNumberPattern.SetActive(false);
    }
    
    void ToHome()
    {
        SharedData.currentTestSentence = 1;
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(2).gameObject));
        if (SharedData.isFindingStarMode)
        {
            StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HomeScene"));
            return;
        }
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/StickerDetail"));
    }
    void ReplaySound()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(3).gameObject));
        AlertFindSound(isFindBig);
      
    }
    void Replay(float afterSecond)
    {
        if (listNumberButton != null)
        {
            
            if (listNumberButton.Count == 2)
            {
                foreach (GameObject go in listNumberButton)
                {
                    Destroy(go);
                }
            }
            listNumberButton.RemoveAt(0);
            listNumberButton.RemoveAt(0);
        }
        
        SharedData.isGameOn = true;
        if (SharedData.currentTestSentence > SharedData.totalTestSentence)
        {
            SharedData.currentTestSentence = 1;
            if (SharedData.isFindingStarMode)
            {
                StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HomeScene"));
            }
            else
            {
                StartCoroutine(SharedData.ToSceneAfterSomeTime(0.25f, "Scenes/StickerDetail"));
            }
        }
        UpdateNumberOfSentence();
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(ReloadNumber(afterSecond));
    }
    IEnumerator ReloadNumber(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        LoadAnimalBoardList();
    }
}
