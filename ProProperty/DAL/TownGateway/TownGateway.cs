using ProProperty.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProProperty.DAL
{
    public class TownGateway : DataGateway<Town>, ITownGateway
    {
        public Town SelectByTownName(string name)
        {
            List<Town> obj = data.SqlQuery("Select * FROM Town Where town_name = '" + name + "'").ToList();
            return (obj.Count > 0)? obj[0] : null;
        }
    }
}