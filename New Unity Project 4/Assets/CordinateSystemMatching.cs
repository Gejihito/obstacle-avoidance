using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CordinateSystemMatching : MonoBehaviour {

	public static int CALCULATE_POSITION_NUMBER = 30;  //Gap計算時におけるpDataとhDataの点数
	private Gap cordinateGap;   //HoloLensとPullDogの座標系のズレ
	private List<JsonTimeAndPositionLog> hData; //HoloLensの歩行軌道。TAPL(TimeAndPositionLog)データの形式。
	private List<JsonTimeAndPositionLog> pData; //PullDogの歩行軌道。TAPL(TimeAndPositionLog)データの形式。
	public OutputLog ol; 
	public TimeStampLogger tsl;
	private ActionBluetooth ab;
	public Camera camera;
    


	// Use this for initialization
	/*
	 * 変数の初期化
	 */
	void Start () {
		hData = new List<JsonTimeAndPositionLog> ();
		pData = new List<JsonTimeAndPositionLog> ();
		ol = GetComponent<OutputLog> ();
		ab = GetComponent<ActionBluetooth> ();
        cordinateGap = new Gap();
	}
	
	// Update is called once per frame
	/*
	 * データのサンプリングを行い、データ数が十分であれば座標系同士のズレを計算する
	 */
	void Update () {
		if (ab != null) {
			pData.AddRange(ab.getTAPLDataFromAndroid ()); //Bluetooth通信でPULLDOGの時間・Location情報を取得
			if (pData.Count != 0) { //返値がNullでなければ
				if (pData.Count  > CALCULATE_POSITION_NUMBER +10) //サンプリングを行うときにデータをpDataのデータ数を10個余分に見ておくことで、一回のサンプリングで完了できるようにしている(10個は適当)
                {
                    List<List<JsonTimeAndPositionLog>> listForCalGap = samplingFromHoloLensLogList(tsl.getJsonTimeAndPositionLogByBaseTime(ab.getBluetoothBaseTime()), pData); //参照する数だけHoloLensの時間・Location情報を取得
                    if (listForCalGap != null)
                    { //返値がNullでなければ
                        cordinateGap = calculateGap(listForCalGap); //Gapを計算
                        for (int i = 0; i < listForCalGap[1].Count || i < listForCalGap[0].Count; i++) //データの表示
                        {
                            if (i < listForCalGap[1].Count)
                            {
                                ol.writeMessage_trajectory("HoloLens,Time," + listForCalGap[1][i].mTime.Minutes + ";" + listForCalGap[1][i].mTime.Seconds + ";" + listForCalGap[1][i].mTime.Milliseconds + ",x," + listForCalGap[1][i].location.x + ",y," + listForCalGap[1][i].location.y);
                            }
                            if (i < listForCalGap[0].Count)
                            {
                                ol.writeMessage_trajectory("PullDog,Time," + listForCalGap[0][i].mTime.Minutes + ";" + listForCalGap[0][i].mTime.Seconds + ";" + listForCalGap[0][i].mTime.Milliseconds + ",x," + listForCalGap[0][i].location.x + ",y," + listForCalGap[0][i].location.y);
                            }
                        }
                        ol.writeMessage_trajectory("Gap,x," + cordinateGap.gapX + ",y," + cordinateGap.gapY + ",Angle," + cordinateGap.gapAngle); //データの表示
                        //cordinateGap = new Gap();
                        pData = new List<JsonTimeAndPositionLog>();
                        tsl.clearjTAPLList();
                        //CALCULATE_POSITION_NUMBER += 10;
                    }
                }
			}
		}
	}

	/*
	 *データのサンプリング
	 * pDataを基準として、タイムスタンプが一致するhDataをサンプリングする
	 * サンプリングが失敗したorデータ数が足りない場合はnullを返す
	 */
	private List<List<JsonTimeAndPositionLog>> samplingFromHoloLensLogList(List<JsonTimeAndPositionLog> h, List<JsonTimeAndPositionLog> p){
		List<JsonTimeAndPositionLog> logH = new List<JsonTimeAndPositionLog>();
        List<JsonTimeAndPositionLog> logP = new List<JsonTimeAndPositionLog>();
		if (h == null || p == null)
			return null;
		if (h.Count < CALCULATE_POSITION_NUMBER || p.Count < CALCULATE_POSITION_NUMBER)
			return null;
        
		int pastDataNum = 0;
        foreach (JsonTimeAndPositionLog pdata in p)
        {
            List<JsonTimeAndPositionLog> tmp = new List<JsonTimeAndPositionLog>();
            JsonTimeAndPositionLog result = null;
           
            for(int i = pastDataNum;i< h.Count ; i++)
			{
                if((Math.Abs(h[i].mTime.Subtract(pdata.mTime).Seconds) + Math.Abs(h[i].mTime.Subtract(pdata.mTime).Minutes)*60) < 1.0) tmp.Add(h[i]);
            }
            double difference = 1000;
            foreach (JsonTimeAndPositionLog t in tmp)
            {
                if (Math.Abs(t.mTime.Subtract(pdata.mTime).Milliseconds ) < difference)
                {
                    result = t;
                    difference = Math.Abs(t.mTime.Subtract(pdata.mTime).Milliseconds);
                }
            }
            if (result != null)
            {
                logP.Add(pdata);
                logH.Add(result);
            }
            if (logH.Count >= CALCULATE_POSITION_NUMBER && logP.Count >= CALCULATE_POSITION_NUMBER)
            {
                List<List<JsonTimeAndPositionLog>> res = new List<List<JsonTimeAndPositionLog>>();
                res.Add(logP);
                res.Add(logH);
                return res;
            }
        }
        return null;
	}

	/*
	 * TAPL(TimeAndPositionLog)の形式のデータをArray形式に変更
	 */
	private double[,] getPositionArrayFromTAPLList(List<JsonTimeAndPositionLog> list){
		double[,] tmp = new double[CALCULATE_POSITION_NUMBER, 2];
		for(int i=0;i<CALCULATE_POSITION_NUMBER;i++){
			tmp [i, 0] = list [i].location.x;
			tmp [i, 1] = list [i].location.y;
		}
		return tmp;
	}

	/*
	 * pDataを渡す
	 */
    public List<JsonTimeAndPositionLog> getPulldogData()
    {
        return pData;
    }

	/*
	 * hDataを渡す
	 */
    public List<JsonTimeAndPositionLog> getHoloLensData()
    {
        return hData;
    }

	/*
	 * Gapを返す
	 */
	public Gap getGap(){
		return cordinateGap;
	}

	/*
	 * 得られたpDataとhDataを統合したListからGapを計算するメソッド
	 * 計算の詳細は別資料を参照のこと
	 */
	public Gap calculateGap(List<List<JsonTimeAndPositionLog>> data)
	{
		Gap gap = new Gap ();
		//double[,] a = new double[,] { { 1, 1 }, { 1, 2 }, { 3, 1 } };//sample data
		double[,] a = getPositionArrayFromTAPLList(data[0]);    //PullDogのデータ
		//double[,] b = new double[,] { { -2, 0 }, { -2, -1 }, { -4, 0 } };
		double[,] b = getPositionArrayFromTAPLList(data[1]);    //HoloLensのデータ
		int[] direction = new int[] { 1, 1, 1 };//次にとる方向 {x,y,Angle} これはどうする？
		double RotationAngle;
		double moveX, moveY;
		double T = 10;//温度パラメータ
		double R = 6.369;//角度の分解能要検証
		//double minX, minY, minAngle;
		double MSE=0;
		double preMSE=0;
		var cRandom = new System.Random(); 
		RotationAngle = 0;//初期値
		moveX = 0;
		moveY = 0;
		preMSE = MSECalc(RotationAngle, a, b, CALCULATE_POSITION_NUMBER, moveX, moveY);
		while (T > 0.01)
		{
			int Random = cRandom.Next(10);
			MSE = MSECalc(RotationAngle, a, b, CALCULATE_POSITION_NUMBER, moveX +(direction[0] * T), moveY);
			if (MSE < preMSE|| Random > (8 + T)){
				moveX += direction[0] * T;
				preMSE = MSE;
			}else{
				direction[0] *= -1;//x方向変える
			}

			MSE = MSECalc(RotationAngle, a, b, CALCULATE_POSITION_NUMBER, moveX, moveY + (direction[1] * T));
			Random = cRandom.Next(10);
			if (MSE < preMSE || Random > (8 + T)){
				moveY += direction[1] * T;
				preMSE = MSE;
			}else{
				direction[1] *= -1;//y方向変える
			}

			MSE = MSECalc(RotationAngle + (direction[2] * T / R), a, b, CALCULATE_POSITION_NUMBER, moveX, moveY);
			Random = cRandom.Next(10);
			if (MSE < preMSE || Random > (8 + T)){
				RotationAngle += direction[2] * T / R;
				preMSE = MSE;
			}else{
				direction[2] *= -1;//r方向変える
			}

			T *= 0.99;
		}
		//Console.WriteLine("{0},{1},{2}", moveX,moveY,degree(RotationAngle));
		//ココから平均二乗誤差計算
		//Console.ReadKey();
		gap.gapX = moveX;
		gap.gapY = moveY;
		gap.gapAngle = degree(RotationAngle);
        return gap;
	}
	//平均二乗誤差計算
	private static double MSECalc(double radian, double[,] a,double[,] b,int num,double x,double y )
	{
		double[] xy = new double[] { 0, 0 };
		double MSE = 0;

		for (int i = 0; i < num; i++){
			xy[0] = b[i, 0] - b[0,0];
			xy[1] = b[i, 1] - b[0,1];
			xy = Rotation(radian, xy);
			MSE += Math.Pow(a[i, 0] - (xy[0] + x + b[0, 0]), 2) + Math.Pow(a[i, 1] - (xy[1] + y + b[0, 1]), 2);

		}
		return MSE;
	}
	//原点からRotationAngle°回転させた座標を出力
	private static double[] Rotation(double radian, double[] coordinate)
	{
		double[] xy = new double[] { 0, 0 };
		xy[0] = Math.Cos(radian) * coordinate[0] - Math.Sin(radian) * coordinate[1];
		xy[1] = Math.Cos(radian) * coordinate[1] + Math.Sin(radian) * coordinate[0];
		return xy;
	}

	//2点から角度を出す。
	private static double getRadian(double x, double y, double x2, double y2) {
		double radian = Math.Atan2(y2 - y, x2 - x);
		return radian; 
	}

	//radianをdegreeに変換
	protected static double degree(double degree){
		var deg = degree * 180d / Math.PI;
		if (deg < 0)
			return deg + 360;
		return deg;
	}

	/*
	 * 計算したGapによって渡された座標の平行移動・回転を行う
	 */
	public JsonLocation transformingCordinatesByGap(JsonLocation loc){
		if(cordinateGap != null){
			JsonLocation newLoc = new JsonLocation (0.0, 0.0);
			newLoc.x = loc.x * Math.Cos (cordinateGap.gapAngle) - loc.y * Math.Sin (cordinateGap.gapAngle);
			newLoc.y = loc.y * Math.Sin (cordinateGap.gapAngle) + loc.y * Math.Cos (cordinateGap.gapAngle);
			newLoc.x = newLoc.x + cordinateGap.gapX;
			newLoc.y = newLoc.y + cordinateGap.gapY;
			return newLoc;
		}
		return null;
	}


	/*
	 * HoloLensの自己位置座標をGapによって座標変換し、PullDog座標系上の座標点として返すメソッド
	 */
	public JsonLocation getCurrentLocationOnPullDogCordinates(){
		return transformingCordinatesByGap(tsl.getCurrentLocationOnHoloLensCordinates());
	}

	/*
	 * HoloLensの向いている方向と自己位置をJsonSelfLocationの形式で渡すメソッド
	 */
	public JsonSelfLocation getSelfLocation(){
		var theta = degree(Math.Atan2 (camera.transform.forward.x,camera.transform.forward.z));
		JsonSelfLocation l = new JsonSelfLocation (theta + cordinateGap.gapAngle, getCurrentLocationOnPullDogCordinates ());
		return l;
	}

	/*
	 *全てのpDataをギャップに基づき座標変換した結果を返すメソッド
	 */
	public List<JsonTimeAndPositionLog> getTransformedPulldogData(List<JsonTimeAndPositionLog> p){
        List<JsonTimeAndPositionLog> d = new List<JsonTimeAndPositionLog>();
        if (p == null) return d;
		foreach (JsonTimeAndPositionLog j in p) {
			d.Add (new JsonTimeAndPositionLog (transformingCordinatesByGap (j.location), j.time));
		}
		return d;
	}

}
	
public class Gap{//HoloLensとPullDogの座標系のズレ。主にCordinateSystemMatchingで用いられる。
	public double gapX = 0.0; //X方向のズレ分
	public double gapY = 0.0; //Y方向のズレ分
	public double gapAngle = 0.0; //回転方向のズレ分
}
