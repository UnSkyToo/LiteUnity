using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using LiteUnity.Runtime.Attributes;
using LiteUnity.Runtime.Common;
using UnityEditor;
using UnityEngine;

namespace LiteUnity.Editor
{
    public static class LiteEditorLayoutDrawer
    {
        private static object DrawIntField(GUIContent title, int v, Type type, object[] attrs)
        {
            var delayedIntAttr = LiteEditorHelper.GetAttribute<LiteDelayedAttribute>(type, attrs);
            return delayedIntAttr == null ? EditorGUILayout.IntField(title, v) : EditorGUILayout.DelayedIntField(title, v);
        }
        
        private static float DrawFloatField(GUIContent title, float v, Type type, object[] attrs)
        {
            var delayedFloatAttr = LiteEditorHelper.GetAttribute<LiteDelayedAttribute>(type, attrs);
            return delayedFloatAttr == null ? EditorGUILayout.FloatField(title, v) : EditorGUILayout.DelayedFloatField(title, v);
        }
        
        private static double DrawDoubleField(GUIContent title, double v, Type type, object[] attrs)
        {
            var delayedDoubleAttr = LiteEditorHelper.GetAttribute<LiteDelayedAttribute>(type, attrs);
            return delayedDoubleAttr == null ? EditorGUILayout.DoubleField(title, v) : EditorGUILayout.DelayedDoubleField(title, v);
        }

        private static string DrawStringField(GUIContent title, string v, Type type, object[] attrs)
        {
            var delayedStringAttr = LiteEditorHelper.GetAttribute<LiteDelayedAttribute>(type, attrs);
            return delayedStringAttr == null ? EditorGUILayout.TextField(title, v) : EditorGUILayout.DelayedTextField(title, v);
        }

        private static Enum DrawEnumField(GUIContent title, Enum v, Type type, object[] attrs)
        {
            var enumFlagsAttr = LiteEditorHelper.GetAttribute<LiteEnumFlagsAttribute>(type, attrs);
            return enumFlagsAttr == null ? EditorGUILayout.EnumPopup(title, v) : EditorGUILayout.EnumFlagsField(title, v);
        }

        public static object DrawObject(GUIContent title, object data)
        {
            if (data == null)
            {
                return null;
            }

            // var obj = DrawObjectInternal(title, data, type);
            var obj = DrawElement(title, data, data.GetType());
            return obj;
        }

        /// <summary>
        /// <para>Support Data Type</para>
        /// <para>Primitive : bool, int, long, float, double, string, Enum</para>
        /// <para>UnityEngine : Rect, RectInt, Vector2, Vector2Int, Vector3, Vector3Int, Vector4, Color</para>
        /// </summary>
        private static object DrawElement(GUIContent title, object data, Type type, object[] attrs = null)
        {
            if (data == null)
            {
                data = LiteTypeSystem.CreateInstance(type);
            }

            switch (data)
            {
                #region Primitive
                case bool v:
                    data = EditorGUILayout.Toggle(title, v);
                    break;
                case int v:
                    data = DrawIntField(title, v, type, attrs);
                    break;
                case long v:
                    data = EditorGUILayout.LongField(title, v);
                    break;
                case float v:
                    data = DrawFloatField(title, v, type, attrs);
                    break;
                case double v:
                    data = DrawDoubleField(title, v, type, attrs);
                    break;
                case string v:
                    data = DrawStringField(title, v, type, attrs);
                    break;
                case Enum v:
                    data = DrawEnumField(title, v, type, attrs);
                    break;
                #endregion
                #region UnityEngine
                case Rect v:
                    data = EditorGUILayout.RectField(title, v);
                    break;
                case RectInt v:
                    data = EditorGUILayout.RectIntField(title, v);
                    break;
                case Vector2 v:
                    data = EditorGUILayout.Vector2Field(title, v);
                    break;
                case Vector2Int v:
                    data = EditorGUILayout.Vector2IntField(title, v);
                    break;
                case Vector3 v:
                    data = EditorGUILayout.Vector3Field(title, v);
                    break;
                case Vector3Int v:
                    data = EditorGUILayout.Vector3IntField(title, v);
                    break;
                case Vector4 v:
                    data = EditorGUILayout.Vector4Field(title, v);
                    break;
                case Color v:
                    data = EditorGUILayout.ColorField(title, v);
                    break;
                #endregion
                #region List
                case IList v:
                    data = DrawList(title, v);
                    break;
                #endregion
                default:
                    data = DrawObjectInternal(title, data, data.GetType());
                    break;
            }
            return data;
        }

        private static readonly Dictionary<object, bool> Foldout_ = new Dictionary<object, bool>();
        private static object DrawObjectInternal(GUIContent title, object data, Type type)
        {
            if (data == null)
            {
                data = LiteTypeSystem.CreateInstance(type);
            }
            
            var depth = 0;
            if (title != GUIContent.none)
            {
                if (!Foldout_.ContainsKey(data))
                {
                    Foldout_.Add(data, true);
                }
                
                Foldout_[data] = EditorGUILayout.Foldout(Foldout_[data], title);
                // EditorGUILayout.LabelField(title);
                depth = 1;
                
                if (!Foldout_[data])
                {
                    return data;
                }
            }
            
            using (new GUILayout.VerticalScope())
            {
                EditorGUI.indentLevel += depth;
                if (!type.IsPrimitive && (type.IsClass || type.IsValueType))
                {
                    var enableSourceData = LiteEnableSourceAttribute.GetSourceData(data);
                    var fields = type.GetFields();

                    foreach (var field in fields)
                    {
                        if (LiteEnableCheckerAttribute.Check(field, enableSourceData))
                        {
                            DrawField(data, field);
                        }
                    }
                }
                else if (type.IsPrimitive)
                {
                    data = DrawElement(title, data, type);
                }
                else
                {
                    LiteEditorHelper.UnsupportedType(type);
                }
                EditorGUI.indentLevel -= depth;
            }

            return data;
        }

        private static void DrawField(object target, FieldInfo info)
        {
            var title = LiteEditorHelper.GetTitleFromFieldInfo(info);
            var fieldValue = info.GetValue(target);
            var attrs = info.GetCustomAttributes(true);
            fieldValue = DrawElement(title, fieldValue, info.FieldType, attrs);
            info.SetValue(target, fieldValue);
        }
        
        private static object DrawList(GUIContent title, IList data)
        {
            LiteReorderableListWrap.GetWrap(data)?.Draw(title);
            return data;
        }
    }
}