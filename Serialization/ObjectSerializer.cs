using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace PUnity.Serialization
{
    public static class ObjectSerializer
    {
        public static string SerializeObject(object targetObject)
        {
            JSONObject js = new JSONObject();
            SerializeObjectFields(targetObject, js);
            return js.ToString();
        }

        //static void SerializeObjectFields(object targetObject, JSONNode data, object rootObject = null)
        static void SerializeObjectFields(object targetObject, JSONNode data)
        {

            FieldInfo[] fieldInfos = targetObject.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                try
                {
                    SerializeField(fieldInfos[i], data, targetObject);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(LogTags.SYSTEM_ERROR + "");
                }
            }
        }

        static void SerializeField(FieldInfo field, JSONNode data, object targetObject)
        {
            Type fieldType = field.FieldType;

            if (fieldType.IsGenericType)
                fieldType = fieldType.GetGenericTypeDefinition();

            if (fieldType.IsArray)
                fieldType = typeof(Array);

            object fieldValue = field.GetValue(targetObject);

            if (fieldType == typeof(int) || fieldType.IsEnum)
            {
                data[field.Name] = (int)fieldValue;
            }
            else if (fieldType == typeof(float))
            {
                data[field.Name] = (float)fieldValue;
            }
            else if (fieldType == typeof(bool))
            {
                data[field.Name] = (bool)fieldValue;
            }
            else if (fieldType == typeof(string))
            {
                data[field.Name] = (string)fieldValue;
            }
            else if (fieldType == typeof(Array))
            {
                Array arr = (Array)fieldValue;
                Type elementType = arr.GetType().GetElementType();

                if (arr.Rank == 1)
                {
                    data[field.Name]["Lenght"].AsInt = arr.GetLength(0);

                    if (elementType.IsPrimitive || elementType == typeof(string))
                    {
                        if (elementType == typeof(int) || elementType.IsEnum)
                        {
                            for (int i = 0; i < arr.Length; i++)
                                data[field.Name]["Array"][i] = (int)arr.GetValue(i);
                        }
                        else if (elementType == typeof(float))
                        {
                            for (int i = 0; i < arr.Length; i++)
                                data[field.Name]["Array"][i] = (float)arr.GetValue(i);
                        }
                        else if (elementType == typeof(bool))
                        {
                            for (int i = 0; i < arr.Length; i++)
                                data[field.Name]["Array"][i] = (bool)arr.GetValue(i);
                        }
                        else if (elementType == typeof(string))
                        {
                            for (int i = 0; i < arr.Length; i++)
                                data[field.Name]["Array"][i] = (string)arr.GetValue(i);
                        }
                    }
                    else if (elementType.IsClass || (elementType.IsValueType && !elementType.IsEnum))
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {
                            object item = arr.GetValue(i);
                            data[field.Name]["Array"][i]["Type"] = item.GetType().FullName;
                            SerializeObjectFields(item, data[field.Name]["Array"][i]["Value"].AsObject);
                        }
                    }
                }
                else if (arr.Rank == 2)
                {
                    data[field.Name]["Lenght1"].AsInt = arr.GetLength(0);
                    data[field.Name]["Lenght2"].AsInt = arr.GetLength(1);

                    if (elementType.IsPrimitive || elementType == typeof(string))
                    {
                        if (elementType == typeof(int) || elementType.IsEnum)
                        {
                            for (int i = 0; i < arr.GetLength(0); i++)
                                for (int j = 0; j < arr.GetLength(1); j++)
                                    data[field.Name]["Array"][i][j] = (int)arr.GetValue(i, j);
                        }
                        else if (elementType == typeof(float))
                        {
                            for (int i = 0; i < arr.GetLength(0); i++)
                                for (int j = 0; j < arr.GetLength(1); j++)
                                    data[field.Name]["Array"][i][j] = (float)arr.GetValue(i, j);
                        }
                        else if (elementType == typeof(bool))
                        {
                            for (int i = 0; i < arr.GetLength(0); i++)
                                for (int j = 0; j < arr.GetLength(1); j++)
                                    data[field.Name]["Array"][i][j] = (bool)arr.GetValue(i, j);
                        }
                        else if (elementType == typeof(string))
                        {
                            for (int i = 0; i < arr.GetLength(0); i++)
                                for (int j = 0; j < arr.GetLength(1); j++)
                                    data[field.Name]["Array"][i][j] = (string)arr.GetValue(i, j);
                        }
                    }
                    else if (elementType.IsClass || (elementType.IsValueType && !elementType.IsEnum))
                    {
                        for (int i = 0; i < arr.GetLength(0); i++)
                        {
                            for (int j = 0; j < arr.GetLength(1); j++)
                            {
                                object item = arr.GetValue(i, j);
                                data[field.Name]["Array"][i][j]["Type"] = item.GetType().AssemblyQualifiedName;
                                SerializeObjectFields(item, data[field.Name]["Array"][i][j]["Value"].AsObject);
                            }
                        }
                    }
                }
            }
            else if (fieldType == typeof(List<>))
            {
                Type genericArgument = field.FieldType.GetGenericArguments()[0];
                ICollection list = fieldValue as ICollection;

                if (genericArgument.IsPrimitive || genericArgument == typeof(string))
                {
                    int i = 0;

                    if (genericArgument == typeof(int) || genericArgument.IsEnum)
                    {
                        foreach (var item in list)
                        {
                            data[field.Name][i] = (int)item;
                            i++;
                        }
                    }
                    else if (genericArgument == typeof(float))
                    {
                        foreach (var item in list)
                        {
                            data[field.Name][i] = (float)item;
                            i++;
                        }
                    }
                    else if (genericArgument == typeof(bool))
                    {
                        foreach (var item in list)
                        {
                            data[field.Name][i] = (bool)item;
                            i++;
                        }
                    }
                    else if (genericArgument == typeof(string))
                    {
                        foreach (var item in list)
                        {
                            data[field.Name][i] = (string)item;
                            i++;
                        }
                    }
                }
                else if (genericArgument.IsClass || (genericArgument.IsValueType && !genericArgument.IsEnum))
                {
                    int i = 0;
                    foreach (var item in list)
                    {
                        data[field.Name][i]["Type"] = item.GetType().AssemblyQualifiedName;
                        SerializeObjectFields(item, data[field.Name][i]["Value"].AsObject);

                        i++;
                    }
                }
            }
            else
            {
                SerializeObjectFields(fieldValue, data[field.Name]["value"].AsObject);
            }
        }
    }
}