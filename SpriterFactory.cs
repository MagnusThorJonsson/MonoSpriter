using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoSpriter.Animation;
using MonoSpriter.Data;
using MonoSpriter.Mainline;
using MonoSpriter.Timeline;

namespace MonoSpriter
{
    /// <summary>
    /// Spriter Factory.
    /// 
    /// A Factory class for SpriterObjects and animations
    /// </summary>
    public class SpriterFactory
    {
        #region Variables & Properties
        private static Dictionary<int, Dictionary<int, Texture2D>> _sprites;
        private static List<SpriterEntity> _entities;
        internal static Dictionary<int, SpriterFolder> _folders;
        #endregion


        #region Constructor
        /// <summary>
        /// Initializes static variables
        /// </summary>
        static SpriterFactory()
        {
            _sprites = new Dictionary<int, Dictionary<int, Texture2D>>();
            _entities = new List<SpriterEntity>();
            _folders = new Dictionary<int, SpriterFolder>();


        }
        #endregion


        #region Factory Methods
        /// <summary>
        /// Creates a new Spriter Object from the passed in data
        /// </summary>
        /// <param name="xmlDoc">The SCML file</param>
        /// <param name="content">The content manager</param>
        /// <param name="path">The path to the entities assets</param>
        /// <param name="name">The name of the entity</param>
        /// <param name="fps">The frames per second the animations run at</param>
        /// <param name="scale">The scale of the object</param>
        /// <param name="offset">The offset</param>
        /// <returns>A new spriter object</returns>
        public static SpriterObject Create(XDocument xmlDoc, ContentManager content, string path, string name, int fps, Vector2 scale, Vector2 offset)
        {
            // Prepare data
            LoadData(path, xmlDoc, content);

            return Create(name, fps, scale, offset);
        }

        /// <summary>
        /// Creates a new Spriter Object from already loaded data
        /// </summary>
        /// <param name="name">The name of the entity</param>
        /// <param name="fps">The frames per second the animations run at</param>
        /// <param name="scale">The scale of the object</param>
        /// <param name="offset">The offset</param>
        /// <returns>A new spriter object</returns>
        public static SpriterObject Create(string name, int fps, Vector2 scale, Vector2 offset)
        {
            // TODO: All of this needs a refactoring, data structure not completely A-OK
            // Get the entity by name
            SpriterEntity entity = _entities.Find(s => string.Equals(s.Name, name));
            if (entity == null)
                throw new ArgumentNullException("name");

            foreach (KeyValuePair<int, SpriterAnimation> animation in entity.Animations)
            {

                int frameStep = 1000 / fps;
                for (int frameId = 0; frameId <= animation.Value.Length; frameId += frameStep)
                {
                    // Prepare tweening process
                    List<SpriterFrameImage> frameImages = new List<SpriterFrameImage>();
                    List<SpriterFrameBone> frameBones = new List<SpriterFrameBone>();
                    float currentTime = 0;
                    float nextTime = 0;

                    // Fetch and set the frames
                    MainlineKey currentFrame = (from kf in animation.Value.MainlineKeys
                                                where kf.Time <= frameId
                                                orderby kf.Time descending
                                                select kf).FirstOrDefault();//animation.Value.FindCurrentFrame(frameId);
                    MainlineKey nextFrame = (from kf in animation.Value.MainlineKeys
                                             where kf.Time > frameId
                                             orderby kf.Time ascending
                                             select kf).FirstOrDefault();//animation.Value.FindNextFrame(frameId);

                    if (nextFrame == null)
                    {
                        nextFrame = animation.Value.IsLoop ? animation.Value.MainlineKeys[0] : currentFrame;
                        nextTime = animation.Value.Length;
                    }
                    else
                        nextTime = nextFrame.Time;

                    currentTime = frameId - currentFrame.Time;
                    nextTime -= currentFrame.Time;

                    // Tween object sprites
                    float amount = MathHelper.Clamp(currentTime / nextTime, 0.0f, 1.0f);
                    foreach (MainlineKeyObject currentKey in currentFrame.ObjectRefs)
                    {
                        MainlineKeyObject nextKey = nextFrame.ObjectRefs.Where(x => x.Timeline == currentKey.Timeline).FirstOrDefault();//.FindObject(currentKey.Id);
                        TimelineKeyObject next = (nextKey == null ? animation.Value.Timelines[currentKey.Timeline].Keys[currentKey.Key].Object
                                                                  : animation.Value.Timelines[nextKey.Timeline].Keys[nextKey.Key].Object);

                        frameImages.Add(
                            Tween(
                                animation.Value.Timelines[currentKey.Timeline].Keys[currentKey.Key].Object,
                                next,
                                amount,
                                animation.Value.Timelines[currentKey.Timeline].Keys[currentKey.Key].DoSpin,
                                currentKey.ZIndex,
                                currentKey.Parent
                            )
                        );
                    }

                    // Tween bones
                    foreach (MainlineKeyBone currentBone in currentFrame.BoneRefs)
                    {
                        MainlineKeyBone nextBone = nextFrame.BoneRefs.Where(x => x.Timeline == currentBone.Timeline).FirstOrDefault();//FindBone(currentBone.Id);
                        TimelineKeyBone next = (nextBone == null ? animation.Value.Timelines[currentBone.Timeline].Keys[currentBone.Key].Bone
                                                                 : animation.Value.Timelines[nextBone.Timeline].Keys[nextBone.Key].Bone);
                        frameBones.Add(
                            Tween(
                                animation.Value.Timelines[currentBone.Timeline].Keys[currentBone.Key].Bone,
                                next,
                                amount,
                                animation.Value.Timelines[currentBone.Timeline].Keys[currentBone.Key].DoSpin,
                                currentBone.Parent
                            )
                        );
                    }


                    // Create Frames
                    SpriterFrame frame = new SpriterFrame(
                        frameImages.OrderBy(x => x.ZIndex).ToList(),
                        frameBones
                    );

                    foreach (SpriterFrameImage image in frame.Frames)
                        image.Transform = frame.Transform(image, scale, offset);

                    animation.Value.Frames.Add(frame);
                }
            }

            return new SpriterObject(name, fps, entity, _sprites);
        }
        #endregion


