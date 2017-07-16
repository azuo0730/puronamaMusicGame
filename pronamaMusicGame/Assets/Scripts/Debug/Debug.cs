using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Debug : MonoBehaviour {

	private static Dictionary<string, DebugTextObj> m_debugTextObjMap = new Dictionary<string, DebugTextObj>();



	private class DebugTextObj
	{
		public string Name { set; get; }
		public GameObject Obj { set; get; }
		public int DeleteCounter { set; get; }

		public DebugTextObj()
		{
			Name = "";
			Obj = null;
			DeleteCounter = 0;
		}

		/// <summary>
		/// デバッグ文字Objを作成する
		/// </summary>
		/// <param name="name">名前</param>
		/// <param name="text">表示するデバッグ文字列</param>
		/// <param name="pos">表示座標 (x, yのみ使用)</param>
		/// <param name="deleteCounter">表示するカウント数</param>
		/// <returns>作成したデバッグ文字Obj</returns>
		public static DebugTextObj Create(string name, string text, Vector3 pos, int deleteCounter)
		{
			DebugTextObj retVal = new DebugTextObj();
			GameObject debutTextObj = Resources.Load("Debug/DebugText") as GameObject;

			GameObject canvasObj = GameObject.Find("Canvas") as GameObject;
			if( canvasObj == null ) return null;

			GameObject obj = Instantiate(debutTextObj, Vector3.zero, Quaternion.identity, canvasObj.transform) as GameObject;
			if( obj == null ) return null;

			Text textObj = obj.GetComponent("Text") as Text;
			if( textObj == null ) return null;
			textObj.text = text;

			RectTransform rect = obj.GetComponent("RectTransform") as RectTransform;
			if( rect == null ) return null;
			rect.anchoredPosition = pos;

			retVal.Name = name;
			retVal.Obj = obj;
			retVal.DeleteCounter = deleteCounter;

			return retVal;
		}

		/// <summary>
		/// デバッグ文字情報を修正する
		/// </summary>
		/// <param name="name">名前</param>
		/// <param name="text">表示するデバッグ文字列</param>
		/// <param name="pos">表示座標 (x, yのみ使用)</param>
		/// <param name="deleteCounter">表示するカウント数</param>
		/// <returns></returns>
		public void SetData(string name, string text, Vector3 pos, int deleteCounter)
		{
			Name = name;

			if( Obj == null ) return;
			Text textObj = Obj.GetComponent("Text") as Text;
			if( textObj == null ) return;
			textObj.text = text;

			RectTransform rect = Obj.GetComponent("RectTransform") as RectTransform;
			if( rect == null ) return;
			rect.anchoredPosition = pos;

			DeleteCounter = deleteCounter;
		}

		/// <summary>
		/// 更新処理
		/// </summary>
		/// <returns>	true	: 終了
		///				false	: まだ出しておく
		/// </returns>
		public bool Update()
		{
			if( DeleteCounter >= 0 ) DeleteCounter--;
			if( DeleteCounter < 0 ) return true;
			return false;
		}
	}


	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		List<string> deleteList = new List<string>();				// ループの中でRemoveするとバグるんで

		foreach (string key in m_debugTextObjMap.Keys)
		{
			bool ret = m_debugTextObjMap[key].Update();
			if (ret)
			{
				deleteList.Add(key);
			}
		}

		for(int i=0; i<deleteList.Count; i++)						// ループの外で Destroy & Remove
		{
			string key = deleteList[i];
			Destroy(m_debugTextObjMap[key].Obj);
			m_debugTextObjMap.Remove(key);
		}
	}

	/// <summary>
	/// デバッグ文字を表示する
	/// </summary>
	/// <param name="name">デバッグ文字表示名 (同じ名前の物がすでに表示済みなら, 上書きする)</param>
	/// <param name="text">			表示したい文字列</param>
	/// <param name="posX">			表示X座標 (左端が0)</param>
	/// <param name="posY">			表示Y座標 (上端が0)</param>
	/// <param name="deleteCounter">何カウント表示するか (-1を渡せば表示しっぱなし)</param>
	public static void DebugText(string name, string text, float posX=0.0f, float posY=0.0f, int deleteCounter=60)
	{
		if(m_debugTextObjMap == null) return;

		if (m_debugTextObjMap.ContainsKey(name))
		{
			// あるヤツを更新
			m_debugTextObjMap[name].SetData(name, text, new Vector3(posX, posY), deleteCounter);
		}
		else{
			// 新規に作成
			DebugTextObj obj = DebugTextObj.Create(name, text, new Vector3(posX, posY), deleteCounter);
			if (obj != null)
			{
				m_debugTextObjMap.Add(name, obj);
			}
		}
	}


	/// <summary>
	/// デバッグ文字を非表示にする
	/// </summary>
	/// <param name="name">非表示にしたいデバッグ文字の名前</param>
	public static void HideDebugText(string name)
	{
		if(m_debugTextObjMap.ContainsKey(name))
		{
			Destroy(m_debugTextObjMap[name].Obj);
			m_debugTextObjMap.Remove(name);
		}
	}
}
