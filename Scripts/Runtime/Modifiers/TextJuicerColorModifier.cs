using TMPro;
using UnityEngine;

namespace BrunoMikoski.TextJuicer.Modifiers
{
    [AddComponentMenu( "UI/Text Juicer Modifiers/Colors Modifier", 11 )]
    public sealed class TextJuicerColorModifier : TextJuicerVertexModifier
    {
        [SerializeField]
        private Color[] colors;

        private Color32[] newVertexColors;

        private Color32 targetColor;

        public override bool ModifyGeometry
        {
            get { return false; }
        }
        public override bool ModifyVertex
        {
            get { return true; }
        }

        public override void ModifyCharacter(CharacterData characterData, TMP_Text textComponent,
            TMP_TextInfo textInfo,
            float progress,
            TMP_MeshInfo[] meshInfo)
        {
            if (colors == null || colors.Length == 0)
                return;

            int materialIndex = characterData.MaterialIndex;

            newVertexColors = textInfo.meshInfo[materialIndex].colors32;

            int vertexIndex = characterData.VertexIndex;

            targetColor = colors[Mathf.CeilToInt(characterData.Progress * (colors.Length-1))];

            newVertexColors[vertexIndex + 0] = targetColor;
            newVertexColors[vertexIndex + 1] = targetColor;
            newVertexColors[vertexIndex + 2] = targetColor;
            newVertexColors[vertexIndex + 3] = targetColor;
        }
    }
}
