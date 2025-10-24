using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class CharacterTouchInputControl : MonoBehaviour
{
	// Token: 0x0600048F RID: 1167 RVA: 0x00014E59 File Offset: 0x00013059
	public void SetMoveInput(Vector2 axisInput, bool holding)
	{
		this.characterInputManager.SetMoveInput(axisInput);
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x00014E67 File Offset: 0x00013067
	public void SetRunInput(bool holding)
	{
		this.characterInputManager.SetRunInput(holding);
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00014E75 File Offset: 0x00013075
	public void SetAdsInput(bool holding)
	{
		this.characterInputManager.SetAdsInput(holding);
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x00014E83 File Offset: 0x00013083
	public void SetGunAimInput(Vector2 axisInput, bool holding)
	{
		this.characterInputManager.SetAimInputUsingJoystick(axisInput);
		this.characterInputManager.SetAimType(AimTypes.normalAim);
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00014E9D File Offset: 0x0001309D
	public void SetCharacterSkillAimInput(Vector2 axisInput, bool holding)
	{
		this.characterInputManager.SetAimInputUsingJoystick(axisInput);
		this.characterInputManager.SetAimType(AimTypes.characterSkill);
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x00014EB7 File Offset: 0x000130B7
	public void StartCharacterSkillAim()
	{
		this.characterInputManager.StartCharacterSkillAim();
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x00014EC4 File Offset: 0x000130C4
	public void CharacterSkillRelease(bool trigger)
	{
		if (!trigger)
		{
			this.characterInputManager.CancleSkill();
			return;
		}
		this.characterInputManager.ReleaseCharacterSkill();
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x00014EE1 File Offset: 0x000130E1
	public void SetItemSkillAimInput(Vector2 axisInput, bool holding)
	{
		this.characterInputManager.SetAimInputUsingJoystick(axisInput);
		this.characterInputManager.SetAimType(AimTypes.handheldSkill);
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x00014EFB File Offset: 0x000130FB
	public void StartItemSkillAim()
	{
		this.characterInputManager.StartItemSkillAim();
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00014F08 File Offset: 0x00013108
	public void ItemSkillRelease(bool trigger)
	{
		if (!trigger)
		{
			this.characterInputManager.CancleSkill();
			return;
		}
		this.characterInputManager.ReleaseItemSkill();
	}

	// Token: 0x040003D7 RID: 983
	public InputManager characterInputManager;
}
