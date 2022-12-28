using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//セーブボタンが押されたとき、入力欄に入力された文字列を数値に変換して、計算し、値を保存する処理をまとめたクラス
public class Save_Values_v2 : MonoBehaviour
{
  public Injection[] injection; //Injectionクラス型を配列化
  public InputField[] input_delayTime = new InputField[3]; //インプットフィールド型の変数(Delay Time 入力用)
  public InputField[] input_duration = new InputField[3]; //インプットフィールド型の変数(Duration入力用)
  public InputField input_repeat; //インプットフィールド型の変数(Repeat_No入力用)
  public InputField input_interval; //インプットフィールド型の変数(Interval入力用)
  private int character_limit; //文字数を制限する変数
  private int repeat_no_limit;//repeat_noの回数を制限する
  private string default_value; //各入力項目の初期値(repeat_no)を除く
  private string repeat_value; //repeat_noの初期値
  private int[] delay_time = new int[3]; //チャンネルの射出開始時間を遅らせる変数.ミリ秒単位
  private decimal[] delay_time_m = new decimal[3]; //delay_time[]を秒に変換
  private int[] duration = new int[3]; //チャンネルの射出時間を制御する変数.ミリ秒単位
  private decimal[] duration_m = new decimal[3]; //duration[]を秒に変換
  private decimal[] difference_m = new decimal[3];
  private decimal[] elapsed_time = new decimal[3]; //チャンネルの合計経過時間
  private int repeat_no; //それぞれ指定したチャネルの射出を何回繰り返すか指定する変数
  private int interval_time; //repeat_noで指定した回数の間の間隔を指定する.ミリ秒単位
  private decimal interval_time_m; //interval_timeを秒に変換
  public StartScript_v3 startscript; //StartScript_v3型の変数
  public AudioSource audioSource; //AudioSource型の変数
  public Button[] _button = new Button[3]; //Button型の変数 _button[0]にSaveボタン,_button[1]にStartボタン、button[2]にEndボタン
  public Image image; //Image型の変数
  public Toggle toggle;//Toggle型の変数
  private bool negative; //メソッドUnityChan()を呼び出すためのフラグ

    /*
      input_delayTime[0], Delay Time1の値

      input_delayTime[1], Delay Time2の値

      input_delayTime[2], Delay Time3の値

      input_duration[0], Duration1の値

      input_duration[1], Duration2の値

      input_duration[2], Duration3の値

      input_repeat, Repeat_Noの値

      input_interval, Interval Timeの値
    */

    private void Start()
  {
    //スプライトで好きな画像を表示する方法は(https://tech.pjin.jp/blog/2018/09/07/unity_sprite_image_display/)を参照
    image.enabled = false; //UnityChanのイラストを非表
    negative = false; //メソッドUnityChan()を呼び出すためのフラグをオフ

    character_limit = 5; //5文字まで
    repeat_no_limit = 3;//3桁.最大999回まで
    default_value = 0.ToString(); //0(int)をstringに変換
    repeat_value = 1.ToString(); //1(int)をstringに変換
    for (int i = 0; i < 3; i++)
    {
      input_delayTime[i].characterLimit = character_limit; //入力文字数を5に制限
      input_delayTime[i].text = default_value; //初期値を0に設定

      input_duration[i].characterLimit = character_limit; //入力文字数を5に制限
      input_duration[i].text = default_value; //初期値を0に設定
    }

    input_repeat.characterLimit = repeat_no_limit; //入力文字数を3に制限
    input_repeat.text = repeat_value; //初期値を1に設定

    input_interval.characterLimit = character_limit; //入力文字数を5に制限
    input_interval.text = default_value; ////初期値を0に設定
  }

