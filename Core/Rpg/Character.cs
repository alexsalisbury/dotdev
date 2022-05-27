namespace Dotdev.Core.Rpg;

public record Character
{
    public string Name { get; set; }
    public CharacterClass MainClass { get; set; }
    public CharacterTitle SubClass { get; set; }
    public IEnumerable<CharacterTitle> Titles { get; set; }
    public IEnumerable<CharacterTrait> Traits { get; set; }
    public IEnumerable<CharacterTitle> Skills { get; set; }
}

public enum TitleType
{
    Unknown = 0,
    Title = 1,
    Mainclass = 2,
    Subclass = 3,
    Trait = 4,
    Skill = 5
}

public record CharacterTitle(TitleType ModifierType, string TitleName, bool Locked, uint? Level, uint XPTarget, uint XPCurrent, uint XPLifetime)
{
    public static CharacterTitle GenLocked(TitleType titleType) => new CharacterTitle(titleType, "Locked", true, null, 0, 0, 0);
}

public record CharacterClass(string TitleName, bool Locked, uint? Level, uint XPTarget, uint XPCurrent, uint XPLifetime) 
    : CharacterTitle(TitleType.Mainclass, TitleName, Locked, Level, XPTarget,XPCurrent, XPLifetime)
{
    public static CharacterTitle GenSubclass(string TitleName, bool Locked, uint? Level, uint XPTarget, uint XPCurrent, uint XPLifetime) =>        
        new CharacterTitle(TitleType.Subclass, TitleName, Locked, Level, XPTarget, XPCurrent, XPLifetime);
}

public record CharacterTrait(string TitleName, bool Locked)
    : CharacterTitle(TitleType.Trait, TitleName, Locked, null, 0, 0, 0);

public record CharacterSkill(string TitleName, bool Locked, uint level=1)
    : CharacterTitle(TitleType.Skill, TitleName, Locked, level, 0, 0, 0);