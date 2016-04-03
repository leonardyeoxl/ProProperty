using ProProperty.Models;
using System.Collections.Generic;

namespace ProProperty.DAL
{
    interface IPremiseGateway : IDataGateway<Premise>
    {
        IEnumerable<Premise> GetPremises(params int[] premise_type_id);
    }
}