  //セーブボタンを押したときに呼ばれるメソッド
  public void ConvertToInt()
  {
    injection = new Injection[3]; //配列の長さを決める

    for (int i = 0; i < 3; i++)
    {
      //InjectionクラスからSpray_Function()メソッドを呼び出して、"Spray"というコルーチンを実行しようと
      //したとき、MonoBehaviour（及びそれを継承したInjectionクラス）をnewで直接生成しているのがエラーの原因
      //injection[i] = new Injection();//インスタンスを生成して実体を作る


      //"Spray"コルーチン呼び出し（この場合は正常に実行できる）.参考記事(https://qiita.com/norikiyo777/items/0bedfdc239f85032ac86)
      injection[i] = (new GameObject("Injection_Class")).AddComponent<Injection>();
    }

    //String型をint型に変換 参考記事(https://teratail.com/questions/44218)

    //入力されたテキストをint型の整数に変換
    for (int i = 0; i < 3; i++)
    {
      //input_delayTime[]を整数に変換してdelay_time[]に代入していく
      delay_time[i] = int.Parse(input_delayTime[i].text);

      //input_duration[]を整数に変換してduration[]に代入していく
      duration[i] = int.Parse(input_duration[i].text);
    }
    repeat_no = int.Parse(input_repeat.text);
    interval_time = int.Parse(input_interval.text);

    //負の値が入力された場合の処理.変数に0を代入し、入力欄にも0を表示して値を入力し直すように促す
    if(repeat_no < 0)
    {
      repeat_no = 0;
      input_repeat.text = repeat_no.ToString();
      negative = true;
      Debug.Log("不正な値です");
    }
    if(interval_time < 0)
    {
      interval_time = 0;
      input_interval.text = interval_time.ToString();
    }
    for (int i = 0; i < 3; i++)
    {
      if (delay_time[i] < 0)
      {
        delay_time[i] = 0;
        input_delayTime[i].text = delay_time[i].ToString();
        negative = true;
        Debug.Log("不正な値です");
      }
      if(duration[i] < 0)
      {
        duration[i] = 0;
        input_duration[i].text = duration[i].ToString();
        negative = true;
        Debug.Log("不正な値です");
      }
    }

    if (!negative)
    {
      decimal[] comparison_value = new decimal[3]; 
      float max_val_del_dur_f; //チャンネルの(delay_time + duration)を足した値.3つのチャンネルのmax_val_del_durを比較して、その最大値を求める
      decimal max_val_del_dur_m;
      //上記の変数群を秒に変換(Float型).小数第3位まで表示させたい
      interval_time_m = (decimal)interval_time / 1000;
      //Debug.Log("Interval_Time: " + interval_time_f);

      for (int i = 0; i < 3; i++)
      {
        delay_time_m[i] = (decimal)delay_time[i] / 1000;

        duration_m[i] = (decimal)duration[i] / 1000;

        //Debug.Log("Delay_Time" + (i + 1) + ": " + delay_time_f[i]);
        //Debug.Log("Duration" + (i + 1) + ": " + duration_f[i]);
      }
      for(int i = 0; i < 3; i++)
      {
        comparison_value[i] = delay_time_m[i] + duration_m[i]; //チャンネル[]の delay_time + duration
        //Debug.Log("comparison_value["+ i + "] :" + comparison_value[i]);
      }
        //N個の値の最大値を求める 参考記事(https://sunagitsune.com/unitymathfmax/#toc1)
        max_val_del_dur_f = Mathf.Max((float)comparison_value[0], (float)comparison_value[1], (float)comparison_value[2]);
        max_val_del_dur_m = (decimal)max_val_del_dur_f * 1000;
        max_val_del_dur_m = max_val_del_dur_m / 1000;
        //Debug.Log("max_val_del_dur_m: " + max_val_del_dur_m);

        if(max_val_del_dur_m == comparison_value[2]) 
        {
          //最大値がcomparison_value[2]だったとき、つまり最大値がチャンネル3(delay_time + duration)のとき
          difference_m[0] = comparison_value[2] - comparison_value[0]; //チャンネル3 - チャンネル1の差分を求める
          difference_m[1] = comparison_value[2] - comparison_value[1]; //チャンネル3 - チャンネル2の差分を求める
          difference_m[2] = comparison_value[2] - comparison_value[2]; //チャンネル3 - チャンネル3の差分を求める
        }
        else if(max_val_del_dur_m == comparison_value[1])
        {
          //でなければ最大値がcomparison_value[1]だったとき、つまり最大値がチャンネル2(delay_time + duration)のとき
          difference_m[0] = comparison_value[1] - comparison_value[0]; //チャンネル2 - チャンネル1の差分を求める
          difference_m[1] = comparison_value[1] - comparison_value[1]; //チャンネル2 - チャンネル2の差分を求める
          difference_m[2] = comparison_value[1] - comparison_value[2]; //チャンネル2 - チャンネル3の差分を求める
        }
        else if(max_val_del_dur_m == comparison_value[0])
        {
          //でなければ最大値がcomparison_value[0]だったとき、つまり最大値がチャンネル1(delay_time + duration)のとき
          difference_m[0] = comparison_value[0] - comparison_value[0]; //チャンネル1 - チャンネル1の差分を求める
          difference_m[1] = comparison_value[0] - comparison_value[1]; //チャンネル1 - チャンネル2の差分を求める
          difference_m[2] = comparison_value[0] - comparison_value[2]; //チャンネル1 - チャンネル3の差分を求める
        }
      /*
      for(int i = 0; i < 3; i++)
      {
        Debug.Log("difference[" + i + "] :" + difference_m[i]);
      }
      */
      for(int i = 0; i < 3; i++)
      {
        elapsed_time[i] = delay_time_m[i] * repeat_no + duration_m[i] * repeat_no + difference_m[i] * repeat_no + interval_time_m * (repeat_no - 1);
        //Debug.Log("Elapsed_Time" + (i + 1) + ": " + elapsed_time[i]);
      }

      Save_values_ch();
    }
    else
    {
      StartCoroutine(UnityChan());
    }
  }

