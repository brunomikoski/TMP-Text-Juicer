using System.Collections;

namespace BrunoMikoski.TextJuicer
{
    public static partial class TMP_TextJuicerExtensions
    {
        public static IEnumerator WaitForCompletionEnumerator( this TMP_TextJuicer textJuicer )
        {
            while ( textJuicer != null && textJuicer.IsPlaying && textJuicer.Progress < 1.0f )
                yield return null;
        }

    }
}
