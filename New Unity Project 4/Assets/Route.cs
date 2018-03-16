using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft;

public class Route {

	private string name = ""; //経路の名前
	private int ID = -1; //経路のID
	private List<TagPlacement> tagList; //経路情報(List)
	private bool isExisting; //経路が存在するか否か
	private float height = 0.0f; //経路の高さ

	public bool isFinished = false; //最後の目標地点に到達したか否か



	/*
	 * コンストラクタ
	 * Routeクラスは経路を表現するクラスである。経路はTagPlacementのListで表現される。TagPlacementの位置情報は原点(別のTagPlacement)とベクトル(座標系上の回転角度と長さ)で表現される。原点であるTagPlacementからの向きと距離をベクトルが表現し、それを位置情報としている。
	 * これは、PullDogの地図において、経路の要素となるのがVertexとEdgeという概念を用いた極座標系から成り立っていることに起因する。つまり、本経路は必ず始点が一つであり、途中で途切れてしまうような経路は表現できない。必ずTagPlacementは原点とのつながりを持たなくてはならない。
	 * TagPlacementクラスは、原点となる別のTagPlacementのプロパティを持っているが、初期化時にその情報は必要ない。Routeクラスで経路として利用されるときにはじめてsetOriginVec()から原点が指定される。つまり、この「別のTagPlacementとのつながりを作る」部分はこのRouteクラス上で行う。
	*/
	public Route(string n, int i){
		name = n;
		ID = i;
		tagList = new List<TagPlacement>();
	}

	/*
	 * 経路にTagPlacement tを追加する。
	 */
	public void setTagPlacement(TagPlacement t){
		isExisting = true;
		tagList.Add (t);
		setOriginVec (tagList.LastIndexOf(t));
	}


	/*
	 * 経路を渡すメソッド
	 */
	public List<TagPlacement> getRoute(){
		return tagList;
	}

	/*
	 * 引数のindexを持つ経路上のTagPlacementを、最後に追加されているTagPlacementの原点として設定する
	 */
	private void setOriginVec(int index){
		tagList [index].setOriginVec(tagList [index - 1].getOriginToDestination());
	}

	/*
	 * nowDestinaitonの属性が付けられ最初の目標地点となる。このメソッドは、経路の先頭のTagPlacementに対してのみ使う。
	 */
	public void setRouteOriginVec(TagPlacement p){
		p.nowDestination = true;
		tagList.Insert (0, p);
	}

	/*
	 * TagPlacementのListをArray形式に変換するメソッド
	 */
	public Vector3[] getArrayRoute(){
		List<Vector3> pointsList = new List<Vector3>{};
		for(int i = tagList.FindIndex(x=> x.nowDestination)+1;i<tagList.Count;i++){
            if (i < 1) break;
			var item = tagList [i];
			var tmp = item.getOriginToDestination ();
			pointsList.Add (new Vector3 (tmp.x, getHeight (), tmp.y));
		}
		Vector3[] pointArray = new Vector3[pointsList.Count];
		for(int i = 0;i<pointsList.Count;i++){
			pointArray.SetValue (pointsList [i], i);
		}
		return pointArray;
	}

	/*
	 * TagPlacementは2次元座標系上で扱われることを想定しているが、Unityでは3次元データで扱う方が便利なので便宜的に高さ情報を付加
	 */
	public void setHeight(float h){
		height = h;
	}

	/*
	 * 経路の高さの情報を渡すメソッド
	 */
	public float getHeight(){
		return height;
	}

	/*
	 * 次の目標地点を返す
	 */
	public TagPlacement getNextDestination (){
		if(tagList.FindIndex (x => x.nowDestination)+1 < tagList.Count){
			var i = tagList [tagList.FindIndex (x => x.nowDestination) + 1];
			return i;
		}
		isFinished = true;
		var tmp = new TagPlacement(0.0f,0.0f);
		tmp.isOnNavMesh = true;
		return tmp;
	}

	/*
	 * nowDestinationパラメータを変化させ、目標地点の変更を行う。removeとあるが実際にはListからは削除せず対象となる要素を変えるだけである。
	 */
	public void removeNextDestination(){
		//tagList.RemoveAt (0);
		int index = tagList.FindIndex (x => x.nowDestination)+1;
        TagPlacement pastDistination = tagList.Find(x => x.nowDestination);
        if (pastDistination != null) {
            pastDistination.isOnNavMesh = false;
            pastDistination.nowDestination = false;
        }
        if(index < tagList.Count){
			tagList [index].nowDestination = true;
		} else {
			/*foreach(TagPlacement item in tagList){
				item.isOnNavMesh = true;
			}*/
		}
		if (tagList.Count == 0)
			isExisting = false;
	}


}
