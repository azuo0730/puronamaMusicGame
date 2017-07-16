using UnityEngine;
using UnityEditor;



/// <summary>
/// 譜面１つを管理するクラス
/// </summary>
public class Note
{
	private float m_timing;
	private Vector3 m_rotate;
	private GameObject m_obj;

	public Note(float timing, Vector3 rotate)
	{
		m_timing = timing;
		m_rotate = rotate;
		m_obj = null;
	}
	~Note() { }

	public float GetTiming() { return m_timing; }
	public Vector3 GetRotate() { return m_rotate; }
	public GameObject GetObj() { return m_obj; }
	public void SetObj(GameObject obj) { m_obj = obj; }

	public void SetObjPos(Vector3 pos)
	{
		if (m_obj == null) return;
		m_obj.transform.position = pos;
	}
}




