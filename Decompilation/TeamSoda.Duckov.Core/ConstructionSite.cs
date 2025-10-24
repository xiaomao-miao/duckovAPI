using System;
using Duckov.Economy;
using Duckov.Scenes;
using Saves;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000D3 RID: 211
public class ConstructionSite : MonoBehaviour
{
	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000676 RID: 1654 RVA: 0x0001D41F File Offset: 0x0001B61F
	private Color KeyFieldColor
	{
		get
		{
			if (string.IsNullOrWhiteSpace(this._key))
			{
				return Color.red;
			}
			return Color.white;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000677 RID: 1655 RVA: 0x0001D43C File Offset: 0x0001B63C
	private string SaveKey
	{
		get
		{
			return "ConstructionSite_" + this._key;
		}
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x0001D450 File Offset: 0x0001B650
	private void Awake()
	{
		this.costTaker.onPayed += this.OnBuilt;
		this.Load();
		SavesSystem.OnCollectSaveData += this.Save;
		this.costTaker.SetCost(this.cost);
		this.RefreshGameObjects();
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x0001D4A2 File Offset: 0x0001B6A2
	private void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0001D4B8 File Offset: 0x0001B6B8
	private void Save()
	{
		if (this.dontSave)
		{
			int inLevelDataKey = this.GetInLevelDataKey();
			if (MultiSceneCore.Instance.inLevelData.ContainsKey(inLevelDataKey))
			{
				MultiSceneCore.Instance.inLevelData[inLevelDataKey] = this.wasBuilt;
				return;
			}
			MultiSceneCore.Instance.inLevelData.Add(inLevelDataKey, this.wasBuilt);
			return;
		}
		else
		{
			if (string.IsNullOrWhiteSpace(this._key))
			{
				Debug.LogError(string.Format("Construction Site {0} 没有配置保存用的key", base.gameObject));
				return;
			}
			SavesSystem.Save<bool>(this.SaveKey, this.wasBuilt);
			return;
		}
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x0001D554 File Offset: 0x0001B754
	private int GetInLevelDataKey()
	{
		Vector3 vector = base.transform.position * 10f;
		int x = Mathf.RoundToInt(vector.x);
		int y = Mathf.RoundToInt(vector.y);
		int z = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int = new Vector3Int(x, y, z);
		return ("ConstSite" + vector3Int.ToString()).GetHashCode();
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x0001D5C0 File Offset: 0x0001B7C0
	private void Load()
	{
		if (!this.dontSave)
		{
			if (string.IsNullOrWhiteSpace(this._key))
			{
				Debug.LogError(string.Format("Construction Site {0} 没有配置保存用的key", base.gameObject));
			}
			this.wasBuilt = SavesSystem.Load<bool>(this.SaveKey);
		}
		else
		{
			int inLevelDataKey = this.GetInLevelDataKey();
			object obj;
			MultiSceneCore.Instance.inLevelData.TryGetValue(inLevelDataKey, out obj);
			if (obj != null)
			{
				this.wasBuilt = (bool)obj;
			}
		}
		if (this.wasBuilt)
		{
			this.OnActivate();
			return;
		}
		this.OnDeactivate();
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x0001D648 File Offset: 0x0001B848
	private void Start()
	{
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x0001D64C File Offset: 0x0001B84C
	private void OnBuilt(CostTaker taker)
	{
		this.wasBuilt = true;
		UnityEvent<ConstructionSite> unityEvent = this.onBuilt;
		if (unityEvent != null)
		{
			unityEvent.Invoke(this);
		}
		this.RefreshGameObjects();
		foreach (GameObject gameObject in this.setActiveOnBuilt)
		{
			if (gameObject)
			{
				gameObject.SetActive(true);
			}
		}
		this.Save();
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x0001D6A6 File Offset: 0x0001B8A6
	private void OnActivate()
	{
		UnityEvent<ConstructionSite> unityEvent = this.onActivate;
		if (unityEvent != null)
		{
			unityEvent.Invoke(this);
		}
		this.RefreshGameObjects();
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x0001D6C0 File Offset: 0x0001B8C0
	private void OnDeactivate()
	{
		UnityEvent<ConstructionSite> unityEvent = this.onDeactivate;
		if (unityEvent != null)
		{
			unityEvent.Invoke(this);
		}
		this.RefreshGameObjects();
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x0001D6DC File Offset: 0x0001B8DC
	public void RefreshGameObjects()
	{
		this.costTaker.gameObject.SetActive(!this.wasBuilt);
		foreach (GameObject gameObject in this.notBuiltGameObjects)
		{
			if (gameObject)
			{
				gameObject.SetActive(!this.wasBuilt);
			}
		}
		foreach (GameObject gameObject2 in this.builtGameObjects)
		{
			if (gameObject2)
			{
				gameObject2.SetActive(this.wasBuilt);
			}
		}
	}

	// Token: 0x04000648 RID: 1608
	[SerializeField]
	private string _key;

	// Token: 0x04000649 RID: 1609
	[SerializeField]
	private bool dontSave;

	// Token: 0x0400064A RID: 1610
	private bool saveInMultiSceneCore;

	// Token: 0x0400064B RID: 1611
	[SerializeField]
	private Cost cost;

	// Token: 0x0400064C RID: 1612
	[SerializeField]
	private CostTaker costTaker;

	// Token: 0x0400064D RID: 1613
	[SerializeField]
	private GameObject[] notBuiltGameObjects;

	// Token: 0x0400064E RID: 1614
	[SerializeField]
	private GameObject[] builtGameObjects;

	// Token: 0x0400064F RID: 1615
	[SerializeField]
	private GameObject[] setActiveOnBuilt;

	// Token: 0x04000650 RID: 1616
	[SerializeField]
	private UnityEvent<ConstructionSite> onBuilt;

	// Token: 0x04000651 RID: 1617
	[SerializeField]
	private UnityEvent<ConstructionSite> onActivate;

	// Token: 0x04000652 RID: 1618
	[SerializeField]
	private UnityEvent<ConstructionSite> onDeactivate;

	// Token: 0x04000653 RID: 1619
	private bool wasBuilt;
}
