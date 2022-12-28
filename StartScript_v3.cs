using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScript_v3 : MonoBehaviour
{
  float loading_time; //射出されるまでの読み込み時間を制御する変数
  public decimal elapsed_time; //チャンネル1-3の中の最大合計経過時間.スクリプトSave_Values_v2のmaxValue_fから値を受取る
  float elapsed_time_f;
  decimal time_tweak; //max_elapsed_timeを微調整するための変数
  public int repeat_no; //スクリプトSave_Values_v2のrepeat_noから来た値を受取る変数
  public float interval_time_f; //スクリプトSave_Values_v2のinterval_time_fから来た値を受取る変数
  bool ClickisON; //Click_Start()が実行中かどうかを表すフラグ;
  public Save_Values_v2 save_values_v2;
  public BeepON beepON;
  public AudioSource audioSource;
  // Use this for initialization
  private void Start()
  {
    Debug.unityLogger.logEnabled = false; //ログを無効にする方法の参考記事(https://www.barbaroiware.net/entry/2019/11/28/212147)
    loading_time = 3.000f;

    ClickisON = false;
  }
  // ボタンが押された場合、最初に呼び出される関数
  public void Click()
  {
    
    //連続クリックによるメソッド大量呼び出しを防ぐため
    //Click_Start()が実行中でない場合
    if (!ClickisON)
    {
      //射出関数呼び出し
       StartCoroutine(Click_Start(loading_time, elapsed_time_f));
    }
  }
  //スタートボタンが押された時に呼ばれるメソッド
  private IEnumerator Click_Start(float loadTime, float elapsedTime)
  {
    ClickisON = true;
    save_values_v2.toggle.interactable = false;
    for (int i = 0; i < 3; i++) 
    {
      save_values_v2._button[i].interactable = false;
      save_values_v2.input_delayTime[i].interactable = false;
      save_values_v2.input_duration[i].interactable = false;
    }
    save_values_v2.input_repeat.interactable = false;
    save_values_v2.input_interval.interactable = false;
    if (beepON.audioSource.enabled == true)
    {
      audioSource.PlayOneShot(audioSource.clip);//ビープ音鳴らす
    }
    yield return new WaitForSeconds(loadTime); //処理を指定秒数のあいだ停止する
    Debug.Log("loadTime: " + loadTime + "秒");

    for (int i = 0; i < 3; i++)
    {
       save_values_v2.injection[i].Spray_Function();
    }
    
    yield return new WaitForSeconds(elapsedTime); //処理を指定秒数のあいだ停止する
    save_values_v2.toggle.interactable = true;
    for (int i = 0; i < 3; i++) 
    {
      save_values_v2._button[i].interactable = true;
      save_values_v2.input_delayTime[i].interactable = true;
      save_values_v2.input_duration[i].interactable = true;
    }
    save_values_v2.input_repeat.interactable = true;
    save_values_v2.input_interval.interactable = true;
    ClickisON = false; //ClickisONがfalseになり、再度クリックしたとき、Click_Start()が呼ばれるようになる
    Debug.Log("リロード完了");
  }
  public void Tweak_elapsed_time()
  {
    time_tweak = 0.2m;
    Debug.Log("リピート回数: " + repeat_no);
    Debug.Log("インターバル: " + interval_time_f + "秒");
    elapsed_time = elapsed_time + time_tweak;
    Debug.Log("経過時間を微調整: " + elapsed_time + "秒");
        elapsed_time_f = (float)elapsed_time * 1000;
        elapsed_time_f = elapsed_time_f / 1000;
  }
}
