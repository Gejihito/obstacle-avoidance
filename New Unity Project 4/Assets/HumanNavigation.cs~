using UnityEngine;
using UnityEngine.AI;

public class HumanNavigation : MonoBehaviour {
	public NavMeshAgent destinationAgent;
	public GameObject destination;
	public NavMeshAgent agent;
	public LineRenderer lineRenderer;
	public OutputLog log;

	private int pathNum = 0; //pathの屈折点の数(Listの要素数)
	public bool isAvoiding = false; //障害物を回避する経路かどうか


	/*
	 * それぞれのAgentがアタッチされた3Dオブジェクトはpathが計算されると勝手に移動(updatePosition)をする設定になっているため、自動更新にならないようにfalseを入れる
	*/
	void Start () {
		//lineRenderer = GetComponent<LineRenderer>();
		agent.updatePosition = false;
		destinationAgent.updatePosition = false;
	}

	/*
	 * Agent(NavMeshAgent:歩行可能エリア上に最短経路を引くためのクラス。Unityのナビゲーションを参照)に目標地点を設定する
	 * Agentが遷移したPositionとしてnextPositionにCamera(ユーザ)の位置を設定
	 * 自己位置と目的地を入れると自動でpathが計算される
	 * pathは3次元座標点のリストとして渡されるため、目標地点までが直線(listの要素数が始点と終点の2つ)だった場合障害物がないことになるので、Listの個数で障害物があるかないかを判定している(うまくいっていない？)
	 * LineRendererでpathを表示
	*/
	void Update () {
		agent.SetDestination (destination.transform.position); //目的地を設定
		agent.nextPosition = Camera.main.transform.position;  //Agentの位置をHoloLensの位置に設定

		var positions = agent.path.corners;
		if (positions.Length <= 2)
			isAvoiding = true;
		else
			isAvoiding = false;
		lineRenderer.positionCount = positions.Length;
		//string pathForLog = "";
        for (int i = 0; i < positions.Length; i++)
        {
            //Debug.Log ("Path point " + i + " : " + positions [i]);
            lineRenderer.SetPosition(i, positions[i]);
            log.writeMessage_path("path"+pathNum+","  + positions[i].x + "," + positions[i].z );
        }
        if(positions.Length !=0 ) pathNum++;
	}

	/*
	 * Destinaitonオブジェクト(ここでは便宜上Destinaitonを表す3Dポリゴンデータ)に対して、ユーザ(ここでは便宜上ユーザを表す3Dの範囲を持ったパラータ)がぶつかった時に呼ばれ、次の目標地点を歩行可能エリアにおけるようであればtrueを返す
	*/
	public bool reachedDestination(){
		NavMeshHit point;
		if(NavMesh.SamplePosition(destination.transform.position,out point,3.0f,NavMesh.AllAreas)){
			if (destination.transform.position.Equals (Vector3.Scale (point.position, new Vector3 (1.0f, 0.0f, 1.0f))))
				return true;
		}
		return false;
	}

	/*
	 * 引数positionが歩行可能エリア上にあるかどうかを確認するメソッド
	*/
	public bool isOnNavMesh(Vector3 position){
		NavMeshHit point;
		if(NavMesh.SamplePosition(position,out point,3.0f,NavMesh.AllAreas)){
			if (Vector3.Scale (position, new Vector3 (1.0f, 0.0f, 1.0f)).Equals (Vector3.Scale (point.position, new Vector3 (1.0f, 0.0f, 1.0f))))
				return true;
		}
		return false;
	}
}