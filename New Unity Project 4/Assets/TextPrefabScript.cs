using UnityEngine;

public class TextPrefabScript : MonoBehaviour
{
	/*
	 * 使っていない
	 */
	public static GameObject Instantiate (Vector3 position, Quaternion rotation, string text){
		GameObject obj = Instantiate (Resources.Load("Resources/TextPrefab"), position, rotation) as GameObject;
		obj.GetComponent<TextMesh> ().text = text;

		return obj;
	}	
}
