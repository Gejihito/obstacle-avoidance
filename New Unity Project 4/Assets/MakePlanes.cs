using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity.InputModule;
using UnityEngine.AI;

public class MakePlanes : MonoBehaviour, IInputClickHandler {

	private float timeOut = 10.0f; //床面推定実行周期10[s]
	private float time = 0.0f; //床面推定実行周期を管理する変数
    private int areaNum = 0;

	public bool makedPlane = false; //床面が推定されたらTrueになる
	public MakeRoute mkRoute;
	public GameObject s;

	/*
	 * 平面推定は床面のみを検出するように別クラスに設定済みであるため、床面のみを平面として検出する
	 */
	// Use this for initialization
	void Start () {
		//	エアタップ(指によるジェスチャー)による入力に関する命令なので、現状として関係はない
		InputManager.Instance.PushFallbackInputHandler(gameObject);

		//	床推定が完了したら呼ぶメソッドをセットする(ここではSurfaceMeshesToPlanes_MakePlanesComplete()メソッドが呼ばれる)
		SurfaceMeshesToPlanes.Instance.MakePlanesComplete += SurfaceMeshesToPlanes_MakePlanesComplete;
	}

	/*
	 * 床面推定は10秒後に行われる。頻繁過ぎると処理高の可能性あり 
	 */
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time >= timeOut) {
			makePlanes();
			time = 0.0f;
		}
	}

	/*
	 * 床面推定をするメソッド
	 */
	public void makePlanes()
	{
		SpatialMappingManager.Instance.StopObserver();
		SurfaceMeshesToPlanes.Instance.MakePlanes();
	}

	/*
	 * 関係なし
	 */
	public void OnInputClicked(InputClickedEventData eventData)
	{
		//	エアタップされたらメッシュ作るの止めて、壁とか作る
		//SpatialMappingManager.Instance.StopObserver();
		//SurfaceMeshesToPlanes.Instance.MakePlanes();
	}

	/*
	 * 床面推定が完了した時に呼ばれるメソッド。推定された床面は3Dオブジェクト(SurfacePlane)として取得できる(ここでは、SurfacePlaneとして取得しなくても良いため、GameObjectとして検索している)。
	 * タグ"Plane"がついたSurfacePlaneが推定された床面であるが、それに対しNavMeshSourceTagという要素を付加することで後にNavMeshによる歩行可能エリアの計算がその面に対して可能になる。
	 */	
	private void SurfaceMeshesToPlanes_MakePlanesComplete(object source, System.EventArgs args)
	{
		RemoveSurfaceVertices.Instance.RemoveSurfaceVerticesWithinBounds (SurfaceMeshesToPlanes.Instance.ActivePlanes);
		//SpatialMappingManager.Instance.DrawVisualMeshes = false;
		SpatialMappingManager.Instance.StartObserver();
		var planes = GameObject.FindGameObjectsWithTag("Plane");
		if (planes.Length != 0) {
			foreach (GameObject item in planes) {
				item.gameObject.AddComponent<NavMeshSourceTag> ();
			}
			makedPlane = true;
			//NavMeshの三角形集合取得
			NavMeshTriangulation triangles = NavMesh.CalculateTriangulation();
			Vector3[] v = triangles.vertices;
			for (int i = 0; i < v.Length; i++) {
                mkRoute.huNav.log.writeMessage_area("area"+ areaNum +"," + v[i].x + "," + v[i].z);
			}
            areaNum++;
        } else
			makedPlane = false;
	}
}
