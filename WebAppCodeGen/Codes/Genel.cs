using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Text.Json;
using WebAppCodeGen.Models;
using WebApp1.Models;

namespace WebApp1.Codes
{
    #region MyApp

    public class MyApp
    {
        public static IWebHostEnvironment? Env { get; set; } = null;

        #region app Version
        public static string Version
        {
            get
            {
                string Version = "1.0.0.0";
                if (Env?.EnvironmentName == "Development")
                {
                    Version += "_";
                    Version += DateTime.Now.ToString("MMddHHmmss");
                }
                return Version;
            }
        }
        #endregion

        #region Translate

        public static string TranslateWithApi(string _text, string _target, string _source, Boolean _titleCase)
        {
            //https://codepen.io/fionnachan/pen/vmaMqm?editors=1111 den alındı bu yöntem
            //https://translate.googleapis.com/translate_a/single?client=gtx&sl=tr&tl=en&dt=t&q=timsah
            string rV = "";
            string url = "https://translate.googleapis.com/translate_a/single?client=gtx&dt=t";
            //
            url += "&sl=" + _source;
            url += "&tl=" + _target;
            url += "&q=" + _text;

            using (var client = new System.Net.Http.HttpClient())
            {
                string jsonText = client.GetStringAsync(url).Result;

                var stuff = JsonSerializer.Deserialize<List<dynamic>>(jsonText);
                if (stuff != null)
                {
                    rV = Convert.ToString(stuff[0][0][0]);
                }
            }

            if (_titleCase)
            {
                rV = rV.MyToTitleCase();
            }

            return rV.Trim();
        }


        #endregion

        #region DB Compare
        public static System.Text.StringBuilder CompareDatabase(string sourceConStr, string targetConStr)
        {
            var logs = new System.Text.StringBuilder();

            var kaynakSchema = new DatabaseSchemaReader.DatabaseReader(new SqlConnection(sourceConStr)).ReadAll();
            var hedefSchema = new DatabaseSchemaReader.DatabaseReader(new SqlConnection(targetConStr)).ReadAll();

            var comparison = new DatabaseSchemaReader.Compare.CompareSchemas(hedefSchema, kaynakSchema);

            logs.Append(comparison.Execute());

            return logs;
        }
        #endregion
    }

    #endregion
}
