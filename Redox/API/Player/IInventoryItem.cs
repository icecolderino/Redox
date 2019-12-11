using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redox.API.Player
{
    public interface IInventoryItem
    {
        string Name { get; }

        int Amount { get; }


    }
}
