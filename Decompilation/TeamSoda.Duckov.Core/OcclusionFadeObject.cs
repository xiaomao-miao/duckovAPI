using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000188 RID: 392
public class OcclusionFadeObject : MonoBehaviour
{
	// Token: 0x06000BAD RID: 2989 RVA: 0x00031893 File Offset: 0x0002FA93
	private void Collect()
	{
		this.CollectTriggers();
		this.CollectRenderers();
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x000318A4 File Offset: 0x0002FAA4
	private void CollectTriggers()
	{
		this.triggers = new OcclusionFadeTrigger[0];
		this.triggers = base.GetComponentsInChildren<OcclusionFadeTrigger>();
		if (this.triggers.Length != 0)
		{
			foreach (OcclusionFadeTrigger occlusionFadeTrigger in this.triggers)
			{
				occlusionFadeTrigger.parent = this;
				Collider[] componentsInChildren = occlusionFadeTrigger.GetComponentsInChildren<Collider>(true);
				if (componentsInChildren.Length != 0)
				{
					Collider[] array2 = componentsInChildren;
					for (int j = 0; j < array2.Length; j++)
					{
						array2[j].isTrigger = true;
					}
				}
			}
		}
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x0003191C File Offset: 0x0002FB1C
	private void CollectRenderers()
	{
		this.topTransform = this.FindFirst(base.transform, this.topName);
		if (this.topTransform == null)
		{
			return;
		}
		this.renderers = this.topTransform.GetComponentsInChildren<Renderer>(true);
		this.originMaterials.Clear();
		foreach (Renderer renderer in this.renderers)
		{
			this.originMaterials.AddRange(renderer.sharedMaterials);
		}
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x00031997 File Offset: 0x0002FB97
	public void OnEnter()
	{
		this.enterCounter++;
		this.Refresh();
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x000319AD File Offset: 0x0002FBAD
	public void OnLeave()
	{
		this.enterCounter--;
		this.Refresh();
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x000319C4 File Offset: 0x0002FBC4
	private void Refresh()
	{
		this.SyncEnable();
		if (!this.triggerEnabled)
		{
			this.hiding = false;
			this.Sync();
			return;
		}
		if (this.enterCounter > 0 && !this.hiding)
		{
			this.hiding = true;
			this.Sync();
			return;
		}
		if (this.enterCounter <= 0 && this.hiding)
		{
			this.hiding = false;
			this.Sync();
		}
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x00031A2A File Offset: 0x0002FC2A
	private void OnEnable()
	{
		this.SyncEnable();
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x00031A32 File Offset: 0x0002FC32
	private void OnDisable()
	{
		this.SyncEnable();
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x00031A3C File Offset: 0x0002FC3C
	private void SyncEnable()
	{
		if (this.triggerEnabled != base.enabled)
		{
			OcclusionFadeTrigger[] array = this.triggers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(base.enabled);
			}
			this.triggerEnabled = base.enabled;
		}
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00031A8C File Offset: 0x0002FC8C
	private void Sync()
	{
		this.SyncEnable();
		OcclusionFadeTypes occlusionFadeTypes = this.fadeType;
		if (occlusionFadeTypes != OcclusionFadeTypes.Fade)
		{
			if (occlusionFadeTypes != OcclusionFadeTypes.ShadowOnly)
			{
				return;
			}
			if (this.hiding)
			{
				foreach (Renderer renderer in this.renderers)
				{
					if (!(renderer == null))
					{
						renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
					}
				}
				return;
			}
			foreach (Renderer renderer2 in this.renderers)
			{
				if (!(renderer2 == null))
				{
					renderer2.shadowCastingMode = ShadowCastingMode.On;
				}
			}
			return;
		}
		else
		{
			if (this.tempMaterials == null)
			{
				this.tempMaterials = new List<Material>();
			}
			if (this.hiding)
			{
				int num = 0;
				foreach (Renderer renderer3 in this.renderers)
				{
					if (!(renderer3 == null))
					{
						this.tempMaterials.Clear();
						for (int j = 0; j < renderer3.materials.Length; j++)
						{
							Material mat = this.originMaterials[num];
							Material maskedMaterial = OcclusionFadeManager.Instance.GetMaskedMaterial(mat);
							this.tempMaterials.Add(maskedMaterial);
							num++;
						}
						renderer3.SetSharedMaterials(this.tempMaterials);
					}
				}
				return;
			}
			int num2 = 0;
			foreach (Renderer renderer4 in this.renderers)
			{
				if (!(renderer4 == null))
				{
					this.tempMaterials.Clear();
					for (int k = 0; k < renderer4.materials.Length; k++)
					{
						this.tempMaterials.Add(this.originMaterials[num2]);
						num2++;
					}
					renderer4.SetSharedMaterials(this.tempMaterials);
				}
			}
			return;
		}
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x00031C2C File Offset: 0x0002FE2C
	private void Hide()
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			Renderer renderer = this.renderers[i];
			if (renderer != null)
			{
				renderer.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x00031C68 File Offset: 0x0002FE68
	private void Show()
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			Renderer renderer = this.renderers[i];
			if (renderer != null)
			{
				renderer.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x00031CA4 File Offset: 0x0002FEA4
	private Transform FindFirst(Transform root, string checkName)
	{
		for (int i = 0; i < root.childCount; i++)
		{
			Transform child = root.GetChild(i);
			if (child.name == checkName)
			{
				return child;
			}
			if (child.childCount > 0)
			{
				Transform transform = this.FindFirst(child, checkName);
				if (transform != null)
				{
					return transform;
				}
			}
		}
		return null;
	}

	// Token: 0x04000A0A RID: 2570
	public OcclusionFadeTypes fadeType;

	// Token: 0x04000A0B RID: 2571
	public string topName = "Fade";

	// Token: 0x04000A0C RID: 2572
	public OcclusionFadeTrigger[] triggers;

	// Token: 0x04000A0D RID: 2573
	public Renderer[] renderers;

	// Token: 0x04000A0E RID: 2574
	public List<Material> originMaterials;

	// Token: 0x04000A0F RID: 2575
	private List<Material> tempMaterials;

	// Token: 0x04000A10 RID: 2576
	private Transform topTransform;

	// Token: 0x04000A11 RID: 2577
	private int enterCounter;

	// Token: 0x04000A12 RID: 2578
	private bool hiding;

	// Token: 0x04000A13 RID: 2579
	private bool triggerEnabled = true;
}