  //UnityChanの画像と音声を表示するメソッド
  private IEnumerator UnityChan()
  {
    float OnDispaly;
    OnDispaly = 1.0f;
    toggle.interactable = false;
    for(int i = 0; i < 3; i++)
    {
      input_delayTime[i].interactable = false;
      input_duration[i].interactable = false;
      _button[i].interactable = false;
    }
    input_repeat.interactable = false;
    input_interval.interactable = false;
    image.enabled = true;
    audioSource.PlayOneShot(audioSource.clip);
    Debug.Log("再度値を入力してください");
    yield return new WaitForSeconds(OnDispaly);
    image.enabled = false;
    for(int i = 0; i < 3; i++)
    {
      input_delayTime[i].interactable = true;
      input_duration[i].interactable = true;
      _button[i].interactable = true;
    }
    input_repeat.interactable = true;
    input_interval.interactable = true;
    toggle.interactable = true;
    negative = false;
  }

  //値を保存するメソッド
  private void Save_values_ch()
  {
     float[] delay_time_f = new float[3];
     float[] duration_f = new float[3];
     float[] difference_f = new float[3];
     float interval_time_f;
     float[] elapsed_time_f = new float[3];
     
     interval_time_f = (float)interval_time_m * 1000;
     interval_time_f = interval_time_f / 1000;
     //Injectionクラス[]の変数群にそれぞれの数値群を代入する
    for (int i = 0; i < 3; i++)
    {
        delay_time_f[i] = (float)delay_time_m[i] * 1000;
        delay_time_f[i] = delay_time_f[i] / 1000;

        duration_f[i] = (float)duration_m[i] * 1000;
        duration_f[i] = duration_f[i] / 1000;

        difference_f[i] = (float)difference_m[i] * 1000;
        difference_f[i] = difference_f[i] / 1000;

        elapsed_time_f[i] = (float)elapsed_time[i] * 1000;
        elapsed_time_f[i] = elapsed_time_f[i] / 1000;

      injection[i].channel = (i + 1); //射出チャンネル
      injection[i].delay_time_f = delay_time_f[i];
      injection[i].duration = duration[i];
      injection[i].duration_f = duration_f[i];
      injection[i].difference_f = difference_f[i];
      injection[i].repeat_no = repeat_no;
      injection[i].interval_time_f = interval_time_f;
      injection[i].elapsed_time = elapsed_time_f[i];
    }

    float Elapsed_Time_f = Mathf.Max((float)elapsed_time[0],(float)elapsed_time[1],(float)elapsed_time[2]);
    decimal Elapsed_Time = (decimal)Elapsed_Time_f * 1000;
    Elapsed_Time = Elapsed_Time / 1000;
    //StartScript_v3クラスの変数群に数値群を代入する
    startscript.repeat_no = repeat_no;
    startscript.interval_time_f = interval_time_f;
    startscript.elapsed_time = Elapsed_Time;
        
    for(int i = 0; i < 3; i++)
    {
      injection[i].values_Change();
    }
    startscript.Tweak_elapsed_time();
  }

}

