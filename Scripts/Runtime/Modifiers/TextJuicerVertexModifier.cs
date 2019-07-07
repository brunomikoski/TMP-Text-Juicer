using TMPro;
using UnityEngine;

namespace BrunoMikoski.TextJuicer.Modifiers
{
    [RequireComponent(typeof(TMP_TextJuicer))]
    public abstract class TextJuicerVertexModifier : MonoBehaviour
    {
        private TMP_TextJuicer cachedTextJuicer;

        public abstract bool ModifyGeometry { get; }
        public abstract bool ModifyVertex { get; }
        protected TMP_TextJuicer TextJuicer
        {
            get
            {
                if (cachedTextJuicer == null)
                    cachedTextJuicer = GetComponent<TMP_TextJuicer>();
                return cachedTextJuicer;
            }
        }

        private void OnValidate()
        {
            TextJuicer.SetDirty();
        }

        public abstract void ModifyCharacter(CharacterData characterData, TMP_Text textComponent,
            TMP_TextInfo textInfo,
            float progress,
            TMP_MeshInfo[] meshInfo);
    }
}
