using System;
using System.Collections.Generic;
using NodeCanvas.DialogueTrees;
using UnityEngine;

// Token: 0x020001B1 RID: 433
public class DuckovDialogueActor : MonoBehaviour, IDialogueActor
{
	// Token: 0x1700024F RID: 591
	// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x00035969 File Offset: 0x00033B69
	private static List<DuckovDialogueActor> ActiveActors
	{
		get
		{
			if (DuckovDialogueActor._activeActors == null)
			{
				DuckovDialogueActor._activeActors = new List<DuckovDialogueActor>();
			}
			return DuckovDialogueActor._activeActors;
		}
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x00035981 File Offset: 0x00033B81
	public static void Register(DuckovDialogueActor actor)
	{
		if (DuckovDialogueActor.ActiveActors.Contains(actor))
		{
			Debug.Log("Actor " + actor.nameKey + " 在重复注册", actor);
			return;
		}
		DuckovDialogueActor.ActiveActors.Add(actor);
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x000359B7 File Offset: 0x00033BB7
	public static void Unregister(DuckovDialogueActor actor)
	{
		DuckovDialogueActor.ActiveActors.Remove(actor);
	}

	// Token: 0x06000CD4 RID: 3284 RVA: 0x000359C8 File Offset: 0x00033BC8
	public static DuckovDialogueActor Get(string id)
	{
		return DuckovDialogueActor.ActiveActors.Find((DuckovDialogueActor e) => e.ID == id);
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x000359F8 File Offset: 0x00033BF8
	public string ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x00035A00 File Offset: 0x00033C00
	public Vector3 Offset
	{
		get
		{
			return this.offset;
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x00035A08 File Offset: 0x00033C08
	public string NameKey
	{
		get
		{
			return this.nameKey;
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00035A10 File Offset: 0x00033C10
	public Texture2D portrait
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x00035A13 File Offset: 0x00033C13
	public Sprite portraitSprite
	{
		get
		{
			return this._portraitSprite;
		}
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00035A1C File Offset: 0x00033C1C
	public Color dialogueColor
	{
		get
		{
			return default(Color);
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06000CDB RID: 3291 RVA: 0x00035A34 File Offset: 0x00033C34
	public Vector3 dialoguePosition
	{
		get
		{
			return default(Vector3);
		}
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x00035A4A File Offset: 0x00033C4A
	private void OnEnable()
	{
		DuckovDialogueActor.Register(this);
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x00035A52 File Offset: 0x00033C52
	private void OnDisable()
	{
		DuckovDialogueActor.Unregister(this);
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x00035A62 File Offset: 0x00033C62
	string IDialogueActor.get_name()
	{
		return base.name;
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x00035A6A File Offset: 0x00033C6A
	Transform IDialogueActor.get_transform()
	{
		return base.transform;
	}

	// Token: 0x04000B14 RID: 2836
	private static List<DuckovDialogueActor> _activeActors;

	// Token: 0x04000B15 RID: 2837
	[SerializeField]
	private string id;

	// Token: 0x04000B16 RID: 2838
	[SerializeField]
	private Sprite _portraitSprite;

	// Token: 0x04000B17 RID: 2839
	[SerializeField]
	[LocalizationKey("Default")]
	private string nameKey;

	// Token: 0x04000B18 RID: 2840
	[SerializeField]
	private Vector3 offset;
}
