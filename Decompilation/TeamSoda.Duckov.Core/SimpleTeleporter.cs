using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

// Token: 0x020000AF RID: 175
public class SimpleTeleporter : InteractableBase
{
	// Token: 0x17000121 RID: 289
	// (get) Token: 0x060005C4 RID: 1476 RVA: 0x00019BEF File Offset: 0x00017DEF
	public Transform TeleportPoint
	{
		get
		{
			if (!this.selfTeleportPoint)
			{
				return base.transform;
			}
			return this.selfTeleportPoint;
		}
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x00019C0B File Offset: 0x00017E0B
	protected override void Awake()
	{
		base.Awake();
		this.teleportVolume.gameObject.SetActive(false);
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x00019C24 File Offset: 0x00017E24
	protected override void OnInteractFinished()
	{
		if (!this.interactCharacter)
		{
			return;
		}
		this.Teleport(this.interactCharacter).Forget();
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x00019C48 File Offset: 0x00017E48
	private UniTask Teleport(CharacterMainControl targetCharacter)
	{
		SimpleTeleporter.<Teleport>d__13 <Teleport>d__;
		<Teleport>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Teleport>d__.<>4__this = this;
		<Teleport>d__.targetCharacter = targetCharacter;
		<Teleport>d__.<>1__state = -1;
		<Teleport>d__.<>t__builder.Start<SimpleTeleporter.<Teleport>d__13>(ref <Teleport>d__);
		return <Teleport>d__.<>t__builder.Task;
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x00019C94 File Offset: 0x00017E94
	private UniTask VolumeFx(bool show, float time)
	{
		SimpleTeleporter.<VolumeFx>d__14 <VolumeFx>d__;
		<VolumeFx>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<VolumeFx>d__.<>4__this = this;
		<VolumeFx>d__.show = show;
		<VolumeFx>d__.time = time;
		<VolumeFx>d__.<>1__state = -1;
		<VolumeFx>d__.<>t__builder.Start<SimpleTeleporter.<VolumeFx>d__14>(ref <VolumeFx>d__);
		return <VolumeFx>d__.<>t__builder.Task;
	}

	// Token: 0x04000544 RID: 1348
	public Transform target;

	// Token: 0x04000545 RID: 1349
	[SerializeField]
	private Transform selfTeleportPoint;

	// Token: 0x04000546 RID: 1350
	[SerializeField]
	private SimpleTeleporter.TransitionTypes transitionType;

	// Token: 0x04000547 RID: 1351
	[FormerlySerializedAs("fxTime")]
	public float transitionTime = 0.28f;

	// Token: 0x04000548 RID: 1352
	private float delay = 0.3f;

	// Token: 0x04000549 RID: 1353
	public Volume teleportVolume;

	// Token: 0x0400054A RID: 1354
	private int fxShaderID = Shader.PropertyToID("TeleportFXStrength");

	// Token: 0x0400054B RID: 1355
	private bool blackScreen;

	// Token: 0x02000458 RID: 1112
	public enum TransitionTypes
	{
		// Token: 0x04001AEF RID: 6895
		volumeFx,
		// Token: 0x04001AF0 RID: 6896
		blackScreen
	}
}
