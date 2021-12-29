using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BOL
{
   public class Enumeracion
    {
        public enum RetornoAsignacion
        {
            ASIGNADO = 0,
            REASIGNADO = 1,
            SINASIGNAR = 5
        }

        public enum TipoUsuarios
        {
            Supervisor = 1,
            Entrevistador = 2
        }

        public enum Role
        {
            SUPERVISOR = 2,
            INTERVIEWER = 4
        }
        public enum EstadosAsignacion
        {
            ASIGNADO = 1,
            REASIGNADO = 2,
            SINASIGNAR = 3
        }

    }
}
