using UnityEngine;

namespace BrunoMikoski.TextJuicer
{
    public sealed class TextJuicer_WaitForCompletion : CustomYieldInstruction
    {
        private TMP_TextJuicer textJuicer;
        public override bool keepWaiting { get { return textJuicer.IsPlaying && textJuicer.Progress < 1; } }

        public TextJuicer_WaitForCompletion(TMP_TextJuicer targetTextJuicer)
        {
            textJuicer = targetTextJuicer;
        }
    }
}
