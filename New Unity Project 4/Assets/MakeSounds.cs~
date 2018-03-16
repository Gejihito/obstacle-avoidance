using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;
using UnityEngine.Windows.Speech;

public class MakeSounds : MonoBehaviour, IInputClickHandler {

	public LineRenderer path;
	public TextToSpeechManager textToSpeech;
	public MakePlanes mkPlanes;
	public Direction pathDirection;
	public ActionBluetooth ab;
	public CordinateSystemMatching cs;

	private float timeOut = 8.0f; //方向指示の発生周期[s]
	private float time = 0.0f; //周期的方向指示に利用
	private string oldGuidance = ""; //前回のアナウンス内容
	private bool guidanceNotStarted = true; //案内が開始されたらTrue
	public bool guidanceFinish = false; //ガイダンスが終了したらTrue
	public int guidanceNum = 1;	//1:clock position, 5:androidと連携 詳細はactionGuidanceを参照の事

	/*
	 * Pathを取得する
	 */
	// Use this for initialization
	void Start () {
		//	エアタップ取得
		//InputManager.Instance.PushFallbackInputHandler(gameObject);
		path = GameObject.Find ("Path").GetComponent<LineRenderer>();
	}

	/*
	 * 床面推定が済んでいたら(Spatial Mappingが終わって案内ができる状況ができたら)、ガイダンスの状態から案内開始・案内途中・案内終了のアナウンスをする
	 */
	// Update is called once per frame
	void Update () {
		if (mkPlanes.makedPlane){
			if (guidanceNotStarted) {
                actionGuidance(3);
				mkPlanes.mkRoute.huNav.log.writeMessage_time ("started");
            } else if (!guidanceFinish)
				actionGuidance (guidanceNum);
			else {
				actionGuidance (2);
				mkPlanes.mkRoute.huNav.log.writeMessage_time ("finished");
			}
		}
			
		//textToSpeech.transform.position = path.GetPosition (1);
	}

	public void OnInputClicked(InputClickedEventData eventData)
	{
		
	}

	/*
	 * クロックポジションに基づきガイダンスを生成する
	 */
	private string getVoiceOfGuidanceOfClockPosition(){
		string guidance = "";
		if(pathDirection != null){
			guidance = pathDirection.translateDirectionToClockPosition (pathDirection.getDirectionBWPathAndMe());
		}
		if(guidance != "") guidance = guidance + "oclock";
		return guidance;
	}

	/*
	 * Android に送るLeftかRightのみの指示を生成する
	 */
	private string getVoiceOfGuidanceOfRightOrLeft(){
		string guidance = "";
		if(pathDirection != null){
			guidance = pathDirection.translateDirectionToRightOrLeft (pathDirection.getDirectionBWPathAndMe());
		}
		return guidance;
	}

	/*
	 * ガイダンスの発生を行う。
	 */
	private void actionGuidance(int version){

		string guidance = "";
		time += Time.deltaTime;

		//Creat String of Navigation　発生するガイダンスの文字列を変数に格納する
		switch(version){
			case 1: //方向の指示。指示が変わらない場合は指示間隔がtimeOut[s]毎になる
				if (path != null && pathDirection != null) {
					guidance = getVoiceOfGuidanceOfClockPosition ();
				}
                timeOut = 8.0f;
				break;
			case 2: //案内終了のアナウンス
				guidance = "finished navigation";
				break;
            case 3: //案内開始のアナウンス
                guidance = "started navigation";
                timeOut = 3.0f;
                break;
			case 4: //スキャンが必要なことを知らせるガイダンス
				guidance = "Required scanning";
				timeOut = 5.0f;
				break;
			case 5: //Androidにデータを送る
				if (ab.getState () == "Connected") {
					if (path != null && pathDirection != null) {
						sendRightOrLeftMessageToAndroid (mkPlanes.mkRoute.huNav.isAvoiding,getVoiceOfGuidanceOfRightOrLeft ());
						return;
					}
				}
				break;
            default:
				break;
		}

		//Speak　音声発生部分
		if(oldGuidance != guidance){
			textToSpeech.SpeakText (guidance);
		}else if (time >= timeOut) {
            if (guidanceNotStarted)
            {
                guidanceNotStarted = false;
                return;
            }
			textToSpeech.SpeakText (guidance);
			time = 0.0f;
		}
		oldGuidance = guidance;
	}

	/*
	 * Andoroidに指示を送る
	 */
	public void sendRightOrLeftMessageToAndroid(bool i, string m){
		JsonMessage message = new JsonMessage ();
		message.message = m;
		message.interrupting = i;
		ab.sendMessageToAndroid (cs.getSelfLocation (), message);
	}
}
