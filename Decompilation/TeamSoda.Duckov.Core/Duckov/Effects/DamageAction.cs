using System;
using Duckov.Buffs;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Effects
{
	// Token: 0x020003EE RID: 1006
	public class DamageAction : EffectAction
	{
		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002456 RID: 9302 RVA: 0x0007E554 File Offset: 0x0007C754
		private CharacterMainControl MainControl
		{
			get
			{
				Effect master = base.Master;
				if (master == null)
				{
					return null;
				}
				Item item = master.Item;
				if (item == null)
				{
					return null;
				}
				return item.GetCharacterMainControl();
			}
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x0007E574 File Offset: 0x0007C774
		protected override void OnTriggeredPositive()
		{
			if (this.MainControl == null)
			{
				return;
			}
			if (this.MainControl.Health == null)
			{
				return;
			}
			this.damageInfo.isFromBuffOrEffect = true;
			if (this.buff != null)
			{
				this.damageInfo.fromCharacter = this.buff.fromWho;
				this.damageInfo.fromWeaponItemID = this.buff.fromWeaponID;
			}
			this.damageInfo.damagePoint = this.MainControl.transform.position + Vector3.up * 0.8f;
			this.damageInfo.damageNormal = Vector3.up;
			if (this.percentDamage && this.MainControl.Health != null)
			{
				this.damageInfo.damageValue = this.percentDamageValue * this.MainControl.Health.MaxHealth * ((this.buff == null) ? 1f : ((float)this.buff.CurrentLayers));
			}
			else
			{
				this.damageInfo.damageValue = this.damageValue * ((this.buff == null) ? 1f : ((float)this.buff.CurrentLayers));
			}
			this.MainControl.Health.Hurt(this.damageInfo);
			if (this.fx)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.fx, this.damageInfo.damagePoint, Quaternion.identity);
			}
		}

		// Token: 0x040018B5 RID: 6325
		[SerializeField]
		private Buff buff;

		// Token: 0x040018B6 RID: 6326
		[SerializeField]
		private bool percentDamage;

		// Token: 0x040018B7 RID: 6327
		[SerializeField]
		private float damageValue = 1f;

		// Token: 0x040018B8 RID: 6328
		[SerializeField]
		private float percentDamageValue;

		// Token: 0x040018B9 RID: 6329
		[SerializeField]
		private DamageInfo damageInfo = new DamageInfo(null);

		// Token: 0x040018BA RID: 6330
		[SerializeField]
		private GameObject fx;
	}
}
