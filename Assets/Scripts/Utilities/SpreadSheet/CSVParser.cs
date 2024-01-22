using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Studio.Utilities.SpreadSheet
{
    public class CSVParser
    {
        private Hashtable columnMap = new Hashtable();
        private Type ClassTemplate;

        public List<T> ParseCSV<T>(string data)
        {
            DefineColumns(typeof(T));
            ArrayList itemList = LoadCSVFromString(data);
            return itemList.Cast<T>().ToList();
        }

        public List<T> ParseCSVFromResources<T>(string path)
        {
            DefineColumns(typeof(T));
            ArrayList itemList = LoadCSVFromString(path);
            return itemList.Cast<T>().ToList();
        }

        public void ExportCSV<T>(List<T> genericList, string finalPath)
        {
            var sb = new StringBuilder();
            var header = string.Empty;
            var info = typeof(T).GetFields();

            if (!File.Exists(finalPath))
            {
                var file = File.Create(finalPath);
                file.Close();
                foreach (var prop in info)
                {
                    header += prop.Name + "; ";
                }
                header = header.Substring(0, header.Length - 2);
                sb.AppendLine(header);
                TextWriter sw = new StreamWriter(finalPath, true);
                sw.Write(sb.ToString());
                sw.Close();
            }

            foreach (var obj in genericList)
            {
                sb = new StringBuilder();
                var line = string.Empty;
                foreach (var prop in info)
                {
                    line += prop.GetValue(obj) + "; ";
                }
                line = line.Substring(0, line.Length - 2);
                sb.AppendLine(line);
                TextWriter sw = new StreamWriter(finalPath, true);
                sw.Write(sb.ToString());
                sw.Close();
            }
        }

        public void DefineColumns(Type classDefinition)
        {
            ClassTemplate = classDefinition;
            columnMap = new Hashtable();
            MemberInfo[] members = classDefinition.GetFields();
            foreach (FieldInfo m in members)
            {
                columnMap[m.Name] = m;
            }
        }

        public ArrayList LoadCSVFromFile(string fileName)
        {
            TextAsset textAsset = (TextAsset)Resources.Load(fileName, typeof(TextAsset));

            if (textAsset == null)
            {
                return null;
            }
            else
            {
                return LoadCSVFromString(textAsset.text);
            }
        }

        public ArrayList LoadCSVFromString(string data)
        {
            string[] lines = data.Split('\n');

            int ctr = 0;
            ArrayList rows = new ArrayList();
            ArrayList columns = new ArrayList();
            foreach (string line in lines)
            {
                Regex csvread = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                string[] c = csvread.Split(line);

                for (int i = 0; i < c.Length; i++)
                {
                    c[i] = c[i].TrimStart(' ', '"');
                    c[i] = c[i].TrimEnd('"');
                }
                if (ctr == 0)
                {
                    foreach (string colName in c)
                    {
                        columns.Add(colName.Trim("\n\r ".ToCharArray())); // TODO remove trim
                    }
                }
                else
                {
                    object templated = Activator.CreateInstance(ClassTemplate);
                    for (int i = 0; i < c.Length; i++)
                    {
                        if (i > columnMap.Count - 1)
                        {
                            continue;
                        }

                        FieldInfo templateInfo = (FieldInfo)columnMap[columns[i]];
                        if (templateInfo == null)
                        {
                            Debug.LogError("CSV Field Not Found In ClassTemplate: " + columns[i].ToString() + "  length: " + columns[i].ToString().Length + "  in " + ClassTemplate);
                        }
                        Type colType = templateInfo.FieldType;

                        if (colType != null && c[i] != null && c[i].Length > 0)
                        {
                            if (colType == typeof(Vector3))
                            {
                                Vector3 useVector = new Vector3(float.Parse(c[i].Split(':')[0]), float.Parse(c[i].Split(':')[1]), float.Parse(c[i].Split(':')[2]));
                                templateInfo.SetValue(templated, useVector);
                            }
                            else if (colType == typeof(string[]))
                            {
                                string[] useList = c[i].Split('|');
                                templateInfo.SetValue(templated, useList);
                            }
                            else
                            {
                                try
                                {
                                    string withLineBreaks = InternalTools.ReplaceLineBreaks(c[i]);
                                    templateInfo.SetValue(templated, Convert.ChangeType(withLineBreaks, colType));
                                }
                                catch
                                {
#if UNITY_EDITOR
                                    Debug.Log("<color=red>Maybe</color> value of " + templateInfo.Name + " was empty: " + c[i] + "  type = " + colType.Name);
#endif
                                }
                            }
                        }
                    }
                    rows.Add(templated);
                }
                ctr++;
            }
            return rows;
        }
    }
}