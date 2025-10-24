using System;
using FOW;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200017B RID: 379
public class FogOfWarManager : MonoBehaviour
{
	// Token: 0x06000B7B RID: 2939 RVA: 0x00030805 File Offset: 0x0002EA05
	private void Start()
	{
		LevelManager.OnMainCharacterDead += this.OnCharacterDie;
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x00030818 File Offset: 0x0002EA18
	private void OnDestroy()
	{
		LevelManager.OnMainCharacterDead -= this.OnCharacterDie;
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x0003082B File Offset: 0x0002EA2B
	private void Init()
	{
		this.inited = true;
		if (!LevelManager.Instance.IsRaidMap || !LevelManager.Rule.FogOfWar)
		{
			this.allVision = true;
		}
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x00030854 File Offset: 0x0002EA54
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (!this.character)
		{
			this.character = CharacterMainControl.Main;
			if (!this.character)
			{
				return;
			}
		}
		if (!this.inited)
		{
			this.Init();
		}
		if (!this.timeOfDayController)
		{
			this.timeOfDayController = LevelManager.Instance.TimeOfDayController;
			if (!this.timeOfDayController)
			{
				return;
			}
		}
		Vector3 vector = this.character.transform.position + Vector3.up * this.mianVisYOffset;
		this.mainVis.transform.position = vector;
		vector = new Vector3((float)Mathf.RoundToInt(vector.x), (float)Mathf.RoundToInt(vector.y), (float)Mathf.RoundToInt(vector.z));
		this.fogOfWar.UpdateWorldBounds(vector, new Vector3(128f, 1f, 128f));
		Vector3 forward = this.character.GetCurrentAimPoint() - this.character.transform.position;
		Debug.DrawLine(this.character.GetCurrentAimPoint(), this.character.GetCurrentAimPoint() + Vector3.up * 2f, Color.green, 0.2f);
		forward.y = 0f;
		forward.Normalize();
		float t = Mathf.Clamp01(this.character.NightVisionAbility + (this.character.FlashLight ? 0.3f : 0f));
		float num = this.character.ViewAngle;
		float num2 = this.character.SenseRange;
		float num3 = this.character.ViewDistance;
		num *= Mathf.Lerp(TimeOfDayController.NightViewAngleFactor, 1f, t);
		num2 *= Mathf.Lerp(TimeOfDayController.NightSenseRangeFactor, 1f, t);
		num3 *= Mathf.Lerp(TimeOfDayController.NightViewDistanceFactor, 1f, t);
		if (num3 < num2 - 2.5f)
		{
			num3 = num2 - 2.5f;
		}
		if (this.allVision)
		{
			num = 360f;
			num2 = 50f;
			num3 = 50f;
		}
		if (num != this.viewAgnel)
		{
			if (this.viewAgnel < 0f)
			{
				this.viewAgnel = num;
			}
			this.viewAgnel = Mathf.MoveTowards(this.viewAgnel, num, 120f * Time.deltaTime);
			this.mainVis.ViewAngle = this.viewAgnel;
		}
		if (num2 != this.senseRange)
		{
			if (this.senseRange < 0f)
			{
				this.senseRange = num2;
			}
			this.senseRange = Mathf.MoveTowards(this.senseRange, num2, 2f * Time.deltaTime);
			this.mainVis.UnobscuredRadius = this.senseRange;
		}
		if (num3 != this.viewDistance)
		{
			if (this.viewDistance < 0f)
			{
				this.viewDistance = num3;
			}
			this.viewDistance = Mathf.MoveTowards(this.viewDistance, num3, 30f * Time.deltaTime);
			this.mainVis.ViewRadius = this.viewDistance;
		}
		this.mainVis.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x00030B79 File Offset: 0x0002ED79
	private void OnCharacterDie(DamageInfo dmgInfo)
	{
		LevelManager.OnMainCharacterDead -= this.OnCharacterDie;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040009C4 RID: 2500
	[FormerlySerializedAs("mianVis")]
	public FogOfWarRevealer3D mainVis;

	// Token: 0x040009C5 RID: 2501
	public float mianVisYOffset = 1f;

	// Token: 0x040009C6 RID: 2502
	private CharacterMainControl character;

	// Token: 0x040009C7 RID: 2503
	public FogOfWarWorld fogOfWar;

	// Token: 0x040009C8 RID: 2504
	private float viewAgnel = -1f;

	// Token: 0x040009C9 RID: 2505
	private float senseRange = -1f;

	// Token: 0x040009CA RID: 2506
	private float viewDistance = -1f;

	// Token: 0x040009CB RID: 2507
	private TimeOfDayController timeOfDayController;

	// Token: 0x040009CC RID: 2508
	private bool allVision;

	// Token: 0x040009CD RID: 2509
	private bool inited;
}
