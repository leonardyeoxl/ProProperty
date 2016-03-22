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

            Town obj = data.SqlQuery("Select * FROM Town Where town_name = '" + name + "'").ToList()[0];
            return obj;
        }
    }
}