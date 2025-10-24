using System;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class SkillProjectileLineHUD : MonoBehaviour
{
	// Token: 0x06000655 RID: 1621 RVA: 0x0001C95B File Offset: 0x0001AB5B
	private void Awake()
	{
		this.obsticleLayers = (GameplayDataSettings.Layers.wallLayerMask | GameplayDataSettings.Layers.groundLayerMask | GameplayDataSettings.Layers.fowBlockLayers);
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x0001C998 File Offset: 0x0001AB98
	public bool UpdateLine(Vector3 start, Vector3 target, float verticleSpeed, ref Vector3 hitPoint)
	{
		float magnitude = Physics.gravity.magnitude;
		if (this.line.points.Length != this.fragmentCount + 1)
		{
			this.line.points = new Vector3[this.fragmentCount + 1];
			this.line.colors = new Color[this.fragmentCount + 1];
		}
		float num = verticleSpeed / magnitude;
		float num2 = Mathf.Sqrt(2f * (num * verticleSpeed * 0.5f + start.y - target.y) / magnitude);
		float num3 = num + num2;
		Vector3 vector = start;
		vector.y = 0f;
		Vector3 vector2 = target;
		vector2.y = 0f;
		float num4 = Vector3.Distance(vector, vector2);
		float d = 0f;
		Vector3 a = vector2 - vector;
		if (a.magnitude > 0f)
		{
			a = a.normalized;
			d = num4 / num3;
		}
		else
		{
			a = Vector3.zero;
		}
		float num5 = num3 / (float)this.fragmentCount;
		bool flag = false;
		for (int i = 0; i < this.fragmentCount + 1; i++)
		{
			float num6 = num5 * (float)i;
			this.line.points[i] = start + Vector3.up * (verticleSpeed - magnitude * num6 * 0.5f) * num6 + a * d * num6;
			Vector3 vector3 = this.line.points[i];
			if (i > 0 && i < this.line.points.Length - 1 && !flag)
			{
				Vector3 vector4 = this.line.points[i - 1];
				flag = this.CheckObsticle(vector4, vector3, ref hitPoint);
				hitPoint = vector4 + (vector3 - vector4).normalized * (hitPoint - vector4).magnitude;
			}
			if (flag)
			{
				this.line.colors[i] = this.obsticleColor;
			}
			else
			{
				this.line.colors[i] = this.lineColor;
			}
		}
		this.line.hitObsticle = flag;
		if (flag)
		{
			this.line.hitPoint = hitPoint;
		}
		this.line.DrawLine();
		return flag;
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x0001CBFC File Offset: 0x0001ADFC
	private bool CheckObsticle(Vector3 from, Vector3 to, ref Vector3 hitPoint)
	{
		if (this.hits == null)
		{
			this.hits = new RaycastHit[3];
		}
		if (Physics.SphereCastNonAlloc(from, 0.2f, (to - from).normalized, this.hits, (to - from).magnitude, this.obsticleLayers) > 0)
		{
			hitPoint = this.hits[0].point;
			return true;
		}
		return false;
	}

	// Token: 0x0400061C RID: 1564
	public ShapesSkillLine line;

	// Token: 0x0400061D RID: 1565
	public int fragmentCount = 20;

	// Token: 0x0400061E RID: 1566
	[ColorUsage(true, true)]
	public Color lineColor;

	// Token: 0x0400061F RID: 1567
	[ColorUsage(true, true)]
	public Color obsticleColor;

	// Token: 0x04000620 RID: 1568
	private LayerMask obsticleLayers;

	// Token: 0x04000621 RID: 1569
	private RaycastHit[] hits;
}
