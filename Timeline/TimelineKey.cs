using MonoSpriter.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MonoSpriter.Timeline
{
    /// <summary>
    /// Spriter TimelineKey data object.
    /// Used with the Timeline animations.
    /// 
    /// DATA:
    ///   <key id="0" spin="0">
    ///     <object folder="1" file="0" x="-20.399667" y="0.10202" angle="180.740013" scale_x="4.080783"/>
    /// </summary>
    internal sealed class TimelineKey 
    {
        #region Variables & Properties
        /// <summary>
        /// The TimelineKey id
        /// </summary>
        public int Id { get { return _id; } }
        private int _id;

        /// <summary>
        /// The time stamp for the key
        /// </summary>
        public int Time { get { return _time; } }
        private int _time;

        /// <summary>
        /// The spin value for the key.
        /// 
        /// 1 (true) for counter-clockwise tweening
        /// -1 (false) for clockwise
        /// </summary>
        public bool DoSpin { get { return _doSpin; } }
        private bool _doSpin;

        /// <summary>
        /// The bone connected to this timeline key
        /// </summary>
        public TimelineKeyBone Bone { get { return _bone; } }
        private TimelineKeyBone _bone;

        /// <summary>
        /// The sprite or object connected to this timeline key
        /// </summary>
        public TimelineKeyObject Object { get { return _object; } }
        private TimelineKeyObject _object;
        #endregion


        #region Constructors
        /// <summary>
        /// Constructor that takes in an XElement containing the Spriter timeline key data
        /// </summary>
        /// <param name="element">The XElement containing the data</param>
        public TimelineKey(XElement element)
        {
            if (element.Attribute("id") != null)
                _id = int.Parse(element.Attribute("id").Value);
            else
                _id = -1;

            if (element.Attribute("time") != null)
                _time = int.Parse(element.Attribute("time").Value);
            else
                _time = 0;

            if (element.Attribute("spin") != null)
                _doSpin = (int.Parse(element.Attribute("spin").Value) == -1 ? true : false);
            else
                _doSpin = false;

            XElement obj = element.Elements().Where(s => string.Equals(s.Name.ToString().ToLower(), "object")).FirstOrDefault();
            if (obj != null)
                _object = new TimelineKeyObject(obj);

            XElement bone = element.Elements().Where(s => string.Equals(s.Name.ToString().ToLower(), "bone")).FirstOrDefault();
            if (bone != null)
                _bone = new TimelineKeyBone(bone);
        }
        #endregion
    }
}
