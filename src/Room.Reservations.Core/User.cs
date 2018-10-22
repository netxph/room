using System;

namespace Room.Reservations.Core
{
    public class User
    {

        public User(int id, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            ID = id;
        }

        public User(string name)
            :this(0, name)
        {
            
        }

        public int ID { get; }
        public string Name { get; }
    }
}