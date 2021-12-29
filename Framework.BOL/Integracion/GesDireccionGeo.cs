using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BOL.Integracion
{
   public class GesDireccionGeo
    {
        public int Interview__id { get; set; }
        public string IdDireccionPrincipal { get; set; }
        public int Quantity { get; set; }
        public string port_df_manzana { get; set; }
        public string port_df_dir_princ { get; set; }
        public string cod_alc { get; set; }
        public string idg_comuna { get; set; }
        public long df_id_dirprin { get; set; }
        public string df_manzana { get; set; }
        public string calle { get; set; }
        public string altura { get; set; }
        public long indice_dirpri { get; set; }
        public long dirs_cant_dirsecu { get; set; }
        public long Dir_Sec { get; set; }
        public string TipoDireccion { get; set; }
        public string IdTipoVia { get; set; }
    }
}
