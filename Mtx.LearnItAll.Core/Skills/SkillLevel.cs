namespace Mtx.LearnItAll.Core
{
    public abstract record SkillLevel
    {
        public abstract int Number { get; }
        public string Description => (Number) switch
        {
            0 => Resources.Text.SkillLevel_Unfamiliar,
            1 => Resources.Text.SkillLevel_Novice,
            2 => Resources.Text.SkillLevel_AdvancedBeginner,
            3 => Resources.Text.SkillLevel_Competent,
            4 => Resources.Text.SkillLevel_Proficient,
            5 => Resources.Text.SkillLevel_Expert,
            _ => Resources.Text.SkillLevel_Unknown

        };
        public static SkillLevel Convert(int level) => (SkillLevel)level;
        public static implicit operator int(SkillLevel level) => level.Number;
        public static implicit operator SkillLevel(int level) => (level) switch
        {
            0 => Unfamiliar,
            1 => Novice,
            2 => AdvancedBeginner,
            3 => Competent,
            4 => Proficient,
            5 => Expert,
            _ => Unknown

        };
        public static SkillLevel Unfamiliar => new Unfamiliar();
        public static SkillLevel Novice => new Novice();
        public static SkillLevel AdvancedBeginner => new AdvancedBeginner();
        public static SkillLevel Competent => new Competent();
        public static SkillLevel Proficient => new Proficient();
        public static SkillLevel Expert => new Expert();
        public static SkillLevel Unknown => new Unknown();

    }
}