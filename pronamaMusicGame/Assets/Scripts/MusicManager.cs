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

	private GameObject	m_gridManager;
	private float		m_gridScrollPos;				// グリッドのスクロール位置
	private GameObject	m_frameObj;						// フレームオブジェクト

	private Vector3		m_beforeAcc;					// 1F前の加速度センサの値

	public MusicScore	m_musicScore;					// スコア管理クラス

	// Use this for initialization
	void Start () {
		float zOffset = 0;
		float zOffsetAdd = -1.0f;
		m_gridManager = GameObject.Find("MusicScore/GridManager");
		if(m_gridManager == null) return;

		GameObject tmp;
		for (int i=0; i<GRID_DRAW_NUM; i++)
		{
			tmp = Instantiate(GRID_OBJECT, GRID_DEFAULT_DRAW_POS + new Vector3(0, 0, zOffset), Quaternion.identity, m_gridManager.transform) as GameObject;
			if( tmp == null ) break;
			zOffset += zOffsetAdd;
		}

		m_frameObj = GameObject.Find("Frame");
		if (m_frameObj == null) return;

		tmp = GameObject.Find("MusicScore");
		if( tmp == null ) return;
		m_musicScore = tmp.GetComponent<MusicScore>();
	}

	// Update is called once per frame
	void Update ()
	{
		// グリッドを流す
		MoveGrid();
		// 加速度センサ
		MoveAcc();

		// タッチでノートを消す
		TapNote();
	}



	/// <summary>
	/// グリッドを流す処理
	/// </summary>
	private void MoveGrid()
	{
		// グリッドを流す処理
		Transform trans = m_gridManager.transform;
		Vector3 newPos = trans.position;
		newPos.z = m_gridScrollPos;
		trans.SetPositionAndRotation(newPos, trans.rotation);
	}

	/// <summary>
	/// グリッドのスクロール位置を設定する
	/// </summary>
	/// <param name="pos">グリッドのスクロール位置</param>
	public void SetGridScrollPos(float pos)
	{
		m_gridScrollPos = pos;

		float depthMax = 1.0f;
		while (m_gridScrollPos >= depthMax)
		{
			m_gridScrollPos -= depthMax;
		}
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

		Quaternion target = Quaternion.Euler(0, 0, -acc.x*90);
		m_frameObj.transform.rotation = target;
		m_gridManager.transform.rotation = target;

		// 1F前の値と比較して, 角度が一定以上, 長さも一定以上差があれば
		float angle = Vector3.Angle(acc, m_beforeAcc);
		float len = acc.sqrMagnitude;
		float befLen = m_beforeAcc.sqrMagnitude;

		debugText += "angle : " + angle.ToString("000.00000" + "\n");
		debugText += "len  : " + len.ToString("000.00000" + "\n");
		debugText += "len2 : " + befLen.ToString("000.00000" + "\n");
		Debug.DebugText("AccDebug", debugText);

/*		if ( angle >= 90 )
		{

			if( len - 10.0f >= befLen )
			{
				Debug.DebugText("Shock", "Shock!!!!!!!!!!!!!!!!!!!!!!!", 0, 0, 1);
			}
		}
*/
		m_beforeAcc = acc;
	}

	/// <summary>
	/// タップでノートを消す
	/// </summary>
	void TapNote()
	{
		if( Input.GetMouseButtonDown(0) )				// タッチ開始
		{
			Note note = m_musicScore.GetTargetNote();
			if( note != null )
			{
				// 距離を測定


				// 
			}
		}
	}
}



