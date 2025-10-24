using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.Buildings.UI
{
	// Token: 0x0200031E RID: 798
	public class GridDisplay : MonoBehaviour
	{
		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001A8C RID: 6796 RVA: 0x0005FFBC File Offset: 0x0005E1BC
		// (set) Token: 0x06001A8D RID: 6797 RVA: 0x0005FFC3 File Offset: 0x0005E1C3
		public static GridDisplay Instance { get; private set; }

		// Token: 0x06001A8E RID: 6798 RVA: 0x0005FFCB File Offset: 0x0005E1CB
		private void Awake()
		{
			GridDisplay.Instance = this;
			GridDisplay.Close();
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x0005FFD8 File Offset: 0x0005E1D8
		public void Setup(BuildingArea buildingArea)
		{
			Vector2Int lowerLeftCorner = buildingArea.LowerLeftCorner;
			Vector4 value = new Vector4((float)lowerLeftCorner.x, (float)lowerLeftCorner.y, (float)(buildingArea.Size.x * 2 - 1), (float)(buildingArea.Size.y * 2 - 1));
			Shader.SetGlobalVector("BuildingGrid_AreaPosAndSize", value);
			GridDisplay.ShowGrid();
			GridDisplay.HidePreview();
			GridDisplay.ShowGrid();
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x00060043 File Offset: 0x0005E243
		public static void Close()
		{
			GridDisplay.HidePreview();
			GridDisplay.HideGrid();
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x00060050 File Offset: 0x0005E250
		public static UniTask SetGridShowHide(bool show, AnimationCurve curve, float duration)
		{
			GridDisplay.<SetGridShowHide>d__12 <SetGridShowHide>d__;
			<SetGridShowHide>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SetGridShowHide>d__.show = show;
			<SetGridShowHide>d__.curve = curve;
			<SetGridShowHide>d__.duration = duration;
			<SetGridShowHide>d__.<>1__state = -1;
			<SetGridShowHide>d__.<>t__builder.Start<GridDisplay.<SetGridShowHide>d__12>(ref <SetGridShowHide>d__);
			return <SetGridShowHide>d__.<>t__builder.Task;
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x000600A3 File Offset: 0x0005E2A3
		public static void HideGrid()
		{
			if (GridDisplay.Instance)
			{
				GridDisplay.SetGridShowHide(false, GridDisplay.Instance.hideCurve, GridDisplay.Instance.animationDuration).Forget();
				return;
			}
			Shader.SetGlobalFloat("BuildingGrid_Building", 0f);
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x000600E0 File Offset: 0x0005E2E0
		public static void ShowGrid()
		{
			if (GridDisplay.Instance)
			{
				GridDisplay.SetGridShowHide(true, GridDisplay.Instance.showCurve, GridDisplay.Instance.animationDuration).Forget();
				return;
			}
			Shader.SetGlobalFloat("BuildingGrid_Building", 1f);
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x0006011D File Offset: 0x0005E31D
		public static void HidePreview()
		{
			Shader.SetGlobalVector("BuildingGrid_BuildingPosAndSize", Vector4.zero);
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x00060130 File Offset: 0x0005E330
		internal void SetBuildingPreviewCoord(Vector2Int coord, Vector2Int dimensions, BuildingRotation rotation, bool validPlacement)
		{
			if (rotation % BuildingRotation.Half > BuildingRotation.Zero)
			{
				dimensions = new Vector2Int(dimensions.y, dimensions.x);
			}
			Vector4 value = new Vector4((float)coord.x, (float)coord.y, (float)dimensions.x, (float)dimensions.y);
			Shader.SetGlobalVector("BuildingGrid_BuildingPosAndSize", value);
			Shader.SetGlobalFloat("BuildingGrid_CanBuild", (float)(validPlacement ? 1 : 0));
		}

		// Token: 0x04001305 RID: 4869
		[HideInInspector]
		[SerializeField]
		private BuildingArea targetArea;

		// Token: 0x04001306 RID: 4870
		[SerializeField]
		private float animationDuration;

		// Token: 0x04001307 RID: 4871
		[SerializeField]
		private AnimationCurve showCurve;

		// Token: 0x04001308 RID: 4872
		[SerializeField]
		private AnimationCurve hideCurve;

		// Token: 0x04001309 RID: 4873
		private static int gridShowHideTaskToken;
	}
}
