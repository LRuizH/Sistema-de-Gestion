using System;

public class StoredProcedureParameter
{
    private string mVariable;
    private object mValor;

    // Nombre de la variable, debe ser igual a la declarada en el procedimiento almacenado
    public string Variable
    {
        get
        {
            return mVariable;
        }
        set
        {
            mVariable = value;
        }
    }

    // Valor de la variable, puede ser de cualquier tipo de dato. preferible que 
    // coincida con las variables declaradas en GetTypeProperty
    public object Valor
    {
        get
        {
            return mValor;
        }
        set
        {
            mValor = value;
        }
    }

    // Procedimiento de creacion de la variable.
    public StoredProcedureParameter(string pVariable, object pValor)
    {
        try
        {
            this.Variable = pVariable;
            this.Valor = pValor;
        }
        catch (Exception ex)
        {
            throw new Exception("Error en la creacion del Parametro" + ex.Message);
        }
    }
}

