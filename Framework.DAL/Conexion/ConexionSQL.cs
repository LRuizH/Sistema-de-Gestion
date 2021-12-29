using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.IO;

public class ConexionSQL
{
    private SqlConnection mSqlConn;
    private bool reportConn;

    public SqlConnection SQLConn
    {
        get
        {
            return mSqlConn;
        }
        set
        {
            mSqlConn = value;
        }
    }

    public ConexionSQL()
    {
        try
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json");
            configurationBuilder.AddJsonFile(path, false);
            var root = configurationBuilder.Build();
            string StringConeccion = "";

            StringConeccion = root.GetConnectionString("DataConnection");
            SQLConn = new SqlConnection
            {
                ConnectionString = StringConeccion
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public ConexionSQL(bool ReportConn)
    {
        try
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json");
            configurationBuilder.AddJsonFile(path, false);
            var root = configurationBuilder.Build();
            string StringConeccion = "";
            StringConeccion = root.GetConnectionString("DataConnection");

            SQLConn = new SqlConnection
            {
                ConnectionString = StringConeccion
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public SqlConnection AbrirConeccion()
    {
        SQLConn.Open();
        return SQLConn;
    }

    public void CerrarConeccion()
    {
        SQLConn.Close();
    }
}
