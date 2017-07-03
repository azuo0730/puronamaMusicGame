using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	/// グリッド表示用オブジェクト
	public Object GRID_OBJECT;
	/// グリッドオブジェクトの表示数
	public readonly int GRID_DRAW_NUM = 10;
	/// グリッドのデフォルト表示座標
	private readonly Vector3 GRID_DEFAULT_DRAW_POS = new Vector3(0.0f, 0.72f, 0.5f);

	private GameObject m_gridManager;

	// Use this for initialization
	void Start () {
		float zOffset = 0;
		float zOffsetAdd = -1.0f;
		m_gridManager = GameObject.Find("MusicScore/GridManager");
		if(m_gridManager == null) return;

		for (int i=0; i<GRID_DRAW_NUM; i++)
		{
			GameObject tmp = Instantiate(GRID_OBJECT, GRID_DEFAULT_DRAW_POS + new Vector3(0, 0, zOffset), Quaternion.identity, m_gridManager.transform) as GameObject;
			if( tmp == null ) break;
			zOffset += zOffsetAdd;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		// グリッドを流す
		MoveGrid();
		// 加速度センサ
		MoveAcc();
	}



	/// <summary>
	/// グリッドを流す処理
	/// </summary>
	private void MoveGrid()
	{
		float moveAdd = 0.01f;
		float depthMax = 1.0f;

		// グリッドを流す処理
		Transform trans = m_gridManager.transform;
		Vector3 newPos = trans.position;
		newPos.z += moveAdd;
		if (newPos.z >= depthMax)
		{
			newPos.z -= depthMax;
		}
		trans.SetPositionAndRotation(newPos, trans.rotation);
	}

	/// <summary>
	/// 加速度センサの値チェック処理
	/// </summary>
	private void MoveAcc()
	{
		Vector3 acc = Input.acceleration;
		string debugText = "Acc X : " + acc.x.ToString("000.00000" + "\n");
		debugText += "Acc Y : " + acc.y.ToString("000.00000" + "\n");
		debugText += "Acc Z : " + acc.z.ToString("000.00000" + "\n");

		Debug.DebugText("AccDebug", debugText);
	}
}



