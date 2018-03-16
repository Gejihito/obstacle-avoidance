using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TimeStampLogger : MonoBehaviour {

    private static double LOGGING_TIMESPAN = 0.2; //ログを取る時間周期[s]
    private List<JsonTimeAndPositionLog> jTAPLList = new List<JsonTimeAndPositionLog> (); //記録されたログのList
    private TimeSpan oldTime; //ログを取る周期管理に利用

    // Use this for initialization
    void Start () {
		
	}

	/*
	 * HoloLensの自己位置をLOGGING_TIMESPAN毎に記録
	 * リストに追加
	 */
	// Update is called once per frame
	void Update () {
		var tapl = new JsonTimeAndPositionLog (transform.position.x,transform.position.z);
        if (DateTime.Now.Subtract(oldTime).Second > LOGGING_TIMESPAN)
        {
            oldTime = DateTime.Now.TimeOfDay;
            jTAPLList.Add(tapl);
        }
        
	}

	/*
	 * 記録されたTAPL(TimaAndPositionLog)のタイムスタンプからbaseTimeを引いた状態にする
	 */
	public List<JsonTimeAndPositionLog> reTimeStampByBaseTime(List<JsonTimeAndPositionLog> list, TimeSpan baseTime){
		List<JsonTimeAndPositionLog> res = new List<JsonTimeAndPositionLog> ();
		for(int i = 0;i<list.Count;i++){
			JsonTime jt = new JsonTime ();
			jt.mTime = list [i].mTime.Subtract (baseTime);
			res.Add(new JsonTimeAndPositionLog(list[i].location,jt));
		}
		return res;
	}

	/*
	 * baseTimeより後に記録されたログを取得するメソッド
	 */
	public List<JsonTimeAndPositionLog> getJsonTimeAndPositionLogByBaseTime(TimeSpan baseTime){
		return reTimeStampByBaseTime(jTAPLList.Where( item => item.mTime.CompareTo(baseTime) == 1).ToList(),baseTime);
	}

	/*
	 * HoloLensの自己位置をHoloLensの座標系上の値で返すメソッド
	 */
	public JsonLocation getCurrentLocationOnHoloLensCordinates(){
		return new JsonLocation (transform.position.x,transform.position.z);
	}

	/*
	 * 記録したログを消す
	 */
	public void clearjTAPLList(){
		jTAPLList.Clear ();
	}
}
