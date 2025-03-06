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
                return Get_Attack();
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
                return Get_Hit();
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
                return Statistics.Speed * 2 + Statistics.Luck;
            }
            set{}
        }
        public int Critic
        {
            get
            {
                return Get_Crit();
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
            Attack = Get_Attack();
            Hit = Get_Hit();
            Critic = Get_Crit();
            CanMove = true;

            Inventory = [];

            if (equipedWeapon != null)
            {
                Inventory.Add(equipedWeapon);
                Inventory.Add(WeaponFactory.CreateWeapon("IronSword"));
                Inventory.Add(WeaponFactory.CreateWeapon("BronzeAxe"));
                Inventory.Add(ItemFactory.CreateGameItem("IronShield"));
                Inventory.Add(WeaponFactory.CreateWeapon("FireTome"));
                Inventory.Add(WeaponFactory.CreateWeapon("WoodenBow"));
                Inventory.Add(WeaponFactory.CreateWeapon("Heal"));
                Inventory.Add(WeaponFactory.CreateWeapon("BronzeLance"));
                EquipedWeapon = (Weapon)Inventory.First(item => item == equipedWeapon)!;
            }
        }

        public Unit()
        {
        }

        public int Get_Attack()
        {
            if (_equipedWeapon == null)
            {
                return _stats.Strength;
            }
            return _stats.Strength > _stats.Magic ? _stats.Strength + _equipedWeapon.Damage : _equipedWeapon.Damage + _stats.Magic;
        }

        public int Get_Hit()
        {
            if (_equipedWeapon == null)
            {
                return _stats.Skill;
            }
            return _equipedWeapon.Hit + _stats.Skill * 2 + _stats.Luck;
        }

        public int Get_Crit()
        {
            if (_equipedWeapon == null)
            {
                return _stats.Skill;
            }
            return _equipedWeapon.Critical + _stats.Skill / 2;
        }



    }
}