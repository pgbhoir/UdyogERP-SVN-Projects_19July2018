using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApplication1
{
    class SqlTableCreator
    {

        #region Static Methods
        public static string GetCreateFromDataTableSQL(string tableName, DataTable table)
        {
            string sql = "CREATE TABLE [" + tableName + "] (\n";
            // columns
         
           foreach (DataColumn column in table.Columns)
            {
                sql += "[" + column.ColumnName + "] " + SQLGetType(column) + ",\n";
            }
            sql = sql.TrimEnd(new char[] { ',', '\n' }) + "\n";
            // primary keys
            if (table.PrimaryKey.Length > 0)
            {
                sql += "CONSTRAINT [PK_" + tableName + "] PRIMARY KEY CLUSTERED (";
                foreach (DataColumn column in table.PrimaryKey)
                {
                    sql += "[" + column.ColumnName + "],";
                }
                sql = sql.TrimEnd(new char[] { ',' }) + "))\n";
            }
            else
                sql += ")";
            string Finalsql = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + tableName + "]') AND type in (N'U'))\n";
            Finalsql += "Begin\n";
            Finalsql += "Drop table "+ tableName + "\n";
            Finalsql += "end\n";// +sql;
            return sql;
        }

        //Added by Priyanka B on 05012018 for Bug-31076 Start
        public static string GetCreateFromDataTableSQL(string tableName, DataTable sqlTable, DataTable xmlTable)
        {
            string sql = "CREATE TABLE [" + tableName + "] (\n";
            string sql_1 = "", sqlPrimaryCol = "";
            bool isFound = false, isPrimary = false;
            // columns
            foreach (DataColumn column in xmlTable.Columns)
            {
                foreach (DataRow row in sqlTable.Rows)
                {
                    if (row.ItemArray[1].ToString() == "UID")
                    {
                    }
                    if (Boolean.Parse(row.ItemArray[7].ToString()) == true)
                    {
                        if (!sqlPrimaryCol.Contains("[" + row.ItemArray[1].ToString() + "]"))
                            sqlPrimaryCol += "[" + row.ItemArray[1].ToString() + "],";
                        else
                            sqlPrimaryCol += "";
                        isPrimary = true;
                    }
                    if (column.ColumnName == row.ItemArray[1].ToString())
                    {
                        sql += row.ItemArray[8].ToString() + ",\n";
                        sqlTable.Rows.Remove(row);
                        isFound = true;
                        break;
                    }
                    else if (isFound == false)
                    {
                        sql_1 = "[" + column.ColumnName + "] " + SQLGetType(column) + ",\n";
                        isFound = true;
                    }
                }
                if (isFound == true)
                {
                    if (!sql.Contains(sql_1.Substring(1, sql_1.IndexOf("]") - 1)))
                        sql = sql + sql_1;
                    else
                        sql += "";
                    isFound = false;
                }
            }
            sql = sql.TrimEnd(new char[] { ',', '\n' }) + "\n";

            // primary keys
            if (isPrimary == true)
            {
                sql += "CONSTRAINT [PK_" + tableName + "] PRIMARY KEY CLUSTERED (" + sqlPrimaryCol;
                sql = sql.TrimEnd(new char[] { ',' }) + "))\n";
            }
            else
                sql += ")";
            return sql;
        }
        //Added by Priyanka B on 05012018 for Bug-31076 End

        // Return T-SQL data type definition, based on schema definition for a column
        public static string SQLGetType(object type, int columnSize, int numericPrecision, int numericScale)
        {
            switch (type.ToString())
            {
                case "System.String":
                    return "VARCHAR(" + ((columnSize == -1) ? 255 : columnSize) + ")";
                case "System.Decimal":
                    if (numericScale > 0)
                        return "REAL";
                    else if (numericPrecision > 10)
                        return "BIGINT";
                    else
                        return "INT";
                case "System.Double":
                case "System.Single":
                    return "REAL";
                case "System.Int64":
                    return "BIGINT";
                case "System.Int16":
                case "System.Int32":
                    return "INT";
                case "System.DateTime":
                    return "DATETIME";
                case "System.Boolean":
                    return "BIT";
                case "System.Byte":
                    return "TINYINT";
                default:
                    throw new Exception(type.ToString() + " not implemented.");
            }
        }
        public static string SQLGetType(DataColumn column)
        {

            return SQLGetType(column.DataType, column.MaxLength, 10, 2);
        }
        #endregion
    }
}

