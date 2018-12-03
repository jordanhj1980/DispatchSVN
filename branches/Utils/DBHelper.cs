﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DispatchApp
{
    public interface DBHelper
    {
        /// <summary>
        /// 执行 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        int ExecuteNonQuery(System.Data.Common.DbConnection conn, System.Data.CommandType cmdType, string cmdText, params System.Data.Common.DbParameter[] cmdParms);
        
        /// <summary>
        /// 在事务中执行 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        int ExecuteNonQuery(System.Data.Common.DbTransaction trans, System.Data.CommandType cmdType, string cmdText, params System.Data.Common.DbParameter[] cmdParms);

        /// <summary>
        /// 在事务中执行查询，返回DataSet
        /// </summary>
        DataSet ExecuteQuery(System.Data.Common.DbTransaction trans, System.Data.CommandType cmdType, string cmdText, params System.Data.Common.DbParameter[] cmdParms);

        /// <summary>
        /// 执行查询，返回DataSet
        /// </summary>
        DataSet ExecuteQuery(System.Data.Common.DbConnection conn, System.Data.CommandType cmdType, string cmdText, params System.Data.Common.DbParameter[] cmdParms);

        /// <summary>
        /// 在事务中执行查询，返回DataReader
        /// </summary>
        System.Data.Common.DbDataReader ExecuteReader(System.Data.Common.DbTransaction trans, System.Data.CommandType cmdType, string cmdText, params System.Data.Common.DbParameter[] cmdParms);

        /// <summary>
        /// 执行查询，返回DataReader
        /// </summary>
        System.Data.Common.DbDataReader ExecuteReader(System.Data.Common.DbConnection conn, System.Data.CommandType cmdType, string cmdText, params System.Data.Common.DbParameter[] cmdParms);

        /// <summary>
        /// 在事务中执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        object ExecuteScalar(System.Data.Common.DbTransaction trans, System.Data.CommandType cmdType, string cmdText, params System.Data.Common.DbParameter[] cmdParms);

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        object ExecuteScalar(System.Data.Common.DbConnection conn, System.Data.CommandType cmdType, string cmdText, params System.Data.Common.DbParameter[] cmdParms);

        /// <summary>
        /// 得到数据条数
        /// </summary>
        /// <param name="tblName">表名</param>
        /// <param name="condition">条件(不需要where)</param>
        /// <returns>数据条数</returns>
        int GetCount(System.Data.Common.DbConnection conn, string tblName, string condition);
    }
}
