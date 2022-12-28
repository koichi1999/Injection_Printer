using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Start_Fprinter_v2 : MonoBehaviour
{
    Image PanelImage;
    Image image;
    private Color PanelColor;
    private Color ImageColor;
    AudioSource audioSource;
    private float rgbSpeed;
    private float rgbValue;
    private bool isSceneChange;
    private bool isChangeEnd;
    private void Awake()
    {
        //コンポーネントを取得
        audioSource = gameObject.GetComponent<AudioSource>();
        PanelImage = gameObject.GetComponent<Image>();
        image = GameObject.FindGameObjectWithTag("UnityChan_Logo").GetComponent<Image>();
        isSceneChange = false;
        isChangeEnd = false;
        rgbSpeed = 0.3f;
        rgbValue = 1.0f;
        PanelColor = PanelImage.color;
        ImageColor = image.color;
    }
    IEnumerator Start() //スタートをコルーチン化
    {
        yield return new WaitForSeconds(0.5f);
        isSceneChange = true;
        audioSource.PlayOneShot(audioSource.clip);
    }
    private void Update()
    {
        if (isSceneChange)
        {
            rgbValue -= rgbSpeed * Time.deltaTime;
            PanelImage.color = new Color(rgbValue, rgbValue, rgbValue, PanelColor.a);
            image.color = new Color(rgbValue, rgbValue, rgbValue, ImageColor.a);
            //Debug.Log(rgbValue);
            if (rgbValue <= 0)
            {
                isSceneChange = false;
                isChangeEnd = true;
            }
        }
        if (isChangeEnd)
            SceneManager.LoadScene("Fprinter_UI");
    }
}
