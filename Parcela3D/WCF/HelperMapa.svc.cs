using Servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Servicios
{
    public class HelperMapa : IHelperMapa
    {
        #region Negocio

        private Negocio.HelperMapaNegocio Negocio
        {
            get { return new Negocio.HelperMapaNegocio(); }
        }

        #endregion

        #region Interface

        public EntidadesServicios.ParcelaJSON ObtenerGeometriaParcela(string referencia)
        {
            DataSet ds = Negocio.ObtenerGeometriaParcela(referencia);

            EntidadesServicios.ParcelaJSON parcela = new EntidadesServicios.ParcelaJSON();

            foreach (DataRow fila in ds.Tables[0].Rows)
            {
                parcela.VPD = Convert.ToInt32(fila["VPD"].ToString());
                parcela.Delegacion = Convert.ToInt32(fila["delegacio"].ToString());
                parcela.Municipio = Convert.ToInt32(fila["municipio"].ToString());
                parcela.Poligono = fila["poligono"].ToString();
                parcela.PCatastral1 = fila["pcat1"].ToString();
                parcela.PCatastral2 = fila["pcat2"].ToString();
                parcela.SRID = Convert.ToInt32(fila["srid"]);
            }

            return parcela;
        }

        public List<EntidadesServicios.SubParcelaJSON> ObtenerGeometriaSubParcela(string referencia)
        {
            DataSet ds = Negocio.ObtenerGeometriaSubParcela(referencia);

            List<EntidadesServicios.SubParcelaJSON> subparcelas = new List<EntidadesServicios.SubParcelaJSON>();

            foreach (DataRow fila in ds.Tables[0].Rows)
            {
                EntidadesServicios.SubParcelaJSON subparcela = new EntidadesServicios.SubParcelaJSON();
                subparcela.VPD = Convert.ToInt32(fila["VPD"].ToString());
                subparcela.Delegacion = Convert.ToInt32(fila["delegacion"].ToString());
                subparcela.Municipio = Convert.ToInt32(fila["municipio"].ToString());
                subparcela.Poligono = fila["poligono"].ToString();
                subparcela.PCatastral1 = fila["pcat1"].ToString();
                subparcela.PCatastral2 = fila["pcat2"].ToString();
                subparcela.Subparcela = fila["subparcela"].ToString();
                subparcela.AC = fila["ac"].ToString();
                subparcela.SRID = Convert.ToInt32(fila["srid"]);

                subparcelas.Add(subparcela);
            }

            return subparcelas;
        }

        public List<string> ObtenerAlturaPorCoordenadas(int x1, int x2, int y1, int y2, int srid, int segmentacion)
        {
            return Negocio.ObtenerAlturaPorCoordenadas(x1, x2, y1, y2, srid, segmentacion);
        }

        public string WpsMagramaPol(string Layer, string poligono, int srid, int sridout)
        {
            return Negocio.WPSMagrama(Layer, poligono, srid, sridout);
        }

        public string WpsMagramaRef(string Layer, string referencia, int sridout)
        {
            return Negocio.WPSMagrama(Layer, referencia, sridout);
        }

        #endregion
    }
}
