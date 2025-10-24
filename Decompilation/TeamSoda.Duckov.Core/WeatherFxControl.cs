using System;
using Duckov;
using Duckov.Scenes;
using Duckov.Weathers;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000196 RID: 406
public class WeatherFxControl : MonoBehaviour
{
	// Token: 0x06000BEC RID: 3052 RVA: 0x00032951 File Offset: 0x00030B51
	private void Start()
	{
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x00032954 File Offset: 0x00030B54
	private void Init()
	{
		this.inited = true;
		this.rainingParticleRate = new float[this.rainyFxParticles.Length];
		for (int i = 0; i < this.rainyFxParticles.Length; i++)
		{
			ParticleSystem.EmissionModule emission = this.rainyFxParticles[i].emission;
			this.rainingParticleRate[i] = emission.rateOverTime.constant;
		}
		this.SetFxActive(false);
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x000329BA File Offset: 0x00030BBA
	private void OnSubSceneChanged()
	{
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x000329BC File Offset: 0x00030BBC
	private void Update()
	{
		if (!this.inited)
		{
			if (!LevelManager.Instance)
			{
				return;
			}
			if (!LevelManager.LevelInited)
			{
				return;
			}
			this.Init();
			this.SetFxActive(false);
			return;
		}
		else
		{
			if (!TimeOfDayController.Instance)
			{
				return;
			}
			if (!MultiSceneCore.Instance)
			{
				return;
			}
			bool flag = TimeOfDayController.Instance.CurrentWeather == this.targetWeather;
			SubSceneEntry subSceneInfo = MultiSceneCore.Instance.GetSubSceneInfo();
			if (this.onlyOutDoor && subSceneInfo.IsInDoor)
			{
				flag = false;
				this.lerpValue = 0f;
			}
			if (flag)
			{
				this.overTimer = this.deactiveDelay;
				if (!this.fxActive)
				{
					this.SetFxActive(true);
				}
			}
			else if (this.lerpValue <= 0.01f)
			{
				this.overTimer -= Time.deltaTime;
				if (this.overTimer <= 0f)
				{
					this.SetFxActive(false);
				}
			}
			if (!this.fxActive)
			{
				return;
			}
			this.lerpValue = Mathf.MoveTowards(this.lerpValue, flag ? 1f : 0f, Time.deltaTime / this.lerpTime);
			for (int i = 0; i < this.rainyFxParticles.Length; i++)
			{
				ParticleSystem.EmissionModule emission = this.rainyFxParticles[i].emission;
				float b = this.rainingParticleRate[i];
				emission.rateOverTime = Mathf.Lerp(0f, b, this.lerpValue);
			}
			if (flag != this.audioPlaying)
			{
				this.audioPlaying = flag;
				if (flag)
				{
					this.weatherSoundInstace = AudioManager.Post(this.rainSoundKey, base.gameObject);
					return;
				}
				if (this.weatherSoundInstace != null)
				{
					this.weatherSoundInstace.Value.stop(STOP_MODE.ALLOWFADEOUT);
				}
			}
			return;
		}
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x00032B68 File Offset: 0x00030D68
	private void SetFxActive(bool active)
	{
		foreach (ParticleSystem particleSystem in this.rainyFxParticles)
		{
			if (!(particleSystem == null))
			{
				particleSystem.gameObject.SetActive(active);
			}
		}
		this.fxActive = active;
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x00032BAC File Offset: 0x00030DAC
	private void OnDestroy()
	{
		if (this.weatherSoundInstace != null)
		{
			this.weatherSoundInstace.Value.stop(STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x04000A5E RID: 2654
	public ParticleSystem[] rainyFxParticles;

	// Token: 0x04000A5F RID: 2655
	[HideInInspector]
	public float[] rainingParticleRate;

	// Token: 0x04000A60 RID: 2656
	public Weather targetWeather;

	// Token: 0x04000A61 RID: 2657
	private float targetParticleRate;

	// Token: 0x04000A62 RID: 2658
	private float lerpValue;

	// Token: 0x04000A63 RID: 2659
	public float lerpTime = 5f;

	// Token: 0x04000A64 RID: 2660
	public float deactiveDelay = 10f;

	// Token: 0x04000A65 RID: 2661
	private float overTimer;

	// Token: 0x04000A66 RID: 2662
	private bool fxActive;

	// Token: 0x04000A67 RID: 2663
	private bool inited;

	// Token: 0x04000A68 RID: 2664
	private EventInstance? weatherSoundInstace;

	// Token: 0x04000A69 RID: 2665
	public string rainSoundKey = "Amb/amb_rain";

	// Token: 0x04000A6A RID: 2666
	private bool audioPlaying;

	// Token: 0x04000A6B RID: 2667
	[FormerlySerializedAs("onlyInDoor")]
	public bool onlyOutDoor = true;
}
