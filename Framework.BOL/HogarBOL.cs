using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BOL
{
    public class HogarBOL
    {
        private string _PK_VIVIENDA;
        private string _PK_HOGAR;
        private int _H_NHOG;
        private int _H_NHOG_VISUAL;
        private string _TEN1;
        private string _TEN2;
        private string _TEN3;
        private string _TEN4;
        private string _ENE1;
        private string _ENE2;
        private string _ENE3;
        private string _ENE4;
        private string _ENE5;
        private string _ENE6;
        private string _ENE7;
        private string _ENE8;
        private string _ENE9;
        private string _ENE10;
        private string _ENE11;
        private string _ENE12;
        private string _RES1;
        private string _RES2;
        private string _EMI1;
        private string _EMI2;
        private int _HOG_TOT_H;
        private int _HOG_TOT_M;
        private int _HOG_TOT_PERS;
        private string _HOG_NOM_PERS;
        private string _HOG_PER8;
        private string _HOG_OTRA_VIV;
        private string _HOG_PERT;
        private string _HOG_JEFE_HOG;
        private string _HOG_ESTADO;

        /// <summary>
        /// Obtiene o establece identificador de vivienda
        /// </summary>
        public string PK_VIVIENDA
        {
            get { return _PK_VIVIENDA; }
            set { _PK_VIVIENDA = value; }
        }

        /// <summary>
        /// Obtiene o establece identificador del hogar
        /// </summary>
        public string PK_HOGAR
        {
            get { return _PK_HOGAR; }
            set { _PK_HOGAR = value; }
        }

        /// <summary>
        /// Obtiene o establece Nro de hogar interno
        /// </summary>
        public int H_NHOG
        {
            get { return _H_NHOG; }
            set { _H_NHOG = value; }
        }

        /// <summary>
        /// Obtiene o establece Nro de hogar visual
        /// </summary>
        public int H_NHOG_VISUAL
        {
            get { return _H_NHOG_VISUAL; }
            set { _H_NHOG_VISUAL = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string TEN1
        {
            get { return _TEN1; }
            set { _TEN1 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string TEN2
        {
            get { return _TEN2; }
            set { _TEN2 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string TEN3
        {
            get { return _TEN3; }
            set { _TEN3 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string TEN4
        {
            get { return _TEN4; }
            set { _TEN4 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string ENE1
        {
            get { return _ENE1; }
            set { _ENE1 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string ENE2
        {
            get { return _ENE2; }
            set { _ENE2 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string ENE3
        {
            get { return _ENE3; }
            set { _ENE3 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string ENE4
        {
            get { return _ENE4; }
            set { _ENE4 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string ENE5
        {
            get { return _ENE5; }
            set { _ENE5 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string ENE6
        {
            get { return _ENE6; }
            set { _ENE6 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string ENE7
        {
            get { return _ENE7; }
            set { _ENE7 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string ENE8
        {
            get { return _ENE8; }
            set { _ENE8 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string ENE9
        {
            get { return _ENE9; }
            set { _ENE9 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string ENE10
        {
            get { return _ENE10; }
            set { _ENE10 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string ENE11
        {
            get { return _ENE11; }
            set { _ENE11 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string ENE12
        {
            get { return _ENE12; }
            set { _ENE12 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string RES1
        {
            get { return _RES1; }
            set { _RES1 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string RES2
        {
            get { return _RES2; }
            set { _RES2 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string EMI1
        {
            get { return _EMI1; }
            set { _EMI1 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string EMI2
        {
            get { return _EMI2; }
            set { _EMI2 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public int HOG_TOT_H
        {
            get { return _HOG_TOT_H; }
            set { _HOG_TOT_H = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public int HOG_TOT_M
        {
            get { return _HOG_TOT_M; }
            set { _HOG_TOT_M = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public int HOG_TOT_PERS
        {
            get { return _HOG_TOT_PERS; }
            set { _HOG_TOT_PERS = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string HOG_NOM_PERS
        {
            get { return _HOG_NOM_PERS; }
            set { _HOG_NOM_PERS = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string HOG_PER8
        {
            get { return _HOG_PER8; }
            set { _HOG_PER8 = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string HOG_OTRA_VIV
        {
            get { return _HOG_OTRA_VIV; }
            set { _HOG_OTRA_VIV = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string HOG_PERT
        {
            get { return _HOG_PERT; }
            set { _HOG_PERT = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string HOG_JEFE_HOG
        {
            get { return _HOG_JEFE_HOG; }
            set { _HOG_JEFE_HOG = value; }
        }

        /// <summary>
        /// Obtiene o establece pregunta
        /// </summary>
        public string HOG_ESTADO
        {
            get { return _HOG_ESTADO; }
            set { _HOG_ESTADO = value; }
        }
    }
}
