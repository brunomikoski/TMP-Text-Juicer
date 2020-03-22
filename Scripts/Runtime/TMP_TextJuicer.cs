using System.Collections.Generic;
using BrunoMikoski.TextJuicer.Modifiers;
using TMPro;
using UnityEngine;

namespace BrunoMikoski.TextJuicer
{
    [ExecuteInEditMode]
    [AddComponentMenu( "UI/Text Juicer" )]
    public sealed class TMP_TextJuicer : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text tmpText;
        private TMP_Text TmpText
        {
            get
            {
                if (tmpText == null)
                {
                    tmpText = GetComponent<TMP_Text>();
                    if (tmpText == null)
                        tmpText = GetComponentInChildren<TMP_Text>();
                }
                return tmpText;
            }
        }

        private RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                    rectTransform = (RectTransform)transform;
                return rectTransform;
            }
        }

        public string Text
        {
            get { return TmpText.text; }
            set
            {
                TmpText.text = value;
                SetDirty();
                UpdateIfDirty();
            }
        }

        [SerializeField]
        private float duration = 0.1f;

        [SerializeField]
        private float delay = 0.05f;

        [SerializeField]
        [Range( 0.0f, 1.0f )]
        private float progress;
        public float Progress { get { return progress; } }

        [SerializeField]
        private bool playWhenReady = true;

        [SerializeField]
        private bool loop;

        [SerializeField]
        private bool playForever;

        [SerializeField]
        private bool animationControlled;

        private bool isPlaying;
        public bool IsPlaying
        {
            get { return isPlaying; }
        }

        private CharacterData[] charactersData;
        private TextJuicerVertexModifier[] vertexModifiers;
        private TMP_MeshInfo[] cachedMeshInfo;
        private TMP_TextInfo textInfo;
        private string cachedText = string.Empty;

        private float internalTime;
        private float realTotalAnimationTime;
        private bool isDirty = true;
        private bool dispatchedAfterReadyMethod;
        private bool updateGeometry;
        private bool updateVertexData;
        private bool forceUpdate;

        #region Unity Methods

        private void OnValidate()
        {
            cachedText = string.Empty;
            SetDirty();

            if (tmpText == null)
            {
                tmpText = GetComponent<TMP_Text>();
                if (tmpText == null)
                    tmpText = GetComponentInChildren<TMP_Text>();
            }
        }

        private void Awake()
        {
            if (!animationControlled && Application.isPlaying)
                SetProgress(0);
        }

        private void OnDisable()
        {
            forceUpdate = true;
        }

        public void Update()
        {
            if ( !IsAllComponentsReady() )
                return;

            UpdateIfDirty();

            if ( !dispatchedAfterReadyMethod )
            {
                AfterIsReady();
                dispatchedAfterReadyMethod = true;
            }

            CheckProgress();
            UpdateTime();
            if ( IsPlaying || animationControlled || forceUpdate )
                ApplyModifiers();
        }

        #endregion

        #region Interaction Methods
        public void Restart()
        {
            internalTime = 0;
        }

        public void Play()
        {
            Play( true );
        }

        public void Play( bool fromBeginning = true )
        {
            if ( !IsAllComponentsReady() )
            {
                playWhenReady = true;
                return;
            }
            if ( fromBeginning )
                Restart();

            isPlaying = true;
        }

        public void Complete()
        {
            if ( IsPlaying )
                progress = 1.0f;
        }

        public void Stop()
        {
            isPlaying = false;
        }

        public void SetProgress( float targetProgress )
        {
            progress = targetProgress;
            internalTime = progress * realTotalAnimationTime;
            UpdateTime();
            ApplyModifiers();
            tmpText.havePropertiesChanged = true;
        }

        public void SetPlayForever( bool shouldPlayForever )
        {
            playForever = shouldPlayForever;
        }

        public CustomYieldInstruction WaitForCompletion()
        {
            return new TextJuicer_WaitForCompletion(this);
        }

        #endregion

        #region Internal
        private void AfterIsReady()
        {
            if (!Application.isPlaying)
                return;

            if ( playWhenReady )
                Play();
            else
                SetProgress( progress );
        }

        private bool IsAllComponentsReady()
        {
            if ( TmpText == null )
                return false;

            if ( TmpText.textInfo == null )
                return false;

            if ( TmpText.mesh == null )
                return false;

            if ( TmpText.textInfo.meshInfo == null )
                return false;
            return true;
        }


        private void ApplyModifiers()
        {
            if (charactersData == null)
                return;

            tmpText.ForceMeshUpdate(true);
            for ( int i = 0; i < charactersData.Length; i++ )
                ModifyCharacter( i, cachedMeshInfo );

            if ( updateGeometry )
            {
                for ( int i = 0; i < textInfo.meshInfo.Length; i++ )
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    TmpText.UpdateGeometry( textInfo.meshInfo[i].mesh, i );
                }
            }

            if ( updateVertexData )
                TmpText.UpdateVertexData( TMP_VertexDataUpdateFlags.Colors32 );
       }

        private void ModifyCharacter( int info, TMP_MeshInfo[] meshInfo )
        {
            for ( int i = 0; i < vertexModifiers.Length; i++ )
            {
                vertexModifiers[i].ModifyCharacter( charactersData[info],
                                                    TmpText,
                                                    textInfo,
                                                    progress,
                                                    meshInfo );
            }
        }

        private void CheckProgress()
        {
            if ( IsPlaying )
            {
                internalTime += Time.deltaTime;
                if (internalTime < realTotalAnimationTime || playForever)
                    return;

                if ( loop )
                    internalTime = 0;
                else
                {
                    internalTime = realTotalAnimationTime;
                    progress = 1.0f;
                    Stop();
                    OnAnimationCompleted();
                }
            }
        }

        private void OnAnimationCompleted()
        {
        }

        private void UpdateTime()
        {
            if ( !IsPlaying || animationControlled )
                internalTime = progress * realTotalAnimationTime;
            else
                progress = internalTime / realTotalAnimationTime;

            if ( charactersData == null )
                return;

            for ( int i = 0; i < charactersData.Length; i++ )
                charactersData[i].UpdateTime( internalTime );
        }

        private void UpdateIfDirty()
        {
            if ( !isDirty )
                return;

            if (!gameObject.activeInHierarchy || !gameObject.activeSelf)
                return;

            TextJuicerVertexModifier[] currentComponents = GetComponents<TextJuicerVertexModifier>();
            if ( vertexModifiers == null || vertexModifiers != currentComponents )
            {
                vertexModifiers = currentComponents;

                for ( int i = 0; i < vertexModifiers.Length; i++ )
                {
                    TextJuicerVertexModifier vertexModifier = vertexModifiers[i];

                    if ( !updateGeometry && vertexModifier.ModifyGeometry )
                        updateGeometry = true;

                    if ( !updateVertexData && vertexModifier.ModifyVertex )
                        updateVertexData = true;
                }
            }

            if ( string.IsNullOrEmpty( cachedText ) || !cachedText.Equals( TmpText.text ) )
            {
                TmpText.ForceMeshUpdate();
                textInfo = TmpText.textInfo;
                cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

                List<CharacterData> newCharacterDataList = new List<CharacterData>();
                int indexCount = 0;
                for ( int i = 0; i < textInfo.characterCount; i++ )
                {
                    if (!textInfo.characterInfo[i].isVisible)
                        continue;

                    CharacterData characterData = new CharacterData( indexCount,
                                                                     delay * indexCount,
                                                                     duration,
                                                                     playForever,
                                                                     textInfo.characterInfo[i]
                                                                             .materialReferenceIndex,
                                                                     textInfo.characterInfo[i].vertexIndex );
                    newCharacterDataList.Add( characterData );
                    indexCount += 1;
                }

                charactersData = newCharacterDataList.ToArray();
                realTotalAnimationTime = duration +
                                         charactersData.Length * delay;

                cachedText = TmpText.text;
            }

            isDirty = false;
        }

        public void SetDirty()
        {
            isDirty = true;
        }
        #endregion
    }
}
