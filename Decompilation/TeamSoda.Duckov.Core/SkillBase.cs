using System;
using Duckov;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000130 RID: 304
public abstract class SkillBase : MonoBehaviour
{
	// Token: 0x17000205 RID: 517
	// (get) Token: 0x060009D9 RID: 2521 RVA: 0x0002A2E5 File Offset: 0x000284E5
	public float LastReleaseTime
	{
		get
		{
			return this.lastReleaseTime;
		}
	}

	// Token: 0x17000206 RID: 518
	// (get) Token: 0x060009DA RID: 2522 RVA: 0x0002A2ED File Offset: 0x000284ED
	public SkillContext SkillContext
	{
		get
		{
			return this.skillContext;
		}
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x0002A2F8 File Offset: 0x000284F8
	public void ReleaseSkill(SkillReleaseContext releaseContext, CharacterMainControl from)
	{
		this.lastReleaseTime = Time.time;
		this.skillReleaseContext = releaseContext;
		this.fromCharacter = from;
		this.fromCharacter.UseStamina(this.staminaCost);
		if (this.hasReleaseSound && this.fromCharacter != null && this.onReleaseSound != "")
		{
			AudioManager.Post(this.onReleaseSound, from.gameObject);
		}
		this.OnRelease();
		Action onSkillReleasedEvent = this.OnSkillReleasedEvent;
		if (onSkillReleasedEvent == null)
		{
			return;
		}
		onSkillReleasedEvent();
	}

	// Token: 0x060009DC RID: 2524
	public abstract void OnRelease();

	// Token: 0x0400089A RID: 2202
	public bool hasReleaseSound;

	// Token: 0x0400089B RID: 2203
	public string onReleaseSound;

	// Token: 0x0400089C RID: 2204
	public Sprite icon;

	// Token: 0x0400089D RID: 2205
	public float staminaCost = 10f;

	// Token: 0x0400089E RID: 2206
	public float coolDownTime = 1f;

	// Token: 0x0400089F RID: 2207
	private float lastReleaseTime = -999f;

	// Token: 0x040008A0 RID: 2208
	[SerializeField]
	protected SkillContext skillContext;

	// Token: 0x040008A1 RID: 2209
	protected SkillReleaseContext skillReleaseContext;

	// Token: 0x040008A2 RID: 2210
	protected CharacterMainControl fromCharacter;

	// Token: 0x040008A3 RID: 2211
	[HideInInspector]
	public Item fromItem;

	// Token: 0x040008A4 RID: 2212
	public Action OnSkillReleasedEvent;
}