//射出の処理をまとめたクラス
public class Injection : MonoBehaviour
{
  public float delay_time_f; //delay_timeを秒に変換
  public int duration; //射出時間.ミリ秒単位 
  public float duration_f; //durationを秒に変換 
  public float difference_f; //他のチャンネルのdelay_time + durationを足した値をこのチャンネルのdelay_time + durationから引いた差
  public float elapsed_time; //チャンネルの合計経過時間
  public int repeat_no; //それぞれ指定したチャネルの射出を何回繰り返すか指定する変数
  public float interval_time_f; //interval_timeを秒に変換
  public int channel; //Spray_1027()関数で射出するタンクのチャンネルを制御する変数

  //StartScript_v3から呼ばれる
  public void Spray_Function()
  {    
    StartCoroutine(Spray(delay_time_f, channel, duration, duration_f, difference_f, repeat_no, interval_time_f));
    //コルーチンの使いかた 参考記事(https://tech.pjin.jp/blog/2021/12/23/unity-coroutine/)
  }
  private IEnumerator Spray(float delayTime, int Channel, int Duration, float duration_F, float Difference, int Repeat_No, float IntervalTime_F)
  {
    for (int i = 0; i < Repeat_No; i++) //Repeat_Noが1より大きい、つまり0回だと射出が行われない
    {
      yield return new WaitForSeconds(delayTime); //処理を指定秒数のあいだ停止する
      Debug.Log("チャンネル" + Channel + " Delay_Time : " + delayTime + "秒経過");
      HumanDll.HumanClass1.Spray_1027(Channel, Duration); //射出関数
      yield return new WaitForSeconds(duration_F); //処理を指定秒数のあいだ停止する
      Debug.Log("チャンネル" + Channel + " Duration :" + duration_F + "秒経過");
      yield return new WaitForSeconds(Difference); //他のチャンネルの射出が終わってインターバルに移行するまでの待機時間
      Debug.Log("チャンネル" + Channel + "Difference : " + Difference + "秒経過");
      Debug.Log("チャンネル" + Channel + " :" + (i + 1) + "セット目終了");
      if (i != Repeat_No - 1) //Repeat_Noがn回のとき、IntervalTime_Fはn-1回実行される
      {
        yield return new WaitForSeconds(IntervalTime_F); //処理を指定秒数のあいだ停止する.次の射出までの待機時間
        //Debug.Log("チャンネル" + channel + " Interval_Time " + (i + 1) + "回目: " + IntervalTime_F + "秒経過");
      }
    }
  }

  //Save_Values_v2で変更されたチャンネルのパラメータを確認する
  public void values_Change()
  {
    Debug.Log("チャンネル" + channel + ": " + "保存された値");
    Debug.Log("delay_time_f: " + delay_time_f);
    Debug.Log("duration: " + duration);
    Debug.Log("duration_f: " + duration_f);
    Debug.Log("difference: " + difference_f);
    Debug.Log("elapsed_time: " + elapsed_time);
  }
}
