using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour {

	private LineRenderer line;
	private List<Vector2> vertexes; //Lineを引きたい座標点のリスト
	private double height = 0.0; //Lineを引きたい高さ
	public bool isLineOn = false; //Lineが引かれているか否か

	/*
 	 * 変数の初期化・紐づけ
 	 */
	// Use this for initialization
	void Start () {
		vertexes = new List<Vector2> ();
		line = this.transform.GetComponent<LineRenderer> ();
		line.positionCount = 2;
		List<Vector3> v = new List<Vector3> ();
		v.Add (new Vector3(0.0f,0.0f,0.0f));
		v.Add (new Vector3(0.0f,0.0f,1.0f));
		line.SetPositions (v.ToArray());
	}

	/*
 	 * vertexesに座標点が入っていればその座標点の通り線を引く
 	 * またisLineOnをtrueにする
 	 */
	// Update is called once per frame
	void Update () {
		if (line != null && vertexes.Count != 0) {
			line.positionCount = vertexes.Count;
			line.SetPositions (translateVec2ToVec3 (vertexes).ToArray());
		}
		if (line.positionCount != 0)
			isLineOn = true;
	}

	/*
 	 * 外部から座標点をvertexesにセットするメソッド
 	 */
	public void setVertexes(List<Vector2> v){
		vertexes = v;
	}

	/*
 	 * JsonTimeAndPositionLogのリストの形式でvertexesに座標点を渡すメソッド
 	 */
	public void setVertexesAsJsonTimeAndPositionLog(List<JsonTimeAndPositionLog> mVertexes){
		vertexes = new List<Vector2> ();
		foreach(JsonTimeAndPositionLog v in mVertexes){
			vertexes.Add (new Vector2 ((float)v.location.x, (float)v.location.y));
		}
	}

	/*
 	 * Lineを引く高さの設定
 	 */
	public void setHeight(double h){
		height = h;
	}

	/*
 	 * 2次元ベクトルを3次元ベクトルに挙げるメソッド。単純に高さを付加しているだけ。
 	 */
	private List<Vector3> translateVec2ToVec3(List<Vector2> v2){
		List<Vector3> lineV3 = new List<Vector3> ();
		foreach(Vector2 v in v2){
			lineV3.Add (new Vector3(v.x,(float)height,v.y));
		}
		return lineV3;
	}
}
