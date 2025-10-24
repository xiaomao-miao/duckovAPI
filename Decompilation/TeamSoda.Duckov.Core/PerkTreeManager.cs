using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.PerkTrees;
using UnityEngine;

// Token: 0x020001E4 RID: 484
public class PerkTreeManager : MonoBehaviour
{
	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06000E54 RID: 3668 RVA: 0x00039992 File Offset: 0x00037B92
	public static PerkTreeManager Instance
	{
		get
		{
			return PerkTreeManager.instance;
		}
	}

	// Token: 0x06000E55 RID: 3669 RVA: 0x00039999 File Offset: 0x00037B99
	private void Awake()
	{
		if (PerkTreeManager.instance == null)
		{
			PerkTreeManager.instance = this;
			return;
		}
		Debug.LogError("检测到多个PerkTreeManager");
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x000399BC File Offset: 0x00037BBC
	public static PerkTree GetPerkTree(string id)
	{
		if (PerkTreeManager.instance == null)
		{
			return null;
		}
		PerkTree perkTree = PerkTreeManager.instance.perkTrees.FirstOrDefault((PerkTree e) => e != null && e.ID == id);
		if (perkTree == null)
		{
			Debug.LogError("未找到PerkTree id:" + id);
		}
		return perkTree;
	}

	// Token: 0x04000BCF RID: 3023
	private static PerkTreeManager instance;

	// Token: 0x04000BD0 RID: 3024
	public List<PerkTree> perkTrees;
}
