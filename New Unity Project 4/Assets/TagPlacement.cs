using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TagPlacement  {

	private float BrailleBlockSize = 0.3f; //点字ブロックのサイズ(過去の遺産)

	private float radius; //回転角度
	private float rotation; //長さ
	private Vector2 originVec; //原点
	private Vector2 tagVec; //回転角度と長さを2次元ベクトルに変換したもの。極座標系からXY座標系へ

	public bool isOnNavMesh = true; //このTagPlacementオブジェクトが歩行可能エリアにあるかどうか
	public bool nowDestination = false; //このTagPlacementオブジェクトが現在の目標地点かどうか

	/*
	 * コンストラクタ
	 * TagPlacementは経路(Route)の要素となるクラスである。RouteクラスはTagPlacementクラスのListを経路情報として保持している。TagPlacementクラスは原点とベクトルを持ち、自身の位置情報は原点とベクトルを足し合わせた結果である。
	 * このベクトルは、HoloLensの座標系上での回転角度(rotation)とベクトル(radius)の長さ情報で構成されている。
	 */

	/*
	 * 回転角度・半径を用いたコンストラクタ。
	 */
	public TagPlacement(float rad, float rot){
		this.radius = rad;
		this.rotation = rot;
		translatePolarToXZ (rad, rot);
	}

	/*
	 * 回転角度・半径を用いたコンストラクタ。
	 */
	public TagPlacement(int rad, float rot){
		this.radius = (float) rad * BrailleBlockSize;
		this.rotation = rot;
		translatePolarToXZ (rad, rot);
	}

	/*
	 * 回転角度と長さの情報を2次元ベクトルに変換
	 */
	private void translatePolarToXZ(float rad, float rot){
		float tmp = Mathf.Deg2Rad * rot;
		var sin = (float)Math.Sin (rad * tmp);
		var cos = (float)Math.Cos (rad * tmp);
		tagVec = new Vector2 (rad * (float)Math.Sin (tmp), rad * (float)Math.Cos (tmp));
	}

	/*
	 * 原点と自己のベクトルを足し合わせた自己の「座標」を返すメソッド
	 */
	public Vector2 getOriginToDestination(){
		return originVec + tagVec;
	}

	/*
	 * 原点をセットするメソッド
	 */
	public void setOriginVec(Vector2 v){
		originVec = v;
	}

}
