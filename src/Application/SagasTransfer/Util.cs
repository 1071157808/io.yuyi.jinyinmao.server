// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Util.cs
// Created          : 2015-08-11  4:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-11  4:40 PM
// ***********************************************************************
// <copyright file="Util.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Serilog;
using Yuyi.Jinyinmao.Domain;

namespace SagasTransfer
{
    public static class Util
    {
        /// <summary>
        ///     保存csv文件
        /// </summary>
        public static string GetCsvContent<T>(IEnumerable<T> listModel) where T : class, new()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                //通过反射 显示要显示的列
                BindingFlags bf = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static; //反射标识
                Type objType = typeof(T);
                PropertyInfo[] propInfoArr = objType.GetProperties(bf);
                string header = string.Empty;
                List<string> listPropertys = new List<string>();
                foreach (PropertyInfo info in propInfoArr.Where(info => string.CompareOrdinal(info.Name.ToUpper(), "ID") != 0))
                {
                    if (!listPropertys.Contains(info.Name))
                    {
                        listPropertys.Add(info.Name);
                    }
                    header += info.Name + ",";
                }
                sb.AppendLine(header.Trim(',')); //csv头

                foreach (T model in listModel)
                {
                    string strModel = string.Empty;
                    foreach (string strProp in listPropertys)
                    {
                        foreach (PropertyInfo modelProperty in from propInfo in propInfoArr where string.CompareOrdinal(propInfo.Name.ToUpper(), strProp.ToUpper()) == 0 select model.GetType().GetProperty(propInfo.Name))
                        {
                            if (modelProperty != null)
                            {
                                object objResult = modelProperty.GetValue(model, null);

                                string result = (objResult ?? string.Empty).ToString().Trim();
                                if (result.IndexOf(',') != -1)
                                {
                                    result = "\"" + result.Replace("\"", "\"\"") + "\""; //特殊字符处理 ？
                                    //result = result.Replace("\"", "“").Replace(',', '，') + "\"";
                                }
                                if (!string.IsNullOrEmpty(result))
                                {
                                    Type valueType = modelProperty.PropertyType;
                                    if (valueType == typeof(decimal?))
                                    {
                                        result = decimal.Parse(result).ToString("#.#");
                                    }
                                    else if (valueType == typeof(decimal))
                                    {
                                        result = decimal.Parse(result).ToString("#.#");
                                    }
                                    else if (valueType == typeof(double?))
                                    {
                                        result = double.Parse(result).ToString("#.#");
                                    }
                                    else if (valueType == typeof(double))
                                    {
                                        result = double.Parse(result).ToString("#.#");
                                    }
                                    else if (valueType == typeof(float?))
                                    {
                                        result = float.Parse(result).ToString("#.#");
                                    }
                                    else if (valueType == typeof(float))
                                    {
                                        result = float.Parse(result).ToString("#.#");
                                    }
                                }
                                strModel += result + ",";
                            }
                            else
                            {
                                strModel += ",";
                            }
                            break;
                        }
                    }
                    strModel = strModel.Substring(0, strModel.Length - 1);
                    sb.AppendLine(strModel);
                }
                string content = sb.ToString();
                return content;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }

        public static SagaStateRecordResult InitData(SagaStateRecord s)
        {
            return new SagaStateRecordResult
            {
                PartitionKey = s.PartitionKey,
                RowKey = s.RowKey,
                BeginTime = s.BeginTime,
                CurrentProcessingStatus = s.CurrentProcessingStatus,
                Info = s.Info,
                Message = s.Message,
                SagaId = s.SagaId,
                SagaState = s.SagaState,
                SagaType = s.SagaType,
                State = s.State,
                UpdateTime = s.UpdateTime
            };
        }

        public static async Task SaveToFile<T>(CloudTable table, string path) where T : TableEntity, new()
        {
            try
            {
                string fullName = Path.Combine(path, DateTime.Now.ToString("yyyyMMdd") + ".csv");
                if (File.Exists(fullName))
                {
                    File.Delete(fullName);
                }

                TableQuery<T> query = new TableQuery<T>();
                TableContinuationToken token = null;
                using (StreamWriter writer = new StreamWriter(
                    new FileStream(fullName, FileMode.Append, FileAccess.Write, FileShare.Write), Encoding.UTF8))
                {
                    do
                    {
                        TableQuerySegment<T> segement =
                            await table.ExecuteQuerySegmentedAsync(query, token);
                        token = segement.ContinuationToken;
                        writer.WriteLine(GetCsvContent(segement.Results));
                    } while (token != null);
                }
            }
            catch (Exception ex)
            {
                new LoggerConfiguration().WriteTo.RollingFile("Error/Log-{Date}.txt")
                    .CreateLogger()
                    .Error("{@ex}", ex.GetBaseException());
            }
        }
    }
}