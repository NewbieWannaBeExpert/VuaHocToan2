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

    public static Sprite[] listStickerImage;
    [Serializable] 
    public struct NumberImage
    {
        public Sprite NormalImage;
        public Sprite ClickedImage;
    }
    //This is the list of image imported from Sprites folder of the Resources folder
    public static Sprite[] sprites;
    private void InitSprites()
    {
        listStickerImage = Resources.LoadAll("Stickers/Sticker1", typeof(Sprite)).Cast<Sprite>().ToArray();
    }
    void Start()
    {
        InitSprites();
        int numRows = 3;
        int numCols = 3;
        int totalItem = 5;
        float paddingY = 2.5f;
        //Debug.Log("Screen ratio is: " + Screen.height / Screen.width);
        if(Screen.height > 1.5f * Screen.width)
        {
            numCols = 2;
            numRows = 3;
            Debug.Log("Screen to long, numCols = 2, numRows = 3");
        }
        Debug.Log("Screen width is: " + Screen.width + " Height is: " + Screen.height);
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
                if(counter >= totalItem)
                {
                    break;
                }
                Debug.Log("Generate for button number i =" + i + ", and j =" + j + ", index:" + (counter));
                g = Instantiate(buttonTemplate, transform);
                g.transform.GetChild(0).GetComponent<Image>().sprite =  listStickerImage[counter];
                g.transform.position = new Vector3(-1.2f + j * 2.4f, 2.0f - i * paddingY);               
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
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(4 + itemIndex).gameObject));
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
