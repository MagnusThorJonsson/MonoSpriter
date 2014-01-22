using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MonoSpriter.Animation
{
    /// <summary>
    /// Spriter Character Map data object.
    /// Instruction map for the bone skins
    /// </summary>
    internal sealed class CharacterMap
    {
        #region Variables & Properties
        /// <summary>
        /// The id of the Character Map
        /// </summary>
        public int Id { get { return _id; } }
        private int _id;

        /// <summary>
        /// The name of the Character Map
        /// </summary>
        public string Name { get { return _name; } }
        private string _name;

        /// <summary>
        /// A list of the Map Instructions associated with this Character Map
        /// </summary>
        internal List<MapInstruction> Maps { get { return _maps; } }
        private List<MapInstruction> _maps;
        #endregion


        #region Constructors
        /// <summary>
        /// Constructor that takes in an XElement containing the Spriter Character Map data
        /// </summary>
        /// <param name="element">The XElement containing the data</param>
        public CharacterMap(XElement element)
        {
            if (element.Attribute("id") != null)
                _id = int.Parse(element.Attribute("id").Value);
            else
                _id = -1;

            if (element.Attribute("name") != null)
                _name = element.Attribute("name").Value;

            _maps = new List<MapInstruction>();
            foreach (var row in element.Elements())
                _maps.Add(new MapInstruction(row));
        }
        #endregion
    }
}
