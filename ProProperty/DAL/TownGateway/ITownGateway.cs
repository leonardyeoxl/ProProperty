using ProProperty.Models;

namespace ProProperty.DAL
{
    interface ITownGateway : IDataGateway<Town>
    {
        Town SelectByTownName(string name);
    }
}
