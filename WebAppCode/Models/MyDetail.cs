using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp1.Models
{
    public class MyDetail
    {
        public int OrderNumber { get; set; } = 0;
        public string TableName { get; set; } = "";
        public string ColumnName { get; set; } = "";

        //ekstra filter yapmak için manuel yazılacak
        public string FilterColumnName { get; set; } = "";
        public string FilterOperator { get; set; } = "";
        public string FilterValue { get; set; } = "";

        //show şekli
        public string ShowType { get; set; } = ""; //Page, Popup
        public string ViewType { get; set; } = ""; //Form, Grid, TreeList
        public Boolean FormViewUse { get; set; } = false;
        public Boolean GridViewUse { get; set; } = false;
        public Boolean TreeListUse { get; set; } = false;
        public Boolean CountShow { get; set; } = false;


    }

    // bu class ı kullanmadan, normal field listesine eklemek daha uygun olur 
    //public class MyComputedField
    //{
    //    /*Computed Field belirleme*/
    //    public int OrderNumber { get; set; } = 0;
    //    public string ColumnName { get; set; } = ""; // Cf+ValueColumnName+RefTextColumnNames vb bir isim verilebilir
    //    public string ValueColumnName { get; set; } = "";
    //    public string RefTableName { get; set; } = "";
    //    public string RefValueColumnName { get; set; } = "";
    //    public string RefTextColumnNames { get; set; } = "";


    //    public Boolean FormViewUse { get; set; } = false;
    //    public Boolean GridViewUse { get; set; } = false;
    //    public Boolean TreeListUse { get; set; } = false;
    //}
}
