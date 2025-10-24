using System;
using Shapes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x020000CA RID: 202
[ExecuteAlways]
public class ShapesSkillLine : MonoBehaviour
{
	// Token: 0x0600064E RID: 1614 RVA: 0x0001C5EB File Offset: 0x0001A7EB
	private void Awake()
	{
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x0001C5F0 File Offset: 0x0001A7F0
	public void DrawLine()
	{
		if (!this.cam)
		{
			if (LevelManager.Instance)
			{
				this.cam = LevelManager.Instance.GameCamera.renderCamera;
			}
			if (!this.cam)
			{
				return;
			}
		}
		if (this.points.Length == 0)
		{
			return;
		}
		using (Draw.Command(this.cam, RenderPassEvent.BeforeRenderingPostProcessing))
		{
			Draw.LineGeometry = LineGeometry.Billboard;
			Draw.BlendMode = this.blendMode;
			Draw.ThicknessSpace = ThicknessSpace.Meters;
			Draw.Thickness = this.lineThickness;
			Draw.ZTest = CompareFunction.Always;
			if (!this.worldSpace)
			{
				Draw.Matrix = base.transform.localToWorldMatrix;
			}
			for (int i = 0; i < this.points.Length - 1; i++)
			{
				Draw.Sphere(this.points[i], this.dotRadius, this.colors[i]);
				Draw.Line(this.points[i], this.points[i + 1], this.colors[i]);
			}
			Draw.Sphere(this.points[this.points.Length - 1], this.dotRadius, this.colors[this.colors.Length - 1]);
			if (this.hitObsticle)
			{
				Draw.Sphere(this.hitPoint, this.dotRadius, this.colors[0]);
			}
		}
	}

	// Token: 0x0400060E RID: 1550
	public Vector3[] points;

	// Token: 0x0400060F RID: 1551
	public Color[] colors;

	// Token: 0x04000610 RID: 1552
	public Vector3 hitPoint;

	// Token: 0x04000611 RID: 1553
	public bool hitObsticle;

	// Token: 0x04000612 RID: 1554
	public ShapesBlendMode blendMode;

	// Token: 0x04000613 RID: 1555
	public bool worldSpace;

	// Token: 0x04000614 RID: 1556
	public float dotRadius = 0.02f;

	// Token: 0x04000615 RID: 1557
	public float lineThickness = 0.02f;

	// Token: 0x04000616 RID: 1558
	private Camera cam;
}
