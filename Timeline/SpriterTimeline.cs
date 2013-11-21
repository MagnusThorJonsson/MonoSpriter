using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MonoSpriter.Timeline;

namespace MonoSpriter.Animation
{    
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

            _keys = new Dictionary<int, TimelineKey>();
            foreach (XElement row in element.Elements())
            {
                TimelineKey key = new TimelineKey(row);
                _keys.Add(key.Id, key);
            }
        }
        #endregion
    }
}
