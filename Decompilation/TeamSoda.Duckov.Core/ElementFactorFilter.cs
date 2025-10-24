using System;
using ItemStatsSystem;

// Token: 0x02000089 RID: 137
[MenuPath("弱属性")]
public class ElementFactorFilter : EffectFilter
{
	// Token: 0x17000103 RID: 259
	// (get) Token: 0x060004DA RID: 1242 RVA: 0x00015F84 File Offset: 0x00014184
	public override string DisplayName
	{
		get
		{
			return string.Format("如果{0}系数{1}{2}", this.element, (this.type == ElementFactorFilter.ElementFactorFilterTypes.GreaterThan) ? "大于" : "小于", this.compareTo);
		}
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x060004DB RID: 1243 RVA: 0x00015FBA File Offset: 0x000141BA
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

	// Token: 0x060004DC RID: 1244 RVA: 0x00015FF4 File Offset: 0x000141F4
	protected override bool OnEvaluate(EffectTriggerEventContext context)
	{
		if (!this.MainControl)
		{
			return false;
		}
		if (!this.MainControl.Health)
		{
			return false;
		}
		float num = this.MainControl.Health.ElementFactor(this.element);
		if (this.type != ElementFactorFilter.ElementFactorFilterTypes.GreaterThan)
		{
			return num < this.compareTo;
		}
		return num > this.compareTo;
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x00016056 File Offset: 0x00014256
	private void OnDestroy()
	{
	}

	// Token: 0x04000412 RID: 1042
	public ElementFactorFilter.ElementFactorFilterTypes type;

	// Token: 0x04000413 RID: 1043
	public float compareTo = 1f;

	// Token: 0x04000414 RID: 1044
	public ElementTypes element;

	// Token: 0x04000415 RID: 1045
	private CharacterMainControl _mainControl;

	// Token: 0x0200043F RID: 1087
	public enum ElementFactorFilterTypes
	{
		// Token: 0x04001A71 RID: 6769
		GreaterThan,
		// Token: 0x04001A72 RID: 6770
		LessThan
	}
}
