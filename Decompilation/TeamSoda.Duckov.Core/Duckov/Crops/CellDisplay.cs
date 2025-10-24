using System;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002E4 RID: 740
	public class CellDisplay : MonoBehaviour
	{
		// Token: 0x060017BC RID: 6076 RVA: 0x00056DEC File Offset: 0x00054FEC
		internal void Setup(Garden garden, int coordx, int coordy)
		{
			this.garden = garden;
			this.coord = new Vector2Int(coordx, coordy);
			bool watered = false;
			Crop crop = garden[this.coord];
			if (crop != null)
			{
				watered = crop.Watered;
			}
			this.RefreshGraphics(watered);
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x00056E33 File Offset: 0x00055033
		private void OnEnable()
		{
			Crop.onCropStatusChange += this.HandleCropEvent;
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x00056E46 File Offset: 0x00055046
		private void OnDisable()
		{
			Crop.onCropStatusChange -= this.HandleCropEvent;
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x00056E5C File Offset: 0x0005505C
		private void HandleCropEvent(Crop crop, Crop.CropEvent e)
		{
			if (crop == null)
			{
				return;
			}
			if (this.garden == null)
			{
				return;
			}
			CropData data = crop.Data;
			if (data.gardenID != this.garden.GardenID || data.coord != this.coord)
			{
				return;
			}
			this.RefreshGraphics(crop.Watered && e != Crop.CropEvent.BeforeDestroy && e != Crop.CropEvent.Harvest);
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x00056ED1 File Offset: 0x000550D1
		private void RefreshGraphics(bool watered)
		{
			if (watered)
			{
				this.ApplyGraphicsStype(this.styleWatered);
				return;
			}
			this.ApplyGraphicsStype(this.styleDry);
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x00056EF0 File Offset: 0x000550F0
		private void ApplyGraphicsStype(CellDisplay.GraphicsStyle style)
		{
			if (this.propertyBlock == null)
			{
				this.propertyBlock = new MaterialPropertyBlock();
			}
			this.propertyBlock.Clear();
			string name = "_TintColor";
			string name2 = "_Smoothness";
			this.propertyBlock.SetColor(name, style.color);
			this.propertyBlock.SetFloat(name2, style.smoothness);
			this.renderer.SetPropertyBlock(this.propertyBlock);
		}

		// Token: 0x0400114A RID: 4426
		[SerializeField]
		private Renderer renderer;

		// Token: 0x0400114B RID: 4427
		[SerializeField]
		private CellDisplay.GraphicsStyle styleDry;

		// Token: 0x0400114C RID: 4428
		[SerializeField]
		private CellDisplay.GraphicsStyle styleWatered;

		// Token: 0x0400114D RID: 4429
		private Garden garden;

		// Token: 0x0400114E RID: 4430
		private Vector2Int coord;

		// Token: 0x0400114F RID: 4431
		private MaterialPropertyBlock propertyBlock;

		// Token: 0x0200057E RID: 1406
		[Serializable]
		private struct GraphicsStyle
		{
			// Token: 0x04001FA8 RID: 8104
			public Color color;

			// Token: 0x04001FA9 RID: 8105
			public float smoothness;
		}
	}
}
