using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using FOW;

// Token: 0x0200017A RID: 378
public class DuckovHider : HiderBehavior
{
	// Token: 0x06000B74 RID: 2932 RVA: 0x0003072F File Offset: 0x0002E92F
	protected override void Awake()
	{
		base.Awake();
		LevelManager.OnMainCharacterDead += this.OnMainCharacterDie;
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x00030748 File Offset: 0x0002E948
	private void OnDestroy()
	{
		LevelManager.OnMainCharacterDead -= this.OnMainCharacterDie;
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x0003075B File Offset: 0x0002E95B
	protected override void OnHide()
	{
		if (!LevelManager.Instance || !LevelManager.Instance.IsRaidMap || this.mainCharacterDied)
		{
			return;
		}
		this.targetHide = true;
		this.HideDelay();
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x0003078C File Offset: 0x0002E98C
	protected override void OnReveal()
	{
		this.targetHide = false;
		this.character.Show();
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x000307A0 File Offset: 0x0002E9A0
	private UniTask HideDelay()
	{
		DuckovHider.<HideDelay>d__8 <HideDelay>d__;
		<HideDelay>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<HideDelay>d__.<>4__this = this;
		<HideDelay>d__.<>1__state = -1;
		<HideDelay>d__.<>t__builder.Start<DuckovHider.<HideDelay>d__8>(ref <HideDelay>d__);
		return <HideDelay>d__.<>t__builder.Task;
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x000307E3 File Offset: 0x0002E9E3
	private void OnMainCharacterDie(DamageInfo damageInfo)
	{
		this.mainCharacterDied = true;
		this.OnReveal();
	}

	// Token: 0x040009C0 RID: 2496
	public CharacterMainControl character;

	// Token: 0x040009C1 RID: 2497
	private float hideDelay = 0.2f;

	// Token: 0x040009C2 RID: 2498
	private bool targetHide;

	// Token: 0x040009C3 RID: 2499
	private bool mainCharacterDied;
}
