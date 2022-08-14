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
public class HocSo_DoVui4 : MonoBehaviour
{
    public AudioSource audioSource;
    public static Sprite[] listAnimalSprite;
    public List<GameObject> listNumberButton;
    public List<GameObject> listDisplayedAnimalImage;
    public AudioClip[] listAlertFindHowManySound;
    private int correctIndex = 0;
    private int correctNumberIndexReal = 0;
    private int startButtonIndex = 3;
   // int correctValue = 0;
    void Start()
    {
        listAlertFindHowManySound = new AudioClip[2];
        listAlertFindHowManySound[0] = Resources.Load<AudioClip>("Sound/Alerts/alertHowManyAnimals");
        listAlertFindHowManySound[1] = Resources.Load<AudioClip>("Sound/Alerts/alertHowManyAnimals3");
        listNumberButton = new List<GameObject>();
        listDisplayedAnimalImage = new List<GameObject>();
        GameObject btnHome = transform.GetChild(startButtonIndex).gameObject;
        audioSource = btnHome.AddComponent<AudioSource>();
        btnHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            Debug.Log("Home clicked");
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(startButtonIndex).gameObject));
            ToHome();
        });
        GameObject btnReplay = transform.GetChild(startButtonIndex + 1).gameObject;
        btnReplay.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            Debug.Log("Replay clicked");
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(startButtonIndex + 1).gameObject));
            Replay(0.5f);
        });
        GameObject btnSound = transform.GetChild(startButtonIndex + 2).gameObject;
        btnSound.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            Debug.Log("Sound clicked");
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(startButtonIndex + 2).gameObject));
            ReplaySound();
        });
        LoadNumberList();
        SoundForAlertFind();

    }
    IEnumerator ReplayAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Replay(0.5f);
        
    }
    void BtnNumberClicked(int itemIndex)
    {
        Debug.Log("You click on index:" + itemIndex);
        GameObject currentClickedNumber = transform.GetChild(4 + itemIndex + startButtonIndex).gameObject;
        if (itemIndex - correctNumberIndexReal == correctIndex)
        {
            // Debug.Log("CORRECT!");
            currentClickedNumber.transform.GetChild(2).GetComponent<Image>().sprite = SharedData.listNumberBgDoVui3[1];          
            SharedData.alertSoundCorrect(true, audioSource);
            StartCoroutine(ReplayAfterDelay(2.5f));
        } else
        {
            //Debug.Log("IN_CORRECT");
            SharedData.alertSoundCorrect(false, audioSource);
            currentClickedNumber.transform.GetChild(2).GetComponent<Image>().sprite = SharedData.listNumberBgDoVui3[2];
        }
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(4 + itemIndex + startButtonIndex).gameObject));
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
        System.Random rand = new System.Random();
        int randIndex = rand.Next(0, 2);
        Debug.Log("Rand index for finding is: " + randIndex);
        audioSource.PlayOneShot(listAlertFindHowManySound[randIndex], 1.0f);
        //audioSource.PlayOneShot(SharedData.numberSound[numberIndex]);
    }
   
    void LoadNumberList()
    {
        int totalItem = 4;
        int numRows = 2;
        int numCols = 2;
        float initX = -1.2f;
        float initY = -1.6f;
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
        
        
        if(correctIndex == 0)
        {
            //SoundForCorrectNumber(num1);
            correctNumberIndexReal = num1;
           
        } else if(correctIndex == 1)
        {
            //SoundForCorrectNumber(num2);
            correctNumberIndexReal = num2;
           
        }
        else if(correctIndex == 2)
        {
            correctNumberIndexReal = num3;
            //SoundForCorrectNumber(num3);
        
        }
        else if (correctIndex == 3)
        {
            correctNumberIndexReal = num4;
            //SoundForCorrectNumber(num4);
           
        }
        LoadListAnimal(correctNumberIndexReal);
       
        Debug.Log("Correct index is: " + correctNumberIndexReal);
        GameObject btnNumberPattern = transform.GetChild(startButtonIndex + 3).gameObject;
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
                btnNumberClone.GetComponent<Button>().AddEventListener(counter + correctNumberIndexReal, BtnNumberClicked);
                counter++;
            }
        }
        btnNumberPattern.SetActive(false);
    }


    void LoadListAnimal(int totalItem)
    {
        Debug.Log("Have to show the number of animal: " + totalItem);
        int numRows = 3;
        int numCols = 3;
        float initX = -2.0f;
        float initY = 2.0f;
        float paddingX = 2.0f;
        float paddingY = 1.75f;
        float scale = 1.0f;
        if (Screen.height > 1.5f * Screen.width)
        {
            numCols = 3;
            numRows = 3;
            Debug.Log("Screen to long");
        }
        if (totalItem > 6)
        {
            initY = 2.4f;
            paddingY = 1.0f;
            paddingX = 1.86f;
            initX = -1.75f;
            scale = 0.86f;
        }
        else if (totalItem > 3)
        {
            numRows = 2;
            numCols = 3;
            initY = 2.5f;
            initX = -1.56f;
            paddingX = 1.63f;
        }
        else if (totalItem == 3)
        {
            numRows = 2;
            numCols = 2;
            scale = 1.5f;
            initX = -1.0f;
            initY = 2.5f;
            paddingX = 2.5f;
        }
        else if (totalItem == 2)
        {
            scale = 1.86f;
            initX = -1.0f;
            initY = 1.86f;
            paddingX = 2.4f;
        }
        else if (totalItem == 1)
        {
            scale = 3.0f;
            initX = 0f;
            initY = 1.5f;
        }
        // int totalItem = 9;
        int animalIndex = 0;
        System.Random myObject = new System.Random();
        animalIndex = myObject.Next(0, 5);
        //Debug.Log("Screen ratio is: " + Screen.height / Screen.width);
        GameObject imageTemplate = transform.GetChild(startButtonIndex - 1).gameObject;
        GameObject g;
        imageTemplate.SetActive(true);
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                Debug.Log("Generate number for colum: " + i + " and row: " + j + " for index:" + (i * numCols + j));
                if (i * numCols + j >= totalItem)
                {
                    break;
                }
                g = Instantiate(imageTemplate, transform);
                g.transform.GetComponent<Image>().sprite = SharedData.listAnimalSprite[animalIndex];
                if (numCols == 2)
                {
                    g.transform.position = new Vector3(initX + (float)j * paddingX, initY - (float)i * paddingY);
                }
                else
                {
                    g.transform.position = new Vector3(initX + (float)j * paddingX, initY - (float)i * paddingY);
                }
                g.transform.localScale = new Vector3(scale, scale, 1);
                listDisplayedAnimalImage.Add(g);
                Debug.Log("List animal image size:" + listDisplayedAnimalImage.Count);
            }
        }
        imageTemplate.SetActive(false);
    }
    void ToHome()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/LamToanHomeScene"));
    }
    
    void ReplaySound()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        SoundForAlertFind();
    }
    void Replay(float afterSecond)
    {
        if (listNumberButton != null)
        {
            foreach (GameObject go in listNumberButton)
            {
                Destroy(go);
            }
            int totalButton = listNumberButton.Count;   
            for(int i = 0; i < totalButton; i++)
            {
                Debug.Log("Remove button number " + i);
                listNumberButton.RemoveAt(0);
            }
        }
        
        if(listDisplayedAnimalImage != null)
        {
            int totalAnimal = listDisplayedAnimalImage.Count;
            Debug.Log("Remove image in list image with total : " + listDisplayedAnimalImage.Count);
            foreach(GameObject go in listDisplayedAnimalImage)
            {
                Destroy(go);
            }
            for(int j = 0; j < totalAnimal;j++)
            {
                Debug.Log("Remove image number " + j);
                listDisplayedAnimalImage.RemoveAt(0);
            }
        }
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(ReloadNumber(afterSecond));
        SoundForAlertFind();
    }
    IEnumerator ReloadNumber(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        LoadNumberList();
    }
}
