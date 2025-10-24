using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class CustomFaceData : ScriptableObject
{
	// Token: 0x17000049 RID: 73
	// (get) Token: 0x060000E5 RID: 229 RVA: 0x0000493C File Offset: 0x00002B3C
	public CustomFacePartCollection Hairs
	{
		get
		{
			return this.hairs;
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x060000E6 RID: 230 RVA: 0x00004944 File Offset: 0x00002B44
	public CustomFacePartCollection Eyes
	{
		get
		{
			return this.eyes;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x060000E7 RID: 231 RVA: 0x0000494C File Offset: 0x00002B4C
	public CustomFacePartCollection Mouths
	{
		get
		{
			return this.mouths;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x060000E8 RID: 232 RVA: 0x00004954 File Offset: 0x00002B54
	public CustomFacePartCollection Eyebrows
	{
		get
		{
			return this.eyebrows;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060000E9 RID: 233 RVA: 0x0000495C File Offset: 0x00002B5C
	public CustomFacePartCollection Decorations
	{
		get
		{
			return this.decorations;
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x060000EA RID: 234 RVA: 0x00004964 File Offset: 0x00002B64
	public CustomFacePartCollection Tails
	{
		get
		{
			return this.tails;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x060000EB RID: 235 RVA: 0x0000496C File Offset: 0x00002B6C
	public CustomFacePartCollection Foots
	{
		get
		{
			return this.foots;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x060000EC RID: 236 RVA: 0x00004974 File Offset: 0x00002B74
	public CustomFacePartCollection Wings
	{
		get
		{
			return this.wings;
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x060000ED RID: 237 RVA: 0x0000497C File Offset: 0x00002B7C
	public CustomFacePreset DefaultPreset
	{
		get
		{
			return this.defaultPreset;
		}
	}

	// Token: 0x04000050 RID: 80
	public string prefabsPath = "Assets/CustomFace/CustomFacePrefabs";

	// Token: 0x04000051 RID: 81
	public string info;

	// Token: 0x04000052 RID: 82
	[SerializeField]
	private CustomFacePartCollection hairs;

	// Token: 0x04000053 RID: 83
	[SerializeField]
	private CustomFacePartCollection eyes;

	// Token: 0x04000054 RID: 84
	[SerializeField]
	private CustomFacePartCollection mouths;

	// Token: 0x04000055 RID: 85
	[SerializeField]
	private CustomFacePartCollection eyebrows;

	// Token: 0x04000056 RID: 86
	[SerializeField]
	private CustomFacePartCollection decorations;

	// Token: 0x04000057 RID: 87
	[SerializeField]
	private CustomFacePartCollection tails;

	// Token: 0x04000058 RID: 88
	[SerializeField]
	private CustomFacePartCollection foots;

	// Token: 0x04000059 RID: 89
	[SerializeField]
	private CustomFacePartCollection wings;

	// Token: 0x0400005A RID: 90
	[SerializeField]
	private CustomFacePreset defaultPreset;
}
