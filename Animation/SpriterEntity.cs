using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq; 

namespace MonoSpriter.Animation
{
    /// <summary>
    /// Spriter entity data object.
    /// 
    /// DATA:
    ///   <spriter_data scml_version="1.0" generator="BrashMonkey Spriter" generator_version="b5">
    ///     <entity id="0" name="Player">
    ///       <animation id="0" name="idle" length="4000">
    /// </summary>
    internal sealed class SpriterEntity
    {
        #region Variables & Properties
        /// <summary>
        /// Spriter Entity id
        /// </summary>
        public int Id { get { return _id; } }
        private int _id;

        /// <summary>
        /// Spriter Entity name
        /// </summary>
        public string Name { get { return _name; } }
        private string _name;

        /// <summary>
        /// The entity character maps (skins)
        /// </summary>
        public List<CharacterMap> CharacterMaps { get { return _characterMaps; } }
        private List<CharacterMap> _characterMaps;

        /// <summary>
        /// The entity animation list
        /// </summary>
        public Dictionary<int, SpriterAnimation> Animations { get { return _animations; } }
        private Dictionary<int, SpriterAnimation> _animations;
        #endregion


        #region Constructors
        /// <summary>
        /// Constructor that takes in an XElement containing the Spriter animation data
        /// </summary>
        /// <param name="element">The XElement containing the data</param>
        public SpriterEntity(XElement element)
        {
            if (element.Attribute("id") != null)
                _id = int.Parse(element.Attribute("id").Value);
            else
                _id = -1;

            if (element.Attribute("name") != null)
                _name = element.Attribute("name").Value;

            _characterMaps = new List<CharacterMap>();
            foreach (XElement row in element.Elements().Where(s => string.Equals(s.Name.ToString().ToLower(), "character_map")))
                _characterMaps.Add(new CharacterMap(row));

            _animations = new Dictionary<int, SpriterAnimation>();
            foreach (XElement row in element.Elements().Where(s => string.Equals(s.Name.ToString().ToLower(), "animation")))
            {
                SpriterAnimation anim = new SpriterAnimation(row);
                _animations.Add(anim.Id, anim);
            }
        }
        #endregion


        #region Helper Methods
        /// <summary>
        /// Gets an animation id
        /// </summary>
        /// <param name="name">The name of the animation</param>
        /// <returns>The id of the animation or -1 if none found</returns>
        public int GetAnimationId(string name)
        {
            foreach (KeyValuePair<int, SpriterAnimation> animation in _animations)
            {
                if (animation.Value.Name.Equals(name))
                    return animation.Key;
            }

            return -1;
        }

        /// <summary>
        /// Gets a specific character map
        /// </summary>
        /// <param name="name">The name of the character map to get</param>
        /// <returns>The character map found</returns>
        public CharacterMap GetCharacterMap(string name)
        {
            foreach (CharacterMap map in _characterMaps)
            {
                if (map.Name.Equals(name))
                    return map;
            }

            return null;
        }

        /// <summary>
        /// Gets a specific character map
        /// </summary>
        /// <param name="id">The id of the character map to get</param>
        /// <returns>The character map found</returns>
        public CharacterMap GetCharacterMap(int id)
        {
            foreach (CharacterMap map in _characterMaps)
            {
                if (map.Id == id)
                    return map;
            }

            return null;
        }
        #endregion
    }
}
