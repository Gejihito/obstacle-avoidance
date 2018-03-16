using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpatialSound : MonoBehaviour
{
	/*
	 * このクラスは使っていません。
	 */
	public void Awake()
	{
		var audio = GetComponent<AudioSource>();
		audio.spatialize = true;
		audio.spatialBlend = 1.0f;
	}
}