using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;


public class StickerDetail : MonoBehaviour
{
    public static AudioSource audioSource;
    private int totalClicked = 0;
    private int totalItem = 6;
    public static Sprite[] listStickerDetail;
   // private static Vector2 aspectRatio;
    [Serializable] 
    public struct NumberImage
    {
        public Sprite NormalImage;
        public Sprite ClickedImage;
    }
    //This is the list of image imported from Sprites folder of the Resources folder
    public static Sprite[] sprites;
    void InitSprites()
    {
        listStickerDetail = Resources.LoadAll("Stickers/Dino/Detail", typeof(Sprite)).Cast<Sprite>().ToArray();
    }
    void ResizeImage(GameObject g)
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        RectTransform transformRect = g.GetComponent<Image>().GetComponent<RectTransform>();
        float targetRatio = transformRect.rect.width/transformRect.rect.height;
        Debug.Log("Screen width is: " + Screen.width + " screen height is:" + Screen.height);
        Debug.Log("Image with is:" + transformRect.rect.width + ", and height is:" + transformRect.rect.height);
    }
    void Start()
    {
        InitSprites();
        int totalStar = SharedData.GetNumberOfStar();
        Debug.Log("Total number of star:" + totalStar);
        UpdateNumberOfStar();
        GameObject imgToDiscover = transform.GetChild(1).gameObject;
        imgToDiscover.GetComponent<Image>().sprite = listStickerDetail[StickerList.clickedItem];
        GameObject btnToHome = transform.GetChild(2).gameObject;
        audioSource = btnToHome.AddComponent<AudioSource>();
        btnToHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(2).gameObject));
            ToHome();
        });
        int maxOpenIndex = StickerList.GetMaxOpenSticker();
        Debug.Log("Max open sticker is: " + maxOpenIndex + " and clickedItem is: " + StickerList.clickedItem);
        if(maxOpenIndex <= StickerList.clickedItem)
        {
            ShowCoverBlocks();
        } else
        {
            GameObject buttonTemplate = transform.GetChild(3).gameObject;
            buttonTemplate.SetActive(false);
        }
        
    }
    void UpdateNumberOfStar()
    {
        GameObject g = transform.GetChild(4).gameObject;
        g.transform.GetChild(1).GetComponent<Text>().text = SharedData.GetNumberOfStar().ToString();
    }
    void ShowCoverBlocks()
    {
        int numRows = 3;
        int numCols = 2;
        float paddingY = 2.5f;
        float paddingX = -1.2f;
        //Debug.Log("Screen ratio is: " + Screen.height / Screen.width);
        if (Screen.height > 1.5f * Screen.width)
        {
            numCols = 2;
            numRows = 3;
            paddingX = -1.2f;
        }
        GameObject buttonTemplate = transform.GetChild(3).gameObject;
        buttonTemplate.SetActive(true);
        GameObject g;
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                // Debug.Log("Generate for button number i =" + i + ", and j =" + j);
                g = Instantiate(buttonTemplate, transform);
                // Sprite oneSprite = sprites[i * numCols + j];
                g.transform.GetChild(0).GetComponent<Image>().sprite = SharedData.listNumberBg[0];
                g.transform.position = new Vector3(paddingX + j * 2.4f, 2.0f - i * paddingY);
                g.GetComponent<Button>().AddEventListener(i * numCols + j, ItemClicked);
            }
        }
        buttonTemplate.SetActive(false);
    }
    void ItemClicked(int itemIndex)
    {
        int totalStar = SharedData.GetNumberOfStar();
        Debug.Log("Total number of star:" + totalStar);
        UpdateNumberOfStar();
        if (totalStar <= 0)
        {
            Debug.Log("Out of star, reload again");
            //StartCoroutine(FadeAnimation(transform.GetChild(5 + itemIndex).gameObject));
            audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
            GameObject clickedButton2 = transform.GetChild(5 + itemIndex).gameObject;
            LeanTween.cancel(clickedButton2);
            clickedButton2.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            // 2
            LeanTween.scale(clickedButton2.gameObject, new Vector3(1.2f, 1.2f), 1.0f).setEase(LeanTweenType.punch);
            return;

        }
        totalClicked++;
        //Debug.Log("Item " + itemIndex + " clicked");
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        GameObject clickedButton = transform.GetChild(5 + itemIndex).gameObject;
        LeanTween.cancel(clickedButton);
        clickedButton.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        // 2
        LeanTween.scale(clickedButton.gameObject, new Vector3(1.0f, 1.0f), 1.0f).setEase(LeanTweenType.punch);
        //StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(4 + itemIndex).gameObject));
        StartCoroutine(FadeAnimation(transform.GetChild(5 + itemIndex).gameObject));
        if(totalClicked == totalItem)
        {
            audioSource.PlayOneShot(SharedData.victorySound[0], 1f);
            Debug.Log("You open on sticker index:" + StickerList.clickedItem);
            int maxOpenSticker = StickerList.GetMaxOpenSticker();
            Debug.Log("Max sticker index is: " + maxOpenSticker);
            if(maxOpenSticker < StickerList.clickedItem)
            {
                StickerList.SetMaxOpenSticker(StickerList.clickedItem);
            }

        }
        
       
        SharedData.SetNumberOfStar(totalStar - 5);
        totalStar = SharedData.GetNumberOfStar();
        Debug.Log("Total number of star:" + totalStar);
        UpdateNumberOfStar();
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
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/StickerList"));
    }
}
