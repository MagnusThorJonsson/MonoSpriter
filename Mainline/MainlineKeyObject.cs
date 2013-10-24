using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace MonoSpriter.Mainline
{
    /// <summary>
    /// Spriter Mainline Key Object data object.
    /// 
    /// DATA:
    ///   <mainline>
    ///     <key id="0">
    ///       <object_ref id="0" parent="6" name="p_arm_idle_a" folder="3" file="0" abs_x="30" abs_y="114.999967" abs_pivot_x="0.388889" abs_pivot_y="0.487179" abs_angle="290.409883" abs_scale_x="0.999999" abs_scale_y="1" abs_a="1" timeline="2" key="0" z_index="0"/>
    /// </summary>
    internal sealed class MainlineKeyObject : MainlineKeyBone
    {
        #region Variables & Properties
        /// <summary>
        /// Object name
        /// </summary>
        public string Name { get { return _name; } }
        private string _name;

        /// <summary>
        /// Object content folder id
        /// </summary>
        public int Folder { get { return _spriterFolder; } }
        private int _spriterFolder;

        /// <summary>
        /// Object content file id
        /// </summary>
        public int File { get { return _spriterFile; } }
        private int _spriterFile;

        /// <summary>
        /// Object pivot
        /// </summary>
        public Vector2 Pivot { get { return _pivot; } }
        private Vector2 _pivot;

        /// <summary>
        /// Object position
        /// </summary>
        public Vector2 Position { get { return _position; } }
        private Vector2 _position;

        /// <summary>
        /// Object angle
        /// </summary>
        public float Angle { get { return _angle; } }
        private float _angle;

        /// <summary>
        /// Object scale
        /// </summary>
        public Vector2 Scale { get { return _scale; } }
        private Vector2 _scale;

        /// <summary>
        /// Object alpha
        /// </summary>
        public float Alpha { get { return _alpha; } }
        private float _alpha;

        /// <summary>
        /// Object Z index
        /// </summary>
        public int ZIndex { get { return _zIndex; } }
        private int _zIndex;
        #endregion


        #region Constructors
        /// <summary>
        /// Constructor that takes in an XElement containing the Spriter mainline key object data
        /// </summary>
        /// <param name="element">The XElement containing the data</param>
        public MainlineKeyObject(XElement element)
            : base(element)
        {
            if (element.Attribute("name") != null)
                _name = element.Attribute("name").Value;

            if (element.Attribute("folder") != null)
                _spriterFolder = int.Parse(element.Attribute("folder").Value);
            else
                _spriterFolder = -1;

            if (element.Attribute("file") != null)
                _spriterFile = int.Parse(element.Attribute("file").Value);
            else
                _spriterFile = -1;

            _position = Vector2.Zero;
            if (element.Attribute("abs_x") != null)
                _position.X = float.Parse(element.Attribute("abs_x").Value, CultureInfo.InvariantCulture);
            if (element.Attribute("abs_y") != null)
                _position.Y = -float.Parse(element.Attribute("abs_y").Value, CultureInfo.InvariantCulture);

            _pivot = Vector2.Zero;
            if (element.Attribute("abs_pivot_x") != null)
                _pivot.X = float.Parse(element.Attribute("abs_pivot_x").Value, CultureInfo.InvariantCulture);
            if (element.Attribute("abs_pivot_y") != null)
                _pivot.Y = -float.Parse(element.Attribute("abs_pivot_y").Value, CultureInfo.InvariantCulture);

            _scale = Vector2.Zero;
            if (element.Attribute("abs_scale_x") != null)
                _scale.X = float.Parse(element.Attribute("abs_scale_x").Value, CultureInfo.InvariantCulture);
            if (element.Attribute("abs_scale_y") != null)
                _scale.Y = float.Parse(element.Attribute("abs_scale_y").Value, CultureInfo.InvariantCulture);

            if (element.Attribute("abs_angle") != null)
                _angle = -MathHelper.ToRadians(float.Parse(element.Attribute("abs_angle").Value, CultureInfo.InvariantCulture));

            if (element.Attribute("abs_a") != null)
                _alpha = float.Parse(element.Attribute("abs_a").Value, CultureInfo.InvariantCulture);

            if (element.Attribute("z_index") != null)
                _zIndex = int.Parse(element.Attribute("z_index").Value);
        }
        #endregion
    }
}
