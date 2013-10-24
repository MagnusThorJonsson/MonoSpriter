using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MonoSpriter.Animation
{
    /// <summary>
    /// Spriter Character Map Instruction data object.
    /// Used with the Character Mapping.
    /// </summary>
    internal sealed class MapInstruction
    {
        #region Variables & Properties
        /// <summary>
        /// The Spriter folder id
        /// </summary>
        public int Folder { get { return _folder; } }
        private int _folder;

        /// <summary>
        /// The Spriter file id
        /// </summary>
        public int File { get { return _file; } }
        private int _file;

        /// <summary>
        /// The Spriter targeted folder id
        /// </summary>
        public int TargetFolder { get { return _targetFolder; } }
        private int _targetFolder;

        /// <summary>
        /// The Spriter targeted file id
        /// </summary>
        public int TargetFile { get { return _targetFile; } }
        private int _targetFile;
        #endregion

        #region Constructors
        /// <summary>
        /// Deserializing constructor that takes in an XElement containing the Spriter MapInstruction data
        /// </summary>
        /// <param name="element">The XElement containing the data</param>
        public MapInstruction(XElement element)
        {
            if (element.Attribute("folder") != null)
                _folder = int.Parse(element.Attribute("folder").Value);
            else
                _folder = -1;

            if (element.Attribute("file") != null)
                _file = int.Parse(element.Attribute("file").Value);
            else
                _file = -1;

            if (element.Attribute("target_folder") != null)
                _targetFolder = int.Parse(element.Attribute("target_folder").Value);
            else
                _targetFolder = -1;

            if (element.Attribute("target_file") != null)
                _targetFile = int.Parse(element.Attribute("target_file").Value);
            else
                _targetFile = -1;
        }
        #endregion
    }
}
