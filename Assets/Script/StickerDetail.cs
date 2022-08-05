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
    public static int clickedItem = -1;
    [Serializable] 
    public struct NumberImage
    {
        public Sprite NormalImage;
        public Sprite ClickedImage;
    }
    //This is the list of image imported from Sprites folder of the Resources folder
    public static Sprite[] sprites;
   
    void Start()
    {
        int numRows = 3;
        int numCols = 2;
        int totalItem = 6;
        float paddingY = 2.5f;
        //Debug.Log("Screen ratio is: " + Screen.height / Screen.width);
        if(Screen.height > 1.5f * Screen.width)
        {
            numCols = 2;
            numRows = 3;
            Debug.Log("Screen to long");
        }
        Debug.Log("Screen width is: " + Screen.width + " Height is: " + Screen.height);
        GameObject btnToHome = transform.GetChild(1).gameObject;
        audioSource = btnToHome.AddComponent<AudioSource>();
        btnToHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToHome();
        });

        GameObject buttonTemplate = transform.GetChild(2).gameObject;
        buttonTemplate.SetActive(true);
        GameObject g;
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++) {
                Debug.Log("Generate for button number i =" + i + ", and j =" + j);
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
    
    void ItemClicked(int itemIndex)
    {
        Debug.Log("Item " + itemIndex + " clicked");
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(2 + itemIndex).gameObject));
        //StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/DetailScene"));
        //SceneManager.LoadScene("Scenes/DetailScene");
        clickedItem = itemIndex;
    }
   
    void ToHome()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.ZoomInAndOutButton(transform.GetChild(1).gameObject));
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HomeScene"));
    }
}
