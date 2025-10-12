/// <summary>
/// A class holding the Levels and other configuration data for the game. (Kept separate so changes are tracked accordingly)
/// The Levels array contains the scene configuration and can be edited to change the relationship between and the existing levels in the level select.
/// </summary>
public class GameConfig
{
    // For some reason when I serialize this it curses my unity
    // Holds the level configuration for the game.
    // I've added two placeholders that refer to the menu and the character select scenes. This is NOT how these scenes
    // are meant to be loaded (they may not load properly right now due to multiple instances of dontdestroyonload objects)
    // they are simply real scene names so my sanity checking script doesn't log error if their scene name is not found in the build list.
    // Currently demonstrated behavior is Level 1 completion unlocks the two placeholders.
    public static readonly Level[] Levels = {
        new("Level 1 - Cafe", "Cafe", LevelStatus.Unlocked, levelArtSpriteName: "LevelCafe"),
    };
}