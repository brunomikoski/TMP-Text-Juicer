using System;
using TMPro;
using UnityEngine;

namespace BrunoMikoski.TextJuicer
{
    [Serializable]
    public struct CharacterData
    {
        private float progress;
        public float Progress
        {
            get { return progress; }
        }

        private float startingTime;
        private float totalAnimationTime;
        private bool playForever;

        private int materialIndex;
        public int MaterialIndex
        {
            get
            {
                return materialIndex;
            }
        }
        private int vertexIndex;
        public int VertexIndex
        {
            get
            {
                return vertexIndex;
            }
        }
        private int index;
        public int Index
        {
            get
            {
                return index;
            }
        }

        public CharacterData( int targetIndex, float startTime, float targetAnimationTime,
            bool isPlayForever, int targetMaterialIndex, int targetVertexIndex )
        {
            index = targetIndex;
            progress = 0.0f;
            startingTime = startTime;
            totalAnimationTime = targetAnimationTime;
            playForever = isPlayForever;
            vertexIndex = targetVertexIndex;
            materialIndex = targetMaterialIndex;
        }

        public void UpdateTime( float time )
        {
            if ( time < startingTime )
            {
                progress = 0;
                return;
            }

            float currentProgress = (time - startingTime) / totalAnimationTime;
            if ( !playForever )
                currentProgress = Mathf.Clamp01( currentProgress );

            progress = currentProgress;
        }
    }
}
