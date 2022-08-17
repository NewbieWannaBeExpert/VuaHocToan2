using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class TestDoVui1 : MonoBehaviour
{
    public AudioSource audioSource;
    public static Sprite[] listAnimalSprite;
    public static AudioClip alertFindSound;
    public List<GameObject> listNumberButton;
    private int correctIndex = 0;
    private int correctNumberIndexReal = 0;
    
    void initAll()
    {
        alertFindSound = Resources.Load<AudioClip>("Sound/Alerts/alertFind");
        UpdateNumberOfStar();
        UpdateNumberOfSentence();
    }
    void ShowFlyingStar(GameObject g)
    {
        //Show Flying star
        GameObject coinStatus = transform.GetChild(4).gameObject;
        GameObject moneyImage = g.transform.GetChild(4).gameObject;
        moneyImage.SetActive(true);
        GameObject moneyDuplicated = Instantiate(moneyImage, transform);
        moneyDuplicated.transform.position = moneyImage.transform.position;
        LeanTween.cancel(moneyDuplicated);
        moneyDuplicated.transform.localScale = new Vector3(0.7f, 0.7f, 1.0f);
        LeanTween.scale(moneyDuplicated, new Vector3(1.0f, 1.0f, 1.0f), 1.0f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.move(moneyDuplicated, coinStatus.transform.position, 0.5f).setEaseInBack();
        StartCoroutine(SharedData.DestroyGameObjectAfterDelay(moneyDuplicated, 0.7f));
        //Destroy(moneyImage);
        moneyImage.SetActive(false);
    }
    void UpdateNumberOfSentence()
    {
        GameObject txtSen = transform.GetChild(5).transform.GetChild(1).gameObject;
        txtSen.gameObject.GetComponent<Text>().text = SharedData.currentTestSentence.ToString() + "/" + SharedData.totalTestSentence.ToString();
    }
    void UpdateNumberOfStar()
    {
        GameObject g = transform.GetChild(4).gameObject;
        g.transform.GetChild(1).GetComponent<Text>().text = SharedData.GetNumberOfStar().ToString();
    }
    void Start()
    {
        initAll();
        
        listNumberButton = new List<GameObject>();
        GameObject btnHome = transform.GetChild(1).gameObject;
        audioSource = btnHome.AddComponent<AudioSource>();
        audioSource.PlayOneShot(alertFindSound,1.0f);
        btnHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToHome();
        });
        GameObject btnReplay = transform.GetChild(2).gameObject;
        btnReplay.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            Replay(0.5f);
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(2).gameObject));
        });
        GameObject btnSound = transform.GetChild(3).gameObject;
        btnSound.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ReplaySound();
        });
        LoadNumberList();

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
        GameObject currentClickedNumber = transform.GetChild(6 + itemIndex).gameObject;
        if (itemIndex == correctIndex)
        {
            SharedData.isGameOn = false;
            // Debug.Log("CORRECT!");
            currentClickedNumber.transform.GetChild(2).GetComponent<Image>().sprite = SharedData.listNumberBg[1];          
            SharedData.alertSoundCorrect(true, audioSource);
            int totalStar = SharedData.GetNumberOfStar();
            SharedData.SetNumberOfStar(totalStar + 5);
            UpdateNumberOfStar();
            SharedData.currentTestSentence++;
            StartCoroutine(ReplayAfterDelay(2.5f));
        } else
        {
            int totalStar = SharedData.GetNumberOfStar();
            if (totalStar > 0)
            {
                SharedData.SetNumberOfStar(totalStar - 5);
                UpdateNumberOfStar();
            }
            //Debug.Log("IN_CORRECT");
            SharedData.alertSoundCorrect(false, audioSource);
            currentClickedNumber.transform.GetChild(2).GetComponent<Image>().sprite = SharedData.listNumberBg[2];
        }
        
        ShowFlyingStar(currentClickedNumber);
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(currentClickedNumber));
    }
    public void SoundForCorrectNumber(int numberIndex)
    {
        StartCoroutine(PlayNumberSound(numberIndex, 1.73f));
    }
    IEnumerator PlayNumberSound(int numberIndex, float afterDelay)
    {
        yield return new WaitForSeconds(afterDelay);
        audioSource.PlayOneShot(SharedData.numberSound[numberIndex]);
    }
    void LoadNumberList()
    {
        int totalItem = 3;
        int numRows = 3;
        int numCols = 1;
        float initX = 0f;
        float initY = 1.5f;
        float paddingX = 2.0f;
        float paddingY = 2.3f;
        float variantMaxY = 0.03f;
        float variantMaxX = 0.1f;
        float scale = 1.0f;
        if (Screen.height > 1.5f * Screen.width)
        {
            numCols = 1;
            numRows = 3;
            Debug.Log("Screen to long");
        }
        int num1,num2,num3;
        System.Random myObject = new System.Random();
        num1 = myObject.Next(1, 9);
        num2 = myObject.Next(1, 9);
        while(num2 == num1)
        {
            num2 = myObject.Next(1, 9);
        }
        num3 = myObject.Next(0, 9); 
        while(num3 == num2 || num3 == num1)
        {
            num3 = myObject.Next(0, 9);
        }
        correctIndex = myObject.Next(0, 3);
        if(correctIndex == 0)
        {
            SoundForCorrectNumber(num1);
            correctNumberIndexReal = num1;
        } else if(correctIndex == 1)
        {
            SoundForCorrectNumber(num2);
            correctNumberIndexReal = num2;
        } else if(correctIndex == 2)
        {
            correctNumberIndexReal = num3;
            SoundForCorrectNumber(num3);
        }
       // Debug.Log("Correct index is: " + correctIndex);
        GameObject btnNumberPattern = transform.GetChild(3).gameObject;
        btnNumberPattern.SetActive(true);
        GameObject btnNumberClone;
        int counter = 0;
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
               // Debug.Log("Generate number for colum: " + i + " and row: " + j + " for index:" + (i * numCols + j));
                if (i * numCols + j >= totalItem)
                {
                    break;
                }
                int variant = myObject.Next(0, 9);
                float mul = 1.0f;
                if(variant % 2 == 0)
                {
                    mul = -1.0f;
                }
                btnNumberClone = Instantiate(btnNumberPattern, transform);
                btnNumberClone.transform.GetChild(2).GetComponent<Image>().sprite = SharedData.listNumberBg[0];//bg normal
                btnNumberClone.transform.GetChild(0).GetComponent<Image>().sprite = SharedData.listNumberBg[1];//bg right
                btnNumberClone.transform.GetChild(1).GetComponent<Image>().sprite = SharedData.listNumberBg[2];//bg wrong
                if (counter == 0)
                {
                    btnNumberClone.transform.GetChild(3).GetComponent<Image>().sprite = SharedData.listNumberDoVui[num1];
                } else if(counter == 1)
                {
                    btnNumberClone.transform.GetChild(3).GetComponent<Image>().sprite = SharedData.listNumberDoVui[num2];
                } else if(counter == 2)
                {
                    btnNumberClone.transform.GetChild(3).GetComponent<Image>().sprite = SharedData.listNumberDoVui[num3];
                }
                GameObject moneyImage = btnNumberClone.transform.GetChild(4).gameObject;
                moneyImage.SetActive(false);
                listNumberButton.Add(btnNumberClone);
               // listButton[counter] = btnNumberClone;
               
                btnNumberClone.transform.position = new Vector3(initX + (float)j * paddingX + mul * (float)variant * variantMaxX, initY + mul * (float) variant * variantMaxY - (float)i * paddingY);
                btnNumberClone.transform.localScale = new Vector3(scale, scale, 1);
                btnNumberClone.GetComponent<Button>().AddEventListener(counter, BtnNumberClicked);
                counter++;
            }
        }
        btnNumberPattern.SetActive(false);
    }
    
    void ToHome()
    {
        SharedData.currentTestSentence = 1;
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(1).gameObject));
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.25f, "Scenes/StickerDetail"));
    }
    void ReplaySound()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(3).gameObject));
        SoundForCorrectNumber(correctNumberIndexReal);
    }
    void Replay(float afterSecond)
    {
        if (listNumberButton != null)
        {
            
            if (listNumberButton.Count == 3)
            {
                foreach (GameObject go in listNumberButton)
                {
                    Destroy(go);
                }
            }
            listNumberButton.RemoveAt(0);
            listNumberButton.RemoveAt(0);
            listNumberButton.RemoveAt(0);
        }
        SharedData.isGameOn = true;
        audioSource.PlayOneShot(alertFindSound, 1.0f);
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        if (SharedData.currentTestSentence > SharedData.totalTestSentence)
        {
            StartCoroutine(SharedData.ToSceneAfterSomeTime(0.25f, "Scenes/StickerDetail"));
        }
        StartCoroutine(ReloadNumber(afterSecond));
        
    }
    IEnumerator ReloadNumber(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        
        UpdateNumberOfSentence();
        LoadNumberList();
    }
}
