#if UNITY_2018_3_OR_NEWER
#define TMP_1_4_0_OR_NEWER
#endif

#if UNITY_2019_1_OR_NEWER
#define TMP_2_1_0_PREVIEW_8_OR_NEWER
#endif

//#if UNITY_2018_3_OR_NEWER /*&& !UNITY_2019_1_OR_NEWER*/
//#define TMP_1_4_0_OR_NEWER
//#endif

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

using TMPro;

namespace GameChatUnity.Extension
{
    public class TMP_GameChatTextUGUI : TextMeshProUGUI
    {
        #region Private Fields

        protected bool m_emojiParsingRequired = true;
        private static Regex regx = new Regex("((http://|https://|www\\.)([A-Z0-9.-:]{1,})\\.[0-9A-Z?;~&#=\\-_\\./]{2,})", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private string _originText = "";

        // Check links in text 
        private string convertHyperLinking(string _text)
        {
            MatchCollection matches = regx.Matches(_text);

            List<string> _bucket = new List<string>();

            foreach (Match match in matches)
                if (!_bucket.Contains(match.Value)) _bucket.Add(match.Value);

            foreach (string matchStr in _bucket)
            {
                string[] _parsed = _text.Split(new string[] { matchStr }, StringSplitOptions.None);
                string _polishStr = string.Format("<#" + LinkTextColor + "><u><link=\"{0}\">{1}</link></u></color>", matchStr, matchStr);
                _text = string.Join(_polishStr, _parsed);
            }
            return _text;
        }

        #endregion

        #region Public Fields

        public TMP_GameChatTextUGUI() { }

        public TMP_GameChatTextUGUI(string msg)
        {
            _originText = msg;
            SetMessage(msg);
        }

        private bool _isHyperLinked = false;
        public bool isHyperLinked
        {
            get
            {
                return _isHyperLinked;
            }
            set
            {
                bool _prev = _isHyperLinked;
                _isHyperLinked = value;

                if (_prev != _isHyperLinked)
                {
                    //Debug.Log("Reset HyperLinking to " + _isHyperLinked);
                    SetMessage(_originText);
                }
            }
        }

        public void SetMessage(string msg)
        {
            _originText = msg;

            if (_isHyperLinked)
            {
                text = convertHyperLinking(msg);
            }
            else
            {
                text = msg;
            }
        }

        public string LinkTextColor { get; set; } = "7f7fe5";

        #endregion


        #region Properties

#if TMP_1_4_0_OR_NEWER
        protected System.Reflection.FieldInfo _isInputParsingRequired_Field = null;
#endif
        protected internal bool IsInputParsingRequired_Internal
        {
            get
            {
#if TMP_1_4_0_OR_NEWER
                if (_isInputParsingRequired_Field == null)
                    _isInputParsingRequired_Field = typeof(TMP_Text).GetField("m_isInputParsingRequired", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                if (_isInputParsingRequired_Field != null)
                    return (bool)_isInputParsingRequired_Field.GetValue(this);
                else
                    return false;
#else
                return m_isInputParsingRequired;
#endif
            }
            protected set
            {
#if TMP_1_4_0_OR_NEWER
                if (_isInputParsingRequired_Field == null)
                    _isInputParsingRequired_Field = typeof(TMP_Text).GetField("m_isInputParsingRequired", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                if (_isInputParsingRequired_Field != null)
                    _isInputParsingRequired_Field.SetValue(this, value);
#else
                m_isInputParsingRequired = value;
#endif
            }
        }

#if TMP_1_4_0_OR_NEWER
        protected enum TextInputSources { Text = 0, SetText = 1, SetCharArray = 2, String = 3 };
        protected System.Reflection.FieldInfo _inputSource_Field = null;
        protected System.Type _textInputSources_Type = null;
#endif
        protected TextInputSources InputSource_Internal
        {
            get
            {
#if TMP_1_4_0_OR_NEWER
                if (_inputSource_Field == null)
                    _inputSource_Field = typeof(TMP_Text).GetField("m_inputSource", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                if (_inputSource_Field != null)
                    return (TextInputSources)System.Enum.ToObject(typeof(TextInputSources), (int)_inputSource_Field.GetValue(this));
                else
                    return TextInputSources.Text;
#else
                return m_inputSource;
#endif
            }
            set
            {
#if TMP_1_4_0_OR_NEWER

                if (_inputSource_Field == null)
                    _inputSource_Field = typeof(TMP_Text).GetField("m_inputSource", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                if (_inputSource_Field != null)
                {
                    //Pick the Type of internal enum to set back in TMP_Text
                    if (_textInputSources_Type == null)
                        _textInputSources_Type = typeof(TMP_Text).GetNestedType("TextInputSources", System.Reflection.BindingFlags.NonPublic);
                    if (_textInputSources_Type != null)
                    {
                        _inputSource_Field.SetValue(this, System.Enum.ToObject(_textInputSources_Type, (int)value));
                    }
                }

#else
                m_inputSource = value;
#endif
            }
        }
        #endregion


        #region Emoji Parser Functions

        protected virtual bool ParseInputTextAndEmojiCharSequence()
        {
            m_emojiParsingRequired = false;

            //Only parse when richtext active (we need the <sprite=index> tag)
            if (m_isRichText)
            {
                var v_parsedEmoji = false;
                var v_oldText = m_text;

                /*//Parse Sprite Chars
                ParseInputText();

                //Parse Emojis
                m_text = CharBufferToString(); //Pick the char unicodes to parse the emojis
                v_parsedEmoji = TMP_GameChatSearchEngine.ParseEmojiCharSequence(spriteAsset, ref m_text);

                //Apply parsed Emoji to char buffer
                if (v_parsedEmoji)
                {
                    StringToCharArray(m_text, ref m_char_buffer);
                    SetArraySizes(m_char_buffer);
                }*/

                v_parsedEmoji = TMP_GameChatSearchEngine.ParseEmojiCharSequence(spriteAsset, ref m_text);

                m_emojiParsingRequired = false;
                IsInputParsingRequired_Internal = false;
                InputSource_Internal = TextInputSources.Text;

                ParseInputText();

                m_emojiParsingRequired = false;
                IsInputParsingRequired_Internal = false;

                //We must revert the original text because we dont want to permanently change the text
                m_text = v_oldText;


#if !TMP_2_1_0_OR_NEWER && !TMP_1_4_0_OR_NEWER
                m_isCalculateSizeRequired = true;
#endif

                return v_parsedEmoji;
            }

            return false;
        }

        #endregion




        #region Text Overriden Functions

        public override void SetVerticesDirty()
        {
            //In textmeshpro 1.4 the parameter "m_isInputParsingRequired" changed to internal, so, to dont use reflection i changed to "m_havePropertiesChanged" parameter
            if (IsInputParsingRequired_Internal)
            {
                m_emojiParsingRequired = m_isRichText;
            }
            base.SetVerticesDirty();
        }

        public override void Rebuild(CanvasUpdate update)
        {
            if (this == null && enabled && gameObject.activeInHierarchy) return;

            if (m_emojiParsingRequired)
                ParseInputTextAndEmojiCharSequence();

            base.Rebuild(update);
        }

        public override string GetParsedText()
        {
            if (m_emojiParsingRequired)
                ParseInputTextAndEmojiCharSequence();

            return base.GetParsedText();
        }

        public override TMP_TextInfo GetTextInfo(string text)
        {
            TMP_GameChatSearchEngine.ParseEmojiCharSequence(spriteAsset, ref text);
            return base.GetTextInfo(text);
        }


#if TMP_2_1_0_PREVIEW_8_OR_NEWER
        protected override Vector2 CalculatePreferredValues(ref float defaultFontSize, Vector2 marginSize, bool ignoreTextAutoSizing, bool isWordWrappingEnabled)
        {
            if (m_emojiParsingRequired)
                ParseInputTextAndEmojiCharSequence();

            return base.CalculatePreferredValues(ref defaultFontSize, marginSize, ignoreTextAutoSizing, isWordWrappingEnabled);
        }
#elif TMP_2_1_0_PREVIEW_3_OR_NEWER
        protected override Vector2 CalculatePreferredValues(ref float defaultFontSize, Vector2 marginSize, bool ignoreTextAutoSizing, bool isWordWrappingEnabled)
        {
            if (m_emojiParsingRequired)
                ParseInputTextAndEmojiCharSequence();

            return base.CalculatePreferredValues(ref defaultFontSize, marginSize, ignoreTextAutoSizing, isWordWrappingEnabled);
        }
#else
        protected override Vector2 CalculatePreferredValues(float defaultFontSize, Vector2 marginSize, bool ignoreTextAutoSizing)
        {
            if (m_emojiParsingRequired)
                ParseInputTextAndEmojiCharSequence();

            return base.CalculatePreferredValues(defaultFontSize, marginSize, ignoreTextAutoSizing);
        }
#endif

        /*public string CharBufferToString()
        {
            System.Text.StringBuilder v_builder = new System.Text.StringBuilder();
            if (m_char_buffer != null)
            {
                for (int i = 0; i < m_char_buffer.Length; i++)
                {
                    var v_intValue = m_char_buffer[i];

                    //Not a surrogate and is valid UTF32 (conditions to use char.ConvertFromUtf32 function)
                    if (v_intValue > 0x000000 && v_intValue < 0x10ffff &&
                                        (v_intValue < 0x00d800 || v_intValue > 0x00dfff))
                    {
                        var v_UTF16Surrogate = char.ConvertFromUtf32(v_intValue);
                        if (!string.IsNullOrEmpty(v_UTF16Surrogate))
                        {
                            //Add chars into cache (we must include the both char paths in fastLookupPath)
                            foreach (var v_surrogateChar in v_UTF16Surrogate)
                            {
                                v_builder.Append(v_surrogateChar);
                            }
                        }
                    }
                    //Simple Append
                    else
                    {
                        v_builder.Append((char)v_intValue);
                    }
                }
            }

            return v_builder.ToString();
        }*/

        #endregion
    }
}
