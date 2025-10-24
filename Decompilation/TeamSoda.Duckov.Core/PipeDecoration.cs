using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000142 RID: 322
[RequireComponent(typeof(PipeRenderer))]
public class PipeDecoration : MonoBehaviour
{
	// Token: 0x06000A2E RID: 2606 RVA: 0x0002B8AC File Offset: 0x00029AAC
	private void OnDrawGizmosSelected()
	{
		if (this.pipeRenderer == null)
		{
			this.pipeRenderer = base.GetComponent<PipeRenderer>();
		}
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x0002B8C8 File Offset: 0x00029AC8
	private void Refresh()
	{
		if (this.pipeRenderer.splineInUse == null || this.pipeRenderer.splineInUse.Length < 1)
		{
			return;
		}
		for (int i = 0; i < this.decorations.Count; i++)
		{
			PipeDecoration.GameObjectOffset gameObjectOffset = this.decorations[i];
			Quaternion localRotation;
			Vector3 positionByOffset = this.pipeRenderer.GetPositionByOffset(gameObjectOffset.offset, out localRotation);
			Vector3 position = this.pipeRenderer.transform.localToWorldMatrix.MultiplyPoint(positionByOffset);
			if (!(gameObjectOffset.gameObject == null))
			{
				gameObjectOffset.gameObject.transform.position = position;
				gameObjectOffset.gameObject.transform.localRotation = localRotation;
				gameObjectOffset.gameObject.transform.Rotate(this.rotate);
				gameObjectOffset.gameObject.transform.localScale = this.scale * this.uniformScale;
			}
		}
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x0002B9B4 File Offset: 0x00029BB4
	public void OnValidate()
	{
		if (this.pipeRenderer == null)
		{
			this.pipeRenderer = base.GetComponent<PipeRenderer>();
		}
		this.Refresh();
	}

	// Token: 0x040008DD RID: 2269
	public PipeRenderer pipeRenderer;

	// Token: 0x040008DE RID: 2270
	[HideInInspector]
	public List<PipeDecoration.GameObjectOffset> decorations = new List<PipeDecoration.GameObjectOffset>();

	// Token: 0x040008DF RID: 2271
	public Vector3 rotate;

	// Token: 0x040008E0 RID: 2272
	public Vector3 scale = Vector3.one;

	// Token: 0x040008E1 RID: 2273
	public float uniformScale = 1f;

	// Token: 0x020004A3 RID: 1187
	[Serializable]
	public class GameObjectOffset
	{
		// Token: 0x04001C29 RID: 7209
		public GameObject gameObject;

		// Token: 0x04001C2A RID: 7210
		public float offset;
	}
}
