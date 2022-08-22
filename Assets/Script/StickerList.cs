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
    private int currentPage = 0;
    private int maxPage = 0;
    private int totalItemPerPage = 12;
    private int totalItem = 20;
    private List<GameObject> ListShownSticker;
    private List<GameObject> ListNextBackBtn;

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
        ListShownSticker = new List<GameObject>();
        ListNextBackBtn = new List<GameObject>();
        //SharedData.SetNumberOfStar(50);
        //SharedData.SetStickerOpenBoxString("");
      //  SetMaxOpenSticker(0);
        UpdateNumberOfStar();
        int totalStar = SharedData.GetNumberOfStar();
        Debug.Log("Number of star: " + totalStar);
        InitSprites();
       
        maxOpenSticker = GetMaxOpenSticker();
        Debug.Log("Max open sticker is:" + maxOpenSticker);
        // Debug.Log("Screen width is: " + Screen.width + " Height is: " + Screen.height);
        GameObject btnToHome = transform.GetChild(2).gameObject;
        audioSource = btnToHome.AddComponent<AudioSource>();
        btnToHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(btnToHome));
            ToHome();
        });
        currentPage = SharedData.GetCurrentStickerPage();
        ShowBtnNextBack();
        ShowItemForPage(currentPage);
        
       
    }
    void ShowBtnNextBack()
    {
        /*foreach(GameObject btn in ListNextBackBtn)
        {
            Destroy(btn);
        }*/
        GameObject btnPre = transform.GetChild(5).gameObject;
        GameObject btnNext = transform.GetChild(6).gameObject;
        GameObject btnPreClone = Instantiate(btnPre, transform);
        GameObject btnNextClone = Instantiate(btnNext, transform);
        btnPre.SetActive(false);
        btnNext.SetActive(false);
        btnPreClone.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(btnPreClone));
            ShowPre();
        });
        btnNextClone.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            StartCoroutine(SharedData.ZoomInAndOutButton(btnNextClone));
            ShowNext();
        });
       // ListNextBackBtn.Add(btnNext);
        //ListNextBackBtn.Add(btnPre);
    }

    void ShowPre()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        if (currentPage <= 0)
        {
            Debug.Log("Can not move Pre");
            return;
        }
        currentPage -= 1;
        ShowItemForPage(currentPage);
        SharedData.SetCurrentStickerPage(currentPage);
        Debug.Log("To Pref called with CUrrent page:" + currentPage);
    }
    void ShowNext()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        
        Debug.Log("Begin Show next called, maxPage:" + maxPage + " currentPage:" + currentPage);
        if (currentPage >= maxPage)
        {
            Debug.Log("Can not move next");
            return;
        }
        currentPage++;
        SharedData.SetCurrentStickerPage(currentPage);
        ShowItemForPage(currentPage);
        Debug.Log("End Show next called, maxPage:" + maxPage + " currentPage:" + currentPage);
    }
    void ShowItemForPage(int page)
    {
        int counterSticker = 0;
        int totalItemInListSticker = ListShownSticker.Count;
        foreach (GameObject stickerGameObject in ListShownSticker)
        {
            Debug.Log("Destroy item number: " + counterSticker);
            Destroy(stickerGameObject);
            counterSticker++;
        }
        for (int i = 0; i < totalItemInListSticker; i++) { 
            ListShownSticker.RemoveAt(0);
        }
        int numRows = 3;
        int numCols = 3;
       // int totalItem = 20;
        float paddingY = 2.5f;
        float paddingX = -2.3f;
        float initY = 2.5f;
        //Debug.Log("Screen ratio is: " + Screen.height / Screen.width);
        if (Screen.height > 1.5f * Screen.width)
        {
            numCols = 2;
            numRows = 3;
            paddingX = -1.2f;
            Debug.Log("Screen to long, numCols = 2, numRows = 3");
        }
        totalItemPerPage = numCols * numRows;
        maxPage = totalItem / totalItemPerPage;
        UpdatePageStatus();
        GameObject buttonTemplate = transform.GetChild(3).gameObject;
        buttonTemplate.SetActive(true);
        buttonTemplate.transform.position = new Vector3(0.5f * currentPage, 1.0f, 1.0f);
        GameObject g;
        int counter = 0;
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                if (page * totalItemPerPage + counter >= totalItem)
                {
                    break;
                }
                g = Instantiate(buttonTemplate, transform);
                ListShownSticker.Add(g);
                if (page * totalItemPerPage +  counter < maxOpenSticker)
                {
                    GameObject coverImage = g.transform.GetChild(2).gameObject;
                    coverImage.SetActive(false);
                }
                else if (page * totalItemPerPage + counter == maxOpenSticker)
                {

                }
                else if (page * totalItemPerPage + counter > maxOpenSticker)
                {
                    GameObject coverImage = g.transform.GetChild(2).gameObject;
                    Sprite sprite = Resources.Load<Sprite>("bgs/btnStickerInActive");
                    coverImage.GetComponent<Image>().sprite = sprite;
                }
                g.transform.GetChild(1).GetComponent<Image>().sprite = listStickerImage[page * totalItemPerPage + counter];
                g.transform.position = new Vector3(paddingX + j * 2.4f, initY - i * paddingY);
                g.GetComponent<Button>().AddEventListener(counter, ItemClicked);
                counter++;
            }
        }
        buttonTemplate.SetActive(false);
        //ShowBtnNextBack();
    }

    void UpdatePageStatus()
    {
        GameObject g = transform.GetChild(7).GetChild(1).gameObject;
        g.GetComponent<Text>().text = (currentPage + 1) + "/" + (maxPage+1);
    }
    
    void ItemClicked(int itemIndex)
    {
        Debug.Log("Item " + itemIndex + " clicked");
        GameObject g = transform.GetChild(10 + itemIndex).gameObject;
        if(itemIndex + currentPage * totalItemPerPage > maxOpenSticker)
        {
            audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
            StartCoroutine(SharedData.ZoomInAndOutButton(g));
            return;
        }
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(g));
        clickedItem = itemIndex + currentPage * totalItemPerPage;
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
        //SharedData.SetCurrentStickerPage(0);
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HomeScene"));
    }
}
