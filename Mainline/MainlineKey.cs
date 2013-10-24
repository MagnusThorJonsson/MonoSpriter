using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MonoSpriter.Animation;

namespace MonoSpriter.Mainline
{
    /// <summary>
    /// Spriter Mainline Key data object.
    /// 
    /// DATA:
    ///   <animation id="0" name="idle" length="4000">
    ///     <mainline>
    ///       <key id="0">
    /// </summary>
    internal sealed class MainlineKey
    {
        #region Variables & Properties
        /// <summary>
        /// Mainline key id
        /// </summary>
        public int Id { get { return _id; } }
        private int _id;

        /// <summary>
        /// The time stamp for the key
        /// </summary>
        public int Time { get { return _time; } }
        private int _time;

        /// <summary>
        /// The list of bone references 
        /// </summary>
        public List<MainlineKeyBone> BoneRefs { get { return _boneRefs; } }
        private List<MainlineKeyBone> _boneRefs;

        /// <summary>
        /// The list of object references
        /// </summary>
        public List<MainlineKeyObject> ObjectRefs { get { return _objectRefs; } }
        private List<MainlineKeyObject> _objectRefs;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor that takes in an XElement containing the Spriter mainline key data
        /// </summary>
        /// <param name="element">The XElement containing the data</param>
        public MainlineKey(XElement element)
        {
            if (element.Attribute("id") != null)
                _id = int.Parse(element.Attribute("id").Value);

            if (element.Attribute("time") != null)
                _time = int.Parse(element.Attribute("time").Value);

            _boneRefs = new List<MainlineKeyBone>();
            foreach (XElement row in element.Elements().Where(s => string.Equals(s.Name.ToString().ToLower(), "bone_ref")))
                _boneRefs.Add(new MainlineKeyBone(row));

            _objectRefs = new List<MainlineKeyObject>();
            foreach (XElement row in element.Elements().Where(s => string.Equals(s.Name.ToString().ToLower(), "object_ref")))
                _objectRefs.Add(new MainlineKeyObject(row));

        }
        #endregion
    }
}
