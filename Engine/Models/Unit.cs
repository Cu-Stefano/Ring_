using Engine.Factories;
using System.Collections.ObjectModel;
using System.ComponentModel;
// ReSharper disable All
namespace Engine.Models
{

    public class Unit : BaseNotification
    {
        private string _name;
        private UnitType _type;
        private ClassType _class;
        
        private int _level;
        private int _move;

        private Stats _stats;
        private int _attack;
        private int _defense;
        private int _dodge;
        private int _critic;

        private Weapon _equipedWeapon;


        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public UnitType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        public ClassType Class
        {
            get
            {
                return _class;
            }
            set
            {
                _class = value;
                OnPropertyChanged(nameof(Class));
            }
        }

        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                OnPropertyChanged(nameof(Level));
            }
        }
        public int Move
        {
            get
            {
                return _move;
            }
            set
            {
                _move = value;
                OnPropertyChanged(nameof(Move));
            }
        }

        public Stats Statistics
        {
            get
            {
                return _stats;
            }
            set
            {
                _stats = value;
                OnPropertyChanged(nameof(Statistics));
            }
        }

        public int Attack
        {
            get
            {
                return _attack;
            }
            set
            {
                _attack = value;
                OnPropertyChanged(nameof(Attack));
            }
        }
        public int Hit
        {
            get
            {
                return _defense;
            }
            set
            {
                _defense = value;
                OnPropertyChanged(nameof(Hit));
            }
        }
        public int Dodge
        {
            get
            {
                return _dodge;
            }
            set
            {
                _dodge = value;
                OnPropertyChanged(nameof(Dodge));
            }
        }
        public int Critic
        {
            get
            {
                return _critic;
            }
            set
            {
                _critic = value;
                OnPropertyChanged(nameof(Critic));
            }
        }

        public Weapon EquipedWeapon
        {
            get
            {
                return _equipedWeapon;
            }
            set
            {
                _equipedWeapon = value;
                OnPropertyChanged(nameof(EquipedWeapon));
            }
        }

        public ObservableCollection<GameItem> Inventory { get; set; }
        public ObservableCollection<QuestStatus> Quests { get; set; }

        public Unit(string name, UnitType type, ClassType classType, int level, int move, Stats stats, Weapon? equipedWeapon)
        {
            Name = name;
            Type = type;
            Class = classType;
            Level = level;
            Move = move;
            Statistics = stats;
            Attack = Get_Attack(Statistics.Strength,Statistics.Magic, equipedWeapon);
            Hit = Get_Hit(Statistics.Skill, Statistics.Luck, equipedWeapon);
            Dodge = 100;
            Critic = 0;
            EquipedWeapon = equipedWeapon;

            Inventory = new ObservableCollection<GameItem>();
            Quests = new ObservableCollection<QuestStatus>();
        }

        private int Get_Attack(int str, int mag, Weapon? equipedWeapon)
        {
            if (equipedWeapon == null)
            {
                return str;
            }
            return str > mag ? str + equipedWeapon.MinDamage : equipedWeapon.MinDamage + mag;
        }

        private int Get_Hit(int skill, int luck, Weapon? equipedWeapon)
        {
            if (equipedWeapon == null)
            {
                return skill;
            }
            return equipedWeapon.Critical + skill*2 + luck;
        }




    }
}