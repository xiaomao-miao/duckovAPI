using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Duckov.Tasks
{
	// Token: 0x02000374 RID: 884
	public class PlayTimelineTask : MonoBehaviour, ITaskBehaviour
	{
		// Token: 0x06001E91 RID: 7825 RVA: 0x0006B8A4 File Offset: 0x00069AA4
		private void Awake()
		{
			this.timeline.stopped += this.OnTimelineStopped;
		}

		// Token: 0x06001E92 RID: 7826 RVA: 0x0006B8BD File Offset: 0x00069ABD
		private void OnDestroy()
		{
			if (this.timeline != null)
			{
				this.timeline.stopped -= this.OnTimelineStopped;
			}
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x0006B8E4 File Offset: 0x00069AE4
		private void OnTimelineStopped(PlayableDirector director)
		{
			this.running = false;
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x0006B8ED File Offset: 0x00069AED
		public void Begin()
		{
			this.running = true;
			this.timeline.Play();
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x0006B901 File Offset: 0x00069B01
		public bool IsComplete()
		{
			return this.timeline.time > this.timeline.duration - 0.009999999776482582 || this.timeline.state != PlayState.Playing;
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x0006B938 File Offset: 0x00069B38
		public bool IsPending()
		{
			return this.timeline.time <= this.timeline.duration - 0.009999999776482582 && this.timeline.state == PlayState.Playing;
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x0006B96C File Offset: 0x00069B6C
		public void Skip()
		{
			this.timeline.Stop();
		}

		// Token: 0x040014E4 RID: 5348
		[SerializeField]
		private PlayableDirector timeline;

		// Token: 0x040014E5 RID: 5349
		private bool running;
	}
}
