using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeRoute : MonoBehaviour {

	public new LineRenderer renderer;
	public GameObject destination; 
	public LineRenderer path;
	public HumanNavigation huNav;
	public MakeSounds mkSounds;
	public LineManager trajectory;


	private Route route; //経路を表現するクラス

	/// <summary>
	/// setDemoRoute()で障害物回避実験用の経路を設定できます
	/// </summary>
	/// <param name="">
	/// 
	/// </param>
	/// <returns>
	/// 
	/// </returns>
	void Start () {
		setDemoRoute8 ();
	}

    /// <summary>
    /// if内コメントアウトは、Androidから得られたグローバル経路を基に目標地点の設定を行う命令。テストしていないため、見送り。
	/// if外コメントアウトは、Androidから得られた歩行経路をHololens上に表示する命令。正しく座標系のGapが計算できていれば歩行した経路が可視化できるはず。
	/// display
    /// </summary>
    /// <param name="">
    /// 
    /// </param>
    /// <returns>
    /// 
    /// </returns>

    void Update () {
        var r = mkSounds.ab.getRouteDataFromAndroid ();
		if(r != null){
			if(r.changed){
				/*route = new Route ("Global Route",0);
				foreach(JsonLocation l in r.route){
					route.setRouteOriginVec (new TagPlacement((float)mkSounds.cs.transformingCordinatesByGap(l).x,(float)mkSounds.cs.transformingCordinatesByGap(l).y));
				}*/

			}
		}

		/*var t = mkSounds.cs.getTransformedPulldogData (mkSounds.cs.getPulldogData()); 
		if(t.Count != 0){
			trajectory.setVertexesAsJsonTimeAndPositionLog (t);
			if (path != null)
				trajectory.setHeight(path.GetPosition (0).y);
		}*/
		displayRoute ();
		updateRoute ();
	}

	/// <summary>
	/// Initialize Demo Route
	/// </summary>
	/// <param name="">
	/// 
	/// </param>
	/// <returns>
	/// 
	/// </returns>
	void setDemoRoute(){
		route = new Route ("demo",0);
		route.setRouteOriginVec (new TagPlacement(0.0f,0.0f));

		//デモルート：情報科棟5階　研究室から男子トイレまで
		route.setTagPlacement (new TagPlacement(3,0.0f));
		route.setTagPlacement (new TagPlacement(3,0.0f));
		route.setTagPlacement (new TagPlacement(3,0.0f));
		route.setTagPlacement (new TagPlacement(3,0.0f));
		route.setTagPlacement (new TagPlacement(3,0.0f));
		route.setTagPlacement (new TagPlacement(3,0.0f));
		route.setTagPlacement (new TagPlacement(3,0.0f));
		route.setTagPlacement (new TagPlacement(3,0.0f));
		route.setTagPlacement (new TagPlacement(4,0.0f));
		route.setTagPlacement (new TagPlacement(2,0.0f));
		route.setTagPlacement (new TagPlacement(4,0.0f));
		route.setTagPlacement (new TagPlacement(3,0.0f));
		route.setTagPlacement (new TagPlacement(3,0.0f));
		route.setTagPlacement (new TagPlacement(3,0.0f));
		route.setTagPlacement (new TagPlacement(2,0.0f));
		route.setTagPlacement (new TagPlacement(3,90.0f));
		route.setTagPlacement (new TagPlacement(2,90.0f));
		route.setTagPlacement (new TagPlacement(4,90.0f));
	}

	void setDemoRoute2(){
		route = new Route ("demo2",1);
		route.setRouteOriginVec (new TagPlacement(0.0f,0.0f));

		//デモルート：適当
		route.setTagPlacement (new TagPlacement(1.0f,0.0f));
		route.setTagPlacement (new TagPlacement(0.5f,90.0f));
		route.setTagPlacement (new TagPlacement(0.5f,0.0f));
		route.setTagPlacement (new TagPlacement(0.5f,270.0f));
		route.setTagPlacement (new TagPlacement(0.5f,0.0f));
		route.setTagPlacement (new TagPlacement(0.5f,90.0f));

	}

	void setDemoRoute3(){
		route = new Route ("demo3",1);
		route.setRouteOriginVec (new TagPlacement(0.0f,0.0f));

		//デモルート:情報科棟5階　男子トイレから研究室まで
		route.setTagPlacement (new TagPlacement(2,0.0f));
		route.setTagPlacement (new TagPlacement(4,0.0f));
		route.setTagPlacement (new TagPlacement(3,0.0f));
		route.setTagPlacement (new TagPlacement(2,270.0f));
		route.setTagPlacement (new TagPlacement(3,270.0f));
		route.setTagPlacement (new TagPlacement(3,270.0f));
		route.setTagPlacement (new TagPlacement(3,270.0f));
		route.setTagPlacement (new TagPlacement(3,270.0f));
		route.setTagPlacement (new TagPlacement(3,270.0f));
		route.setTagPlacement (new TagPlacement(3,270.0f));
		route.setTagPlacement (new TagPlacement(3,270.0f));
		route.setTagPlacement (new TagPlacement(3,270.0f));
		route.setTagPlacement (new TagPlacement(3,270.0f));
		route.setTagPlacement (new TagPlacement(4,270.0f));
		route.setTagPlacement (new TagPlacement(2,270.0f));
		route.setTagPlacement (new TagPlacement(4,270.0f));
		route.setTagPlacement (new TagPlacement(3,270.0f));
		route.setTagPlacement (new TagPlacement(3,270.0f));
		route.setTagPlacement (new TagPlacement(3,270.0f));
		route.setTagPlacement (new TagPlacement(2,270.0f));


	}

    void setDemoRoute4()
    {
        route = new Route("demo4", 1);
        route.setRouteOriginVec(new TagPlacement(0.0f, 0.0f));

        //デモルート:情報科棟5階　階段から研究室まで
        route.setTagPlacement(new TagPlacement(0.9f, 0.0f));
        route.setTagPlacement(new TagPlacement(0.9f, 0.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
        route.setTagPlacement(new TagPlacement(0.3f, 270.0f));
    }

	void setDemoRoute5()
	{
		route = new Route("demo5", 1);
		route.setRouteOriginVec(new TagPlacement(0.0f, 0.0f));

		//デモルート:情報科棟5階　階段から研究室まで
		route.setTagPlacement(new TagPlacement(1.5f, 0.0f));
		route.setTagPlacement(new TagPlacement(1.0f, 0.0f));
		for(int i =0;i<10;i++)
			route.setTagPlacement(new TagPlacement(0.6f, 270.0f));
		
	}

    void setDemoRoute6()
    {
        route = new Route("demo6", 1);
        route.setRouteOriginVec(new TagPlacement(0.0f, 0.0f));

        //デモルート:情報科棟5階　階段から研究室まで
        for (int i = 0; i < 10; i++)
            route.setTagPlacement(new TagPlacement(0.6f, 0.0f));

    }

	void setDemoRoute7()
	{
		route = new Route("demo7", 1);
		route.setRouteOriginVec(new TagPlacement(0.0f, 0.0f));
    }

	void setDemoRoute8(){
        route = new Route("demo8", 1);
        route.setRouteOriginVec(new TagPlacement(0.0f, 0.0f));

        //デモルート:情報科棟5階　階段から研究室まで
        route.setTagPlacement(new TagPlacement(2.1f, 0.0f));
        for (int i = 0; i < 15; i++)
            route.setTagPlacement(new TagPlacement(1.0f, 270.0f));
    }

    /// <summary>
    /// Display Route (A Path from Now Position to first distination is not displayed)
    /// </summary>
    /// <param name="">
    /// 
    /// </param>
    /// <returns>
    /// 
    /// </returns>
    void displayRoute(){
		if (route != null) {
			route.setHeight (path.GetPosition (0).y);
			var v = route.getArrayRoute ();
			renderer.positionCount = v.Length;
			if (v.Length != 0) {
				for (int i = 0; i < v.Length ; i++) {
					//Debug.Log ("Route point " + i + " : " + v [i]);
					renderer.SetPosition (i, v [i]);
				}
			}
		}
	}

	/*
	 * 次の目標地点が歩行可能エリアに置けない場合、remainRouteOnNavMeshを呼び、その次の目標地点が置けるか確かめるため、次の目標地点をとばす(remove)
	 */
	public void updateRoute(){
		if(!route.getNextDestination().isOnNavMesh && route != null){
			if(remainRouteOnNavMesh ()) route.removeNextDestination ();
		}
	}

	/*
	 * 経路を渡すメソッド
	 */
	public Route getRoute(){
		return route;
	}

	/*
	 * 未だ到達していない目標地点の中で、どの目標地点も歩行可能エリア上に設置できない場合、スキャンを促すアナウンスを出し、falseを返す。
	 * 歩行可能エリアに、一つでも目標地点が置ける場合はtrueを返し、目標地点を順々に先に進めるように促す。この時、方向指示を発声するように命令するが、経路ができていない場合にはアナウンスはされない。
	 */
	public bool remainRouteOnNavMesh(){
		Vector3[] tmp = route.getArrayRoute ();
		for (int i = 0; i < tmp.Length; i++)
			if (huNav.isOnNavMesh (tmp [i])) {
				mkSounds.guidanceNum = 1;
				return true;
			}
		mkSounds.guidanceNum = 4;
		return false;
	}
}
