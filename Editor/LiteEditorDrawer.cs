using System;
using System.Collections;
using System.Reflection;
using LiteUnity.Runtime.Attributes;
using LiteUnity.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace LiteUnity.Editor
{
    internal static class LiteEditorDrawer
    {
        private static object DrawIntField(Rect rect, GUIContent title, int v, Type type, object[] attrs)
        {
            var delayedIntAttr = LiteEditorHelper.GetAttribute<LiteDelayedAttribute>(type, attrs);
            return delayedIntAttr == null ? EditorGUI.IntField(rect, title, v) : EditorGUI.DelayedIntField(rect, title, v);
        }

        private static float DrawFloatField(Rect rect, GUIContent title, float v, Type type, object[] attrs)
        {
            var delayedFloatAttr = LiteEditorHelper.GetAttribute<LiteDelayedAttribute>(type, attrs);
            return delayedFloatAttr == null ? EditorGUI.FloatField(rect, title, v) : EditorGUI.DelayedFloatField(rect, title, v);
        }

        private static double DrawDoubleField(Rect rect, GUIContent title, double v, Type type, object[] attrs)
        {
            var delayedDoubleAttr = LiteEditorHelper.GetAttribute<LiteDelayedAttribute>(type, attrs);
            return delayedDoubleAttr == null ? EditorGUI.DoubleField(rect, title, v) : EditorGUI.DelayedDoubleField(rect, title, v);
        }

        private static string DrawStringField(Rect rect, GUIContent title, string v, Type type, object[] attrs)
        {
            var delayedStringAttr = LiteEditorHelper.GetAttribute<LiteDelayedAttribute>(type, attrs);
            return delayedStringAttr == null ? EditorGUI.TextField(rect, title, v) : EditorGUI.DelayedTextField(rect, title, v);
        }

        private static Enum DrawEnumField(Rect rect, GUIContent title, Enum v, Type type, object[] attrs)
        {
            var enumFlagsAttr = LiteEditorHelper.GetAttribute<LiteEnumFlagsAttribute>(type, attrs);
            return enumFlagsAttr == null ? EditorGUI.EnumPopup(rect, title, v) : EditorGUI.EnumFlagsField(rect, title, v);
        }
        
        /// <summary>
        /// <para>Support Data Type</para>
        /// <para>Primitive : bool, int, long, float, double, string, Enum</para>
        /// <para>UnityEngine : Rect, RectInt, Vector2, Vector2Int, Vector3, Vector3Int, Vector4, Color</para>
        /// </summary>
        public static object DrawElement(Rect rect, GUIContent title, object data, Type type, object[] attrs = null)
        {
            if (data == null)
            {
                data = LiteTypeSystem.CreateInstance(type);
            }

            switch (data)
            {
                #region Primitive
                case bool v:
                    data = EditorGUI.Toggle(rect, title, v);
                    break;
                case int v:
                    data = DrawIntField(rect, title, v, type, attrs);
                    break;
                case long v:
                    data = EditorGUI.LongField(rect, title, v);
                    break;
                case float v:
                    data = DrawFloatField(rect, title, v, type, attrs);
                    break;
                case double v:
                    data = DrawDoubleField(rect, title, v, type, attrs);
                    break;
                case string v:
                    data = DrawStringField(rect, title, v, type, attrs);
                    break;
                case Enum v:
                    data = DrawEnumField(rect, title, v, type, attrs);
                    break;
                #endregion
                #region UnityEngine
                case Rect v:
                    data = EditorGUI.RectField(rect, title, v);
                    break;
                case RectInt v:
                    data = EditorGUI.RectIntField(rect, title, v);
                    break;
                case Vector2 v:
                    data = EditorGUI.Vector2Field(rect, title, v);
                    break;
                case Vector2Int v:
                    data = EditorGUI.Vector2IntField(rect, title, v);
                    break;
                case Vector3 v:
                    data = EditorGUI.Vector3Field(rect, title, v);
                    break;
                case Vector3Int v:
                    data = EditorGUI.Vector3IntField(rect, title, v);
                    break;
                case Vector4 v:
                    data = EditorGUI.Vector4Field(rect, title, v);
                    break;
                case Color v:
                    data = EditorGUI.ColorField(rect, title, v);
                    break;
                #endregion
                default:
                    LiteEditorHelper.UnsupportedType(type);
                    break;
            }
            return data;
        }
    }
}