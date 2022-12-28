using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit_App : MonoBehaviour
{
    public void Quit() 
    {
    //Unity のゲームを終了させる方法 参考記事(https://web-dev.hatenablog.com/entry/unity/quit-game#:~:text=%E3%81%84%E3%82%8B%E5%A0%B4%E5%90%88%E3%81%AF%E3%80%81-,UnityEngine.,%E3%81%A7%E7%B5%82%E4%BA%86%E3%81%97%E3%81%BE%E3%81%99%E3%80%82)
    #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
    #elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
    #endif
    }
}
