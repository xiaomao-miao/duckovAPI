using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.MiniGames
{
	// Token: 0x0200027D RID: 637
	public class VirtualCursor : MiniGameBehaviour
	{
		// Token: 0x0600143E RID: 5182 RVA: 0x0004AEC8 File Offset: 0x000490C8
		private void Awake()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = (base.transform as RectTransform);
			}
			if (this.moveArea == null)
			{
				this.moveArea = (this.rectTransform.parent as RectTransform);
			}
			if (this.canvas == null)
			{
				this.canvas = base.GetComponentInParent<Canvas>();
			}
			this.canvasRectTransform = (this.canvas.transform as RectTransform);
			if (this.raycaster == null)
			{
				this.raycaster = base.GetComponentInParent<GraphicRaycaster>();
			}
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0004AF64 File Offset: 0x00049164
		private void Update()
		{
			if (base.Game == null)
			{
				return;
			}
			if (base.Game.Console && !base.Game.Console.Interacting)
			{
				return;
			}
			Vector2 mouseDelta = UIInputManager.MouseDelta;
			Vector3 vector = this.rectTransform.localPosition + mouseDelta * this.sensitivity;
			Rect rect = this.moveArea.rect;
			vector.x = Mathf.Clamp(vector.x, rect.min.x, rect.max.x);
			vector.y = Mathf.Clamp(vector.y, rect.min.y, rect.max.y);
			this.rectTransform.localPosition = vector;
			List<RaycastResult> list = new List<RaycastResult>();
			this.Raycast(list);
			RaycastResult raycastResult = VirtualCursor.FindFirstRaycast(list);
			if (raycastResult.gameObject != VirtualCursor.raycastGO)
			{
				VirtualCursorTarget virtualCursorTarget = VirtualCursor.target;
				VirtualCursorTarget virtualCursorTarget2;
				if (raycastResult.gameObject != null)
				{
					virtualCursorTarget2 = raycastResult.gameObject.GetComponent<VirtualCursorTarget>();
				}
				else
				{
					virtualCursorTarget2 = null;
				}
				if (virtualCursorTarget2 != virtualCursorTarget)
				{
					VirtualCursor.target = virtualCursorTarget2;
					this.OnChange(virtualCursorTarget2, virtualCursorTarget);
				}
			}
			if (UIInputManager.WasClickedThisFrame && VirtualCursor.target != null)
			{
				VirtualCursor.target.OnClick();
			}
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x0004B0C4 File Offset: 0x000492C4
		private void OnChange(VirtualCursorTarget newTarget, VirtualCursorTarget oldTarget)
		{
			if (newTarget != null)
			{
				newTarget.OnCursorEnter();
			}
			if (oldTarget != null)
			{
				oldTarget.OnCursorExit();
			}
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x0004B0E4 File Offset: 0x000492E4
		private void Raycast(List<RaycastResult> resultAppendList)
		{
			if (this.canvas == null)
			{
				return;
			}
			IList<Graphic> raycastableGraphicsForCanvas = GraphicRegistry.GetRaycastableGraphicsForCanvas(this.canvas);
			VirtualCursor.s_canvasGraphics.Clear();
			if (raycastableGraphicsForCanvas == null || raycastableGraphicsForCanvas.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < raycastableGraphicsForCanvas.Count; i++)
			{
				VirtualCursor.s_canvasGraphics.Add(raycastableGraphicsForCanvas[i]);
			}
			Camera eventCamera = this.raycaster.eventCamera;
			Vector3 vector = eventCamera.WorldToScreenPoint(base.transform.position);
			vector.z = 0f;
			this.eventPositionWatch = vector;
			this.m_RaycastResults.Clear();
			VirtualCursor.Raycast(this.canvas, eventCamera, vector, raycastableGraphicsForCanvas, this.m_RaycastResults);
			int count = this.m_RaycastResults.Count;
			for (int j = 0; j < count; j++)
			{
				GameObject gameObject = this.m_RaycastResults[j].gameObject;
				float distance = 0f;
				Vector3 forward = gameObject.transform.forward;
				RaycastResult item = new RaycastResult
				{
					gameObject = gameObject,
					module = this.raycaster,
					distance = distance,
					screenPosition = vector,
					displayIndex = 0,
					index = (float)resultAppendList.Count,
					depth = this.m_RaycastResults[j].depth,
					sortingLayer = this.canvas.sortingLayerID,
					sortingOrder = this.canvas.sortingOrder,
					worldPosition = vector,
					worldNormal = -forward
				};
				resultAppendList.Add(item);
			}
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x0004B294 File Offset: 0x00049494
		private static void Raycast(Canvas canvas, Camera eventCamera, Vector2 pointerPosition, IList<Graphic> foundGraphics, List<Graphic> results)
		{
			int count = foundGraphics.Count;
			for (int i = 0; i < count; i++)
			{
				Graphic graphic = foundGraphics[i];
				if (graphic.raycastTarget && !graphic.canvasRenderer.cull && graphic.depth != -1 && RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, pointerPosition, eventCamera, graphic.raycastPadding) && (!(eventCamera != null) || eventCamera.WorldToScreenPoint(graphic.rectTransform.position).z <= eventCamera.farClipPlane) && graphic.Raycast(pointerPosition, eventCamera))
				{
					VirtualCursor.s_SortedGraphics.Add(graphic);
				}
			}
			VirtualCursor.s_SortedGraphics.Sort((Graphic g1, Graphic g2) => g2.depth.CompareTo(g1.depth));
			count = VirtualCursor.s_SortedGraphics.Count;
			for (int j = 0; j < count; j++)
			{
				results.Add(VirtualCursor.s_SortedGraphics[j]);
			}
			VirtualCursor.s_SortedGraphics.Clear();
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x0004B388 File Offset: 0x00049588
		private static RaycastResult FindFirstRaycast(List<RaycastResult> candidates)
		{
			int count = candidates.Count;
			for (int i = 0; i < count; i++)
			{
				if (!(candidates[i].gameObject == null))
				{
					return candidates[i];
				}
			}
			return default(RaycastResult);
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0004B3D0 File Offset: 0x000495D0
		internal static bool IsHovering(VirtualCursorTarget virtualCursorTarget)
		{
			return virtualCursorTarget == VirtualCursor.target;
		}

		// Token: 0x04000ED6 RID: 3798
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x04000ED7 RID: 3799
		[SerializeField]
		private RectTransform moveArea;

		// Token: 0x04000ED8 RID: 3800
		[SerializeField]
		private Canvas canvas;

		// Token: 0x04000ED9 RID: 3801
		private RectTransform canvasRectTransform;

		// Token: 0x04000EDA RID: 3802
		[SerializeField]
		private GraphicRaycaster raycaster;

		// Token: 0x04000EDB RID: 3803
		[SerializeField]
		private float sensitivity = 0.5f;

		// Token: 0x04000EDC RID: 3804
		private static GameObject raycastGO;

		// Token: 0x04000EDD RID: 3805
		private static VirtualCursorTarget target;

		// Token: 0x04000EDE RID: 3806
		[NonSerialized]
		private List<Graphic> m_RaycastResults = new List<Graphic>();

		// Token: 0x04000EDF RID: 3807
		private Vector3 eventPositionWatch;

		// Token: 0x04000EE0 RID: 3808
		[NonSerialized]
		private static List<Graphic> s_canvasGraphics = new List<Graphic>();

		// Token: 0x04000EE1 RID: 3809
		[NonSerialized]
		private static readonly List<Graphic> s_SortedGraphics = new List<Graphic>();
	}
}
