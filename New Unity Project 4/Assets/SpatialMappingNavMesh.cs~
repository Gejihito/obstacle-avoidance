using HoloToolkit.Unity.SpatialMapping;
using UnityEngine;
using HoloToolkit.Unity;

public class SpatialMappingNavMesh : MonoBehaviour {
	public GameObject SpatialMapping;

	/*
	 * SpatialMappingの結果が得られたらSpatialMappingSource_SurfaceAdded();を、更新されたらSpatialMappingSource_SurfaceUpdated();を呼び出す
	 */
	private void Awake () {
		var spatialMappingSources = SpatialMapping.GetComponents<SpatialMappingSource>();
		foreach (var source in spatialMappingSources)
		{
			source.SurfaceAdded += SpatialMappingSource_SurfaceAdded;
			source.SurfaceUpdated += SpatialMappingSource_SurfaceUpdated;
		}
	}

	void Start()
	{
	}

	/*
	 * 得られたSurfaceデータに対して、NavMeshSourceTagという要素(Component)をつけていく。これによって、NavMeshがその3Dオブジェクトに対して歩行可能エリアを設定できる
	 */
	private void SpatialMappingSource_SurfaceAdded(object sender, DataEventArgs<SpatialMappingSource.SurfaceObject> e)
	{
		e.Data.Object.AddComponent<NavMeshSourceTag>();
	}

	/*
	 * 更新されたSurfaceデータに対して、NavMeshSourceTagという要素(Component)をつけていく。これによって、NavMeshがその3Dオブジェクトに対して歩行可能エリアを設定できる
	 */
	private void SpatialMappingSource_SurfaceUpdated(object sender, DataEventArgs<SpatialMappingSource.SurfaceUpdate> e)
	{
		var navMeshSourceTag = e.Data.New.Object.GetComponent<NavMeshSourceTag>();
		if(navMeshSourceTag == null)
		{
			e.Data.New.Object.AddComponent<NavMeshSourceTag>();
		}
	}
}