using System;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x0200009B RID: 155
public class TagUtilities
{
	// Token: 0x0600052D RID: 1325 RVA: 0x000175D4 File Offset: 0x000157D4
	public static Tag TagFromString(string name)
	{
		name = name.Trim();
		Tag tag = GameplayDataSettings.Tags.AllTags.FirstOrDefault((Tag e) => e != null && e.name == name);
		if (tag == null)
		{
			Debug.LogError("未找到Tag: " + name);
		}
		return tag;
	}
}
