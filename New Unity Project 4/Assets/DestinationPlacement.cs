using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class DestinationPlacement : MonoBehaviour, IInputClickHandler
{
	public MakeRoute mkRoute;
	public MakePlanes mkPlane;
	public MakeSounds mkSounds;
	public HumanNavigation humanNav;


	private TagPlacement oldDestination;
	private Route route;

	/*
	 * HoloLens上のRouteをRouteオブジェクトとして取得
	 */
	void Start()
	{
		InputManager.Instance.PushFallbackInputHandler(gameObject);
		route = mkRoute.getRoute ();
	}

	/*
	 * HoloLens上のRoute(Routeオブジェクト)を逐次取得
	 * Routeオブジェクトにおいて次の目標地点を設定し、そこをDestinationとして設定
	 * HoloLensは基本的にDestinationと現在地点を歩行可能エリア上で最短で結びそれをローカル経路としている
	 */
	void Update()
	{
		route = mkRoute.getRoute (); //経路を逐次取得
		if (route != null && mkPlane.makedPlane) { //Routeがnullでない and 歩行可能エリアを計算するための床面推定がすんでいる
			if (oldDestination != route.getNextDestination () || !oldDestination.isOnNavMesh) { 
				//前回のDestinaitonが現在のDestinationと違っていたら(つまり、Destinationに到着するというプロセスが発生したら)。または、初めて設置するoldDestiantion(このループの中でoldDesitinationにnextDestinaitonが入るため、実質的にはnextDestination)が歩行可能エリアに設置できない時
				//要約すると、Destiantionが置けないまたはDestinantionに到着した時このifに入る
				if(route.isFinished){ //案内が終わっている場合、True
					mkSounds.guidanceFinish = true;
					return;
				}
				this.transform.position = new Vector3 (route.getNextDestination ().getOriginToDestination ().x, 0.0f, route.getNextDestination ().getOriginToDestination ().y); //Destinaitonは便宜的に円柱のオブジェクトになっているがそれの位置決定
				oldDestination = route.getNextDestination (); //oldDestinaitonを更新
				if(humanNav.reachedDestination()) oldDestination.isOnNavMesh = true; //Destinaitonに到着した時、oldDestiantionのisOnNavMeshをtrueにしてoldDestiantionが更新されるようにする
				else oldDestination.isOnNavMesh = false; 
			}

		}else {
			route = mkRoute.getRoute ();
		}
	}
	/*
	 *現在は利用無し 
	 */
	public void OnInputClicked(InputClickedEventData eventData)
	{
		/*if (GazeManager.Instance.IsGazingAtObject)
		{
			var hitInfo = GazeManager.Instance.HitInfo;
			transform.position = hitInfo.point + transform.localScale.y * Vector3.up;

		}*/
	}

	/*
	 * Destinaiton3Dポリゴン(便宜的に設置された円柱状のポリゴン)に3Dポリゴンが衝突した時このメソッドが呼ばれ、衝突したポリゴンのタグが"MainCamera"の場合(つまりはユーザとDestinaitonが衝突した時)に、Routeにおける次のDestinaitonを削除し目標地点を更新する
	 * 
	 */
	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.tag == "MainCamera"){
			if(route != null){
				if(mkRoute.remainRouteOnNavMesh()) route.removeNextDestination ();
			}
		}
	}

}