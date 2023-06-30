using System;
using System.Collections.Generic;

namespace WebApp1.Models
{
    public class MyTableOption
    {
        public string TableName { get; set; } = "";
        public string PrimaryKey { get; set; } = "";
        public string PrimaryKeyNetType { get; set; } = "";
        public Boolean PrimaryKeyIsNumeric { get; set; } = false;
        public string SequenceName { get; set; } = "";
        public string TableCuds { get; set; } = "C,U,D"; // C:Create, U:Update, D:Delete

        public string LangKeyRoot { get; set; } = "";
        public string TableDictionaryTitleTr { get; set; } = "";
        public string TableDictionaryTitleEn { get; set; } = "";
        public string TableDictionaryShortTitleTr { get; set; } = "";
        public string TableDictionaryShortTitleEn { get; set; } = "";

        public string DataTransferObjectName { get; set; } = "";
        public string DataTransferObjectNameSpace { get; set; } = "";

        public string DataManipulationObjectName { get; set; } = "";

        public string RepositoryName { get; set; } = "";

        public string ControllerName { get; set; } = "";

        public string FormViewName { get; set; } = "";
        public string FormViewTabDictionaryTr { get; set; } = "";
        public string FormViewTabDictionaryEn { get; set; } = "";

        public string GridViewName { get; set; } = "";
        public string GridViewTabDictionaryTr { get; set; } = "";
        public string GridViewTabDictionaryEn { get; set; } = "";
        public string GridViewCrudEditorType { get; set; } = ""; //Inline, Page, Popup        
        public string GridViewMasterColumnName { get; set; } = ""; //bir gridin detayı, grid olduğunda detaydakinin master alanı soruluyor
        public string GridViewRowStyleStatusColumnName { get; set; } = ""; //bir gridin row text color belirlenmesindeki data column name
        public int GridViewDataSourcePageSize { get; set; } = 10;

        public string TreeListName { get; set; } = "";
        public string ParentColumnName { get; set; } = "";

        public string SearchViewName { get; set; } = "";
        public string SearchViewType { get; set; } = ""; //GridView, TreeList   
        public string SearchViewCrudEditorType { get; set; } = ""; //Inline, Page, Popup

        public string SearchViewTabDictionaryTr { get; set; } = "";
        public string SearchViewTabDictionaryEn { get; set; } = "";


        public Boolean DataTransferObjectLock { get; set; } = false;
        public Boolean DataManipulationObjectLock { get; set; } = false;
        public Boolean ControllerLock { get; set; } = false;
        public Boolean DictionaryLock { get; set; } = false;
        public Boolean FormViewLock { get; set; } = false;
        public Boolean GridViewLock { get; set; } = false;
        public Boolean TreeListLock { get; set; } = false;
        public Boolean SearchViewLock { get; set; } = false;

        public Boolean DataTransferObjectAutoCreate { get; set; } = false;
        public Boolean DataManipulationObjectAutoCreate { get; set; } = false;
        public Boolean ControllerAutoCreate { get; set; } = false;
        public Boolean DictionaryAutoCreate { get; set; } = false;
        public Boolean FormViewAutoCreate { get; set; } = false;
        public Boolean GridViewAutoCreate { get; set; } = false;
        public Boolean TreeListAutoCreate { get; set; } = false;
        public Boolean SearchViewAutoCreate { get; set; } = false;

        public List<MyField> Fields { get; set; } = new List<MyField>();

        public List<MyDetail> Details { get; set; } = new List<MyDetail>();

    }

}
