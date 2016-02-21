using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProProperty.Models;

namespace ProProperty.DAL
{
    public class TownDatagateway : DataGateway<Town>
    {
        public Town SelectByTownName(string name)
        {
            Town obj = data.Find(name);
            return obj;
        }
    }
}