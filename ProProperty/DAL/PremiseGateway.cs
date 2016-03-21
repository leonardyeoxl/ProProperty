using ProProperty.Models;
using System.Linq;
using System.Collections.Generic;

namespace ProProperty.DAL
{
    public class PremiseGateway : DataGateway<Premise>
    {
        public IEnumerable<Premise> GetPremises(params int[] premise_type_id)
        {
            string typeID = "";
            foreach(int id in premise_type_id)
            {
                typeID += id + ", ";
            }
            typeID = typeID.Remove(typeID.Length - 2);
            string query = string.Format("SELECT * FROM Premises WHERE premises_type_id IN ({0})", typeID);
            return data.SqlQuery(query).ToList();       
        }
    }
}