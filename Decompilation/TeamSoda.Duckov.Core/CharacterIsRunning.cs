using System;
using ItemStatsSystem;

// Token: 0x02000088 RID: 136
[MenuPath("角色/角色正在奔跑")]
public class CharacterIsRunning : EffectFilter
{
	// Token: 0x17000101 RID: 257
	// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00015F2C File Offset: 0x0001412C
	public override string DisplayName
	{
		get
		{
			return "角色正在奔跑";
		}
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x060004D6 RID: 1238 RVA: 0x00015F33 File Offset: 0x00014133
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

	// Token: 0x060004D7 RID: 1239 RVA: 0x00015F6D File Offset: 0x0001416D
	protected override bool OnEvaluate(EffectTriggerEventContext context)
	{
		return this.MainControl.Running;
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x00015F7A File Offset: 0x0001417A
	private void OnDestroy()
	{
	}

	// Token: 0x04000411 RID: 1041
	private CharacterMainControl _mainControl;
}
