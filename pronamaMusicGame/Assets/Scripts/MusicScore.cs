using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 譜面１曲分全部を管理するクラス
/// </summary>
public class MusicScore : MonoBehaviour {

	List<Note>			m_scoreNoteList;			// 譜面１曲分全部のノート
	float				m_timeElapsed;				// 経過時間

	GameObject			m_scoreManager;				// スコア管理オブジェクト (全てのスコアの親になる)

	/// <summary>
	/// 譜面１つを管理するクラス
	/// </summary>
	private class Note
	{
		private float			m_timing;
		private Vector3			m_rotate;
		private GameObject		m_obj;

		public Note(float timing, Vector3 rotate)
		{
			m_timing = timing;
			m_rotate = rotate;
			m_obj = null;
		}
		~Note(){}

		public float GetTiming(){ return m_timing; }
		public GameObject GetObj(){ return m_obj; }
		public void SetObj(GameObject obj){ m_obj = obj; }

		public void SetObjPos(Vector3 pos)
		{
			if( m_obj == null ) return;
			m_obj.transform.position = pos;
		}
	}


	// Use this for initialization
	void Start () {
		m_scoreNoteList = new List<Note>();
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
		m_scoreNoteList.Add(new Note(7.25f, new Vector3(0.0f, 0.0f, 0.0f)));
		m_scoreNoteList.Add(new Note(7.5f, new Vector3(0.0f, 0.0f, 0.0f)));
		m_timeElapsed = 0;

		m_scoreManager = GameObject.Find("MusicScore/ScoreManager");
	}

	// Update is called once per frame
	void Update () {
		m_timeElapsed += Time.deltaTime;

		Debug.DebugText("TimeElapsed", m_timeElapsed.ToString("00000.00"), 0, 32);

		float showTime = m_timeElapsed + 5.0f;
		for(int i=0; i< m_scoreNoteList.Count; i++)
		{
			if(showTime >= m_scoreNoteList[i].GetTiming() )
			{
				if( m_scoreNoteList[i].GetObj() == null )
				{
					// 新規作成
					Vector3 pos = new Vector3(0, 0, -m_scoreNoteList[i].GetTiming()) + m_scoreManager.transform.position;
					m_scoreNoteList[i].SetObj( Instantiate(Resources.Load("Line/Prefab/Note"), pos, Quaternion.identity, m_scoreManager.transform) as GameObject );
				}
			}
		}

		float moveAdd = 0.01f;
		m_scoreManager.transform.position += new Vector3(0, 0, moveAdd);
	}
}
