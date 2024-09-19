#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Shared_GUI
    {
        #region Properties
        #region Consts
        private const float initialLabelWidth = 25f;
        private static readonly Color clearColor = new Color(0f, 0f, 0f, 0f);
        #endregion
        #region GUIStyles
        private static GUIStyle headerGUIStyle = null;
        private static GUIStyle contentGUIStyle = null;
        private static GUIStyle messageGUIStyle = null;
        private static GUIStyle inspectorHeaderGUIStyleGUIStyle = null;
        private static GUIStyle inspectorContentGUIStyleGUIStyle = null;
        private static GUIStyle inspectorMessageItalicGUIStyle = null;
        private static GUIStyle inspectorMessageBoldGUIStyle = null;
        private static GUIStyle inactiveLabelGUIStyle = null;
        #endregion
        #region Cache
        private static GUIStyle cachedLeftPanel;
        private static GUIStyle cachedTitleLabel;
        private static GUIStyle cachedVersionLabel;
        private static GUIStyle cachedRightPanel;
        private static GUIStyle cachedCategoryLabel;
        private static GUIStyle cachedPrimaryButtonStyle;
        private static GUIStyle cachedSecondaryButtonStyle;
        private static GUIStyle cachedContentPanel;
        private static GUIStyle cachedContentLabel;
        private static GUIStyle cachedFieldsLabel;
        private static GUIStyle cachedUnassignedLabel;
        private static GUIStyle cachedMessageLabel;
        private static Dictionary<Color, GUIStyle> guiStyleCache = new Dictionary<Color, GUIStyle>();
        private static Dictionary<Color, Texture2D> textureCache = new Dictionary<Color, Texture2D>();
        #endregion
        #endregion

        #region Label
        public struct LabelWidth : IDisposable
        {
            private readonly float originalLabelWidth;

            public LabelWidth(float newLabelWidth)
            {
                originalLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = newLabelWidth;
            }

            public void Dispose()
            {
                EditorGUIUtility.labelWidth = originalLabelWidth;
            }
        }

        public static float CalculateMaxLabelWidth(IEnumerable<string> names)
        {
            float labelWidth = initialLabelWidth;
            GUIStyle labelStyle = GUI.skin.label;
            foreach (string name in names)
            {
                GUIContent content = new GUIContent(name);
                Vector2 size = labelStyle.CalcSize(content);
                if (size.x > labelWidth) labelWidth = size.x;
            }
            return labelWidth;
        }

        public static float CalculateMaxLabelWidth(Transform parent)
        {
            float maxWidth = 0;
            GatherChildNamesAndCalculateMaxWidth(parent, ref maxWidth);
            return maxWidth + 18f;
        }

        private static void GatherChildNamesAndCalculateMaxWidth(Transform parent, ref float maxWidth)
        {
            GUIStyle labelStyle = GUI.skin.label;
            foreach (Transform child in parent)
            {
                GUIContent content = new GUIContent(child.name);
                Vector2 size = labelStyle.CalcSize(content);
                if (size.x > maxWidth) maxWidth = size.x;
                GatherChildNamesAndCalculateMaxWidth(child, ref maxWidth);
            }
        }
        #endregion

        #region GUIStyles
        public static GUIStyle HeaderGUIStyle
        {
            get
            {
                if (headerGUIStyle == null)
                {
                    if (EditorStyles.boldLabel != null)
                    {
                        headerGUIStyle = new GUIStyle(EditorStyles.boldLabel)
                        {
                            fontSize = 18,
                            fontStyle = FontStyle.Normal,
                            alignment = TextAnchor.MiddleCenter,
                            fixedHeight = 27,
                            normal = { background = GetOrCreateTexture(2, 2, HierarchyDesigner_Shared_ColorUtility.GetDefaultEditorBackgroundColor()) }
                        };
                    }
                }
                return headerGUIStyle;
            }
        }

        public static GUIStyle ContentGUIStyle
        {
            get
            {
                if (contentGUIStyle == null)
                {
                    if (EditorStyles.boldLabel != null)
                    {
                        contentGUIStyle = new GUIStyle(EditorStyles.boldLabel)
                        {
                            fontSize = 16,
                            fontStyle = FontStyle.Bold,
                            alignment = TextAnchor.MiddleLeft,
                        };
                    }
                }
                return contentGUIStyle;
            }
        }

        public static GUIStyle MessageGUIStyle
        {
            get
            {
                if (messageGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        messageGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 13,
                            fontStyle = FontStyle.Italic,
                        };
                    }
                }
                return messageGUIStyle;
            }
        }

        public static GUIStyle InspectorHeaderGUIStyle
        {
            get
            {
                if (inspectorHeaderGUIStyleGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        inspectorHeaderGUIStyleGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 16,
                            fontStyle = FontStyle.Normal,
                            alignment = TextAnchor.MiddleLeft
                        };
                    }
                }
                return inspectorHeaderGUIStyleGUIStyle;
            }
        }

        public static GUIStyle InspectorContentGUIStyle
        {
            get
            {
                if (inspectorContentGUIStyleGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        inspectorContentGUIStyleGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 14,
                            fontStyle = FontStyle.Bold,
                            alignment = TextAnchor.MiddleLeft
                        };
                    }
                }
                return inspectorContentGUIStyleGUIStyle;
            }
        }

        public static GUIStyle InspectorMessageBoldGUIStyle
        {
            get
            {
                if (inspectorMessageBoldGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        inspectorMessageBoldGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 12,
                            fontStyle = FontStyle.Bold,
                        };
                    }
                }
                return inspectorMessageBoldGUIStyle;
            }
        }

        public static GUIStyle InspectorMessageItalicGUIStyle
        {
            get
            {
                if (inspectorMessageItalicGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        inspectorMessageItalicGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 12,
                            fontStyle = FontStyle.Italic,
                        };
                    }
                }
                return inspectorMessageItalicGUIStyle;
            }
        }

        public static GUIStyle InactiveLabelGUIStyle
        {
            get
            {
                if (inactiveLabelGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        inactiveLabelGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 12,
                        };
                        Color textColor = inactiveLabelGUIStyle.normal.textColor;
                        textColor.a = 0.5f;
                        inactiveLabelGUIStyle.normal.textColor = textColor;
                    }
                }
                return inactiveLabelGUIStyle;
            }
        }
        #endregion

        #region Methods
        public static void GetHierarchyDesignerGUIStyles(out GUIStyle leftPanel, 
            out GUIStyle titleLabel, 
            out GUIStyle versionLabel,
            out GUIStyle rightPanel, 
            out GUIStyle categoryLabel, 
            out GUIStyle primaryButtonStyle,
            out GUIStyle secondaryButtonStyle, 
            out GUIStyle contentPanel,
            out GUIStyle contentLabel,
            out GUIStyle fieldsLabel,
            out GUIStyle unassignedLabel,
            out GUIStyle messageLabel)
        {
            if (cachedLeftPanel == null)
            {
                cachedLeftPanel = new GUIStyle
                {
                    name = "Left Panel",
                    normal = new GUIStyleState
                    {
                        background = GetOrCreateTexture(2, 2, HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorI()),
                    },
                    border = new RectOffset(4, 4, 4, 4),
                    padding = new RectOffset(4, 4, 10, 4),
                    margin = new RectOffset(4, 4, 4, 4),
                    overflow = new RectOffset(2, 2, 2, 2),
                    fixedWidth = 160,
                    stretchWidth = true,
                    stretchHeight = true,
                };
            }

            if (cachedTitleLabel == null)
            {
                cachedTitleLabel = new GUIStyle(EditorStyles.label)
                {
                    name = "Title Label",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorText(),
                    },
                    font = HierarchyDesigner_Shared_Resources.DefaultFontBold,
                    fontSize = 24,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.MiddleCenter,
                    wordWrap = true,
                    clipping = TextClipping.Overflow,
                    richText = true,
                    fixedHeight = 64
                };
            }

            if (cachedVersionLabel == null)
            {
                cachedVersionLabel = new GUIStyle(EditorStyles.label)
                {
                    name = "Version Label",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.HexToColor("FFFFFF80"),
                    },
                    font = HierarchyDesigner_Shared_Resources.DefaultFontBold,
                    fontSize = 12,
                    fontStyle = FontStyle.Italic,
                    alignment = TextAnchor.LowerLeft,
                    wordWrap = true,
                    clipping = TextClipping.Overflow,
                    richText = true,
                    stretchHeight = true,
                };
            }

            if (cachedRightPanel == null)
            {
                cachedRightPanel = new GUIStyle
                {
                    name = "Right Panel",
                    normal = new GUIStyleState
                    {
                        background = GetOrCreateTexture(2, 2, HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorI()),
                    },
                    border = new RectOffset(4, 4, 4, 4),
                    padding = new RectOffset(4, 4, 4, 4),
                    margin = new RectOffset(4, 4, 4, 4),
                    overflow = new RectOffset(2, 2, 2, 2),
                    stretchWidth = true,
                    stretchHeight = true,
                };
            }

            if (cachedCategoryLabel == null)
            {
                cachedCategoryLabel = new GUIStyle(EditorStyles.label)
                {
                    name = "Category Label",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorText(),
                        background = GetOrCreateTexture(2, 2, HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorIII()),
                    },
                    font = HierarchyDesigner_Shared_Resources.DefaultFont,
                    fontSize = 22,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.MiddleCenter,
                    contentOffset = new Vector2(0, 2),
                    wordWrap = true,
                    clipping = TextClipping.Overflow,
                    richText = true,
                    fixedHeight = 32
                };
            }

            if (cachedPrimaryButtonStyle == null)
            {
                cachedPrimaryButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    name = "Primary Button Style",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorText(),
                        background = GetOrCreateTexture(2, 2, clearColor)
                    },
                    hover = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorHighlight(),
                        background = GetOrCreateTexture(2, 2, clearColor)
                    },
                    active = new GUIStyleState
                    {
                        textColor = Color.gray,
                        background = GetOrCreateTexture(2, 2, clearColor)
                    },
                    font = HierarchyDesigner_Shared_Resources.DefaultFontBold,
                    border = new RectOffset(2, 2, 2, 2),
                    padding = new RectOffset(0, 0, 0, 0),
                    fontSize = 13,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.MiddleLeft,
                    stretchWidth = true,
                };
            }

            if (cachedSecondaryButtonStyle == null)
            {
                cachedSecondaryButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    name = "Secondary Button Style",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorText(),
                        background = GetOrCreateTexture(2, 2, clearColor)
                    },
                    hover = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorHighlight(),
                        background = GetOrCreateTexture(2, 2, clearColor)
                    },
                    active = new GUIStyleState
                    {
                        textColor = Color.gray,
                        background = GetOrCreateTexture(2, 2, clearColor)
                    },
                    font = HierarchyDesigner_Shared_Resources.DefaultFont,
                    border = new RectOffset(2, 2, 2, 2),
                    padding = new RectOffset(14, 11, 2, 2),
                    fontSize = 11,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.MiddleLeft,
                };
            }

            if(cachedContentPanel == null)
            {
                cachedContentPanel = new GUIStyle
                {
                    name = "Content Panel",
                    normal = new GUIStyleState
                    {
                        background = GetOrCreateTexture(2, 2, HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorII()),
                    },
                    border = new RectOffset(2, 2, 2, 2),
                    padding = new RectOffset(2, 2, 2, 2),
                    margin = new RectOffset(2, 2, 4, 2),
                    overflow = new RectOffset(2, 2, 2, 2),
                    stretchWidth = true,
                    stretchHeight = false,
                };
            }

            if (cachedContentLabel == null)
            {
                cachedContentLabel = new GUIStyle(EditorStyles.label)
                {
                    name = "Content Label",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorHighlight(),
                    },
                    font = HierarchyDesigner_Shared_Resources.DefaultFontBold,
                    fontSize = 16,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.MiddleLeft,
                    clipping = TextClipping.Overflow,
                    richText = true,
                };
            }

            if (cachedFieldsLabel == null)
            {
                cachedFieldsLabel = new GUIStyle(EditorStyles.label)
                {
                    name = "Fields Label",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorText(),
                    },
                    font = HierarchyDesigner_Shared_Resources.DefaultFont,
                    fontSize = 12,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.MiddleLeft,
                    clipping = TextClipping.Overflow,
                };
            }

            if (cachedUnassignedLabel == null)
            {
                cachedUnassignedLabel = new GUIStyle(EditorStyles.label)
                {
                    name = "Unassigned Label",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.HexToColor("B4B4B4"),
                    },
                    font = HierarchyDesigner_Shared_Resources.DefaultFont,
                    fontSize = 12,
                    fontStyle = FontStyle.Italic,
                    alignment = TextAnchor.MiddleLeft,
                    clipping = TextClipping.Overflow,
                };
            }

            if (cachedMessageLabel == null)
            {
                cachedMessageLabel = new GUIStyle(EditorStyles.label)
                {
                    name = "Message Label",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorText(),
                    },
                    fontSize = 12,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.UpperLeft,
                    clipping = TextClipping.Overflow,
                    wordWrap = true,
                    richText = true
                };
            }

            leftPanel = cachedLeftPanel;
            titleLabel = cachedTitleLabel;
            versionLabel = cachedVersionLabel;
            rightPanel = cachedRightPanel;
            categoryLabel = cachedCategoryLabel;
            primaryButtonStyle = cachedPrimaryButtonStyle;
            secondaryButtonStyle = cachedSecondaryButtonStyle;
            contentPanel = cachedContentPanel;
            contentLabel = cachedContentLabel;
            fieldsLabel = cachedFieldsLabel;
            unassignedLabel = cachedUnassignedLabel;
            messageLabel = cachedMessageLabel;
        }

        public static void DrawGUIStyles(out GUIStyle headerGUIStyle, out GUIStyle contentGUIStyle, out GUIStyle messageGUIStyle, out GUIStyle outerBackgroundGUIStyle, out GUIStyle innerBackgroundGUIStyle, out GUIStyle contentBackgroundGUIStyle)
        {
            headerGUIStyle = HeaderGUIStyle;
            contentGUIStyle = ContentGUIStyle;
            messageGUIStyle = MessageGUIStyle;
            outerBackgroundGUIStyle = CreateCustomStyle(0);
            innerBackgroundGUIStyle = CreateCustomStyle(1, new RectOffset(4, 4, 4, 4), new RectOffset(5, 5, 5, 5));
            contentBackgroundGUIStyle = CreateCustomStyle(2, new RectOffset(2, 2, 2, 2), new RectOffset(5, 5, 5, 5));
        }

        public static GUIStyle CreateCustomStyle(int backgroundNumber = 0, RectOffset margin = null, RectOffset padding = null)
        {
            Color backgroundColor;
            switch (backgroundNumber)
            {
                case 0:
                    backgroundColor = HierarchyDesigner_Shared_ColorUtility.GetOuterGUIColor();
                    break;
                case 1:
                    backgroundColor = HierarchyDesigner_Shared_ColorUtility.GetInnerGUIColor();
                    break;
                case 2:
                    backgroundColor = HierarchyDesigner_Shared_ColorUtility.GetContentGUIColor();
                    break;
                default:
                    backgroundColor = HierarchyDesigner_Shared_ColorUtility.GetDefaultEditorBackgroundColor();
                    break;
            }

            if (guiStyleCache.TryGetValue(backgroundColor, out GUIStyle cachedStyle) && cachedStyle.normal.background != null)
            {
                Color cachedColor = cachedStyle.normal.background.GetPixel(0, 0);
                if (cachedColor == backgroundColor)
                {
                    return cachedStyle;
                }
            }

            margin ??= new RectOffset(4, 4, 4, 4);
            padding ??= new RectOffset(2, 2, 4, 4);

            GUIStyle newStyle = new GUIStyle(EditorStyles.helpBox)
            {
                normal = { background = GetOrCreateTexture(2, 2, backgroundColor) },
                stretchHeight = true,
                margin = margin,
                padding = padding
            };

            guiStyleCache[backgroundColor] = newStyle;
            return newStyle;
        }

        #region GUILayout
        public static bool DrawToggle(string label, float labelWidth, bool currentValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            bool newValue = EditorGUILayout.Toggle(currentValue);
            EditorGUILayout.EndHorizontal();
            return newValue;
        }

        public static T DrawEnumPopup<T>(string label, float labelWidth, T selectedValue) where T : Enum
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            T newValue = (T)EditorGUILayout.EnumPopup(selectedValue);
            EditorGUILayout.EndHorizontal();
            return newValue;
        }

        public static int DrawMaskField(string label, float labelWidth, int mask, string[] options)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            int newMask = EditorGUILayout.MaskField(mask, options);
            EditorGUILayout.EndHorizontal();
            return newMask;
        }

        public static float DrawSlider(string label, float labelWidth, float value, float leftValue, float rightValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            float newValue = EditorGUILayout.Slider(value, leftValue, rightValue);
            EditorGUILayout.EndHorizontal();
            return newValue;
        }

        public static int DrawIntSlider(string label, float labelWidth, int value, int leftValue, int rightValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            int newValue = EditorGUILayout.IntSlider(value, leftValue, rightValue);
            EditorGUILayout.EndHorizontal();
            return newValue;
        }

        public static Color DrawColorField(string label, float labelWidth, Color colorValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            Color newColorValue = EditorGUILayout.ColorField(colorValue);
            EditorGUILayout.EndHorizontal();
            return newColorValue;
        }

        public static Gradient DrawGradientField(string label, float labelWidth, Gradient gradientValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            Gradient newGradientValue = EditorGUILayout.GradientField(gradientValue);
            EditorGUILayout.EndHorizontal();
            return newGradientValue;
        }
        #endregion

        #region Operations
        private static Texture2D GetOrCreateTexture(int width, int height, Color color)
        {
            if (textureCache.TryGetValue(color, out Texture2D existingTexture) && existingTexture != null)
            {
                return existingTexture;
            }

            Texture2D newTexture = new Texture2D(width, height);
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = color;
            }

            newTexture.SetPixels(pix);
            newTexture.Apply();
            newTexture.hideFlags = HideFlags.DontSave;

            textureCache[color] = newTexture;
            return newTexture;
        }
        #endregion
        #endregion
    }
}
#endif