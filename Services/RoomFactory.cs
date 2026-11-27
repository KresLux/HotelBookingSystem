using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelsManager
{
    public class RoomFactory
    {
        public IRoom CreateStandardRoom(string roomNumber)
        {
            ValidateRoomNumber(roomNumber);
            return new StandardRoom(roomNumber);
        }

        public IRoom CreateDeluxeRoom(string roomNumber)
        {
            ValidateRoomNumber(roomNumber);
            return new DeluxeRoom(roomNumber);
        }

        public IRoom CreateSuite(string roomNumber)
        {
            ValidateRoomNumber(roomNumber);
            return new Suite(roomNumber);
        }

        public IRoom CreateFamilyRoom(string roomNumber)
        {
            ValidateRoomNumber(roomNumber);
            return new FamilyRoom(roomNumber);
        }

        public IRoom Create(RoomType type, string roomNumber)
        {
            return type switch
            {
                RoomType.Standard => CreateStandardRoom(roomNumber),
                RoomType.Deluxe => CreateDeluxeRoom(roomNumber),
                RoomType.Suite => CreateSuite(roomNumber),
                RoomType.Family => CreateFamilyRoom(roomNumber),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Неизвестный тип номера.")
            };
        }

        private static void ValidateRoomNumber(string roomNumber)
        {
            if (string.IsNullOrWhiteSpace(roomNumber))
            {
                throw new ArgumentException("Номер комнаты не может быть пустым.", nameof(roomNumber));
            }
        }
    }
}
