namespace Core.Domain
{
    public class SkillModel
    {
        public string Name { get; private set; }
        public SkillModel(ModelName name)
        {
            Name = name;
        }
    }
}