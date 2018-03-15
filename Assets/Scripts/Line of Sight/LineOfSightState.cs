public enum LineOfSightState : byte
{
    None = 0,

    //Cell is currently in sight of an actor
    Active = 1,

    //Cell is not currently in sight, but has been before
    Passive = 2,

    //Cell has never been seen
    Disabled = 3,
}