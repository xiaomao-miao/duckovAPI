using System;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class CharacterSoundMaker : MonoBehaviour
{
	// Token: 0x170000CC RID: 204
	// (get) Token: 0x0600038C RID: 908 RVA: 0x0000F729 File Offset: 0x0000D929
	public float walkSoundDistance
	{
		get
		{
			if (!this.characterMainControl)
			{
				return 0f;
			}
			return this.characterMainControl.WalkSoundRange;
		}
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x0600038D RID: 909 RVA: 0x0000F749 File Offset: 0x0000D949
	public float runSoundDistance
	{
		get
		{
			if (!this.characterMainControl)
			{
				return 0f;
			}
			return this.characterMainControl.RunSoundRange;
		}
	}

	// Token: 0x0600038E RID: 910 RVA: 0x0000F76C File Offset: 0x0000D96C
	private void Update()
	{
		if (this.characterMainControl.movementControl.Velocity.magnitude < 0.5f)
		{
			this.moveSoundTimer = 0f;
			return;
		}
		this.moveSoundTimer += Time.deltaTime;
		bool running = this.characterMainControl.Running;
		float num = 1f / (running ? this.runSoundFrequence : this.walkSoundFrequence);
		if (this.moveSoundTimer >= num)
		{
			this.moveSoundTimer = 0f;
			if (this.characterMainControl.IsInAdsInput)
			{
				return;
			}
			if (!this.characterMainControl.CharacterItem)
			{
				return;
			}
			bool flag = this.characterMainControl.CharacterItem.TotalWeight / this.characterMainControl.MaxWeight >= 0.75f;
			AISound sound = default(AISound);
			sound.pos = base.transform.position;
			sound.fromTeam = this.characterMainControl.Team;
			sound.soundType = SoundTypes.unknowNoise;
			sound.fromObject = this.characterMainControl.gameObject;
			sound.fromCharacter = this.characterMainControl;
			if (this.characterMainControl.Running)
			{
				if (this.runSoundDistance > 0f)
				{
					sound.radius = this.runSoundDistance * (flag ? 1.5f : 1f);
					Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl> onFootStepSound = CharacterSoundMaker.OnFootStepSound;
					if (onFootStepSound != null)
					{
						onFootStepSound(base.transform.position, flag ? CharacterSoundMaker.FootStepTypes.runHeavy : CharacterSoundMaker.FootStepTypes.runLight, this.characterMainControl);
					}
				}
			}
			else if (this.walkSoundDistance > 0f)
			{
				sound.radius = this.walkSoundDistance * (flag ? 1.5f : 1f);
				Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl> onFootStepSound2 = CharacterSoundMaker.OnFootStepSound;
				if (onFootStepSound2 != null)
				{
					onFootStepSound2(base.transform.position, flag ? CharacterSoundMaker.FootStepTypes.walkHeavy : CharacterSoundMaker.FootStepTypes.walkLight, this.characterMainControl);
				}
			}
			AIMainBrain.MakeSound(sound);
		}
	}

	// Token: 0x040002A7 RID: 679
	public CharacterMainControl characterMainControl;

	// Token: 0x040002A8 RID: 680
	private float moveSoundTimer;

	// Token: 0x040002A9 RID: 681
	public float walkSoundFrequence = 4f;

	// Token: 0x040002AA RID: 682
	public float runSoundFrequence = 7f;

	// Token: 0x040002AB RID: 683
	public static Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl> OnFootStepSound;

	// Token: 0x02000432 RID: 1074
	public enum FootStepTypes
	{
		// Token: 0x04001A20 RID: 6688
		walkLight,
		// Token: 0x04001A21 RID: 6689
		walkHeavy,
		// Token: 0x04001A22 RID: 6690
		runLight,
		// Token: 0x04001A23 RID: 6691
		runHeavy
	}
}
