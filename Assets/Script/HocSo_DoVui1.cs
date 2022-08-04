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
public class HocSo_DoVui1 : MonoBehaviour
{
    public AudioSource audioSource;
    public static Sprite[] listAnimalSprite;
    public List<GameObject> listNumberButton;
    private int correctIndex = 0;
    private int correctNumberIndexReal = 0;
    void Start()
    {
        listNumberButton = new List<GameObject>();
        GameObject btnHome = transform.GetChild(1).gameObject;
        audioSource = btnHome.AddComponent<AudioSource>();
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
        Debug.Log("You click on index:" + itemIndex);
        GameObject currentClickedNumber = transform.GetChild(5 + itemIndex).gameObject;
        if (itemIndex == correctIndex)
        {
            // Debug.Log("CORRECT!");
            currentClickedNumber.transform.GetChild(2).GetComponent<Image>().sprite = SharedData.listNumberBg[1];          
            SharedData.alertSoundCorrect(true, audioSource);
            StartCoroutine(ReplayAfterDelay(2.5f));
        } else
        {
            //Debug.Log("IN_CORRECT");
            SharedData.alertSoundCorrect(false, audioSource);
            currentClickedNumber.transform.GetChild(2).GetComponent<Image>().sprite = SharedData.listNumberBg[2];
        }
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(5 + itemIndex).gameObject));
    }
    public void SoundForCorrectNumber(int numberIndex)
    {
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
        Debug.Log("Correct index is: " + correctIndex);
        GameObject btnNumberPattern = transform.GetChild(4).gameObject;
        btnNumberPattern.SetActive(true);
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
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(1).gameObject));
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HocSoHomeScene"));
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
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(ReloadNumber(afterSecond));
        
    }
    IEnumerator ReloadNumber(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        LoadNumberList();
    }
}
