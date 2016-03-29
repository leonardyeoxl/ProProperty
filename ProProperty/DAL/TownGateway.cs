using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProProperty.Models;

namespace ProProperty.DAL
{
    public class TownGateway : DataGateway<Town>
    {
        public Town SelectByTownName(string name)
        {
            List<Town> obj = data.SqlQuery("Select * FROM Town Where town_name = '" + name + "'").ToList();
            return (obj.Count > 0)? obj[0] : null;
        }
    }
}