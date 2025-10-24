using System;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class SetActiveByChance : MonoBehaviour
{
	// Token: 0x060005BB RID: 1467 RVA: 0x000199B0 File Offset: 0x00017BB0
	private void Awake()
	{
		bool flag = UnityEngine.Random.Range(0f, 1f) < this.activeChange;
		if (this.saveInLevel && MultiSceneCore.Instance)
		{
			object obj;
			if (MultiSceneCore.Instance.inLevelData.TryGetValue(this.keyCached, out obj) && obj is bool)
			{
				bool flag2 = (bool)obj;
				Debug.Log(string.Format("存在门存档信息：{0}", flag2));
				flag = flag2;
			}
			MultiSceneCore.Instance.inLevelData[this.keyCached] = flag;
		}
		base.gameObject.SetActive(flag);
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x00019A50 File Offset: 0x00017C50
	private int GetKey()
	{
		Vector3 vector = base.transform.position * 10f;
		int x = Mathf.RoundToInt(vector.x);
		int y = Mathf.RoundToInt(vector.y);
		int z = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int = new Vector3Int(x, y, z);
		return string.Format("Door_{0}", vector3Int).GetHashCode();
	}

	// Token: 0x0400053A RID: 1338
	public bool saveInLevel;

	// Token: 0x0400053B RID: 1339
	private int keyCached;

	// Token: 0x0400053C RID: 1340
	[Range(0f, 1f)]
	public float activeChange = 0.5f;
}
