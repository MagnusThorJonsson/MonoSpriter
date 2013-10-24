using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MonoSpriter.Mainline;

namespace MonoSpriter.Animation
{
    /// <summary>
    /// Spriter animation data object.
    /// 
    /// DATA:
    ///   <entity id="0" name="Player">
    ///     <animation id="0" name="idle" length="4000">
    ///       <mainline>
    /// </summary>
    internal sealed class SpriterAnimation
    {
        #region Variables & Properties
        /// <summary>
        /// Animation id
        /// </summary>
        public int Id { get { return _id; } }
        private int _id;

        /// <summary>
        /// Animation name
        /// </summary>
        public string Name { get { return _name; } }
        private string _name;

        /// <summary>
        /// Animation length in milliseconds
        /// </summary>
        public int Length { get { return _length; } }
        private int _length;

        /// <summary>
        /// Is animation a loop?
        /// </summary>
        public bool IsLoop { get { return _isLoop; } }
        private bool _isLoop;

        /// <summary>
        /// The mainline keys in this animation
        /// </summary>
        public List<MainlineKey> MainlineKeys { get { return _mainlineKeys; } }
        private List<MainlineKey> _mainlineKeys;

        /// <summary>
        /// The timelines used by this animation
        /// </summary>
        public Dictionary<int, SpriterTimeline> Timelines { get { return _timelines; } }
        private Dictionary<int, SpriterTimeline> _timelines;

        /// <summary>
        /// The frames used by this animation
        /// </summary>
        public List<SpriterFrame> Frames { get { return _frames; } }
        private List<SpriterFrame> _frames;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor that takes in an XElement containing the Spriter animation data
        /// </summary>
        /// <param name="element">The XElement containing the data</param>
        public SpriterAnimation(XElement element)
        {
            if (element.Attribute("id") != null)
                _id = int.Parse(element.Attribute("id").Value);
            else
                _id = -1;

            if (element.Attribute("name") != null)
                _name = element.Attribute("name").Value;

            if (element.Attribute("length") != null)
                _length = int.Parse(element.Attribute("length").Value);

            if (element.Attribute("looping") != null)
                _isLoop = bool.Parse(element.Attribute("looping").Value);
            else
                _isLoop = true;

            _mainlineKeys = new List<MainlineKey>();
            foreach (XElement row in element.Elements("mainline").Elements("key"))
                _mainlineKeys.Add(new MainlineKey(row));

            _timelines = new Dictionary<int, SpriterTimeline>();
            foreach (XElement row in element.Elements("timeline"))
            {
                SpriterTimeline timeline = new SpriterTimeline(row);
                _timelines.Add(timeline.Id, timeline);
            }

            _frames = new List<SpriterFrame>();
        }
        #endregion
    }
}
