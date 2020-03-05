# TMP-Text-Juicer
[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/badawe/TMP-Text-Juicer/blob/develop/LICENSE)

Text Juicer for Text Mesh PRO
Is a plugin to allow you do "per-character-animation" on text fields, like this one:

![](https://github.com/badawe/TMP-Text-Juicer/blob/develop/ReadmeAssets/example-1.gif?raw=true)

![](https://github.com/badawe/TMP-Text-Juicer/blob/develop/ReadmeAssets/example-2.gif?raw=true)

###### Component Work flow
![](https://github.com/badawe/TMP-Text-Juicer/blob/develop/ReadmeAssets/text-juicer-component.gif?raw=true)

###### Controlling the animation
This git is a ready to be used as sub-module, so just add to your project anywhere inside the Assets Folder, something like Assets/Text Juicer/

If you don't know how to add as a sub-module you can check this [guide](https://blog.sourcetreeapp.com/2012/02/01/using-submodules-and-subrepositories/)

###### Controlling the animation
Basically you can access and change the progress of the animation by the animator itself, or using the helpers inside the TextAnimation, by simply caling, `Play()`, `Stop()` and `Restart()`

###### Adding new effects
Is quite simple, you just need to extend the TextJuicerVertexModifier

```csharp
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
```


###### Multiple Effects
Current Effects:
- Transform Modifier (Position / Scale / Rotation)
- Color Modifier
- Gradient Modifier
- Color Modifier
- Warp Modifier





> Inspired in this post from [reddit]( https://www.reddit.com/r/Unity3D/comments/3tzwb9/percharacter_text_animations_with_unity_ui/), and the awesome [ui-extensions](https://bitbucket.org/ddreaper/unity-ui-extensions)


