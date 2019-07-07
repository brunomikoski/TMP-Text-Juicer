using System;
using System.Collections;
using UnityEngine;

namespace BrunoMikoski.TextJuicer
{
    public sealed class TextJuicerSequence : MonoBehaviour
    {
        [Serializable]
        private struct TextJuicerSequenceItemData
        {
            [SerializeField]
            private TMP_TextJuicer textJuicer;
            public TMP_TextJuicer TextJuicer { get { return textJuicer; } }
            [SerializeField]
            private float afterInterval;
            public float AfterInterval { get { return afterInterval; } }
        }

        [SerializeField]
        private TextJuicerSequenceItemData[] sequence;

        [SerializeField]
        private bool playOnStart;
        private Coroutine playCoroutine;

        private void Start()
        {
            if (playOnStart)
                Play();
        }

        public void Play()
        {
            if (playCoroutine != null)
                StopCoroutine(playCoroutine);
            playCoroutine = StartCoroutine(PlayEnumerator());
        }

        private IEnumerator PlayEnumerator()
        {
            int playedItems = 0;
            while (playedItems < sequence.Length)
            {
                TextJuicerSequenceItemData textJuicerSequenceItemData = sequence[playedItems];

                textJuicerSequenceItemData.TextJuicer.Play();
                yield return textJuicerSequenceItemData.TextJuicer.WaitForCompletionEnumerator();
                yield return new WaitForSeconds(textJuicerSequenceItemData.AfterInterval);
                playedItems++;
            }
        }
    }
}
