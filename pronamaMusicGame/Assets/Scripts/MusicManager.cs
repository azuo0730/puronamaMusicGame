using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	/// グリッド表示用オブジェクト
	public Object GRID_OBJECT;
	/// グリッドオブジェクトの表示数
	public readonly int GRID_DRAW_NUM = 10;
	/// グリッドのデフォルト表示座標
	private readonly Vector3 GRID_DEFAULT_DRAW_POS = new Vector3(0.0f, 0.72f, 1.0f);

	private List<GameObject> m_gridObject;

	private float m_gridMoveTmp;

	// Use this for initialization
	void Start () {

		m_gridMoveTmp = 0.0f;

		float zOffset = 0;
		float zOffsetAdd = -1.0f;
		m_gridObject = new List<GameObject>();
		m_gridObject.Clear();
		for (int i=0; i<GRID_DRAW_NUM; i++)
		{
			GameObject tmp = Instantiate(GRID_OBJECT, GRID_DEFAULT_DRAW_POS + new Vector3(0, 0, zOffset), Quaternion.identity) as GameObject;
			m_gridObject.Add( tmp );
			zOffset += zOffsetAdd;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		float moveAdd = 0.01f;
		float depthMax = 1.0f;
		m_gridMoveTmp += moveAdd;

		for (int i=0; i<m_gridObject.Count; i++)
		{
			Transform trans = m_gridObject[i].transform;
			Vector3 newPos = trans.position;
			newPos.z += moveAdd;
			if (m_gridMoveTmp >= depthMax)
			{
				newPos.z -= depthMax;
			}
			trans.SetPositionAndRotation(newPos, trans.rotation);
		}
		if (m_gridMoveTmp >= depthMax)
		{
			m_gridMoveTmp -= depthMax;
		}
	}
}


