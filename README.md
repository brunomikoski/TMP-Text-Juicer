# TMP-Text-Juicer
Text Juicer for Text Mesh PRO
Is a plugin to allow you do "per-character-animation" on text fields, like this one:
![](https://thumbs.gfycat.com/UntimelyDazzlingBrahmancow-size_restricted.gif)

![alt tag](https://github.com/badawe/TMP-Text-Juicer/blob/develop/Readme-Assets/example-1.gif?raw=true)

![alt tag](https://github.com/badawe/TMP-Text-Juicer/blob/develop/Readme-Assets/example-2.gif?raw=true)

###### Controlling the animation
This git is a ready to be used as sub-module, so just add to your project anywhere inside the Assets Folder, something like Assets/Text Juicer/

If you don't know how to add as a sub-module you can check this [guide](https://blog.sourcetreeapp.com/2012/02/01/using-submodules-and-subrepositories/)

###### Controlling the animation
Basically you can access and change the progress of the animation by the animator itself, or using the helpers inside the TextAnimation, by simply caling, `Play()`, `Stop()` and `Restart()`

###### Adding new effects
Is quite simple, you just need to extend the BaseVertexModifier, and you have access to change whatever you want, in the example bellow, is using a curve to simple multiply the Y from the position itself, generating this effect

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
You can add multiple effects at same time, like the PerCharacter and the X Modifier
![](https://thumbs.gfycat.com/BestGrayCusimanse-size_restricted.gif)

Current Effects:
- Transform Modifier (Position / Scale / Rotation)
- Color Modifier
- Gradient Modifier
- Color Modifier
- Warp Modifier 





> Inspired in this post from [reddit]( https://www.reddit.com/r/Unity3D/comments/3tzwb9/percharacter_text_animations_with_unity_ui/), and the awesome [ui-extensions](https://bitbucket.org/ddreaper/unity-ui-extensions)  


