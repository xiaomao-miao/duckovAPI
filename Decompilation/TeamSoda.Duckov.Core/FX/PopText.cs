using System;
using System.Collections.Generic;
using UnityEngine;

namespace FX
{
	// Token: 0x0200020E RID: 526
	public class PopText : MonoBehaviour
	{
		// Token: 0x06000F9C RID: 3996 RVA: 0x0003D4F1 File Offset: 0x0003B6F1
		private void Awake()
		{
			PopText.instance = this;
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x0003D4FC File Offset: 0x0003B6FC
		private PopTextEntity GetOrCreateEntry()
		{
			PopTextEntity popTextEntity;
			if (this.inactiveEntries.Count > 0)
			{
				popTextEntity = this.inactiveEntries[0];
				this.inactiveEntries.RemoveAt(0);
			}
			popTextEntity = UnityEngine.Object.Instantiate<PopTextEntity>(this.popTextPrefab, base.transform);
			this.activeEntries.Add(popTextEntity);
			popTextEntity.gameObject.SetActive(true);
			return popTextEntity;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0003D55C File Offset: 0x0003B75C
		public void InstancePop(string text, Vector3 worldPosition, Color color, float size, Sprite sprite = null)
		{
			PopTextEntity orCreateEntry = this.GetOrCreateEntry();
			orCreateEntry.Color = color;
			orCreateEntry.size = size;
			orCreateEntry.transform.localScale = Vector3.one * size;
			Transform transform = orCreateEntry.transform;
			transform.position = worldPosition;
			transform.rotation = PopText.LookAtMainCamera(worldPosition);
			float x = UnityEngine.Random.Range(-this.randomAngle, this.randomAngle);
			float z = UnityEngine.Random.Range(-this.randomAngle, this.randomAngle);
			Vector3 a = Quaternion.Euler(x, 0f, z) * Vector3.up;
			orCreateEntry.SetupContent(text, sprite);
			orCreateEntry.velocity = a * this.spawnVelocity;
			orCreateEntry.spawnTime = Time.time;
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0003D610 File Offset: 0x0003B810
		private static Quaternion LookAtMainCamera(Vector3 position)
		{
			if (Camera.main)
			{
				Transform transform = Camera.main.transform;
				return Quaternion.LookRotation(-(transform.position - position), transform.up);
			}
			return Quaternion.identity;
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0003D656 File Offset: 0x0003B856
		public void Recycle(PopTextEntity entry)
		{
			entry.gameObject.SetActive(false);
			this.activeEntries.Remove(entry);
			this.inactiveEntries.Add(entry);
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0003D680 File Offset: 0x0003B880
		private void Update()
		{
			float deltaTime = Time.deltaTime;
			Vector3 a = Vector3.up * this.gravityValue;
			bool flag = false;
			foreach (PopTextEntity popTextEntity in this.activeEntries)
			{
				if (popTextEntity == null)
				{
					flag = true;
				}
				else
				{
					Transform transform = popTextEntity.transform;
					transform.position += popTextEntity.velocity * deltaTime;
					transform.rotation = PopText.LookAtMainCamera(transform.position);
					popTextEntity.velocity += a * deltaTime;
					popTextEntity.transform.localScale = this.sizeOverLife.Evaluate(popTextEntity.timeSinceSpawn / this.lifeTime) * popTextEntity.size * Vector3.one;
					float t = Mathf.Clamp01(popTextEntity.timeSinceSpawn / this.lifeTime * 2f - 1f);
					Color color = Color.Lerp(popTextEntity.Color, popTextEntity.EndColor, t);
					popTextEntity.SetColor(color);
					if (popTextEntity.timeSinceSpawn > this.lifeTime)
					{
						this.recycleList.Add(popTextEntity);
					}
				}
			}
			if (this.recycleList.Count > 0)
			{
				foreach (PopTextEntity entry in this.recycleList)
				{
					this.Recycle(entry);
				}
				this.recycleList.Clear();
			}
			if (flag)
			{
				this.activeEntries.RemoveAll((PopTextEntity e) => e == null);
			}
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0003D884 File Offset: 0x0003BA84
		private void PopTest()
		{
			Vector3 worldPosition = base.transform.position;
			CharacterMainControl main = CharacterMainControl.Main;
			if (main != null)
			{
				worldPosition = main.transform.position + Vector3.up * 2f;
			}
			this.InstancePop("Test", worldPosition, Color.white, 1f, this.debugSprite);
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0003D8E8 File Offset: 0x0003BAE8
		public static void Pop(string text, Vector3 worldPosition, Color color, float size, Sprite sprite = null)
		{
			if (DevCam.devCamOn)
			{
				return;
			}
			if (PopText.instance)
			{
				PopText.instance.InstancePop(text, worldPosition, color, size, sprite);
			}
		}

		// Token: 0x04000C94 RID: 3220
		public static PopText instance;

		// Token: 0x04000C95 RID: 3221
		public PopTextEntity popTextPrefab;

		// Token: 0x04000C96 RID: 3222
		public List<PopTextEntity> inactiveEntries;

		// Token: 0x04000C97 RID: 3223
		public List<PopTextEntity> activeEntries;

		// Token: 0x04000C98 RID: 3224
		public float spawnVelocity = 5f;

		// Token: 0x04000C99 RID: 3225
		public float gravityValue = -9.8f;

		// Token: 0x04000C9A RID: 3226
		public float lifeTime = 1f;

		// Token: 0x04000C9B RID: 3227
		public AnimationCurve sizeOverLife;

		// Token: 0x04000C9C RID: 3228
		public float randomAngle = 10f;

		// Token: 0x04000C9D RID: 3229
		public Sprite debugSprite;

		// Token: 0x04000C9E RID: 3230
		private List<PopTextEntity> recycleList = new List<PopTextEntity>();
	}
}
