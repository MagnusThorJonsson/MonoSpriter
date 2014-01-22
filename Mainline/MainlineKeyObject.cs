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
            if (element.Attribute("z_index") != null)
                _zIndex = int.Parse(element.Attribute("z_index").Value);
        }
        #endregion
    }
}
