using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public static class ButtonExtension
{
    public static void AddEventListener<T>(this Button button, T param, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate()
        {
            OnClick(param);
        });
    }
}
public class ListNumber : MonoBehaviour
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
        int numRows = 4;
        int numCols = 3;
        int totalItem = 10;
        //Debug.Log("Screen ratio is: " + Screen.height / Screen.width);
        if(Screen.height > 1.5f * Screen.width)
        {
            numCols = 2;
            numRows = 5;
            Debug.Log("Screen to long");
        }
        Debug.Log("Screen width is: " + Screen.width + " Height is: " + Screen.height);
        SetupSprites();
        GameObject btnToHome = transform.GetChild(1).gameObject;
        audioSource = btnToHome.AddComponent<AudioSource>();
        btnToHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToHome();
        });

        GameObject buttonTemplate = transform.GetChild(2).gameObject;
        GameObject g;
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++) {
                Debug.Log("Generate number for colum: " + i + " and row: " + j + " for index:" + (i * numCols + j));
                if(i * numCols + j >= totalItem)
                {
                    break;
                }
                g = Instantiate(buttonTemplate, transform);
                Sprite oneSprite = sprites[i * numCols + j];
                g.transform.GetChild(0).GetComponent<Image>().sprite =  sprites[i * numCols + j];
                if(numCols == 2)
                {
                    g.transform.position = new Vector3(-1.2f + (float)j * 2.4f, 3.0f - (float)i * 1.82f);
                } else
                {
                    g.transform.position = new Vector3(-2.2f + (float)j * 2.4f, 2.5f - (float)i * 2.0f);
                }
                
                g.GetComponent<Button>().AddEventListener(i * numCols + j, ItemClicked);
               
            }
        }
        Destroy(buttonTemplate);
    }
    //This function is of no use
    private void PlaceRandomly(GameObject sprite)
    {
        float tempX = UnityEngine.Random.Range(100f, 200f);
        float tempY = UnityEngine.Random.Range(100f, 200f);
        sprite.gameObject.transform.position = new Vector3(tempX, tempY);
    }
    void ItemClicked(int itemIndex)
    {
        //Debug.Log("Item " + itemIndex + " clicked");
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.MyCoroutine(transform.GetChild(2 + itemIndex).gameObject));
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/DetailScene"));
        //SceneManager.LoadScene("Scenes/DetailScene");
        clickedItem = itemIndex;
    }
    void SetupSprites()
    {
        sprites = Resources.LoadAll("Numbers", typeof(Sprite)).Cast<Sprite>().ToArray();
    }
    void ToHome()
    {
        // Debug.Log("You click on home button");
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.MyCoroutine(transform.GetChild(1).gameObject));
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HocSoHomeScene"));
        //SceneManager.LoadScene("Scenes/HocSoHomeScene");
    }
}
