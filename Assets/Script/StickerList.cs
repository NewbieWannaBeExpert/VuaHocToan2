using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;


public class StickerList : MonoBehaviour
{
    public static AudioSource audioSource;
    public static int clickedItem = -1;
    public static int maxOpenSticker = 0;
    public static Sprite[] listStickerImage;
    public static string openStickerDinoRefName = "OpenStickerDino";

    [Serializable] 
    public struct NumberImage
    {
        public Sprite NormalImage;
        public Sprite ClickedImage;
    }
    /*
    public static void SetMaxOpenSticker(int m)
    {
        maxOpenSticker = m;
        PlayerPrefs.SetInt(openStickerDinoRefName, m);
    }*/
    public static int GetMaxOpenSticker()
    {
        if(PlayerPrefs.HasKey(openStickerDinoRefName))
        {
            maxOpenSticker = PlayerPrefs.GetInt(openStickerDinoRefName);
            Debug.Log("Max open sticker for dino list is:" + maxOpenSticker);
            return maxOpenSticker;
        } else
        {
            PlayerPrefs.SetInt(openStickerDinoRefName, 0);
            maxOpenSticker = 0;
            return maxOpenSticker;
        }
    }
    public static void SetMaxOpenSticker(int value)
    {
        PlayerPrefs.SetInt(openStickerDinoRefName, value);
        maxOpenSticker =value;
       /* if (PlayerPrefs.HasKey(openStickerDinoRefName))
        {
            PlayerPrefs.SetInt(openStickerDinoRefName, value);
        } else
        {
            PlayerPrefs.SetInt(openStickerDinoRefName, 0);
        }*/
    }
    //This is the list of image imported from Sprites folder of the Resources folder
    public static Sprite[] sprites;
    private void InitSprites()
    {
        listStickerImage = Resources.LoadAll("Stickers/Dino/IconList", typeof(Sprite)).Cast<Sprite>().ToArray();
    }
    void UpdateNumberOfStar()
    {
        GameObject g = transform.GetChild(4).gameObject;
        g.transform.GetChild(1).GetComponent<Text>().text =  SharedData.GetNumberOfStar().ToString();
    }
    void Start()
    {
        SharedData.SetNumberOfStar(10);
        SharedData.SetStickerOpenBoxString("");
        UpdateNumberOfStar();
        int totalStar = SharedData.GetNumberOfStar();
        Debug.Log("Number of star: " + totalStar);
        InitSprites();
        int numRows = 3;
        int numCols = 3;
        int totalItem = 8;
        float paddingY = 2.5f;
        float paddingX = -2.3f;
        //Debug.Log("Screen ratio is: " + Screen.height / Screen.width);
        if(Screen.height > 1.5f * Screen.width)
        {
            numCols = 2;
            numRows = 3;
            paddingX = -1.2f;
            Debug.Log("Screen to long, numCols = 2, numRows = 3");
        }
        maxOpenSticker = GetMaxOpenSticker();
        Debug.Log("Max open sticker is:" + maxOpenSticker);
        // Debug.Log("Screen width is: " + Screen.width + " Height is: " + Screen.height);
        GameObject btnToHome = transform.GetChild(2).gameObject;
        audioSource = btnToHome.AddComponent<AudioSource>();
        btnToHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(2).gameObject));
            ToHome();
        });

        GameObject buttonTemplate = transform.GetChild(3).gameObject;
        buttonTemplate.SetActive(true);
        GameObject g;
        int counter = 0;
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++) {
                if (counter >= totalItem)
                {
                    break;
                }
                g = Instantiate(buttonTemplate, transform);
                if (counter < maxOpenSticker)
                {
                    GameObject coverImage = g.transform.GetChild(2).gameObject;
                    coverImage.SetActive(false);
                } else if (counter == maxOpenSticker)
                {

                } else if(counter > maxOpenSticker)
                {
                    GameObject coverImage = g.transform.GetChild(2).gameObject;
                    Sprite sprite = Resources.Load<Sprite>("Btns/mini");
                    coverImage.GetComponent<Image>().sprite = sprite;
                }
                g.transform.GetChild(1).GetComponent<Image>().sprite =  listStickerImage[counter];
                g.transform.position = new Vector3(paddingX + j * 2.4f, 2.0f - i * paddingY);               
                g.GetComponent<Button>().AddEventListener(counter, ItemClicked);
                counter++;
            }
        }
        buttonTemplate.SetActive(false);
    }
    //This function is of no use
    
    void ItemClicked(int itemIndex)
    {
        Debug.Log("Item " + itemIndex + " clicked");
        if(itemIndex > maxOpenSticker)
        {
            audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(5 + itemIndex).gameObject));
            return;
        }
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(5 + itemIndex).gameObject));
        clickedItem = itemIndex;
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/StickerDetail"));

        //        StartCoroutine(FadeAnimation(transform.GetChild(4 + itemIndex).gameObject));
    }
    IEnumerator FadeAnimation(GameObject g)
    {
        yield return new WaitForSeconds(0.6f);
        FadeItem(g);
    }
    void FadeItem(GameObject g)
    {
        g.SetActive(false);
    }

    void ToHome()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HomeScene"));
    }
}
