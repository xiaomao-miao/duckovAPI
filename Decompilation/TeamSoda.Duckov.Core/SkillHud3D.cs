using System;
using UnityEngine;

// Token: 0x020000CB RID: 203
public class SkillHud3D : MonoBehaviour
{
	// Token: 0x06000651 RID: 1617 RVA: 0x0001C78E File Offset: 0x0001A98E
	private void Awake()
	{
		this.HideAll();
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x0001C796 File Offset: 0x0001A996
	private void HideAll()
	{
		this.skillRangeHUD.gameObject.SetActive(false);
		this.projectileLine.gameObject.SetActive(false);
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x0001C7BC File Offset: 0x0001A9BC
	private void LateUpdate()
	{
		if (!this.character)
		{
			this.character = LevelManager.Instance.MainCharacter;
			return;
		}
		this.currentSkill = null;
		this.currentSkill = this.character.skillAction.CurrentRunningSkill;
		if (this.aiming != (this.currentSkill != null))
		{
			this.aiming = !this.aiming;
			if (this.currentSkill != null)
			{
				this.currentSkill = this.character.skillAction.CurrentRunningSkill;
				this.skillRangeHUD.gameObject.SetActive(true);
				float range = 1f;
				if (this.currentSkill.SkillContext.effectRange > 1f)
				{
					range = this.currentSkill.SkillContext.effectRange;
				}
				this.skillRangeHUD.SetRange(range);
				if (this.currentSkill.SkillContext.isGrenade)
				{
					this.projectileLine.gameObject.SetActive(true);
				}
			}
			else
			{
				this.HideAll();
			}
		}
		Vector3 currentSkillAimPoint = this.character.GetCurrentSkillAimPoint();
		Vector3 one = Vector3.one;
		if (this.projectileLine.gameObject.activeSelf)
		{
			bool flag = this.projectileLine.UpdateLine(this.character.CurrentUsingAimSocket.position, currentSkillAimPoint, this.currentSkill.SkillContext.grenageVerticleSpeed, ref one);
		}
		this.skillRangeHUD.transform.position = currentSkillAimPoint;
		this.skillRangeHUD.SetProgress(this.character.skillAction.GetProgress().progress);
	}

	// Token: 0x04000617 RID: 1559
	private CharacterMainControl character;

	// Token: 0x04000618 RID: 1560
	private bool aiming;

	// Token: 0x04000619 RID: 1561
	public SkillRangeHUD skillRangeHUD;

	// Token: 0x0400061A RID: 1562
	public SkillProjectileLineHUD projectileLine;

	// Token: 0x0400061B RID: 1563
	private SkillBase currentSkill;
}
