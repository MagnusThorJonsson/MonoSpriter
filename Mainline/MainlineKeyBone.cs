using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MonoSpriter.Mainline
{
    /// <summary>
    /// Spriter Mainline Key Bone data object.
    /// 
    /// DATA:
    ///   <mainline>
    ///     <key id="0">
    ///       <bone_ref id="0" timeline="15" key="0"/>
    /// </summary>
    internal class MainlineKeyBone
    {
        #region Variables & Properties
        /// <summary>
        /// Bone reference id
        /// </summary>
        public int Id { get { return id; } }
        protected int id;

        /// <summary>
        /// Bone parent id
        /// </summary>
        public int Parent { get { return parent; } }
        protected int parent;

        /// <summary>
        /// Bone timeline id
        /// </summary>
        public int Timeline { get { return timeline; } }
        protected int timeline;

        /// <summary>
        /// Bone key id
        /// </summary>
        public int Key { get { return key; } }
        protected int key;
        #endregion


        #region Constructors
        /// <summary>
        /// Constructor that takes in an XElement containing the Spriter mainline key bone data
        /// </summary>
        /// <param name="element">The XElement containing the data</param>
        public MainlineKeyBone(XElement element)
        {
            if (element.Attribute("id") != null)
                id = int.Parse(element.Attribute("id").Value);
            else
                id = -1;

            if (element.Attribute("parent") != null)
                parent = int.Parse(element.Attribute("parent").Value);
            else
                parent = -1;

            if (element.Attribute("timeline") != null)
                timeline = int.Parse(element.Attribute("timeline").Value);

            if (element.Attribute("key") != null)
                key = int.Parse(element.Attribute("key").Value);
        }
        #endregion
    }
}
