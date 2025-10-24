using System;
using UnityEngine;

namespace ItemStatsSystem.Stats
{
	// Token: 0x02000027 RID: 39
	public class Modifier
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000802A File Offset: 0x0000622A
		public int Order
		{
			get
			{
				if (this.overrideOrder)
				{
					return this.overrideOrderValue;
				}
				return (int)this.type;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00008041 File Offset: 0x00006241
		public ModifierType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00008049 File Offset: 0x00006249
		// (set) Token: 0x06000206 RID: 518 RVA: 0x00008051 File Offset: 0x00006251
		public float Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (value != this.value)
				{
					this.value = value;
					if (this.target != null)
					{
						this.target.SetDirty();
					}
				}
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00008076 File Offset: 0x00006276
		public object Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000807E File Offset: 0x0000627E
		public Modifier(ModifierType type, float value, object source) : this(type, value, false, 0, source)
		{
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000808B File Offset: 0x0000628B
		public Modifier(ModifierType type, float value, bool overrideOrder, int overrideOrderValue, object source)
		{
			this.type = type;
			this.value = value;
			this.overrideOrder = overrideOrder;
			this.overrideOrderValue = overrideOrderValue;
			this.source = source;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x000080B8 File Offset: 0x000062B8
		public void NotifyAddedToStat(Stat stat)
		{
			if (this.target != null && this.target != stat)
			{
				Debug.LogError("Modifier被赋予给了多了个不同的Stat");
				this.target.RemoveModifier(this);
			}
			this.target = stat;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x000080E9 File Offset: 0x000062E9
		public void RemoveFromTarget()
		{
			if (this.target != null)
			{
				this.target.RemoveModifier(this);
				this.target = null;
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00008108 File Offset: 0x00006308
		public override string ToString()
		{
			string text = "";
			bool flag = this.value > 0f;
			ModifierType modifierType = this.type;
			if (modifierType != ModifierType.Add)
			{
				if (modifierType != ModifierType.PercentageAdd)
				{
					if (modifierType == ModifierType.PercentageMultiply)
					{
						text = string.Format("x(1{0}{1})", flag ? "+" : "", this.value);
					}
				}
				else
				{
					text = string.Format("x(..{0}{1})", flag ? "+" : "", this.value);
				}
			}
			else
			{
				text = string.Format("{0}{1}", flag ? "+" : "", this.value);
			}
			string text2 = flag ? "#55FF55" : "#FF5555";
			text = string.Concat(new string[]
			{
				"<color=",
				text2,
				">",
				text,
				"</color>"
			});
			return text + " 来自 " + this.source.ToString();
		}

		// Token: 0x040000B4 RID: 180
		private Stat target;

		// Token: 0x040000B5 RID: 181
		[SerializeField]
		private ModifierType type;

		// Token: 0x040000B6 RID: 182
		[SerializeField]
		private float value;

		// Token: 0x040000B7 RID: 183
		[SerializeField]
		private bool overrideOrder;

		// Token: 0x040000B8 RID: 184
		[SerializeField]
		private int overrideOrderValue;

		// Token: 0x040000B9 RID: 185
		[SerializeField]
		private object source;

		// Token: 0x040000BA RID: 186
		public static readonly Comparison<Modifier> OrderComparison = (Modifier a, Modifier b) => a.Order - b.Order;
	}
}
