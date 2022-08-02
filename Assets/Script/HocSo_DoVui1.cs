using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class HocSo_DoVui1 : MonoBehaviour
{
    public AudioSource audioSource;
    public static Sprite[] listAnimalSprite;
    void Start()
    {
        GameObject btnHome = transform.GetChild(1).gameObject;
        audioSource = btnHome.AddComponent<AudioSource>();
        btnHome.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            ToHome();
        });
        GameObject btnReplay = transform.GetChild(2).gameObject;
        btnReplay.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            Replay();
        });
        LoadNumberList();

    }
    void LoadNumberList()
    {
        int totalItem = 3;
        int numRows = 3;
        int numCols = 1;
        float initX = 0f;
        float initY = 2.5f;
        float paddingX = 2.0f;
        float paddingY = 2.3f;
        float variantMaxY = 0.03f;
        float variantMaxX = 0.1f;
        float scale = 1.0f;
        if (Screen.height > 1.5f * Screen.width)
        {
            numCols = 1;
            numRows = 3;
            Debug.Log("Screen to long");
        }
        int num1,num2,num3 = 2;
        System.Random myObject = new System.Random();
        num1 = myObject.Next(0, 9);
        num2 = myObject.Next(0, 9);
        while(num2 == num1)
        {
            num2 = myObject.Next(0, 9);
        }
        num3 = myObject.Next(0, 9); 
        while(num3 == num2 || num3 == num1)
        {
            num3 = myObject.Next(0, 9);
        }
        GameObject btnNumberPattern = transform.GetChild(3).gameObject;
        GameObject btnNumberClone;
        int counter = 0;
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                Debug.Log("Generate number for colum: " + i + " and row: " + j + " for index:" + (i * numCols + j));
                if (i * numCols + j >= totalItem)
                {
                    break;
                }
                int variant = myObject.Next(0, 9);
                float mul = 1.0f;
                if(variant % 2 == 0)
                {
                    mul = -1.0f;
                }
                btnNumberClone = Instantiate(btnNumberPattern, transform);
                btnNumberClone.transform.GetChild(0).GetComponent<Image>().sprite = SharedData.listNumberBg[0];
                if(counter == 0)
                {
                    btnNumberClone.transform.GetChild(1).GetComponent<Image>().sprite = SharedData.listNumberDoVui[num1];
                } else if(counter == 1)
                {
                    btnNumberClone.transform.GetChild(1).GetComponent<Image>().sprite = SharedData.listNumberDoVui[num2];
                } else if(counter == 2)
                {
                    btnNumberClone.transform.GetChild(1).GetComponent<Image>().sprite = SharedData.listNumberDoVui[num3];
                }
                counter++;
                btnNumberClone.transform.position = new Vector3(initX + (float)j * paddingX + mul * (float)variant * variantMaxX, initY + mul * (float) variant * variantMaxY - (float)i * paddingY);
                btnNumberClone.transform.localScale = new Vector3(scale, scale, 1);
            }
        }
        Destroy(btnNumberPattern);
    }
    
    void ToHome()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.MyCoroutine(transform.GetChild(1).gameObject));
        StartCoroutine(SharedData.ToSceneAfterSomeTime(0.75f, "Scenes/HocSoHomeScene"));
    }
    void Replay()
    {
        audioSource.PlayOneShot(SharedData.buttonClickSound[1], 1f);
        StartCoroutine(SharedData.MyCoroutine(transform.GetChild(2).gameObject));
        StartCoroutine(ReloadNumber());
    }
    IEnumerator ReloadNumber()
    {
        yield return new WaitForSeconds(0.5f);
        LoadNumberList();
    }
}
