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
    public bool isAlertOutOfMoneyShown = false;
    public List<int> realIndexList;
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
        realIndexList = new List<int>();
        int totalStar = SharedData.GetNumberOfStar();
        Debug.Log("Total number of star:" + totalStar);
        UpdateNumberOfStar();
        GameObject imgToDiscover = transform.GetChild(1).gameObject;
        imgToDiscover.GetComponent<Image>().sprite = listStickerDetail[StickerList.clickedItem];
        GameObject btnToHome = transform.GetChild(2).gameObject;
        audioSource = btnToHome.AddComponent<AudioSource>();
        btnToHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(btnToHome));
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
        GameObject alertBoard = transform.GetChild(5).gameObject;
        alertBoard.SetActive(false);
        
    }
    void UpdateNumberOfStar()
    {
        GameObject g = transform.GetChild(4).gameObject;
        GameObject txtObj = g.transform.GetChild(1).GetComponent<Text>().gameObject;
        txtObj.GetComponent<Text>().text = SharedData.GetNumberOfStar().ToString();
        //g.transform.GetChild(1).GetComponent<Text>().text = SharedData.GetNumberOfStar().ToString();
        LeanTween.cancel(txtObj.gameObject);
        txtObj.transform.rotation = Quaternion.Euler(0.0f,0.0f,0.0f);
        txtObj.transform.localScale = Vector3.one;
        LeanTween.rotateZ(txtObj.gameObject, 15.0f, 0.5f).setEasePunch();
        LeanTween.scaleX(txtObj.gameObject, 1.5f, 0.5f).setEasePunch();
    }
    void ShowCoverBlocks()
    {
        //SharedData.ResetStickerOpenBoxString();
        string openBoxListStr = SharedData.GetStickerOpenBoxString();
        Debug.Log("Open box list string is: " + openBoxListStr);
        List<string> ListOpenBoxes = new List<string>();
        if(openBoxListStr != "")
        {
            ListOpenBoxes = openBoxListStr.Split("_").ToList();
        }
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
        int counter = 0;
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                if(ListOpenBoxes[i * numCols + j] == "1") {
                    continue;
                }
                realIndexList.Add(i * numCols + j);
                Debug.Log("Assign realIndexList index " + counter + " with value:" + (i * numCols + j));
                // Debug.Log("Generate for button number i =" + i + ", and j =" + j);
                g = Instantiate(buttonTemplate, transform);
                // Sprite oneSprite = sprites[i * numCols + j];
                g.transform.GetChild(0).GetComponent<Image>().sprite = SharedData.listNumberBg[0];
               // GameObject moneyImage = g.transform.GetChild(1).gameObject;
               // moneyImage.SetActive(false);
                g.transform.position = new Vector3(paddingX + j * 2.4f, 2.0f - i * paddingY);
                g.GetComponent<Button>().AddEventListener(counter, ItemClicked);
                counter++;
            }
        }
        buttonTemplate.SetActive(false);
    }
    public void CloseAlertMoney(GameObject g)
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.DestroyGameObjectAfterSomeTime(g, 0.35f));
        isAlertOutOfMoneyShown = false;
    }
    public bool CheckOpenAllCoverBoxes()
    {
        string openString = SharedData.GetStickerOpenBoxString();
        Debug.Log("Open string is: " + openString);
        List<string> openList = new List<string>();
        openList = openString.Split("_").ToList();
        foreach(string open in openList)
        {
         //   Debug.Log("Open string in for is: " + open);
            if (open == "0")
            {
                return false;
            }
        }
        return true;
    }
    public void UpdateOpenString(int openIndex)
    {
        string openString = SharedData.GetStickerOpenBoxString();
        List<string> openList = new List<string>();
       // List<string> returnList = new List<string>();
        openList = openString.Split("_").ToList();
        openList[openIndex] = "1";
        string retStr = "";
        int counter = 0;
        foreach(string str in openList)
        {
            if(counter == 0 )
            {
                retStr = str;   
            }else
            {
                retStr = retStr + "_" + str;
            }
            counter ++;
        }
        Debug.Log("After open, retStr is: " + retStr);
        SharedData.SetStickerOpenBoxString(retStr);
    }
    void ItemClicked(int itemIndex)
    {
        if(isAlertOutOfMoneyShown)
        {
            return;
        }
        int totalStar = SharedData.GetNumberOfStar();
        Debug.Log("Total number of star:" + totalStar);
        UpdateNumberOfStar();
        if (totalStar <= 0)
        {
            Debug.Log("Out of star, reload again");
            //StartCoroutine(FadeAnimation(transform.GetChild(5 + itemIndex).gameObject));
            audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
            GameObject clickedButton2 = transform.GetChild(6 + itemIndex).gameObject;
            LeanTween.cancel(clickedButton2);
            clickedButton2.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            // 2
            LeanTween.scale(clickedButton2.gameObject, new Vector3(1.2f, 1.2f), 1.0f).setEase(LeanTweenType.punch);

            GameObject alertBoardFrame = transform.GetChild(5).gameObject;
            alertBoardFrame.SetActive(true);
            GameObject alertBoard = Instantiate(alertBoardFrame, transform);
            alertBoard.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            
            GameObject btnClose = alertBoard.transform.GetChild(3).gameObject;
            btnClose.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                StartCoroutine(SharedData.ZoomInAndOutButton(btnClose));
                CloseAlertMoney(alertBoard);
            });

            GameObject btnToTest = alertBoard.transform.GetChild(2).gameObject;
            btnToTest.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                StartCoroutine(SharedData.ZoomInAndOutButton(btnToTest));
                ToTestScene();
            });
            alertBoardFrame.SetActive(false);
            isAlertOutOfMoneyShown = true;
            return;

        }
        //totalClicked++;
        UpdateOpenString(realIndexList[itemIndex]);
        //Debug.Log("Item " + itemIndex + " clicked");
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        GameObject clickedButton = transform.GetChild(6 + itemIndex).gameObject;
        LeanTween.cancel(clickedButton);
        clickedButton.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        // 2
        LeanTween.scale(clickedButton.gameObject, new Vector3(1.0f, 1.0f), 1.0f).setEase(LeanTweenType.punch);
        //StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(4 + itemIndex).gameObject));
        StartCoroutine(FadeAnimation(transform.GetChild(6 + itemIndex).gameObject));
        //Animate the star
        //First get the destination of the stop
        GameObject coinStatus = transform.GetChild(4).gameObject;
        GameObject moneyImage = clickedButton.transform.GetChild(1).gameObject;
        GameObject moneyDuplicated = Instantiate(moneyImage,transform);
        moneyDuplicated.transform.position = moneyImage.transform.position; 
        LeanTween.cancel(moneyDuplicated);
        moneyDuplicated.transform.localScale = new Vector3(0.7f, 0.7f, 1.0f);   
        LeanTween.scale(moneyDuplicated,new Vector3(1.0f,1.0f,1.0f),1.0f).setEase (LeanTweenType.easeOutQuint);
        LeanTween.move(moneyDuplicated, coinStatus.transform.position, 0.5f).setEaseInBack();
        StartCoroutine(DestroyGameObjectAfterDelay(moneyDuplicated, 0.7f));
        Destroy(moneyImage);
        /*string openBoxListStr = SharedData.GetStickerOpenBoxString();
        if(openBoxListStr == "")
        {
            openBoxListStr = itemIndex.ToString();
        }
         else
        {
            openBoxListStr = openBoxListStr + "_" + itemIndex.ToString();
        }
        SharedData.SetStickerOpenBoxString(openBoxListStr);*/
        if(CheckOpenAllCoverBoxes())
        {
            SharedData.ResetStickerOpenBoxString();
            audioSource.PlayOneShot(SharedData.victorySound[0], 1f);
            Debug.Log("You open on sticker index:" + StickerList.clickedItem);
            int maxOpenSticker = StickerList.GetMaxOpenSticker();
            Debug.Log("Max sticker index is: " + maxOpenSticker);
            if(maxOpenSticker <= StickerList.clickedItem)
            {
                StickerList.SetMaxOpenSticker(StickerList.clickedItem + 1);
            }

        }
        
       
        SharedData.SetNumberOfStar(totalStar - 5);
        totalStar = SharedData.GetNumberOfStar();
        Debug.Log("Total number of star:" + totalStar);
        UpdateNumberOfStar();
    }
    IEnumerator DestroyGameObjectAfterDelay(GameObject g, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(g);
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
    void ToTestScene()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        System.Random g = new System.Random();
        int randNum = g.Next(0, 2) + 5 ;
        if(randNum == 0)
        {
            StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/TestDienSo"));
        }
        else if(randNum == 1)
        {
            StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/TestDoVui1"));
        } else
        {
            StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/TestDoVui2"));
        }
    }
    void ToHome()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/StickerList"));
    }
}
