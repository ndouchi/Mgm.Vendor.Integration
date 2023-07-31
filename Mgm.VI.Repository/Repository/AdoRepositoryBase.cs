//////////////////////////////////////////////////////////////////
using Mgm.VI.Common;
using Mgm.VI.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static Mgm.VI.Common.Constants.Db;

namespace Mgm.VI.Repository
{
    public abstract class AdoRepositoryBase<T> where T : class
    {
        private SqlConnection _connection;

        //private List<T> items;
        //public List<T> Items { get; set; }
        public IErrorMessages ErrorMessages { get; set; }

        protected string ConnectionString { get; private set; }

        public AdoRepositoryBase(string connectionString, IErrorMessages errorMessages = null)
        {
            Initialize(connectionString, errorMessages);
        }

        private void Initialize(string connectionString, IErrorMessages errorMessages)
        {
            ErrorMessages = errorMessages ?? new ErrorMessages();
            ConnectionString = connectionString;
            _connection = new SqlConnection(ConnectionString);
        }

        protected bool IsSqlSafe(string content)
        {
            bool isSqlSafe = !(content.Contains("'") || content.Contains("\"") || content.Contains(";"));
            if (!isSqlSafe) ErrorMessages.Add("AdoRepositoryBase::IsSqlSafe(content)", content);
            return isSqlSafe;
        }
        protected int Add(SqlCommand command)
        {
            int addedReturnValue = 0;
            command.Connection = _connection;
            _connection.Open();
            try
            {
                addedReturnValue = (int)command.ExecuteScalar();

                if (addedReturnValue <= 0)
                    ErrorMessages.Add("AdoRepositoryBase::Add(...)",
                                        String.Format("Error returned: {0}", addedReturnValue));
            }
            catch (Exception e)
            {
                ErrorMessages.Add(String.Format("AdoRepositoryBase::Add({0})", command.CommandText), e.Message, e);
            }
            finally
            {
                _connection.Close();
            }
            return addedReturnValue;
        }
        protected int Delete(SqlCommand command)
        {
            int recordsDeleted = 0;
            command.Connection = _connection;
            _connection.Open();
            try
            {
                recordsDeleted = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ErrorMessages.Add(String.Format("AdoRepositoryBase::Delete({0})", command.CommandText), e.Message, e);
            }
            finally
            {
                _connection.Close();
            }
            return recordsDeleted;
        }
        protected int Update(SqlCommand command)
        {
            int recordsUpdated = 0;
            command.Connection = _connection;
            _connection.Open();
            try
            {
                recordsUpdated = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ErrorMessages.Add(String.Format("AdoRepositoryBase::Update({0})", command.CommandText), e.Message, e);
            }
            finally
            {
                _connection.Close();
            }
            return recordsUpdated;
        }
        protected IEnumerable<TC> GetRecords<TC, IT>(SqlCommand command) 
                                                    where TC : IT, new()
                                                    where IT : IModel
        {
            var list = new List<TC>();
            TC record;
            command.Connection = _connection;
            _connection.Open();
            try
            {
                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        record = new TC();
                        ModelBase.FillModelFromReader(reader, record);
                        list.Add(record);
                    }
                }
                #region catch inner
                catch (Exception e)
                {
                    ErrorMessages.Add(String.Format("AdoRepositoryBase::GetRecords({0})::Read", command.CommandText), e.Message, e);
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
                #endregion catch inner
            }
            #region catch outer
            catch (Exception e)
            {
                ErrorMessages.Add(String.Format("AdoRepositoryBase::GetRecords({0})", command.CommandText), e.Message, e);
            }
            finally
            {
                _connection.Close();
            }
            #endregion catch outer
            return list;
        }
        protected TC GetRecord<TC, IT>(SqlCommand command) where TC : IT, new()
                                                           where IT : IModel
        {
            TC record = new TC();
            command.Connection = _connection;
            _connection.Open();
            try
            {
                var reader = command.ExecuteReader();
                try
                {
                    ModelBase.FillModelFromReader(reader, record);
                }
                #region catch inner
                catch (Exception e)
                {
                    ErrorMessages.Add(String.Format("AdoRepositoryBase::GetRecord({0})::reader.Read()", command.CommandText), e.Message, e);
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
                #endregion catch inner
            }
            #region catch outer
            catch (Exception e)
            {
                ErrorMessages.Add(String.Format("AdoRepositoryBase::GetRecord({0})", command.CommandText), e.Message, e);
            }
            finally
            {
                _connection.Close();
            }
            #endregion catch outer
            return record;
        }
        protected IEnumerable<TC> ExecuteStoredProcedure<TC,IT>(SqlCommand command) 
                                                                where TC : IT, new()
                                                                where IT : IModel
        {
            var list = new List<TC>();
            TC record;
            command.Connection = _connection;
            command.CommandType = CommandType.StoredProcedure;
            _connection.Open();
            try
            {
                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        record = new TC();
                        ModelBase.FillModelFromReader(reader, record);
                        if (record != null) list.Add(record);
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }
            finally
            {
                _connection.Close();
            }
            return list;
        }

    }
}