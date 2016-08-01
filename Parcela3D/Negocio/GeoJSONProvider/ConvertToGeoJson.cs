using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Converters;

namespace Negocio
{
    public class ConvertToGeoJson
    {
        /// <summary>
        /// Return a GeoJson from a datatable with column Geometry in Json format.
        /// </summary>
        /// <param name="dt">DataTable with columns properties and column geometry</param>
        /// <param name="GeometryColumn">Column Name of Geometry</param>
        /// <returns></returns>
        public static string fromDataset(DataTable dt, string GeometryColumn = "Geom")
        {
            var model = new FeatureCollection();
            
            int numColumns = dt.Columns.Count;
            string[] keys = new string[numColumns];

            for (int i = 0; i < numColumns; i++) {
                var columnName = dt.Columns[i].ColumnName;
                if (string.Compare(columnName, GeometryColumn, CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase) != 0)
                    keys[i] = columnName;
            }

            foreach (DataRow row in dt.Rows)
            {
                object geom = null;
                var geomDB = row[GeometryColumn].ToString();
                var json = row[GeometryColumn].ToString();
                
                JsonTextReader reader = new JsonTextReader(new StringReader(json));

                while (reader.Read())
                {
                    geom = new GeometryConverter().ReadJson(reader, null, null, null);
                }

                Dictionary<string, object> props = new Dictionary<string, object>();
                for ( int j = 0; j < keys.Length; j++ )
                    if (!string.IsNullOrEmpty(keys[j]))
                        props.Add(keys[j], row[j]);

                var feature = new GeoJSON.Net.Feature.Feature((IGeometryObject)geom, props);
                model.Features.Add(feature);

            }

            return JsonConvert.SerializeObject(model);
        }
    }
}
