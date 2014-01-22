using Microsoft.Xna.Framework;
using System;

namespace MonoSpriter.Timeline
{
    public interface ITimelineKeyItem
    {
        Vector2 Position { get; }
        float Angle { get; }
        Vector2 Scale { get; }
        float Alpha { get; }
    }
}
