using System;
using UnityEngine;

// Token: 0x02000140 RID: 320
public abstract class ShapeProvider : MonoBehaviour
{
	// Token: 0x06000A2B RID: 2603
	public abstract PipeRenderer.OrientedPoint[] GenerateShape();
}
