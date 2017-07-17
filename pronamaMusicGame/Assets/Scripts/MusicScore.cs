using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 譜面１曲分全部を管理するクラス
/// </summary>
public class MusicScore : MonoBehaviour {

	// 楽曲譜面を制御する用メンバ
	List<Note>			m_scoreNoteList;				// 譜面１曲分全部のノート
	int					m_targetScoreNoteNo;			// 今タップしたらこれが対象ってノート番号

	string				m_musicResourceName;			// 楽曲名
	float				m_bgmBPM;						// 楽曲のBPM
	float				m_noteScrollSpeed;				// BPMより計算したノートのスクロールスピード (m_currentTimeに掛け算して流すスピードを決める)

	GameObject			m_scoreManagerObj;				// スコア管理オブジェクト (全てのスコアの親になる)
	MusicManager		m_musicManager;


	float				m_noteStartOffset;				// BGM再生開始から, ノートのスクロールを開始する時間差


	// Use this for initialization
	void Start () {
		m_scoreNoteList = new List<Note>();

		// 外部ファイルから読み込み予定
		m_scoreNoteList.Add(new Note(2.0f, new Vector3(0.0f, 0.0f, 0.0f)));
		m_scoreNoteList.Add(new Note(2.25f, new Vector3(0.0f, 0.0f, 0.0f)));
		m_scoreNoteList.Add(new Note(2.5f, new Vector3(0.0f, 0.0f, 0.0f)));
		m_scoreNoteList.Add(new Note(2.75f, new Vector3(0.0f, 0.0f, 0.0f)));
		m_scoreNoteList.Add(new Note(3.0f, new Vector3(0.0f, 0.0f, 0.0f)));

		m_scoreNoteList.Add(new Note(4.0f, new Vector3(0.0f, 0.0f, 0.0f)));
		m_scoreNoteList.Add(new Note(4.5f, new Vector3(0.0f, 0.0f, 0.0f)));
		m_scoreNoteList.Add(new Note(5.0f, new Vector3(0.0f, 0.0f, 0.0f)));
		m_scoreNoteList.Add(new Note(5.5f, new Vector3(0.0f, 0.0f, 0.0f)));
		m_scoreNoteList.Add(new Note(6.0f, new Vector3(0.0f, 0.0f, 0.0f)));
		m_scoreNoteList.Add(new Note(6.5f, new Vector3(0.0f, 0.0f, 0.0f)));
		m_scoreNoteList.Add(new Note(7.0f, new Vector3(0.0f, 0.0f, 0.0f)));

		m_scoreNoteList.Add(new Note(10.0f, new Vector3(0.0f, 0.0f, 30.0f)));
		m_scoreNoteList.Add(new Note(11.0f, new Vector3(0.0f, 0.0f, 30.0f)));
		m_scoreNoteList.Add(new Note(12.0f, new Vector3(0.0f, 0.0f, -30.0f)));
		m_scoreNoteList.Add(new Note(13.0f, new Vector3(0.0f, 0.0f, -30.0f)));
		m_scoreNoteList.Add(new Note(14.0f, new Vector3(0.0f, 0.0f, 30.0f)));
		m_scoreNoteList.Add(new Note(15.0f, new Vector3(0.0f, 0.0f, 30.0f)));
		m_scoreNoteList.Add(new Note(16.0f, new Vector3(0.0f, 0.0f, -30.0f)));
		m_scoreNoteList.Add(new Note(17.0f, new Vector3(0.0f, 0.0f, -30.0f)));
		m_scoreNoteList.Add(new Note(18.0f, new Vector3(0.0f, 0.0f, 0.0f)));

#if true
		m_musicResourceName = "Music/Prefab/oldest dreamer";
		m_bgmBPM = 170;
		m_noteStartOffset = 0.25f;
#else
		m_musicResourceName = "Music/Prefab/BPM60Checker";
		m_bgmBPM = 120;
		m_noteStartOffset = 0.25f;
#endif
		m_noteScrollSpeed = 1.0f / (60.0f*4) * m_bgmBPM;
		// ここまで

		m_scoreManagerObj = GameObject.Find("MusicScore/ScoreManager");
		GameObject tmp = GameObject.Find("MusicManager");
		if (tmp == null) return;
		m_musicManager = tmp.GetComponent<MusicManager>();
		if (m_musicManager == null) return;

		m_targetScoreNoteNo = 0;
	}

	// Update is called once per frame
	void Update ()
	{
		Debug.DebugText("TimeElapsed2", m_targetScoreNoteNo.ToString("0"), 128, 32);

		float currentTime = m_musicManager.GetCurrentTime();
		float showTime = currentTime + 5.0f;
		for(int i=0; i< m_scoreNoteList.Count; i++)
		{
			if(showTime >= m_scoreNoteList[i].GetTiming())
			{
				if( m_scoreNoteList[i].GetObj() == null )
				{
					// 新規作成
					Vector3 pos = new Vector3(0, 0, -m_scoreNoteList[i].GetTiming());
					pos += m_scoreManagerObj.transform.position;
					m_scoreNoteList[i].SetObj( Instantiate(Resources.Load("Line/Prefab/Note"), pos, Quaternion.identity, m_scoreManagerObj.transform) as GameObject );

					Quaternion target = Quaternion.Euler(m_scoreNoteList[i].GetRotate());
					m_scoreNoteList[i].GetObj().transform.GetChild(0).transform.rotation = target;
				}
			}
		}

		if((currentTime - m_noteStartOffset) >= 0)
		{
			float scrollPos = (currentTime - m_noteStartOffset) * m_noteScrollSpeed;
			Vector3 newPos = m_scoreManagerObj.transform.position;
			newPos.z = scrollPos;
			m_scoreManagerObj.transform.position = newPos;
			m_musicManager.SetGridScrollPos(scrollPos);

			// ターゲット設定
			if( m_targetScoreNoteNo < m_scoreNoteList.Count )
			{
				float timing = m_scoreNoteList[m_targetScoreNoteNo].GetTiming();
				float timing2 = m_scoreNoteList[m_targetScoreNoteNo].GetTiming();
				if( m_targetScoreNoteNo+1 < m_scoreNoteList.Count ){
					timing2 = m_scoreNoteList[m_targetScoreNoteNo+1].GetTiming();
				}

				float dist1 = timing - scrollPos;
				float dist2 = timing2 - scrollPos;
				if( dist1 < 0 ) dist1 *= -1;
				if( dist2 < 0 ) dist2 *= -1;

				// 次のノートの方が近かったら, ターゲットを次のノートに変える
				if( dist1 > dist2 ) m_targetScoreNoteNo++;
			}
		}
	}

	/// <summary>
	/// ターゲットのノートを取得する
	/// </summary>
	/// <returns>ターゲットのノート</returns>
	public Note GetTargetNote()
	{
		if( m_targetScoreNoteNo < m_scoreNoteList.Count )
		{
			return m_scoreNoteList[m_targetScoreNoteNo];
		}
		return null;
	}

	/// <summary>
	/// 楽曲のリソース名を取得る
	/// </summary>
	/// <returns>楽曲のリソース名</returns>
	public string GetMusicResourceName()
	{
	return m_musicResourceName;
	}
}
