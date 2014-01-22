using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MonoSpriter.Timeline;

namespace MonoSpriter.Animation
{
    /// <summary>
    /// The Animation type.
    /// Used to categorize the type of timeline being animated.
    /// </summary>
    public enum TimelineType
    {
        Object,
        Bone,
        Point,
        Box
    }


    /// <summary>
    /// Spriter Timeline data object.
    /// Handles the animation timelines
    /// 
    /// DATA:
    ///     <animation id="0" name="idle" length="4000">
    ///       <timeline id="0" name="p_torso_idle">
    /// </summary>
    internal sealed class SpriterTimeline
    {
        #region Variables & Properties
        /// <summary>
        /// The type of items contain within this timeline key
        /// </summary>
        public TimelineType Type { get { return _type; } }
        private TimelineType _type;

        
        /// <summary>
        /// Timeline id
        /// </summary>
        public int Id { get { return _id; } }
        private int _id;

        /// <summary>
        /// Timeline name
        /// </summary>
        public string Name { get { return _name; } }
        private string _name;

        /// <summary>
        /// Timeline keys
        /// </summary>
        public Dictionary<int, TimelineKey> Keys { get { return _keys; } }
        private Dictionary<int, TimelineKey> _keys;
        #endregion


        #region Constructors
        /// <summary>
        /// Constructor that takes in an XElement containing the Spriter timeline data
        /// </summary>
        /// <param name="element">The XElement containing the data</param>
        public SpriterTimeline(XElement element)
        {
            if (element.Attribute("id") != null)
                _id = int.Parse(element.Attribute("id").Value);

            if (element.Attribute("name") != null)
                _name = element.Attribute("name").Value;

            if (element.Attribute("object_type") != null)
            {
                if (element.Attribute("object_type").Value.Equals("bone"))
                    _type = TimelineType.Bone;
                else if (element.Attribute("object_type").Value.Equals("box"))
                    _type = TimelineType.Box;
                else if (element.Attribute("object_type").Value.Equals("point"))
                    _type = TimelineType.Point;
                else // Sprite Object
                    _type = TimelineType.Object;
            }

            _keys = new Dictionary<int, TimelineKey>();
            foreach (XElement row in element.Elements())
            {
                TimelineKey key = new TimelineKey(row, _type);
                _keys.Add(key.Id, key);
            }
        }
        #endregion
    }
}
