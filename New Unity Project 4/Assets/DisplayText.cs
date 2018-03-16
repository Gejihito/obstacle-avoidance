using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisplayText : MonoBehaviour {

	private TextMesh[] txt; //HoloLensCameraの子要素であるTextMeshと紐づけされるTextMeshの配列
	private string[] textForDisp; //表示するstringのArray
	private int numOfChild = 0; //HoloLensCameraの子要素であるTextMeshの数

	public GameObject destination;
	public Direction direction;

	public DestinationPlacement dp;

	public ActionBluetooth blue;
	public CordinateSystemMatching csm;
	public MakeRoute mr;
	public LineManager trajectory;
	public TimeStampLogger tsl;


	/*
	 * TextMeshの数を取得してその数分だけ、表示用のTextMesh、表示用のstringを生成する
	 * そして実際に表示に使いたいTextMeshを空のTextMeshのArrayと紐づけ
	 * 最後に全てのTextMeshに"Ready..."と表示する
	 */
	void Start () {
		numOfChild = gameObject.GetComponentsInChildren<TextMesh> ().Length;
		txt = new TextMesh[numOfChild];
		textForDisp = new string[numOfChild];
		txt = gameObject.GetComponentsInChildren<TextMesh> ();
		for (int i =0;i<numOfChild;i++) {
			setText("Ready ...",i);
		}
	}

	/*
	 * setText ("表示したいString", 表示先のTextMeshの番号);でHoloLensの画面上のTextMeshにStringが表示可能
	 * TextMeshは
	 * 1．sceneのHoloLensCameraの子要素としてTextMeshを新たに追加(他のをコピーするとやりやすい)
	 * 2．表示したい位置を決める(他のをコピーすることで位置決めが容易)
	 * 3．このUpdate()にsetText("表示したいString", 表示先のTextMeshの番号);と記述し、完了
	 */
	void Update () {
		display ();
		setText ("Pos: " + destination.transform.position, 0);
		setText ("Dis: " + Vector3.Distance (destination.transform.position, transform.position), 1);
		setText ("Rot: " + getDistinationVec(), 2);
		setText ("Guidance: " + getDirection (), 3);
		setText ("Bluetooth State: " + getBluetoothState (), 4);
		setText ("GapX: " + csm.getGap ().gapX + " GapY: " + csm.getGap ().gapY + " Angle: " + csm.getGap ().gapAngle, 5);
		setText ("Avoiding :" + mr.huNav.isAvoiding, 6);
		setText ("Trajectory :" + trajectory.isLineOn, 7);
        if (csm.getPulldogData() != null && csm.getHoloLensData() != null)
        {
            setText("pDataNum :" + csm.getPulldogData().Count + " hDataNum：" + csm.getHoloLensData().Count, 8);
        }
        setText ("Test :" + getTestText (), 9);
        if (csm.getPulldogData() != null)
        {
            if (csm.getPulldogData().Count != 0)
            {
                List<JsonTimeAndPositionLog> pulldogData = csm.getPulldogData();
                setText(pulldogData.Last().mTime.Minutes + "," + pulldogData.Last().mTime.Seconds + "," + pulldogData.Last().location.x + "," + pulldogData.Last().location.y, 10);
            }
        }
        
        if (tsl.getJsonTimeAndPositionLogByBaseTime(blue.getBluetoothBaseTime()).Count != 0)
        {
            List<JsonTimeAndPositionLog> taldList = tsl.getJsonTimeAndPositionLogByBaseTime(blue.getBluetoothBaseTime());
            var now = taldList[taldList.Count - 1];

            setText(now.mTime.Subtract( blue.getBluetoothBaseTime()).Minutes + "," + now.mTime.Subtract( blue.getBluetoothBaseTime()).Seconds + "," + now.location.x + "," + now.location.y, 11);
        }
		setText ("Bluetooth :" + blue.getMessageForDisp (), 12);


		dp.humanNav.log.writeMessage_me ("me,"+transform.position.x+","+ transform.position.z);
	}

	/*
	 * TextMeshにStringを書き込む
	 */
	void display(){
		for (int i = 0; i < numOfChild; i++) {
			if (txt [i].text != textForDisp [i])
				txt [i].text = textForDisp [i];
		}
	}

	/*
	 * textForDispに表示したいstringをセットする
	 */
	void setText(string t, int index){
		textForDisp[index] = t;
	}

	/*
	 * 自分(Camera)から次の目標地点(Destination)までの距離を計算する(表示のため)
	*/
	private float getDistinationVec(){
		Vector3 tmp = Camera.main.transform.forward;
		Vector3 tmp2 = (destination.transform.position - Camera.main.transform.position).normalized;
		return Vector2.Angle (new Vector2 (tmp.x, tmp.z), new Vector2 (tmp2.x, tmp2.z));
	}

	/*
	 * Directionからクロックポジションに基づく指示方向を取得する(表示のため)
	*/
	private string getDirection(){
		if (direction != null){
			var dir = direction.translateDirectionToClockPosition (direction.getDirectionBWPathAndMe()) ;
			if(dir != "") return  dir + "oclock";
		}	
		return "";
	}

	/*
	 * テスト用のメソッド
	*/
	private string getTestText(){
		/*Vector2 tmp = new Vector2(-Camera.main.transform.forward.x,Camera.main.transform.forward.z);
		var tmp2 = (destination.transform.position - Camera.main.transform.position).normalized;
		Vector2 tmp3 = new Vector2 (tmp2.x,tmp2.z);
		if(Vector2.Dot(tmp,tmp3)>0) return "Left";
		else if(Vector2.Dot(tmp,tmp3)<0) return "Right";
		else return "Forward";
		*/
		return "";
	}

	/*
	 * bluetoothの状態を取得する(表示のため)
	*/
	private string getBluetoothState(){
		if(blue == null) return "";
		return blue.getState ();
	}
}
