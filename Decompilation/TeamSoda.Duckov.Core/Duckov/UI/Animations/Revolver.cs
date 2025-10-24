using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003DE RID: 990
	public class Revolver : MonoBehaviour
	{
		// Token: 0x060023EC RID: 9196 RVA: 0x0007D3E4 File Offset: 0x0007B5E4
		private void Update()
		{
			Quaternion rotation = Quaternion.AngleAxis(Time.deltaTime * this.rPM / 60f * 360f, this.axis);
			Vector3 point = base.transform.localPosition - this.pivot;
			Vector3 b = rotation * point;
			Vector3 localPosition = this.pivot + b;
			base.transform.localPosition = localPosition;
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x0007D44C File Offset: 0x0007B64C
		private void OnDrawGizmosSelected()
		{
			if (base.transform.parent != null)
			{
				Gizmos.matrix = base.transform.parent.localToWorldMatrix;
			}
			Gizmos.DrawLine(this.pivot, base.transform.localPosition);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.pivot, 1f);
		}

		// Token: 0x0400185E RID: 6238
		public Vector3 pivot;

		// Token: 0x0400185F RID: 6239
		public Vector3 axis = Vector3.forward;

		// Token: 0x04001860 RID: 6240
		public float rPM;
	}
}
