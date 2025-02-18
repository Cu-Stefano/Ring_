using Engine.Factories;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace Engine.Models
{

    public class Unit : BaseNotification
    {
        private string _name;
        private UnitType _type;
        private ClassType _class;
        
        private int _level;

        private Stats _stats;
        private int _attack;
        private int _defense;
        private int _dodge;
        private int _critic;
        private bool _canMove;

        private Weapon _equipedWeapon;
        public List<GameItem?> Inventory { get; set; }
        public List<QuestStatus> Quests { get; set; }


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

        public bool CanMove
        {
            get
            {
                return _canMove;
            }
            set
            {
                _canMove = value;
                OnPropertyChanged(nameof(CanMove));
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

        public Unit(string name, UnitType type, ClassType classType, int level, Stats stats, Weapon? equipedWeapon)
        {
            Name = name;
            Type = type;
            Class = classType;
            Level = level;
            Statistics = stats;
            Attack = Get_Attack(Statistics.Strength,Statistics.Magic, equipedWeapon);
            Hit = Get_Hit(Statistics.Skill, Statistics.Luck, equipedWeapon);
            Dodge = 100;
            Critic = 0;
            CanMove = true;

            Inventory = new List<GameItem?>();
            Quests = new List<QuestStatus>();

            if (equipedWeapon != null)
            {
                Inventory.Add(equipedWeapon);
                EquipedWeapon = (Weapon)Inventory.First(item => item == equipedWeapon);
            }
        }

        private int Get_Attack(int str, int mag, Weapon? equipedWeapon)
        {
            if (equipedWeapon == null)
            {
                return str;
            }
            return str > mag ? str + equipedWeapon.MaxDamage : equipedWeapon.MaxDamage + mag;
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