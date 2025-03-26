namespace Engine.Models
{
    public class Stats
    {
        private int _hp;
        public int Hp
        {
            get => _hp;
            set => _hp = value < 0 ? 0 : value;
        }
        public int HpMax { get; set; }

        private int _strength;
        public int Strength
        {
            get => _strength;
            set => _strength = value < 0 ? 0 : value;
        }

        private int _magic;
        public int Magic
        {
            get => _magic;
            set => _magic = value < 0 ? 0 : value;
        }

        private int _skill;
        public int Skill
        {
            get => _skill;
            set => _skill = value < 0 ? 0 : value;
        }

        private int _speed;
        public int Speed
        {
            get => _speed;
            set => _speed = value < 0 ? 0 : value;
        }

        private int _luck;
        public int Luck
        {
            get => _luck;
            set => _luck = value < 0 ? 0 : value;
        }

        private int _defense;
        public int Defense
        {
            get => _defense;
            set => _defense = value < 0 ? 0 : value;
        }

        private int _resistance;
        public int Resistance
        {
            get => _resistance;
            set => _resistance = value < 0 ? 0 : value;
        }
    }
}