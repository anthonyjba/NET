using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Negocio
{
    public class HelperMapaNegocio
    {
        #region Persistencia

        /// <summary>
        /// Definimos su Repositorio
        /// </summary>
        private Persistencias.HelperMapaPersistencia Persistencia
        {
            get { return new Persistencias.HelperMapaPersistencia(); }
        }

        #endregion

        #region Metodos Publicos

        public DataSet ObtenerGeometriaParcela(string referencia)
        {
            return Persistencia.ObtenerGeometria(referencia, "PARCELA", 3857);
        }

        public DataSet ObtenerGeometriaSubParcela(string referencia)
        {
            return Persistencia.ObtenerGeometria(referencia, "SUBPARCE", 3857);
        }

        public List<string> ObtenerAlturaPorCoordenadas(int x1, int x2, int y1, int y2, int srid, int segmentacion)
        {
            return Persistencia.ObtenerAlturaPorCoordenadas(x1, x2, y1, y2, srid, segmentacion);
        }

        public string WPSMagrama(string Layer, string poligono, int srid, int sridout)
        {
            string result = string.Empty;

            DataSet ds = Persistencia.WPSMagrama(Layer, poligono, srid, sridout);

            if ( ds.Tables[0].Rows.Count > 0 ){
                //Convertir los datos de un dataset a formato GeoJson

                result = Negocio.ConvertToGeoJson.fromDataset(ds.Tables[0]);
            }

            return result;
        }

        public string WPSMagrama(string Layer, string referencia, int sridout)
        {
            if (!validaParametros.PorReferenciaCatastral(referencia))
            {
                return string.Empty;
            }
            string result = string.Empty;

            DataSet ds = Persistencia.WPSMagrama(Layer, referencia, sridout);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //Convertir los datos de un dataset a formato GeoJson

                result = Negocio.ConvertToGeoJson.fromDataset(ds.Tables[0]);
            }

            return result;
        }

        

        #endregion


        
    }
}
