using System;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200008A RID: 138
[MenuPath("Health/一段时间没受伤")]
public class NotHurtForSeconds : EffectFilter
{
	// Token: 0x17000105 RID: 261
	// (get) Token: 0x060004DF RID: 1247 RVA: 0x0001606B File Offset: 0x0001426B
	public override string DisplayName
	{
		get
		{
			return this.time.ToString() + "秒内没受伤";
		}
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x060004E0 RID: 1248 RVA: 0x00016082 File Offset: 0x00014282
	private CharacterMainControl MainControl
	{
		get
		{
			if (this._mainControl == null)
			{
				Effect master = base.Master;
				CharacterMainControl mainControl;
				if (master == null)
				{
					mainControl = null;
				}
				else
				{
					Item item = master.Item;
					mainControl = ((item != null) ? item.GetCharacterMainControl() : null);
				}
				this._mainControl = mainControl;
			}
			return this._mainControl;
		}
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x000160BC File Offset: 0x000142BC
	protected override bool OnEvaluate(EffectTriggerEventContext context)
	{
		if (!this.binded && this.MainControl)
		{
			this.MainControl.Health.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnHurt));
			this.binded = true;
		}
		return Time.time - this.lastHurtTime > this.time;
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x0001611A File Offset: 0x0001431A
	private void OnDestroy()
	{
		if (this.MainControl)
		{
			this.MainControl.Health.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnHurt));
		}
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x0001614A File Offset: 0x0001434A
	private void OnHurt(DamageInfo dmgInfo)
	{
		this.lastHurtTime = Time.time;
	}

	// Token: 0x04000416 RID: 1046
	public float time;

	// Token: 0x04000417 RID: 1047
	private float lastHurtTime = -9999f;

	// Token: 0x04000418 RID: 1048
	private bool binded;

	// Token: 0x04000419 RID: 1049
	private CharacterMainControl _mainControl;
}
