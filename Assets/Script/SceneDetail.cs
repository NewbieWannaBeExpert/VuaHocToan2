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
    public static Sprite[] listAnimalSprite;
    int startButtonIndex = 3;
    public bool isReplay = false;
    private List<GameObject> listDisplayedAnimalSprites = new List<GameObject>();
    void Start()
    {
        listDisplayedAnimalSprites = new List<GameObject> ();
        SetupSprites();
        Debug.Log("You click on item index: " + ListNumber.clickedItem);
        GameObject buttonTemplate = transform.GetChild(startButtonIndex+1).gameObject;
        //GameObject g;
        //g = Instantiate(buttonTemplate, transform);
        audioSource = buttonTemplate.AddComponent<AudioSource>();
        buttonTemplate.transform.GetChild(0).GetComponent<Image>().sprite = ListNumber.sprites[ListNumber.clickedItem];
        GameObject btnHome = transform.GetChild(startButtonIndex).gameObject;
       // GameObject gHome = Instantiate(homeTemplate,transform);
        btnHome.GetComponent<Button>().onClick.AddListener(delegate()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(startButtonIndex).gameObject));
            ToHome();
        });
        GameObject btnReplay = transform.GetChild(startButtonIndex-2).gameObject;
        btnReplay.GetComponent<Button>().onClick.AddListener(delegate () {
            isReplay = true;
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(startButtonIndex-2).gameObject));
            ReloadAnimalList();
        });
        if (Screen.height > 1.5f * Screen.width)
        {
        } else
        {
            GameObject listImageBg = transform.GetChild(startButtonIndex + 1).gameObject;
            listImageBg.transform.localScale = new Vector3(1.35f, 1.0f, 1.0f);
        }
        InvokeRepeating("ReloadAnimalList", 5f, 5f);
        LoadListAnimal();
        isReplay = true;
    }
    void LoadListAnimal()
    {
        if(isReplay)
        {
            audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        }
        else
        {
            audioSource.PlayOneShot(SharedData.numberSound[ListNumber.clickedItem], 1f);
        }
        if (ListNumber.clickedItem == 0)
        {
            transform.GetChild(startButtonIndex - 1).gameObject.SetActive(false);
        } else
        {
            transform.GetChild(startButtonIndex - 1).gameObject.SetActive(true);
        }
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
            initX = -1.55f;
            paddingX = 1.62f;
        } else if(totalItem > 3)
        {
            numRows = 2;
            numCols = 3;
            initX = -1.55f;
            paddingX = 1.62f;
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
        int totalAnimal = listAnimalSprite.Count();
        System.Random myObject = new System.Random();
        animalIndex = myObject.Next(0, totalAnimal-1);
        GameObject imageTemplate = transform.GetChild(startButtonIndex + 2).gameObject;
        imageTemplate.SetActive(true);
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
                listDisplayedAnimalSprites.Add(g);
            }
        }
        imageTemplate.SetActive(false);
        //Destroy(imageTemplate);
    }
    void SetupSprites()
    {
        listAnimalSprite = Resources.LoadAll("Animals", typeof(Sprite)).Cast<Sprite>().ToArray();
    }
    void ToHome()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/ListScene"));

      //  SceneManager.LoadScene("Scenes/ListScene");
    }
    void ReloadAnimalList()
    {
        if(listDisplayedAnimalSprites !=null)
        {
            foreach(GameObject go in listDisplayedAnimalSprites)
            {
                Destroy(go);
            }
            int totalImage = listDisplayedAnimalSprites.Count();
            for(int i = 0; i < totalImage; i++)
            {
                listDisplayedAnimalSprites.RemoveAt(0);
            }
        }
        LoadListAnimal();
    }

}
