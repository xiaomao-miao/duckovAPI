using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Sounds
{
	// Token: 0x02000249 RID: 585
	public class SoundVisualization : MonoBehaviour
	{
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06001227 RID: 4647 RVA: 0x00045160 File Offset: 0x00043360
		private PrefabPool<SoundDisplay> DisplayPool
		{
			get
			{
				if (this._displayPool == null)
				{
					this._displayPool = new PrefabPool<SoundDisplay>(this.displayTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._displayPool;
			}
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x00045199 File Offset: 0x00043399
		private void Awake()
		{
			AIMainBrain.OnPlayerHearSound += this.OnHeardSound;
			if (this.layoutCenter == null)
			{
				this.layoutCenter = (base.transform as RectTransform);
			}
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x000451CB File Offset: 0x000433CB
		private void OnDestroy()
		{
			AIMainBrain.OnPlayerHearSound -= this.OnHeardSound;
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x000451E0 File Offset: 0x000433E0
		private void Update()
		{
			using (IEnumerator<SoundDisplay> enumerator = this.DisplayPool.ActiveEntries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SoundDisplay soundDisplay = enumerator.Current;
					if (soundDisplay.Value <= 0f)
					{
						this.releaseBuffer.Enqueue(soundDisplay);
					}
					else
					{
						this.RefreshEntryPosition(soundDisplay);
					}
				}
				goto IL_71;
			}
			IL_50:
			SoundDisplay soundDisplay2 = this.releaseBuffer.Dequeue();
			if (!(soundDisplay2 == null))
			{
				this.DisplayPool.Release(soundDisplay2);
			}
			IL_71:
			if (this.releaseBuffer.Count <= 0)
			{
				return;
			}
			goto IL_50;
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0004527C File Offset: 0x0004347C
		private void OnHeardSound(AISound sound)
		{
			this.Trigger(sound);
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x00045288 File Offset: 0x00043488
		private void Trigger(AISound sound)
		{
			if (GameCamera.Instance == null)
			{
				return;
			}
			SoundDisplay soundDisplay = null;
			if (sound.fromCharacter != null)
			{
				foreach (SoundDisplay soundDisplay2 in this.DisplayPool.ActiveEntries)
				{
					AISound currentSount = soundDisplay2.CurrentSount;
					if (!(currentSount.fromCharacter != sound.fromCharacter) && currentSount.soundType == sound.soundType && Vector3.Distance(currentSount.pos, sound.pos) < this.retriggerDistanceThreshold)
					{
						soundDisplay = soundDisplay2;
					}
				}
			}
			if (soundDisplay == null)
			{
				soundDisplay = this.DisplayPool.Get(null);
			}
			this.RefreshEntryPosition(soundDisplay);
			soundDisplay.Trigger(sound);
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x00045358 File Offset: 0x00043558
		private void RefreshEntryPosition(SoundDisplay e)
		{
			Vector3 pos = e.CurrentSount.pos;
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(GameCamera.Instance.renderCamera, pos);
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.layoutCenter, screenPoint, null, out vector);
			Vector2 normalized = vector.normalized;
			e.transform.localPosition = normalized * this.displayOffset;
			e.transform.rotation = Quaternion.FromToRotation(Vector2.up, normalized);
		}

		// Token: 0x04000DF4 RID: 3572
		[SerializeField]
		private RectTransform layoutCenter;

		// Token: 0x04000DF5 RID: 3573
		[SerializeField]
		private SoundDisplay displayTemplate;

		// Token: 0x04000DF6 RID: 3574
		[SerializeField]
		private float retriggerDistanceThreshold = 1f;

		// Token: 0x04000DF7 RID: 3575
		[SerializeField]
		private float displayOffset = 400f;

		// Token: 0x04000DF8 RID: 3576
		private PrefabPool<SoundDisplay> _displayPool;

		// Token: 0x04000DF9 RID: 3577
		private Queue<SoundDisplay> releaseBuffer = new Queue<SoundDisplay>();
	}
}
