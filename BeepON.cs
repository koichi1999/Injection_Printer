using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BeepON : MonoBehaviour
{
    public AudioSource audioSource;
    Toggle tgl;
    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        tgl = gameObject.GetComponent<Toggle>();
        Checkbox();
    }
    public void Checkbox() //toggleにチェックを入れたり、外したりしたときに呼び出される
    { 
        //チェックボックスのON/OFF
        if(tgl.isOn == true)
        {
            audioSource.enabled = true;
            //Debug.Log("選択されています");
        }
        else
        {
            audioSource.enabled = false;
            //Debug.Log("選択されていません");
        }
    }
}
