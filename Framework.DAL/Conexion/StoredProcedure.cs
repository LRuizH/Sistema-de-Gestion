using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualBasic;

public class StoredProcedure
{
    private string mNombreProcedimiento;
    private List<StoredProcedureParameter> mParametros;
    
    public string Nombre
    {
        get
        {
            return mNombreProcedimiento;
        }
        set
        {
            mNombreProcedimiento = value;
        }
    }

    public List<StoredProcedureParameter> Parametros
    {
        get
        {
            return mParametros;
        }
        set
        {
            mParametros = value;
        }
    }

    // Solo recibe el nombre del procedimiento e inicializa la colección.
    public StoredProcedure(string nNombre)
    {
        try
        {
            Nombre = nNombre;
            Parametros = new List<StoredProcedureParameter>();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    // Constructor Vacio.
    public StoredProcedure()
    {
        Parametros = new List<StoredProcedureParameter>();
    }

    /// <summary>
    // Agrega los parametros del procedimiento y su respectivo valor.
    /// </summary>
    public void AgregarParametro(string pVariable, object pValor)
    {
        try
        {
            StoredProcedureParameter iParametro = new StoredProcedureParameter("@" + pVariable, pValor);
            this.Parametros.Add(iParametro);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// Ejecuta el procedimiento almacenado retornando un DataSet
    /// </summary>
    public DataSet EjecutarProcedimiento()
    {
        try
        {
            ConexionSQL Conn = new ConexionSQL();
            SqlCommand sqlCmd = new SqlCommand(this.Nombre, Conn.AbrirConeccion())
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 4000
            };

            //StoredProcedureParameter mParametro;

            // Agrega las variables al procedimiento almacenado
            foreach (var mParametro in this.Parametros)
            {
                if (mParametro.Valor != null)
                    sqlCmd.Parameters.AddWithValue(mParametro.Variable, mParametro.Valor);
                else
                    sqlCmd.Parameters.AddWithValue(mParametro.Variable, DBNull.Value);
            }

            // SqlAdapter utiliza el SqlCommand para llenar el Dataset
            SqlDataAdapter sda = new SqlDataAdapter(sqlCmd);

            // Se llena el dataset
            DataSet ds = new DataSet();
            sda.Fill(ds);
            sda.Dispose();

            Conn.CerrarConeccion();
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///     ''' Ejecuta el procedimiento almacenado retornando una lista generica
    ///     ''' </summary>
    public List<T> EjecutarProcedimiento<T>()
    {
        try
        {
            ConexionSQL Conn = new ConexionSQL();
            SqlCommand sqlCmd = new SqlCommand(this.Nombre, Conn.AbrirConeccion())
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 4000
            };

            // Agrega las variables al procedimiento almacenado
            foreach (var mParametro in this.Parametros)
            {
                if (mParametro.Valor != null)
                    sqlCmd.Parameters.AddWithValue(mParametro.Variable, mParametro.Valor);
                else
                    sqlCmd.Parameters.AddWithValue(mParametro.Variable, DBNull.Value);
            }

            // SqlAdapter utiliza el SqlCommand para llenar el Dataset
            SqlDataReader reader = sqlCmd.ExecuteReader();

            var result = DataReaderMapToList<T>(reader);
            Conn.CerrarConeccion();
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    // Ejecuta un script de consulta retornando una lista generica.
    public List<T> EjecutarConsulta<T>(string consulta)
    {
        try
        {
            ConexionSQL Conn = new ConexionSQL();
            SqlCommand sqlCmd = new SqlCommand(this.Nombre, Conn.AbrirConeccion())
            {
                CommandType = CommandType.Text,
                CommandTimeout = 4000,
                CommandText = consulta
            };

            if (this.Parametros != null)
            {
                // Agrega las variables al procedimiento almacenado
                foreach (var mParametro in this.Parametros)
                {
                    if (mParametro.Valor != null)
                        sqlCmd.Parameters.AddWithValue(mParametro.Variable, mParametro.Valor);
                    else
                        sqlCmd.Parameters.AddWithValue(mParametro.Variable, DBNull.Value);
                }
            }

            // 'SqlAdapter utiliza el SqlCommand para llenar el Dataset
            SqlDataReader reader = sqlCmd.ExecuteReader();

            var result = DataReaderMapToList<T>(reader);
            Conn.CerrarConeccion();
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    // Ejecuta un script de consulta
    // Public Function EjecutarConsulta(ByVal servidor As String, ByVal base_datos As String, ByVal usuario As String, ByVal password As String, ByVal consulta As String) As DataSet
    public DataSet EjecutarConsulta(string consulta)
    {
        try
        {
            ConexionSQL Conn = new ConexionSQL();
            SqlCommand sqlCmd = new SqlCommand(this.Nombre, Conn.AbrirConeccion())
            {
                CommandType = CommandType.Text,
                CommandTimeout = 4000,
                CommandText = consulta
            };

            if (this.Parametros != null)
            {
                // Agrega las variables al procedimiento almacenado
                foreach (var mParametro in this.Parametros)
                {
                    if (mParametro.Valor != null)
                        sqlCmd.Parameters.AddWithValue(mParametro.Variable, mParametro.Valor);
                    else
                        sqlCmd.Parameters.AddWithValue(mParametro.Variable, DBNull.Value);
                }
            }

            // SqlAdapter utiliza el SqlCommand para llenar el Dataset
            SqlDataAdapter sda = new SqlDataAdapter(sqlCmd);

            // Se llena el dataset
            DataSet ds = new DataSet();
            sda.Fill(ds);

            Conn.CerrarConeccion();
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    #region "Consultas con conexión a Reportes"

    /// <summary>
    /// Ejecuta el procedimiento almacenado retornando un DataSet
    /// </summary>
    public DataSet EjecutarProcedimientoReportes()
    {
        try
        {
            ConexionSQL Conn = new ConexionSQL(true);
            SqlCommand sqlCmd = new SqlCommand(this.Nombre, Conn.AbrirConeccion())
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 4000
            };

            //StoredProcedureParameter mParametro;

            // Agrega las variables al procedimiento almacenado
            foreach (var mParametro in this.Parametros)
            {
                if (mParametro.Valor != null)
                    sqlCmd.Parameters.AddWithValue(mParametro.Variable, mParametro.Valor);
                else
                    sqlCmd.Parameters.AddWithValue(mParametro.Variable, DBNull.Value);
            }

            // SqlAdapter utiliza el SqlCommand para llenar el Dataset
            SqlDataAdapter sda = new SqlDataAdapter(sqlCmd);

            // Se llena el dataset
            DataSet ds = new DataSet();
            sda.Fill(ds);

            Conn.CerrarConeccion();
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// Ejecuta el procedimiento almacenado retornando una lista generica
    /// </summary>
    public List<T> EjecutarProcedimientoReportes<T>()
    {
        try
        {
            ConexionSQL Conn = new ConexionSQL(true);
            SqlCommand sqlCmd = new SqlCommand(this.Nombre, Conn.AbrirConeccion())
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 4000
            };

            // Agrega las variables al procedimiento almacenado
            foreach (var mParametro in this.Parametros)
            {
                if (mParametro.Valor != null)
                    sqlCmd.Parameters.AddWithValue(mParametro.Variable, mParametro.Valor);
                else
                    sqlCmd.Parameters.AddWithValue(mParametro.Variable, DBNull.Value);
            }

            // SqlAdapter utiliza el SqlCommand para llenar el Dataset
            SqlDataReader reader = sqlCmd.ExecuteReader();

            var result = DataReaderMapToList<T>(reader);
            Conn.CerrarConeccion();
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    // Ejecuta un script de consulta retornando una lista generica.
    public List<T> EjecutarConsultaReportes<T>(string consulta)
    {
        try
        {
            ConexionSQL Conn = new ConexionSQL(true);
            SqlCommand sqlCmd = new SqlCommand(this.Nombre, Conn.AbrirConeccion())
            {
                CommandType = CommandType.Text,
                CommandTimeout = 4000,
                CommandText = consulta
            };

            if (this.Parametros != null)
            {
                // Agrega las variables al procedimiento almacenado
                foreach (var mParametro in this.Parametros)
                {
                    if (mParametro.Valor != null)
                        sqlCmd.Parameters.AddWithValue(mParametro.Variable, mParametro.Valor);
                    else
                        sqlCmd.Parameters.AddWithValue(mParametro.Variable, DBNull.Value);
                }
            }

            // 'SqlAdapter utiliza el SqlCommand para llenar el Dataset
            SqlDataReader reader = sqlCmd.ExecuteReader();

            var result = DataReaderMapToList<T>(reader);
            Conn.CerrarConeccion();
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    // Ejecuta un script de consulta
    public DataSet EjecutarConsultaReportes(string consulta)
    {
        try
        {
            ConexionSQL Conn = new ConexionSQL(true);
            SqlCommand sqlCmd = new SqlCommand(this.Nombre, Conn.AbrirConeccion())
            {
                CommandType = CommandType.Text,
                CommandTimeout = 4000,
                CommandText = consulta
            };

            if (this.Parametros != null)
            {
                // Agrega las variables al procedimiento almacenado
                foreach (var mParametro in this.Parametros)
                {
                    if (mParametro.Valor != null)
                        sqlCmd.Parameters.AddWithValue(mParametro.Variable, mParametro.Valor);
                    else
                        sqlCmd.Parameters.AddWithValue(mParametro.Variable, DBNull.Value);
                }
            }

            // SqlAdapter utiliza el SqlCommand para llenar el Dataset
            SqlDataAdapter sda = new SqlDataAdapter(sqlCmd);

            // Se llena el dataset
            DataSet ds = new DataSet();
            sda.Fill(ds);

            Conn.CerrarConeccion();
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

#endregion

    public static List<T> DataReaderMapToList<T>(SqlDataReader dr)
    {
        List<T> list = new List<T>();
        T obj;
        while (dr.Read())
        {
            obj = Activator.CreateInstance<T>();
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                if ((ExistsColumn(dr, prop.Name)))
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                        prop.SetValue(obj, dr[prop.Name], null);
                }
            }
            list.Add(obj);
        }
        return list;
    }

    private static bool ExistsColumn(SqlDataReader dr, string col)
    {
        for (int i = 0; i <= dr.FieldCount - 1; i++)
        {
            if (dr.GetName(i) == col)
                return true;
        }
        return false;
    }

    public int Ejecutar()
    {
        int i = 0;
        try
        {
            ConexionSQL Conn = new ConexionSQL();
            SqlCommand sqlCmd = new SqlCommand(this.Nombre, Conn.AbrirConeccion())
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 4000
            };

            // Agrega las variables al procedimiento almacenado
            foreach (var mParametro in this.Parametros)
            {
                if (mParametro.Valor != null)
                    sqlCmd.Parameters.AddWithValue(mParametro.Variable, mParametro.Valor);
                else
                    sqlCmd.Parameters.AddWithValue(mParametro.Variable, DBNull.Value);
            }

            i = sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return i;
    }

    public int Ejecutar(string consulta)
    {
        int i = 0;
        try
        {
            ConexionSQL Conn = new ConexionSQL();
            SqlCommand sqlCmd = new SqlCommand(this.Nombre, Conn.AbrirConeccion())
            {
                CommandType = CommandType.Text,
                CommandTimeout = 4000,
                CommandText = consulta
            };

            // Agrega las variables al procedimiento almacenado
            foreach (var mParametro in this.Parametros)
            {
                if (mParametro.Valor != null)
                    sqlCmd.Parameters.AddWithValue(mParametro.Variable, mParametro.Valor);
                else
                    sqlCmd.Parameters.AddWithValue(mParametro.Variable, DBNull.Value);
            }

            i = sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return i;
    }
}
