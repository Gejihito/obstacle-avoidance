using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction : MonoBehaviour {

	public LineRenderer path;
	public Camera me;

	public float degreeOfDirection = -1.0f; //指示に使う角度。通常0～360°で返るが角度がでない場合は―1。

	// Use this for initialization
	void Start () {
		
	}

	/*
	 * ローカル経路と自分(HoloLens)が向いている方向の差分を逐次計算する
	 */
	// Update is called once per frame
	void Update () {
		calculateDirectionBWPathAndMe ();
	}

	/// <summary>
	/// Calculate degree between Path foward to first destination and gaze.
	/// Set degree (0 to 360) to degreeOfDirection.
	/// </summary>
	/// <param name="">
	/// 
	/// </param>
	/// <returns>
	/// 
	/// </returns>
	private void calculateDirectionBWPathAndMe(){
		if(me != null && path != null){
			if(path.positionCount >= 1){
				Vector2 cameraForward = new Vector2 (me.transform.forward.x, me.transform.forward.z);
				var tmp = (path.GetPosition (1) - me.transform.position).normalized;
				Vector2 pathForward = new Vector2(tmp.x,tmp.z);
				degreeOfDirection = Vector2.Angle (cameraForward,pathForward);
				if (Vector2.Dot(new Vector2(-cameraForward.y, cameraForward.x), pathForward) > 0)
					degreeOfDirection = 360.0f - degreeOfDirection;
			}
		}else degreeOfDirection = -1.0f;
	}

	/// <summary>
	/// Get Degree Between Path and My gaze.
	/// </summary>
	/// <param name="">
	/// 
	/// </param>
	/// <returns>
	/// degree of direction (0 to 360) except when first destination doesn't exit (return -1).
	/// </returns>
	public float getDirectionBWPathAndMe(){
		return degreeOfDirection;
	}

	/// <summary>
	/// Translate Degree of direcion to String (Clock Position)
	/// </summary>
	/// <param name="deg">
	/// degree of direction
	/// </param>
	/// <returns>
	/// string of Clock Postion.
	/// if deg is under 0, this function return "".
	/// </returns>
	public string translateDirectionToClockPosition(float deg){
		deg = deg + 15.0f;
		if (deg > 360.0f)
			deg -= 360.0f;
		string str = "";
		if (deg < 0.0f)
			return str;
		str = ((int)(deg / 30.0f)).ToString();
		if((int)(deg / 30.0f) == 0)
			str = "12";
		return str;
	}

	/*
	 * "Left"または"Ringt"のみの指示を発生させる
	 * 360度のうち0～180°をRight、それ以外をLeftとする
	 */
	public string translateDirectionToRightOrLeft(float deg){
		if(0 < deg && deg <180) return "Right";
		return "Left";
	}
}
