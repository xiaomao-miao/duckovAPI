using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

// Token: 0x02000184 RID: 388
public class NightVisionVisual : MonoBehaviour
{
	// Token: 0x06000B9B RID: 2971 RVA: 0x000311DC File Offset: 0x0002F3DC
	public void Awake()
	{
		this.CollectRendererData();
		this.Refresh();
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x000311EA File Offset: 0x0002F3EA
	private void OnDestroy()
	{
		this.nightVisionType = 0;
		this.Refresh();
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x000311FC File Offset: 0x0002F3FC
	private void CollectRendererData()
	{
		if (this.rendererData == null)
		{
			return;
		}
		for (int i = 0; i < this.rendererData.rendererFeatures.Count; i++)
		{
			if (this.rendererData.rendererFeatures[i].name == this.thermalCharacterRednerFeatureKey)
			{
				this.thermalCharacterRednerFeature = this.rendererData.rendererFeatures[i];
			}
			else if (this.rendererData.rendererFeatures[i].name == this.thermalBackgroundRednerFeatureKey)
			{
				this.thermalBackgroundRednerFeature = this.rendererData.rendererFeatures[i];
			}
		}
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x000312AC File Offset: 0x0002F4AC
	private void Update()
	{
		bool flag = false;
		int num = this.CheckNightVisionType();
		if (num >= this.nightVisionTypes.Length)
		{
			num = 1;
		}
		if (this.nightVisionType != num)
		{
			this.nightVisionType = num;
			flag = true;
		}
		if (LevelManager.LevelInited != this.levelInited)
		{
			this.levelInited = LevelManager.LevelInited;
			flag = true;
		}
		if (flag)
		{
			this.Refresh();
		}
		if (this.character && this.nightVisionLight.gameObject.activeInHierarchy)
		{
			this.nightVisionLight.transform.position = this.character.transform.position + Vector3.up * 2f;
		}
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x00031357 File Offset: 0x0002F557
	private int CheckNightVisionType()
	{
		if (!this.character)
		{
			if (LevelManager.LevelInited)
			{
				this.character = CharacterMainControl.Main;
			}
			return 0;
		}
		return Mathf.RoundToInt(this.character.NightVisionType);
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x0003138C File Offset: 0x0002F58C
	public void Refresh()
	{
		bool flag = this.nightVisionType > 0;
		this.thermalVolume.gameObject.SetActive(flag);
		this.nightVisionLight.gameObject.SetActive(flag);
		NightVisionVisual.NightVisionType nightVisionType = this.nightVisionTypes[this.nightVisionType];
		bool flag2 = nightVisionType.thermalOn && flag;
		bool active = nightVisionType.thermalBackground && flag;
		this.thermalVolume.profile = nightVisionType.profile;
		this.thermalCharacterRednerFeature.SetActive(flag2);
		this.thermalBackgroundRednerFeature.SetActive(active);
		Shader.SetGlobalFloat("ThermalOn", flag2 ? 1f : 0f);
		if (LevelManager.LevelInited)
		{
			if (flag2)
			{
				LevelManager.Instance.FogOfWarManager.mainVis.ObstacleMask = GameplayDataSettings.Layers.fowBlockLayersWithThermal;
				return;
			}
			LevelManager.Instance.FogOfWarManager.mainVis.ObstacleMask = GameplayDataSettings.Layers.fowBlockLayers;
		}
	}

	// Token: 0x040009E6 RID: 2534
	private int nightVisionType;

	// Token: 0x040009E7 RID: 2535
	public Volume thermalVolume;

	// Token: 0x040009E8 RID: 2536
	public NightVisionVisual.NightVisionType[] nightVisionTypes;

	// Token: 0x040009E9 RID: 2537
	private CharacterMainControl character;

	// Token: 0x040009EA RID: 2538
	public ScriptableRendererData rendererData;

	// Token: 0x040009EB RID: 2539
	public List<string> renderFeatureNames;

	// Token: 0x040009EC RID: 2540
	private ScriptableRendererFeature thermalCharacterRednerFeature;

	// Token: 0x040009ED RID: 2541
	private ScriptableRendererFeature thermalBackgroundRednerFeature;

	// Token: 0x040009EE RID: 2542
	public Transform nightVisionLight;

	// Token: 0x040009EF RID: 2543
	public string thermalCharacterRednerFeatureKey = "ThermalCharacter";

	// Token: 0x040009F0 RID: 2544
	public string thermalBackgroundRednerFeatureKey = "ThermalBackground";

	// Token: 0x040009F1 RID: 2545
	private bool levelInited;

	// Token: 0x020004B9 RID: 1209
	[Serializable]
	public struct NightVisionType
	{
		// Token: 0x04001C84 RID: 7300
		public string intro;

		// Token: 0x04001C85 RID: 7301
		public VolumeProfile profile;

		// Token: 0x04001C86 RID: 7302
		[FormerlySerializedAs("thermalCharacter")]
		public bool thermalOn;

		// Token: 0x04001C87 RID: 7303
		public bool thermalBackground;
	}
}
