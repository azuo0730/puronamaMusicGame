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

	float				m_currentTime;					// 経過時間
	AudioSource			m_bgmAudioSource;				// 楽曲BGM再生用オーディオ

	private GameObject	m_gridManager;
	private float		m_gridScrollPos;				// グリッドのスクロール位置
	private GameObject	m_frameObj;						// フレームオブジェクト

	private Vector3		m_beforeAcc;					// 1F前の加速度センサの値

	public MusicScore	m_musicScore;					// スコア管理クラス

	// Use this for initialization
	void Start () {
		float zOffset = 0;
		float zOffsetAdd = -1.0f;
		// オブジェクトの生成, 初期化
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

		// その他変数初期化
		m_currentTime = 0;
	}

	// Update is called once per frame
	void Update ()
	{
		bool ret = InitMusic();
		if( ret == false ) return;				// 楽曲読み込みに失敗

		// グリッドを流す
		MoveGrid();
		// 加速度センサ
		MoveAcc();

		// タッチでノートを消す
		TapNote();
	}

	/// <summary>
	/// 楽曲を初期化する
	/// </summary>
	/// <returns>true = 成功, false = 失敗</returns>
	private bool InitMusic()
	{
		GameObject tmp;
		if( m_bgmAudioSource == null )
		{
			tmp = GameObject.Find("BGMAudioSource") as GameObject;
			if( tmp == null ) return false;
			m_bgmAudioSource = tmp.GetComponent<AudioSource>();
			if(m_bgmAudioSource == null) return false;

			if( m_musicScore == null ) return false;
			tmp = Instantiate(Resources.Load(m_musicScore.GetMusicResourceName())) as GameObject;
			if (tmp == null) return false;
			AudioSource audioSource = tmp.GetComponent<AudioSource>();
			if(audioSource == null) return false;
			m_bgmAudioSource.clip = audioSource.clip;

			if (m_bgmAudioSource.isPlaying == false)
			{
				m_bgmAudioSource.Play();
			}
		}

		return true;
	}

	/// <summary>
	/// グリッドを流す処理
	/// </summary>
	private void MoveGrid()
	{
		m_currentTime = m_bgmAudioSource.time;					// 楽曲の経過時間から, スクロール位置を決める

		// グリッドを流す処理
		Transform trans = m_gridManager.transform;
		Vector3 newPos = trans.position;
		newPos.z = m_gridScrollPos;
		trans.SetPositionAndRotation(newPos, trans.rotation);

		Debug.DebugText("TimeElapsed", m_currentTime.ToString("00000.00"), 0, 32);
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
	private void TapNote()
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

	/// <summary>
	/// 現在の楽曲経過時間を取得する
	/// </summary>
	/// <returns>現在の楽曲経過時間</returns>
	public float GetCurrentTime()
	{
		return m_currentTime;
	}
}