        #region Private Helper Methods
        /// <summary>
        /// Loads all the relevant data from the SCML file
        /// </summary>
        /// <param name="path">The path to the entities assets</param>
        /// <param name="xmlDoc">The SCML data</param>
        /// <param name="content">The content manager to hold the sprites</param>
        private static void LoadData(string path, XDocument xmlDoc, ContentManager content)
        {
            // Reset content
            _sprites.Clear();
            _entities.Clear();
            _folders.Clear();

            // Ready Folders and Textures
            foreach (XElement folderRow in xmlDoc.Descendants("spriter_data").Elements("folder"))
            {
                SpriterFolder folder = new SpriterFolder(folderRow);
                _folders.Add(folder.Id, folder);

                Dictionary<int, Texture2D> spriteList = new Dictionary<int, Texture2D>();
                foreach (SpriterFile file in folder.Files.Values)
                {
                    try
                    {
                        string assetName = file.Name.Substring(0, file.Name.Length - System.IO.Path.GetExtension(file.Name).Length);
                        Texture2D tex = content.Load<Texture2D>(path + assetName.Replace("/", "\\"));
                        spriteList[file.Id] = tex;
                    }
                    catch (ContentLoadException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception) { }
                }
                _sprites.Add(folder.Id, spriteList);
            }

            // Ready Entities
            foreach (XElement entityRow in xmlDoc.Descendants("spriter_data").Elements("entity"))
                _entities.Add(new SpriterEntity(entityRow));
        }
        #endregion


        #region Tween Methods
        /// <summary>
        /// Makes a tween of two objects
        /// </summary>
        /// <param name="prev">Previous frame</param>
        /// <param name="next">Next frame</param>
        /// <param name="amount">The amount to tween</param>
        /// <param name="isClockwise">Whether this is a clockwise tween</param>
        /// <param name="zIndex">The zIndex of the frame</param>
        /// <param name="parent">The parent (-1 for root)</param>
        /// <returns>A new Spriter Frame Image</returns>
        private static SpriterFrameImage Tween(TimelineKeyObject prev, TimelineKeyObject next, float amount, bool isClockwise, int zIndex, int parent)
        {
            SpriterFrameImage result = new SpriterFrameImage(parent, prev.Folder, prev.File, Vector2.Lerp(prev.Position, next.Position, amount));
            result.IsClockwise = isClockwise;
            result.ZIndex = zIndex;
            result.Pivot = Vector2.Lerp(prev.Pivot, next.Pivot, amount);
            result.Alpha = MathHelper.Lerp(prev.Alpha, next.Alpha, amount);
            result.Scale = Vector2.Lerp(prev.Scale, next.Scale, amount);

            float angleA = prev.Angle;
            float angleB = next.Angle;

            if (isClockwise)
            {
                if (angleB - angleA < 0.0f)
                {
                    angleB += MathHelper.TwoPi;
                }
                else
                {
                    angleA %= MathHelper.TwoPi;
                    angleB %= MathHelper.TwoPi;
                }
            }
            else
            {
                if (angleB - angleA > 0.0f)
                {
                    angleB -= MathHelper.TwoPi;
                }
                else
                {
                    angleA %= MathHelper.TwoPi;
                    angleB %= MathHelper.TwoPi;
                }
            }

            result.Angle = MathHelper.Lerp(angleA, angleB, amount);
            return result;
        }


        /// <summary>
        /// Makes a tween of two bones
        /// </summary>
        /// <param name="prev">Previous bone</param>
        /// <param name="next">Next bone</param>
        /// <param name="amount">The amount to tween</param>
        /// <param name="isClockwise">Whether this is a clockwise tween</param>
        /// <param name="parent">The parent (-1 for root)</param>
        /// <returns>A new frame bone</returns>
        private static SpriterFrameBone Tween(TimelineKeyBone prev, TimelineKeyBone next, float amount, bool isClockwise, int parent)
        {
            SpriterFrameBone result = new SpriterFrameBone();
            result.Parent = parent;
            result.Scale = Vector2.Lerp(prev.Scale, next.Scale, amount);
            result.Position = Vector2.Lerp(prev.Position, next.Position, amount);
            result.Alpha = MathHelper.Lerp(prev.Alpha, next.Alpha, amount);

            float angleA = prev.Angle;
            float angleB = next.Angle;
            if (isClockwise)
            {
                if (angleB - angleA < 0)
                {
                    angleB += MathHelper.TwoPi;
                }
                else
                {
                    angleA %= MathHelper.TwoPi;
                    angleB %= MathHelper.TwoPi;
                }
            }
            else
            {
                if (angleB - angleA > 0.0f)
                {
                    angleB -= MathHelper.TwoPi;
                }
                else
                {
                    angleA %= MathHelper.TwoPi;
                    angleB %= MathHelper.TwoPi;
                }
            }

            result.Angle = MathHelper.Lerp(angleA, angleB, amount);
            return result;
        }
        #endregion
    }
}
