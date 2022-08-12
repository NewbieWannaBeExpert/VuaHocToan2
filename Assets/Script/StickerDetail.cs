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
    private static Vector2 aspectRatio;
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
    void Start()
    {
        InitSprites();
        Debug.Log("Click on index: " + StickerList.clickedItem);
        int numRows = 3;
        int numCols = 2;
        float paddingY = 2.5f;
        //Debug.Log("Screen ratio is: " + Screen.height / Screen.width);
        if(Screen.height > 1.5f * Screen.width)
        {
            numCols = 2;
            numRows = 3;
            Debug.Log("Screen to long");
        }
        Debug.Log("Screen width is: " + Screen.width + " Height is: " + Screen.height);
        GameObject imgToDiscover = transform.GetChild(1).gameObject;
        imgToDiscover.GetComponent<Image>().sprite = listStickerDetail[StickerList.clickedItem];
        //ResizeBgImage(imgToDiscover);
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
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++) {
               // Debug.Log("Generate for button number i =" + i + ", and j =" + j);
                g = Instantiate(buttonTemplate, transform);
               // Sprite oneSprite = sprites[i * numCols + j];
                g.transform.GetChild(0).GetComponent<Image>().sprite =  SharedData.listNumberBg[0];
                g.transform.position = new Vector3(-1.2f + j * 2.4f, 2.0f - i * paddingY);               
                g.GetComponent<Button>().AddEventListener(i * numCols + j, ItemClicked);
            }
        }
        buttonTemplate.SetActive(false);
        //Destroy(buttonTemplate);
    }
    //This function is of no use
   /* void ResizeBgImage(GameObject g)
    {
        SpriteRenderer sr = g.GetComponent<SpriteRenderer>();

        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        g.transform.localScale = new Vector3(
            worldScreenWidth / sr.sprite.bounds.size.x,
            worldScreenHeight / sr.sprite.bounds.size.y, 1);
    }*/
    public static Vector2 getTransVel(Vector2 Velocity)
    {
        aspectRatio = AspectRatio.GetAspectRatio(Screen.width, Screen.height);
        return new Vector2(Velocity.x * (aspectRatio.x / 16f), Velocity.y * (aspectRatio.y / 9f));
    }
    void ItemClicked(int itemIndex)
    {
        totalClicked++;
        //Debug.Log("Item " + itemIndex + " clicked");
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        GameObject clickedButton = transform.GetChild(4 + itemIndex).gameObject;
        LeanTween.cancel(clickedButton);
        clickedButton.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        // 2
        LeanTween.scale(clickedButton.gameObject, new Vector3(1.0f, 1.0f), 1.0f).setEase(LeanTweenType.punch);
        //StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(4 + itemIndex).gameObject));
        StartCoroutine(FadeAnimation(transform.GetChild(4 + itemIndex).gameObject));
        if(totalClicked == totalItem)
        {
            audioSource.PlayOneShot(SharedData.victorySound[0], 1f);
        }
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
