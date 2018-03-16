using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

#if UNITY_UWP

using AssemblyCSharpWSA;
#endif

public class ActionBluetooth : MonoBehaviour
{
	private static double READ_MESSAGE_TIMESPAN = 0.5; //Bluetoothからのデータをreadする周期[s]
	private TimeSpan bluetoothBaseTime; //Bluetoothベース時間
	private string state = "None"; //Bluetoothのステータス　None, Listening, Connected
	private string messageForDisp = ""; //Bluetoothで送られたデータを保存している。DispTextはこの変数を逐次HoloLensに表示する。
	private List<JsonTimeAndPositionLog> taplDataFromAndroid; //Andoridから送られた歩行軌道のデータ。TAPL(TimeAndPositionLog)形式。
	private JsonRoute routeDataFromAndroid; //Androidから送られる経路(Route)のデータ
	private List<JsonTimeAndPositionLog> taplList; //現在利用していない
	private bool taplIsAvailable = false; //現在利用されていない
	private bool taplIsChanged = false; //Androidから送られた歩行軌道のデータ(TAPL形式)が変更された場合にTrueになる
    private TimeSpan oldTime ; //Androidからのデータをreadする周期を管理するのに利用


#if UNITY_UWP
    RFCOMM rfcomm;
#endif

    // Use this for initialization
	/*
	 * Bluetooth接続待機
	 * 変数初期化
	 * Bluetoothでデータを読む周期を管理するoldTimeの初期設定
	 */
    void Start()
	{
		onConnect();
		taplList = new List<JsonTimeAndPositionLog>();
		taplDataFromAndroid = new List<JsonTimeAndPositionLog> ();
        oldTime = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(DateTime.Now.TimeOfDay.Days, DateTime.Now.TimeOfDay.Hours,DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds-(int)READ_MESSAGE_TIMESPAN-1, DateTime.Now.TimeOfDay.Milliseconds));
	}

	// Update is called once per frame
	/*
	 * Bluetoothの状態がNoneなら接続待機状態へ
	 * Bluetoothベース時間の設定
	 * READ_MESSAGE＿TIMESPAN周期でメッセージをread
	 */
	void Update()
	{
#if UNITY_UWP
        state = rfcomm.state;
		if(state == "None") onConnect();
        if (rfcomm.isConnectedBluetooth)
        {
            setBluetoothBaseTime();
            rfcomm.isConnectedBluetooth = false;
        }
        //readMessage();
        if (DateTime.Now.Subtract(oldTime).Second > READ_MESSAGE_TIMESPAN)
        {
            oldTime = DateTime.Now.TimeOfDay;
            readMessage();
        }
#endif

    }

	/*
	 * RFCOMMによってHoloLensをBluetooth接続待機状態にするためにメソッド
	 */
    void onConnect()
	{
#if UNITY_UWP
        rfcomm = new RFCOMM();
        rfcomm.mainAsync().Wait();
        state = "Listening";
#endif


    }


#if UNITY_UWP
	/*
	* Bluetoothによるデータ送信
	* RFCOMMのSendMessageメソッドにデータ(string)を渡す
	*/
    private void sendMessage(string message)
    {
        if (rfcomm == null) return;
        rfcomm.SendMessage(message);
    }

	/*
	* Bluetoothからのデータ受信
	* RFCOMMのgetMessageからデータ(string)を受け取る
	* messageForDispはDispTextでHoloLensの画面に受信データを表示するための変数
	* parseJsonMessageはデータのパースを行う
	*/
    private void readMessage()
    {
        if (rfcomm == null) return;
        string message = rfcomm.getMessage();
        if (message != "")
        {
			messageForDisp = "";
			messageForDisp = message;
            parseJsonMessage(message);

        }
    }

