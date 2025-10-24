using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000029 RID: 41
[Serializable]
public class CustomFacePartCollection
{
	// Token: 0x17000052 RID: 82
	// (get) Token: 0x060000EF RID: 239 RVA: 0x00004997 File Offset: 0x00002B97
	public int totalCount
	{
		get
		{
			return this.parts.Count;
		}
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x000049A4 File Offset: 0x00002BA4
	public void Clear()
	{
		this.parts.Clear();
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x000049B4 File Offset: 0x00002BB4
	public CustomFacePart GetNextOrPrevPrefab(int currentID, int direction)
	{
		int num = this.parts.FindIndex((CustomFacePartMeta part) => part.id == currentID);
		if (num == -1)
		{
			num = 0;
		}
		else
		{
			num += direction;
		}
		if (num < 0)
		{
			num = this.parts.Count - 1;
		}
		else if (num >= this.parts.Count)
		{
			num = 0;
		}
		return this.parts[num].part;
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00004A28 File Offset: 0x00002C28
	public CustomFacePart GetPartPrefab(int id)
	{
		int num = this.parts.FindIndex((CustomFacePartMeta part) => part.id == id);
		if (num < 0)
		{
			return this.parts[0].part;
		}
		return this.parts[num].part;
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00004A81 File Offset: 0x00002C81
	public void Sort()
	{
		this.parts.Sort((CustomFacePartMeta a, CustomFacePartMeta b) => a.id.CompareTo(b.id));
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00004AB0 File Offset: 0x00002CB0
	public void AddNewCollection(CustomFacePart newPart)
	{
		CustomFacePartMeta item = default(CustomFacePartMeta);
		item.part = newPart;
		item.id = newPart.id;
		this.parts.Add(item);
	}

	// Token: 0x0400005B RID: 91
	[SerializeField]
	private List<CustomFacePartMeta> parts;
}
