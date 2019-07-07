using TMPro;
using UnityEngine;

namespace BrunoMikoski.TextJuicer.Modifiers
{
    [AddComponentMenu("UI/Text Juicer Modifiers/Warp Modifier", 11)]
    public sealed class TextJuicerWarpModifier : TextJuicerVertexModifier
    {
        [SerializeField]
        private float curveMultipler = 10;

        [SerializeField]
        private AnimationCurve intensityCurve =
            new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        private Matrix4x4 matrix;
        private bool modifyGeomery;
        private bool modifyVertex;

        private Vector3[] vertices;

        [SerializeField]
        private AnimationCurve warpCurve = new AnimationCurve(new Keyframe(0, 0),
            new Keyframe(0.25f, 2.0f),
            new Keyframe(0.5f, 0), new Keyframe(0.75f, 2.0f),
            new Keyframe(1, 0f));

        public override bool ModifyGeometry
        {
            get { return true; }
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
            float boundsMinX = textComponent.bounds.min.x;
            float boundsMaxX = textComponent.bounds.max.x;

            vertices = textInfo.meshInfo[characterData.MaterialIndex].vertices;

            int characterDataVertexIndex = characterData.VertexIndex;
            Vector3 offsetToMidBaseline = new Vector2(
                (vertices[characterDataVertexIndex + 0].x
                 + vertices[characterDataVertexIndex + 2].x) / 2,
                textInfo.characterInfo[characterData.Index].baseLine);

            vertices[characterDataVertexIndex + 0] += -offsetToMidBaseline;
            vertices[characterDataVertexIndex + 1] += -offsetToMidBaseline;
            vertices[characterDataVertexIndex + 2] += -offsetToMidBaseline;
            vertices[characterDataVertexIndex + 3] += -offsetToMidBaseline;

            float x0 = (offsetToMidBaseline.x - boundsMinX) /
                       (boundsMaxX - boundsMinX);
            float x1 = x0 + 0.0001f;
            float y0 = warpCurve.Evaluate(x0) * curveMultipler;
            float y1 = warpCurve.Evaluate(x1) * curveMultipler;

            y0 *= intensityCurve.Evaluate(characterData.Progress);
            y1 *= intensityCurve.Evaluate(characterData.Progress);

            Vector3 horizontal = new Vector3(1, 0, 0);
            Vector3 tangent = new Vector3(x1 * (boundsMaxX - boundsMinX) + boundsMinX, y1) -
                              new Vector3(offsetToMidBaseline.x, y0);

            float dot = Mathf.Acos(Vector3.Dot(horizontal, tangent.normalized)) * 57.2957795f;
            Vector3 cross = Vector3.Cross(horizontal, tangent);
            float angle = cross.z > 0 ? dot : 360 - dot;

            matrix = Matrix4x4.TRS(new Vector3(0, y0, 0), Quaternion.Euler(0, 0, angle),
                Vector3.one);

            vertices[characterDataVertexIndex + 0] =
                matrix.MultiplyPoint3x4(vertices[characterDataVertexIndex + 0]);
            vertices[characterDataVertexIndex + 1] =
                matrix.MultiplyPoint3x4(vertices[characterDataVertexIndex + 1]);
            vertices[characterDataVertexIndex + 2] =
                matrix.MultiplyPoint3x4(vertices[characterDataVertexIndex + 2]);
            vertices[characterDataVertexIndex + 3] =
                matrix.MultiplyPoint3x4(vertices[characterDataVertexIndex + 3]);

            vertices[characterDataVertexIndex + 0] += offsetToMidBaseline;
            vertices[characterDataVertexIndex + 1] += offsetToMidBaseline;
            vertices[characterDataVertexIndex + 2] += offsetToMidBaseline;
            vertices[characterDataVertexIndex + 3] += offsetToMidBaseline;
        }
    }
}
