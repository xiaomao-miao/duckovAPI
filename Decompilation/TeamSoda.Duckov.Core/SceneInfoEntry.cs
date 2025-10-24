using System;
using Eflatun.SceneReference;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x02000128 RID: 296
[Serializable]
public class SceneInfoEntry
{
	// Token: 0x060009A7 RID: 2471 RVA: 0x00029B3C File Offset: 0x00027D3C
	public SceneInfoEntry()
	{
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x00029B44 File Offset: 0x00027D44
	public SceneInfoEntry(string id, SceneReference sceneReference)
	{
		this.id = id;
		this.sceneReference = sceneReference;
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x060009A9 RID: 2473 RVA: 0x00029B5A File Offset: 0x00027D5A
	public int BuildIndex
	{
		get
		{
			if (this.sceneReference.UnsafeReason != SceneReferenceUnsafeReason.None)
			{
				return -1;
			}
			return this.sceneReference.BuildIndex;
		}
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x060009AA RID: 2474 RVA: 0x00029B76 File Offset: 0x00027D76
	public string ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x060009AB RID: 2475 RVA: 0x00029B7E File Offset: 0x00027D7E
	public SceneReference SceneReference
	{
		get
		{
			return this.sceneReference;
		}
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x060009AC RID: 2476 RVA: 0x00029B86 File Offset: 0x00027D86
	public string Description
	{
		get
		{
			return this.description.ToPlainText();
		}
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x060009AD RID: 2477 RVA: 0x00029B93 File Offset: 0x00027D93
	public string DisplayName
	{
		get
		{
			if (string.IsNullOrEmpty(this.displayName))
			{
				return this.id;
			}
			return this.displayName.ToPlainText();
		}
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x060009AE RID: 2478 RVA: 0x00029BB4 File Offset: 0x00027DB4
	public string DisplayNameRaw
	{
		get
		{
			if (string.IsNullOrEmpty(this.displayName))
			{
				return this.id;
			}
			return this.displayName;
		}
	}

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x060009AF RID: 2479 RVA: 0x00029BD0 File Offset: 0x00027DD0
	public bool IsLoaded
	{
		get
		{
			return this.sceneReference != null && this.sceneReference.UnsafeReason == SceneReferenceUnsafeReason.None && this.sceneReference.LoadedScene.isLoaded;
		}
	}

	// Token: 0x0400086F RID: 2159
	[SerializeField]
	private string id;

	// Token: 0x04000870 RID: 2160
	[SerializeField]
	private SceneReference sceneReference;

	// Token: 0x04000871 RID: 2161
	[LocalizationKey("Default")]
	[SerializeField]
	private string displayName;

	// Token: 0x04000872 RID: 2162
	[LocalizationKey("Default")]
	[SerializeField]
	private string description;
}
