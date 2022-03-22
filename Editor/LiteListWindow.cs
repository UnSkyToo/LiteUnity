using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LiteUnity.Editor
{
    public class LiteListWindow : EditorWindow
    {
        public static void Show(IList<string> list, Action<int> callback)
        {
            var window = GetWindow<LiteListWindow>(true, "List Window", true);
            window.Data_.Clear();
            window.Data_.AddRange(list);
            window.Callback_ = callback;
            window.ShowAuxWindow();
        }

        private readonly List<string> Data_ = new List<string>();
        private Action<int> Callback_ = null;
        private Vector2 ScrollPos_ = Vector2.zero;
        private string MatchText_ = string.Empty;

        private GUIStyle ListStyle_;
        private GUIStyle HeaderStyle_;
        private GUIStyle SearchStyle_;
        private GUIStyle SearchCancelBtnEmpty_;
        private GUIStyle SearchCancelBtn_;
        private GUIStyle ItemStyle_;

        private void OnEnable()
        {
            ListStyle_ = "HelpBox";
            HeaderStyle_ = "HelpBox";
            SearchStyle_ = "SearchTextField";
            SearchCancelBtnEmpty_ = "SearchCancelButtonEmpty";
            SearchCancelBtn_ = "SearchCancelButton";
            ItemStyle_  = "toolbarbutton";
            ItemStyle_.alignment = TextAnchor.MiddleLeft;
        }

        public void OnGUI()
        {
            DrawHeader();
            DrawList();
        }

        private void DrawHeader()
        {
            using (new EditorGUILayout.VerticalScope(HeaderStyle_))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    MatchText_ = EditorGUILayout.TextField(MatchText_, SearchStyle_);
                    
                    if (string.IsNullOrEmpty(MatchText_))
                    {
                        GUILayout.Label(string.Empty, SearchCancelBtnEmpty_);
                    }
                    else
                    {
                        if (GUILayout.Button(string.Empty, SearchCancelBtn_))
                        {
                            GUI.FocusControl(null);
                            MatchText_ = string.Empty;
                        }
                    }
                }
            }
        }

        private void DrawList()
        {
            using (new EditorGUILayout.VerticalScope(ListStyle_))
            {
                using (var sv = new GUILayout.ScrollViewScope(ScrollPos_))
                {
                    for (var index = 0; index < Data_.Count; ++index)
                    {
                        var text = Data_[index];

                        if (!string.IsNullOrWhiteSpace(MatchText_))
                        {
                            if (!text.ToLower().Contains(MatchText_.ToLower()))
                            {
                                continue;
                            }
                        }

                        if (GUILayout.Button(text, ItemStyle_))
                        {
                            GUI.FocusControl(null);
                            OnSelected(index);
                        }
                        
                        GUILayout.Space(2);
                    }

                    ScrollPos_ = sv.scrollPosition;
                }
            }
        }

        private void OnSelected(int index)
        {
            Callback_?.Invoke(index);
            Close();
        }
    }
}