using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MonoSpriter.Animation;

namespace MonoSpriter.Data
{
    /// <summary>
    /// Spriter folder data object.
    /// Contains the folder content data deserialized from the SCML file.
    /// 
    /// DATA: 
    ///   <spriter_data scml_version="1.0" generator="BrashMonkey Spriter" generator_version="b5">
    ///     <folder id="0" name="guides">
    ///       <file id="0" name="guides/Idle__000.png" width="0" height="0" pivot_x="0" pivot_y="1"/>
    /// </summary>
    internal sealed class SpriterFolder
    {
        #region Variables & Properties
        /// <summary>
        /// Spriter folder id
        /// </summary>
        public int Id { get { return _id; } }
        private int _id;

        /// <summary>
        /// Spriter folder name
        /// </summary>
        public string Name { get { return _name; } }
        private string _name;

        /// <summary>
        /// Container for all the assets associated with the Spriter object
        /// </summary>
        public Dictionary<int, SpriterFile> Files { get { return _files; } }
        private Dictionary<int, SpriterFile> _files;
        #endregion


        #region Constructors
        /// <summary>
        /// Constructor that takes in an XElement containing the Spriter File data
        /// </summary>
        /// <param name="element">The XElement containing the data</param>
        public SpriterFolder(XElement element)
        {
            if (element.Attribute("id") != null)
                _id = int.Parse(element.Attribute("id").Value);
            else
                _id = -1;

            if (element.Attribute("name") != null)
                _name = element.Attribute("name").Value;

            _files = new Dictionary<int, SpriterFile>();
            foreach (XElement xmlFile in element.Elements("file"))
            {
                SpriterFile file = new SpriterFile(xmlFile);
                _files.Add(file.Id, file);
            }
        }
        #endregion
    }
}
