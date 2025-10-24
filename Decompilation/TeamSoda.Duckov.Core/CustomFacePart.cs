using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class CustomFacePart : MonoBehaviour
{
	// Token: 0x17000055 RID: 85
	// (get) Token: 0x06000111 RID: 273 RVA: 0x000054E0 File Offset: 0x000036E0
	public CustomFaceInstance Parent
	{
		get
		{
			return this.parent;
		}
	}

	// Token: 0x06000112 RID: 274 RVA: 0x000054E8 File Offset: 0x000036E8
	private void CollectRenders()
	{
		this.renderers.Clear();
		MeshRenderer[] componentsInChildren = base.transform.GetComponentsInChildren<MeshRenderer>();
		SkinnedMeshRenderer[] componentsInChildren2 = base.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
		this.renderers.AddRange(componentsInChildren);
		this.renderers.AddRange(componentsInChildren2);
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00005530 File Offset: 0x00003730
	public void SetInfo(CustomFacePartInfo info, CustomFaceInstance _parent)
	{
		this.parent = _parent;
		if (this.mirror)
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else
		{
			base.transform.localScale = Vector3.one;
		}
		Quaternion rotation = Quaternion.Euler(0f, -info.leftRightAngle, 0f);
		Quaternion rotation2 = Quaternion.Euler(0f, -info.leftRightAngle - info.distanceAngle, 0f);
		Quaternion rotation3 = Quaternion.Euler(0f, -info.leftRightAngle + info.distanceAngle, 0f);
		if (this.centerObject)
		{
			Vector3 vector = rotation * Vector3.forward;
			this.centerObject.localPosition = vector * info.radius + Vector3.up * (info.height + info.heightOffset);
			this.centerObject.localRotation = Quaternion.LookRotation(vector);
			this.centerObject.localRotation = Quaternion.Euler(0f, 0f, info.twist) * this.centerObject.localRotation;
			this.centerObject.localScale = Vector3.one * info.scale;
		}
		if (this.leftObject)
		{
			Vector3 vector2 = rotation2 * Vector3.forward;
			this.leftObject.localPosition = vector2 * info.radius + Vector3.up * (info.height + info.heightOffset);
			this.leftObject.localRotation = Quaternion.LookRotation(vector2, Vector3.up);
			this.leftObject.localRotation = Quaternion.AngleAxis(info.twist, vector2) * this.leftObject.localRotation;
			this.leftObject.localScale = Vector3.one * info.scale;
		}
		if (this.rightObject)
		{
			Vector3 vector3 = rotation3 * Vector3.forward;
			this.rightObject.localPosition = vector3 * info.radius + Vector3.up * (info.height + info.heightOffset);
			this.rightObject.localRotation = Quaternion.LookRotation(vector3, Vector3.up);
			this.rightObject.localRotation = Quaternion.AngleAxis(-info.twist, vector3) * this.rightObject.localRotation;
			this.rightObject.localScale = Vector3.one * info.scale;
		}
		if (this.block == null)
		{
			this.block = new MaterialPropertyBlock();
		}
		info.color.a = 1f;
		this.block.SetColor(this.customColorKey, info.color);
		foreach (Renderer renderer in this.customColorRenderers)
		{
			if (renderer)
			{
				renderer.SetPropertyBlock(this.block);
			}
		}
	}

	// Token: 0x04000097 RID: 151
	public int id;

	// Token: 0x04000098 RID: 152
	public bool mirror;

	// Token: 0x04000099 RID: 153
	public Transform centerObject;

	// Token: 0x0400009A RID: 154
	public Transform leftObject;

	// Token: 0x0400009B RID: 155
	public Transform rightObject;

	// Token: 0x0400009C RID: 156
	public List<Renderer> customColorRenderers;

	// Token: 0x0400009D RID: 157
	public List<Renderer> renderers;

	// Token: 0x0400009E RID: 158
	public string customColorKey = "_Tint";

	// Token: 0x0400009F RID: 159
	private CustomFaceInstance parent;

	// Token: 0x040000A0 RID: 160
	private MaterialPropertyBlock block;
}
