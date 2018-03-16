using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;


/*
 * Bluetoothでやり取りするデータを格納するデータ形式 
 */
public class JSONStructures{

}

/*
 * HoloLensから送るデータ形式
 * self_location:自己位置
 * message:方向指示(Left or Right)
 */
public class JsonDataFromHoloLens{
    [JsonProperty("self_location")]
    public JsonSelfLocation selfLocation;
    [JsonProperty("message")]
    public JsonMessage message;

	public JsonDataFromHoloLens(JsonSelfLocation jsl, JsonMessage jm){
		selfLocation = jsl;
		message = jm;
	}
}

/*
 * Androidから送られるデータ形式
 * trajectory:歩行軌道
 * global_route:グローバル経路
 */
public class JsonDataFromAndroid{
	public List<JsonTimeAndPositionLog> trajectory;
    public JsonRoute global_route;

	public JsonDataFromAndroid(List<JsonTimeAndPositionLog> t, JsonRoute gr){
		trajectory = t;
        global_route = gr;
	}
}

/*
 * 時間とロケーションを組み合わせて持つクラス。歩行軌道はこのクラスのListで形成される
 * location:位置情報
 * time:タイムスタンプ
 */
public class JsonTimeAndPositionLog {
	public JsonLocation location;
    public JsonTime time;
    [JsonIgnore]
	public TimeSpan mTime { 
		get{
            if(time == null)
            {
                return DateTime.Now.TimeOfDay;
            }
			return new TimeSpan (DateTime.Now.TimeOfDay.Days, DateTime.Now.TimeOfDay.Hours,time.minutes,time.seconds,time.milliseconds);
        }
	}

	public JsonTimeAndPositionLog(double x, double y){
		 location = new JsonLocation(x,y);
        time = new JsonTime();
        time.mTime = DateTime.Now.TimeOfDay;
	}
    
    public JsonTimeAndPositionLog()
    {
        
    }

	public JsonTimeAndPositionLog(JsonLocation l, JsonTime t)
	{
		location = l;
		time = t;
	}

}

/*
 * 座標点を格納するクラス
 * x:X座標
 * y:Y座標
 */
[JsonObject("location")]
public class JsonLocation{
    [JsonProperty("x")]
    public double x;
    [JsonProperty("y")]
    public double y;

	public JsonLocation(double X, double Y){
		x = X;
		y = Y;
	}
}

/*
 * 時間を格納するクラス。主にタイムスタンプで使われるため、時間・日の情報は持たない
 * minutes:分
 * seconds:秒
 * millisecounds:ミリ秒
 * mTime:他のクラスから呼び出すとき分・秒・ミリ秒が分かれていると使い勝手がわるいため、TimeSpan形式で取り扱えるように
 */
public class JsonTime
{
    public int minutes = 0;
    public int seconds = 0;
	public int milliseconds = 0;
    [JsonIgnore]
	public TimeSpan mTime{
		set{ 
			minutes = value.Minutes;
			seconds = value.Seconds;
			milliseconds = value.Milliseconds;
		}	
	}

    public JsonTime()
    {

    }
}

/*
 * 経路情報
 * changed:経路が変更されたか否か
 * route:経路の座標点情報
 */
public class JsonRoute{
	public bool changed;
	public List<JsonLocation> route;

	public JsonRoute(){
		route = new List<JsonLocation> ();
	}
}

/*
 * 自己位置。自分が向く方向も合わせて格納している
 * rotation:HoloLensの向く方向
 * location:自己位置の座標点
 */
[JsonObject("self_location")]
public class JsonSelfLocation{
    [JsonProperty("rotation")]
    public double rotation;
    [JsonProperty("location")]
    public JsonLocation location;

	public JsonSelfLocation(double r, JsonLocation jl){
		rotation = r;
		location = jl;
	}
}

/*
 * Hololnsからの方向指示(Message)
 * interrupting:trueならPullDogの指示に割り込んでHoloLensの指示を優先させる(障害物が発生した時にTrue)
 * message:Left or Right (現状)
 */
[JsonObject("message")]
public class JsonMessage{
    [JsonProperty("interrupting")]
    public bool interrupting = false;
    [JsonProperty("message")]
    public string message;
}