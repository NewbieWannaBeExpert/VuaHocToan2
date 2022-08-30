using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
/*public static class ButtonExtension
{
    public static void AddEventListener<T>(this Button button, T param, Action<T> OnNumberClick)
    {
        button.onClick.AddListener(delegate ()
        {
            OnNumberClick(param);
        });
    }
}*/
public class TestDienSo : MonoBehaviour
{
    public AudioSource audioSource;
    public static Sprite[] listAnimalSprite;
    public List<GameObject> listNumberButton;
    private AudioClip soundAlertFind;
    private int correctIndex = 0;
    private int correctNumberIndexReal = 0;
    private int startButtonIndex = 3;
    private bool isGameOn = true;
   // private int currentSentence = 1;
   // private int totalSentence = 2;
    void Start()
    {
        UpdateNumberOfStar();
        soundAlertFind = Resources.Load<AudioClip>("Sound/Alerts/findResult");
        listNumberButton = new List<GameObject>();
        GameObject btnHome = transform.GetChild(startButtonIndex).gameObject;
        audioSource = btnHome.AddComponent<AudioSource>();
        btnHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            Debug.Log("Home clicked");
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(startButtonIndex).gameObject));
            ToHome();
        });
        LoadNumberList();
        UpdateNumberOfSentence();
        SoundForAlertFind();
    }
   void UpdateNumberOfSentence()
    {
        GameObject txtSen = transform.GetChild(6).transform.GetChild(1).gameObject;
        txtSen.gameObject.GetComponent<Text>().text = SharedData.currentTestSentence.ToString() + "/" + SharedData.totalTestSentence.ToString();
    }
    IEnumerator ReplayAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Replay(0.5f);
    }
    IEnumerator DestroyGameObjectAfterDelay(GameObject g, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(g);
    }
    void BtnNumberClicked(int itemIndex)
    {
        if(isGameOn == false)
        {
            return;
        }
        Debug.Log("You click on index:" + itemIndex);
        System.Random rand = new System.Random();
        
        GameObject currentClickedNumber = transform.GetChild(4 + itemIndex + startButtonIndex).gameObject;
        if (itemIndex == correctIndex)
        {
            isGameOn = false;
            // Debug.Log("CORRECT!");
            currentClickedNumber.transform.GetChild(2).GetComponent<Image>().sprite = SharedData.listNumberBgDoVui3[1];          
            SharedData.alertSoundCorrect(true, audioSource);
            StartCoroutine(ReplayAfterDelay(2.5f));
            int totalStar = SharedData.GetNumberOfStar();
            SharedData.SetNumberOfStar(totalStar + SharedData.starPlus);
            UpdateNumberOfStar();
            SharedData.currentTestSentence++;
        } else
        {
            int totalStar = SharedData.GetNumberOfStar();
            if(totalStar > 0)
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
            //Debug.Log("IN_CORRECT");
            SharedData.alertSoundCorrect(false, audioSource);
            currentClickedNumber.transform.GetChild(2).GetComponent<Image>().sprite = SharedData.listNumberBgDoVui3[2];
        }

        //Show Flying star
        GameObject coinStatus = transform.GetChild(5).gameObject;
        GameObject moneyImage = currentClickedNumber.transform.GetChild(4).gameObject;
        moneyImage.SetActive(true);
        GameObject moneyDuplicated = Instantiate(moneyImage, transform);
        moneyDuplicated.transform.position = moneyImage.transform.position;
        LeanTween.cancel(moneyDuplicated);
        moneyDuplicated.transform.localScale = new Vector3(0.7f, 0.7f, 1.0f);
        LeanTween.scale(moneyDuplicated, new Vector3(1.0f, 1.0f, 1.0f), 1.0f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.move(moneyDuplicated, coinStatus.transform.position, 0.5f).setEaseInBack();
        StartCoroutine(DestroyGameObjectAfterDelay(moneyDuplicated, 0.7f));
        //Destroy(moneyImage);
        moneyImage.SetActive(false);
        //Do some other stuffs
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(4 + itemIndex + startButtonIndex).gameObject));
    }
    void UpdateNumberOfStar()
    {
        GameObject g = transform.GetChild(5).gameObject;
        g.transform.GetChild(1).GetComponent<Text>().text = SharedData.GetNumberOfStar().ToString();
    }
    public string GenerateMathEquotion(int forResult)
    {
        string result = "";
        System.Random rand = new System.Random();   
        if(forResult < 4)
        {
            int num1 = rand.Next(9);
            int num2 = num1 + forResult;
            while(num2 > 9 || num1 == 0)
            {
                num1 = rand.Next(9);
                num2 = num1 + forResult;
            }
            result = num2 + " - " + num1 + " = ?";
        } else
        {
            int num1 = rand.Next(9);
            int num2 =  forResult - num1;
            while (num2 < 0 || num1 == 0)
            {
                num1 = rand.Next(9);
                num2 = forResult - num1;
            }
            result = num1 + " + " + num2 + " = ?";
        }
        return result;
    }
    public void SoundForAlertFind()
    {
        audioSource.PlayOneShot(soundAlertFind,1.0f);
        //audioSource.PlayOneShot(SharedData.numberSound[numberIndex]);
    }
    void LoadNumberList()
    {
        int totalItem = 4;
        int numRows = 2;
        int numCols = 2;
        float initX = -1.2f;
        float initY = -0.3f;
        float paddingX = 2.3f;
        float paddingY = 2.3f;
        float scale = 1.0f;
        if (Screen.height > 1.5f * Screen.width)
        {
            numCols = 2;
            numRows = 2;
        }
        int num1,num2,num3,num4;
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
        num4 = myObject.Next(0, 9); 
        while( num4 == num1 || num4 == num2 || num4 == num3)
        {
            num4 = myObject.Next(0, 9);
        }
        correctIndex = myObject.Next(0, 4);
        string equotion = "";
        int correctValue = 0;
        if(correctIndex == 0)
        {
            //SoundForCorrectNumber(num1);
            correctNumberIndexReal = num1;
            correctValue = num1;
        } else if(correctIndex == 1)
        {
            //SoundForCorrectNumber(num2);
            correctNumberIndexReal = num2;
            correctValue = num2;
        }
        else if(correctIndex == 2)
        {
            correctNumberIndexReal = num3;
            //SoundForCorrectNumber(num3);
            correctValue = num3;
        }
        else if (correctIndex == 3)
        {
            correctNumberIndexReal = num4;
            //SoundForCorrectNumber(num4);
            correctValue = num4;
        }
        equotion = GenerateMathEquotion(correctValue);
        transform.GetChild(startButtonIndex-1).GetComponent<Text>().text = equotion;
        GameObject btnNumberPattern = transform.GetChild(startButtonIndex + 1).gameObject;
        btnNumberPattern.SetActive(true);
        GameObject btnNumberClone;
        int counter = 0;
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                if (i * numCols + j * numRows >= totalItem)
                {
                    //break;
                }
                
                btnNumberClone = Instantiate(btnNumberPattern, transform);
                btnNumberClone.transform.GetChild(2).GetComponent<Image>().sprite = SharedData.listNumberBgDoVui3[0];//bg normal
                btnNumberClone.transform.GetChild(0).GetComponent<Image>().sprite = SharedData.listNumberBgDoVui3[1];//bg right
                btnNumberClone.transform.GetChild(1).GetComponent<Image>().sprite = SharedData.listNumberBgDoVui3[2];//bg wrong
                GameObject moneyObj = btnNumberClone.transform.GetChild(4).gameObject;
                moneyObj.SetActive(false);
                if (counter == 0)
                {
                    btnNumberClone.transform.GetChild(3).GetComponent<Image>().sprite = SharedData.listNumberDoVui3[num1];
                } else if(counter == 1)
                {
                    btnNumberClone.transform.GetChild(3).GetComponent<Image>().sprite = SharedData.listNumberDoVui3[num2];
                } else if(counter == 2)
                {
                    btnNumberClone.transform.GetChild(3).GetComponent<Image>().sprite = SharedData.listNumberDoVui3[num3];
                }
                else if (counter == 3)
                {
                    btnNumberClone.transform.GetChild(3).GetComponent<Image>().sprite = SharedData.listNumberDoVui3[num4];
                }
                listNumberButton.Add(btnNumberClone);
                btnNumberClone.transform.position = new Vector3(initX + (float)j * paddingX , initY - (float)i * paddingY);
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
        if(SharedData.isFindingStarMode)
        {
            StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HomeScene"));
            return;
        }
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/StickerDetail"));
    }
    void ReplaySound()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        SoundForAlertFind();
    }
    void Replay(float afterSecond)
    {
        isGameOn = true;
        if (listNumberButton != null)
        {
            foreach (GameObject go in listNumberButton)
            {
                Destroy(go);
            }
            for(int i = 0; i < listNumberButton.Count; i++)
            {
                listNumberButton.RemoveAt(0);
            }
        }
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(ReloadNumber(afterSecond));
    }
    IEnumerator ReloadNumber(float waitSeconds)
    {
        if (SharedData.currentTestSentence > SharedData.totalTestSentence)
        {
            SharedData.currentTestSentence = 1;
            if (SharedData.isFindingStarMode)
            {
                StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HomeScene"));
            } else
            {
                StartCoroutine(SharedData.ToSceneAfterSomeTime(0.25f, "Scenes/StickerDetail"));
            }
        }
        yield return new WaitForSeconds(waitSeconds);
        LoadNumberList();
        UpdateNumberOfSentence();
    }
}
