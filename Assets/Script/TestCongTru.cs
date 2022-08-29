using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class TestCongTru : MonoBehaviour
{
    public AudioSource audioSource;
    public static Sprite[] listAnimalSprite;
    public List<GameObject> listNumberButton;
    private int correctIndex = 0;
    private AudioClip alertSoundFindResult;
    private int correctNumberIndexReal = 0;
    private int startButtonIndex = 3;
    void Start()
    {
        alertSoundFindResult = Resources.Load<AudioClip>("Sound/Alerts/findWithResult");
        listNumberButton = new List<GameObject>();
        GameObject btnHome = transform.GetChild(startButtonIndex).gameObject;
        audioSource = btnHome.AddComponent<AudioSource>();
        btnHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(btnHome));
            ToHome();
        });
        GameObject btnSound = transform.GetChild(startButtonIndex + 1).gameObject;
        btnSound.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(btnSound));
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
        System.Random r = new System.Random();
        GameObject currentClickedNumber = transform.GetChild(4 + itemIndex + startButtonIndex).gameObject;
        if (itemIndex == correctIndex)
        {
            int randCorrect = r.Next(5, 8);
            GameObject imgBoyGirl = transform.GetChild(startButtonIndex - 1).gameObject;
            imgBoyGirl.GetComponent<Image>().sprite = SharedData.listBoyAnimated[randCorrect];
            currentClickedNumber.transform.GetChild(2).GetComponent<Image>().sprite = SharedData.listNumberBgLamToan[1];          
            SharedData.alertSoundCorrect(true, audioSource);
            StartCoroutine(ReplayAfterDelay(2.5f));
            ShakeBoyGirl(imgBoyGirl);
        }
        else
        {
            int randIncorrect = r.Next(1, 5);
            GameObject imgBoyGirl = transform.GetChild(startButtonIndex - 1).gameObject;
            imgBoyGirl.GetComponent<Image>().sprite = SharedData.listBoyAnimated[randIncorrect];
            //Debug.Log("IN_CORRECT");
            SharedData.alertSoundCorrect(false, audioSource);
            currentClickedNumber.transform.GetChild(2).GetComponent<Image>().sprite = SharedData.listNumberBgLamToan[2];
            ShakeBoyGirl(imgBoyGirl);
        }
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(4 + itemIndex + startButtonIndex).gameObject));
    }
    void ShakeBoyGirl(GameObject g)
    {
        LeanTween.cancel(g);
        g.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        LeanTween.scale(g.gameObject, new Vector3(1.5f, 1.5f), 1.0f).setEase(LeanTweenType.punch);
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
            result = num2 + " - " + num1;
        } else
        {
            int num1 = rand.Next(9);
            int num2 =  forResult - num1;
            while (num2 < 0 || num1 == 0)
            {
                num1 = rand.Next(9);
                num2 = forResult - num1;
            }
            result = num1 + " + " + num2;
        }
        return result;
    }
    public void SoundForCorrectNumber(int numberIndex)
    {
        audioSource.PlayOneShot(alertSoundFindResult, 1.0f) ;
        StartCoroutine(SoundForCorrectNumberAfterDelay(numberIndex, 2.5f));
    }
    IEnumerator SoundForCorrectNumberAfterDelay(int numberIndex,float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        audioSource.PlayOneShot(SharedData.numberOnlySound[numberIndex]);
        //audioSource.PlayOneShot(SharedData.numberSound[numberIndex]);
    }
    void LoadNumberList()
    {
        GameObject imgBoyGirl = transform.GetChild(startButtonIndex - 1).gameObject;
        if (HomeLamToan.CongTruNum == 2)
        {
            imgBoyGirl.GetComponent<Image>().sprite = SharedData.listBoyAnimated[0];
        } else
        {
            imgBoyGirl.SetActive(false);
        }
            
        int totalItem = HomeLamToan.CongTruNum;
        float initX = 0f;
        float initY = 1.5f;
        float paddingY = 2.3f;
        float scale = 0.87f;
        if (Screen.height > 1.5f * Screen.width)
        {
           
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
        if(HomeLamToan.CongTruNum == 3)
        {
            correctIndex = myObject.Next(0, 3);
        } else
        {
            correctIndex = myObject.Next(0, 2);
        }
        
        string equotion = "";
        int correctValue = 0;
        if(correctIndex == 0)
        {
            SoundForCorrectNumber(num1);
            correctNumberIndexReal = num1;
            correctValue = num1;
        } else if(correctIndex == 1)
        {
            SoundForCorrectNumber(num2);
            correctNumberIndexReal = num2;
            correctValue = num2;
        }
        else if(correctIndex == 2)
        {
            correctNumberIndexReal = num3;
            SoundForCorrectNumber(num3);
            correctValue = num3;
        }
        GameObject btnNumberPattern = transform.GetChild(startButtonIndex + 3).gameObject;
        btnNumberPattern.SetActive(true);
        GameObject btnNumberClone;
      
        for (int i = 0; i < totalItem; i++)
        {
            btnNumberClone = Instantiate(btnNumberPattern, transform);
            btnNumberClone.transform.GetChild(2).GetComponent<Image>().sprite = SharedData.listNumberBgLamToan[0];//bg normal
            btnNumberClone.transform.GetChild(0).GetComponent<Image>().sprite = SharedData.listNumberBgLamToan[1];//bg right
            btnNumberClone.transform.GetChild(1).GetComponent<Image>().sprite = SharedData.listNumberBgLamToan[2];//bg wrong
            if (i == 0)
            {
                equotion = GenerateMathEquotion(num1);
                
            } else if(i == 1)
            {
                equotion = GenerateMathEquotion(num2);
               
            } else if(i == 2)
            {
                equotion = GenerateMathEquotion(num3);
            }
            btnNumberClone.transform.GetChild(3).GetComponent<Text>().text = equotion;
            listNumberButton.Add(btnNumberClone);
            btnNumberClone.transform.position = new Vector3(initX , initY - (float)i * paddingY);
            btnNumberClone.transform.localScale = new Vector3(scale, scale, 1);
            btnNumberClone.GetComponent<Button>().AddEventListener(i, BtnNumberClicked);
        }
        btnNumberPattern.SetActive(false);
    }
    
    void ToHome()
    {
        SharedData.currentTestSentence = 1;
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
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
        SoundForCorrectNumber(correctNumberIndexReal);
    }
    void Replay(float afterSecond)
    {
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
            }
            else
            {
                StartCoroutine(SharedData.ToSceneAfterSomeTime(0.25f, "Scenes/StickerDetail"));
            }
        }
        yield return new WaitForSeconds(waitSeconds);
        LoadNumberList();
    }
}
