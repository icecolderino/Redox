using System;
using System.Collections.Generic;


namespace Redox.API.Player
{
    /// <summary>
    /// Represents the inventory of an player
    /// 
    /// <para>
    /// An inventory contains items that the player can interact with
    /// </para>
    /// </summary>
    public interface IPlayerInv
    {
        /// <summary>
        /// List of items in the inventory
        /// </summary>
        IEnumerable<IPlayerItem> Items { get; }

        /// <summary>
        /// Adds a item to the inventory
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Amount"></param>
        void AddItem(string name, int Amount = 1);

        /// <summary>
        /// Adds a item to the inventory
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Amount"></param>
        /// <param name="Slot"></param>
        void AddItem(string name, int Amount = 1, int Slot = -1);

        /// <summary>
        /// Removes the item from the inventory
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Amount"></param>
        void RemoveItem(string name, int Amount = 0);

        /// <summary>
        /// Returns true if inventory contains the item
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Amount"></param>
        bool HasItem(string name, int Amount = 1);

        /// <summary>
        /// Removes all items from the inventory
        /// </summary>
        void Clear();
    }
}
