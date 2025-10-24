using System;
using Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020000A7 RID: 167
[Obsolete]
public class InvisibleTeleporter : MonoBehaviour, IDrawGizmos
{
	// Token: 0x1700011F RID: 287
	// (get) Token: 0x060005AB RID: 1451 RVA: 0x00019521 File Offset: 0x00017721
	private bool UsePosition
	{
		get
		{
			return this.target == null;
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x060005AC RID: 1452 RVA: 0x00019530 File Offset: 0x00017730
	private Vector3 TargetWorldPosition
	{
		get
		{
			if (this.target != null)
			{
				return this.target.transform.position;
			}
			Space space = this.space;
			if (space == Space.World)
			{
				return this.position;
			}
			if (space != Space.Self)
			{
				return default(Vector3);
			}
			return base.transform.TransformPoint(this.position);
		}
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x00019590 File Offset: 0x00017790
	public void Teleport()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		GameCamera instance = GameCamera.Instance;
		Vector3 b = instance.transform.position - main.transform.position;
		main.SetPosition(this.TargetWorldPosition);
		Vector3 vector = main.transform.position + b;
		instance.transform.position = vector;
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x000195F7 File Offset: 0x000177F7
	private void LateUpdate()
	{
		if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
		{
			this.Teleport();
		}
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x00019618 File Offset: 0x00017818
	public void DrawGizmos()
	{
		if (!GizmoContext.InActiveSelection(this))
		{
			return;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			Draw.Arrow(base.transform.position, this.TargetWorldPosition);
			return;
		}
		Draw.Arrow(main.transform.position, this.TargetWorldPosition);
	}

	// Token: 0x04000526 RID: 1318
	[SerializeField]
	private Transform target;

	// Token: 0x04000527 RID: 1319
	[SerializeField]
	private Vector3 position;

	// Token: 0x04000528 RID: 1320
	[SerializeField]
	private Space space;
}
