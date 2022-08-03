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
public class HocSo_DoVui2 : MonoBehaviour
{
    public AudioSource audioSource;
    public static Sprite[] listAnimalSprite;
    public List<GameObject> listNumberButton;
    public static Sprite[] listBtnBg;
    private int correctIndex = 0;
    private int correctNumberIndexReal = 0;
    private float maxRatios = 228 / 108;
    void SetupGraphics()
    {
        listBtnBg = Resources.LoadAll("HocSo_DoVui2/BtnBg", typeof(Sprite)).Cast<Sprite>().ToArray();
    }
    void RescaleBgImage()
    {
        GameObject bgFrame = transform.GetChild(1).gameObject;
        Debug.Log("Frame width:" + bgFrame.GetComponent<RectTransform>().rect.width + " height:" + bgFrame.GetComponent<RectTransform>().rect.height);
        Debug.Log("Screen with: " + Screen.width + " Screen height:" + Screen.height);
        float scaleX =  Screen.width/ bgFrame.GetComponent<RectTransform>().rect.width * 0.27f;
        float scaleY = Screen.height / bgFrame.GetComponent<RectTransform>().rect.height * 0.3f;
        //bgFrame.transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
    void Start()
    {
        SetupGraphics();
        //RescaleBgImage();
        listNumberButton = new List<GameObject>();
        GameObject btnHome = transform.GetChild(2).gameObject;
        audioSource = btnHome.AddComponent<AudioSource>();
        btnHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToHome();
        });
        GameObject btnReplay = transform.GetChild(3).gameObject;
        btnReplay.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            Replay(0.5f);
            StartCoroutine(SharedData.MyCoroutine(transform.GetChild(3).gameObject));
        });
        GameObject btnSound = transform.GetChild(4).gameObject;
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
        Debug.Log("You click on index:" + itemIndex);
        GameObject currentClickedNumber = transform.GetChild(6 + itemIndex).gameObject;
        if (itemIndex == correctIndex)
        {
            // Debug.Log("CORRECT!");
            currentClickedNumber.transform.GetChild(2).GetComponent<Image>().sprite = listBtnBg[1];          
            SharedData.alertSoundCorrect(true, audioSource);
            StartCoroutine(ReplayAfterDelay(2.5f));
        } else
        {
            //Debug.Log("IN_CORRECT");
            SharedData.alertSoundCorrect(false, audioSource);
            currentClickedNumber.transform.GetChild(2).GetComponent<Image>().sprite = listBtnBg[2];
        }
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.MyCoroutine(transform.GetChild(6 + itemIndex).gameObject));
    }
    public void SoundForCorrectNumber(int numberIndex)
    {
        audioSource.PlayOneShot(SharedData.numberSound[numberIndex]);
    }
   
    void LoadAnimalBoardList()
    {
        int totalItem = 2;
        int numRows = 2;
        int numCols = 1;
        float initX = 0f;
        float initY = 1.5f;
        float paddingX = 2.0f;
        float paddingY = 3.5f;
        GameObject btnNumberPattern = transform.GetChild(5).gameObject;
        btnNumberPattern.SetActive(true);
        float scaleWidth=1.0f, scaleHeigth=1.0f;
        float buttonWidth = btnNumberPattern.GetComponent<RectTransform>().rect.width;
        float buttonHeight = btnNumberPattern.GetComponent<RectTransform>().rect.height;
       // scaleWidth = (Screen.width / buttonWidth) * (100 / Screen.dpi)  * (800/Screen.width);
        //scaleHeigth = Screen.height / buttonHeight * 0.063f;
       // Debug.Log("buttonWidth is: " + buttonWidth + "Scale width is: " + scaleWidth);
        Debug.Log("Screen dpi:" + Screen.dpi + ", width:" + Screen.width + ", height:" + Screen.currentResolution.height);
        Debug.Log("Screen width is: " + Screen.width + " buttonWith is:" + buttonWidth + " Screen height:" + Screen.height);
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
        Debug.Log("Correct index is: " + correctIndex);
       
        GameObject btnNumberClone;
        int counter = 0;
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                Debug.Log("Generate number for colum: " + i + " and row: " + j + " for index:" + (i * numCols + j));
                if (i * numCols + j >= totalItem)
                {
                    break;
                }
               
                btnNumberClone = Instantiate(btnNumberPattern, transform);
                btnNumberClone.transform.GetChild(2).GetComponent<Image>().sprite = listBtnBg[0];//bg normal
                btnNumberClone.transform.GetChild(0).GetComponent<Image>().sprite = listBtnBg[1];//bg right
                btnNumberClone.transform.GetChild(1).GetComponent<Image>().sprite = listBtnBg[2];//bg wrong
                
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
                
                listNumberButton.Add(btnNumberClone);
               // listButton[counter] = btnNumberClone;
               
                btnNumberClone.transform.position = new Vector3(initX + (float)j * paddingX , initY  - (float)i * paddingY);
                //btnNumberClone.transform.localScale = new Vector3(scale, scale, 1);
                //var spriteSize = btnNumberClone.GetComponent<Button>().GetComponent<SpriteRenderer>().bounds.size;
                //var scaleFactorX = Screen.width / spriteSize.x;
                //Debug.Log("LOcal scale x: " + scaleFactorX);
                btnNumberClone.transform.GetChild(2).GetComponent<Image>().transform.localScale = new Vector3(scaleWidth, scaleHeigth, 1);
                btnNumberClone.GetComponent<Button>().AddEventListener(counter, BtnNumberClicked);
                counter++;
            }
        }
        btnNumberPattern.SetActive(false);
    }
    
    void ToHome()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.MyCoroutine(transform.GetChild(1).gameObject));
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HocSoHomeScene"));
    }
    void ReplaySound()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.MyCoroutine(transform.GetChild(4).gameObject));
        SoundForCorrectNumber(correctNumberIndexReal);
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
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(ReloadNumber(afterSecond));
        
    }
    IEnumerator ReloadNumber(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        LoadAnimalBoardList();
    }
}
