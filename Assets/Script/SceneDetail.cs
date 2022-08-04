using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class SceneDetail : MonoBehaviour
{
    public AudioSource audioSource;
    //public AudioClip[] clips;
   // public AudioClip number0Clip;
    public static Sprite[] listAnimalSprite;
    void LoadSoundClip()
    {
       // number0Clip = Resources.Load("Sound/SoundNumbers/sound1") as AudioClip;
       // clips = Resources.LoadAll("Sound/Number2", typeof(AudioClip)).Cast<AudioClip>().ToArray();
       // Debug.Log("Total sound is:" + clips.Length + " sound 0 is:" + clips[0].ToString());
    }
    void Start()
    {
        //Start things here
        LoadSoundClip();
       
        SetupSprites();
        Debug.Log("You click on item index: " + ListNumber.clickedItem);
        GameObject buttonTemplate = transform.GetChild(1).gameObject;
        //GameObject g;
        //g = Instantiate(buttonTemplate, transform);
        audioSource = buttonTemplate.AddComponent<AudioSource>();
        audioSource.PlayOneShot(SharedData.numberSound[ListNumber.clickedItem], 1f);
        buttonTemplate.transform.GetChild(0).GetComponent<Image>().sprite = ListNumber.sprites[ListNumber.clickedItem];
        GameObject homeTemplate = transform.GetChild(2).gameObject;
       // GameObject gHome = Instantiate(homeTemplate,transform);
        homeTemplate.GetComponent<Button>().onClick.AddListener(delegate()
        {
            ToHome();
        });

        if (Screen.height > 1.5f * Screen.width)
        {
           
            Debug.Log("Screen to long");
        } else
        {
            GameObject listImageBg = transform.GetChild(3).gameObject;
            listImageBg.transform.localScale = new Vector3(1.35f, 1.0f, 1.0f);
        }
       // Destroy(buttonTemplate);
       // Destroy(homeTemplate);
        LoadListAnimal();
    }
    void LoadListAnimal()
    {
        Debug.Log("You click on item index: " + ListNumber.clickedItem);
        int totalItem = ListNumber.clickedItem;
        int numRows = 3;
        int numCols = 3;
        float initX = -2.0f;
        float initY = -2.0f;
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
            paddingY = 1.3f;
            initY = -1.4f;
        } else if(totalItem > 3)
        {
            numRows = 2;
            numCols = 3;
        } else if(totalItem == 3)
        {
            numRows = 2;
            numCols = 2;
            scale = 1.5f;
            initX = -1.0f;
            paddingX = 2.5f;
        } else if(totalItem == 2)
        {
            scale = 2.0f;
            initX = -1.0f;
            initY = -2.5f;
            paddingX = 2.5f;
        } else if(totalItem == 1)
        {
            scale = 3.0f;
            initX = 0f;
            initY = -3.0f;
        }
       // int totalItem = 9;
        int animalIndex = 0;
        System.Random myObject = new System.Random();
        animalIndex = myObject.Next(0, 5);
        //Debug.Log("Screen ratio is: " + Screen.height / Screen.width);
        GameObject imageTemplate = transform.GetChild(4).gameObject;
        GameObject g;
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
                g.transform.GetComponent<Image>().sprite = listAnimalSprite[animalIndex];
                if (numCols == 2)
                {
                    g.transform.position = new Vector3(initX + (float)j * paddingX, initY - (float)i * paddingY);
                }
                else
                {
                    g.transform.position = new Vector3(initX + (float)j * paddingX, initY -  (float)i * paddingY);
                }
                g.transform.localScale = new Vector3(scale, scale, 1);
            }
        }
        Destroy(imageTemplate);
    }
    void SetupSprites()
    {
        listAnimalSprite = Resources.LoadAll("Animals", typeof(Sprite)).Cast<Sprite>().ToArray();
    }
    void ToHome()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(2).gameObject));
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/ListScene"));

      //  SceneManager.LoadScene("Scenes/ListScene");
    }
}
