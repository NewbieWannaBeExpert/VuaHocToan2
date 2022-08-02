using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class HocSo_DoVui1 : MonoBehaviour
{
    public AudioSource audioSource;
    //public AudioClip[] clips;
    // public AudioClip number0Clip;
    public static Sprite[] listAnimalSprite;
   
    void Start()
    {
        GameObject buttonTemplate = transform.GetChild(1).gameObject;
        audioSource = buttonTemplate.AddComponent<AudioSource>();
        //buttonTemplate.transform.GetChild(0).GetComponent<Image>().sprite = ListNumber.sprites[2];
        GameObject homeTemplate = transform.GetChild(1).gameObject;
        homeTemplate.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToHome();
        });
        LoadNumberList();
    }
    void LoadNumberList()
    {

        int totalItem = 3;
        int numRows = 1;
        int numCols = 3;
        float initX = -2.0f;
        float initY = -2.0f;
        float paddingX = 2.0f;
        float paddingY = 1.75f;
        float scale = 1.0f;
        if (Screen.height > 1.5f * Screen.width)
        {
            numCols = 3;
            numRows = 1;
            Debug.Log("Screen to long");
        }
        
        // int totalItem = 9;
        int numberIndex = 2;
        System.Random myObject = new System.Random();
        numberIndex = myObject.Next(0, 9);
        //Debug.Log("Screen ratio is: " + Screen.height / Screen.width);
        GameObject imageTemplate = transform.GetChild(3).gameObject;
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
                g.transform.GetComponent<Image>().sprite = SharedData.listNumberDoVui[numberIndex];
                if (numCols == 2)
                {
                    g.transform.position = new Vector3(initX + (float)j * paddingX, initY - (float)i * paddingY);
                }
                else
                {
                    g.transform.position = new Vector3(initX + (float)j * paddingX, initY - (float)i * paddingY);
                }
                g.transform.localScale = new Vector3(scale, scale, 1);
            }
        }
        Destroy(imageTemplate);
    }
    
    void ToHome()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.MyCoroutine(transform.GetChild(2).gameObject));
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HocSoHomeScene"));
    }
}
