using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using MonoSpriter.Animation;

namespace MonoSpriter.Data
{
    /// <summary>
    /// Spriter file data object.
    /// Contains the file content data deserialized from the SCML file.
    /// 
    /// DATA: 
    ///   <folder id="0" name="guides">
    ///     <file id="0" name="guides/Idle__000.png" width="0" height="0" pivot_x="0" pivot_y="1"/>
    /// </summary>
    internal sealed class SpriterFile
    {
        #region Variables & Properties
        /// <summary>
        /// Spriter file id
        /// </summary>
        public int Id { get { return _id; } }
        private int _id;

        /// <summary>
        /// The name of the file (file ending and all)
        /// </summary>
        public string Name { get { return _name; } }
        private string _name;

        /// <summary>
        /// The asset name of the file (sans file ending)
        /// </summary>
        public string AssetName { get { return _assetName; } }
        private string _assetName;

        /// <summary>
        /// The width of the asset in pixels
        /// </summary>
        public int Width { get { return _width; } }
        private int _width;

        /// <summary>
        /// The height of the asset in pixels
        /// </summary>
        public int Height { get { return _height; } }
        private int _height;

        /// <summary>
        /// The X axis pivot
        /// </summary>
        public float PivotX { get { return _pivotX; } }
        private float _pivotX;

        /// <summary>
        /// The Y axis pivot
        /// </summary>
        public float PivotY { get { return _pivotY; } }
        private float _pivotY;
        #endregion


        #region Constructors
        /// <summary>
        /// Constructor that takes in an XElement containing the Spriter File data
        /// </summary>
        /// <param name="element">The XElement containing the data</param>
        public SpriterFile(XElement element)
        {
            if (element.Attribute("id") != null)
                _id = int.Parse(element.Attribute("id").Value);
            else
                _id = -1;

            if (element.Attribute("name") != null)
            {
                _name = element.Attribute("name").Value;
                _assetName = createAssetName(_name);
            }

            if (element.Attribute("width") != null)
                _width = int.Parse(element.Attribute("width").Value);
            if (element.Attribute("height") != null)
                _height = int.Parse(element.Attribute("height").Value);

            if (element.Attribute("pivot_x") != null)
                _pivotX = float.Parse(element.Attribute("pivot_x").Value, CultureInfo.InvariantCulture);
            if (element.Attribute("pivot_y") != null)
                _pivotY = float.Parse(element.Attribute("pivot_y").Value, CultureInfo.InvariantCulture);
        }
        #endregion


        // TODO: This might be stupid
        /// <summary>
        /// Generates the asset name by removing the file ending
        /// </summary>
        /// <param name="name">The file name</param>
        /// <returns>The generated asset name</returns>
        private string createAssetName(string name)
        {
            if (name.Length > 0)
            {
                return name
                    .Substring(0, name.Length - System.IO.Path.GetExtension(name).Length)
                    .Replace("/", "\\");
            }

            return null;
        }
    }
}
