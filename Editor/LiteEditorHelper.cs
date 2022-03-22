using System;
using System.IO;
using System.Reflection;
using LiteUnity.Runtime.Attributes;
using UnityEngine;

namespace LiteUnity.Editor
{
    public static class LiteEditorHelper
    {
        public static T GetAttribute<T>(Type type, object[] attrs) where T : Attribute
        {
            T result = null;
            if (attrs != null)
            {
                result = (T)Array.Find(attrs, t => t is T);
            }

            if (result == null)
            {
                result = type.GetCustomAttribute<T>();
            }
            return result;
        }
        
        public static GUIContent GetTitleFromFieldInfo(FieldInfo info)
        {
            var labelAttr = info.GetCustomAttribute<LiteLabelAttribute>();
            var title = labelAttr != null ? new GUIContent(labelAttr.Label) : new GUIContent(info.Name);
            return title;
        }
        
        public static void UnsupportedType(Type type)
        {
            Debug.LogError($"Unsupported type: {type}");
        }

        public static void OpenFolder(string folderPath)
        {
            if (!Path.IsPathRooted(folderPath))
            {
                folderPath = Path.Combine(Application.dataPath, "..", folderPath).Replace("\\", "/");
            }
            var path = $"file://{folderPath}";
            Application.OpenURL(path);
        }
    }
}