#endif

	/*
	 * Bluetoothのステータスを渡すメソッド
	 * ステータス(string)：
	 * 		"None"(接続がない状態)・・・接続待機に失敗したなどの場合
	 * 		"Listenning"(接続待機中)・・・アプリを起動するとデフォルトで待機状態になる
	 * 		"Connected"(接続中)・・・接続が完了されるとこの状態になる
	*/
    public string getState(){
		return state;
	}

	/*
	 * DispTextクラスにmessageForDispを渡すメソッド
	*/
	public string getMessageForDisp(){
		return messageForDisp;
	}

	/*
	 * Bluetoothから送られたメッセージをパースするメソッド
	 * 受信したデータはJSONDataStructureの形にパースされる
	 * データ形式はJSONDataStructureを参照のこと
	*/
	public void parseJsonMessage(string message){
		JsonDataFromAndroid dataFromAndroid = JsonConvert.DeserializeObject<JsonDataFromAndroid>(message); 
		if (dataFromAndroid != null) {
			taplDataFromAndroid.AddRange(dataFromAndroid.trajectory);
            taplIsChanged = true;
			routeDataFromAndroid = dataFromAndroid.global_route;
		}
	}

	/*
	 * 以前使用されていたメソッドで現在は利用無し
	*/
	public List<JsonTimeAndPositionLog> getTAPL(){
		if (taplIsAvailable) {
			List<JsonTimeAndPositionLog> tmp = new List<JsonTimeAndPositionLog>();
            if (taplList.Count >= CordinateSystemMatching.CALCULATE_POSITION_NUMBER)
            {
                for(int i = 0; i<CordinateSystemMatching.CALCULATE_POSITION_NUMBER; i ++)
                {
                    tmp.Add(taplList[taplList.Count - i - 1]);
                }
                return tmp;
            }
			taplIsAvailable = false;
			return taplList;
		}else
			return null;
	}


	/*
	* Andoridから得られたデータをParseした結果のうちTAPL(TimeAndPositionLog)を渡すメソッド
	* 受信データが更新されたとき、taplIsChangedがTrueになりデータを渡すことができる
	* データをこのメソッドを通して渡したときは、taplIsChangedがfalseになり、同じデータは渡さないことになる
	* データがない場合は、空のデータ型を返す
	*/
	public List<JsonTimeAndPositionLog> getTAPLDataFromAndroid(){
		if (taplDataFromAndroid != null && taplIsChanged) {
			List<JsonTimeAndPositionLog> tmp = taplDataFromAndroid;
			taplDataFromAndroid = new List<JsonTimeAndPositionLog>();
            taplIsChanged = false;
			return tmp;
		}
		return new List<JsonTimeAndPositionLog>();
	}

	/*
	 * Andoridから得られたデータをParseした結果のうちRouteを渡すメソッド
	 * データがない場合は、空のデータ型を返す
	 */
	public JsonRoute getRouteDataFromAndroid(){
		if (routeDataFromAndroid != null) {
			JsonRoute tmp = routeDataFromAndroid;
			routeDataFromAndroid = null;
			return tmp;
		}
		return null;
	}

	/*
	 * AndroidにJSONに成形したデータを渡すメソッド
	 * データ型はJSONStructureを参照のこと
	*/
	public void sendMessageToAndroid(JsonSelfLocation jsl, JsonMessage message){
		JsonDataFromHoloLens jdfh = new JsonDataFromHoloLens (jsl, message);
        string json = JsonConvert.SerializeObject(jdfh);
		#if UNITY_UWP
		sendMessage(json);
		#endif
	}

	/*
	 * BluetoothでAndroidと接続した瞬間にこのメソッドが呼ばれ、その瞬間の時間をベース時間として記録するメソッド
	 * AndroidとHoloLensはBluetothが接続された瞬間の時間をそれぞれベース時間として記録しており、座標系マッチングにおいて利用されるタイムスタンプはベース時間からの経過分として記録される
	 * これは、それぞれのデバイスで持つ時間が違う事が予想され、それぞれの時間でタイムスタンプを取るとオフセットのズレが発生するためである
	*/
    public void setBluetoothBaseTime()
    {
		bluetoothBaseTime = DateTime.Now.TimeOfDay;
    }

	/*
	 * Bluetoothのベース時間を渡すメソッド
	 * AndroidとHoloLensはBluetothが接続された瞬間の時間をそれぞれベース時間として記録しており、座標系マッチングにおいて利用されるタイムスタンプはベース時間からの経過分として記録される
	 * これは、それぞれのデバイスで持つ時間が違う事が予想され、それぞれの時間でタイムスタンプを取るとオフセットのズレが発生するためである
	*/
    public TimeSpan getBluetoothBaseTime()
    {
        return bluetoothBaseTime;
    }
}

