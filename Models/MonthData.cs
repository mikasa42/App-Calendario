using SQLite;

namespace Calendar.Models
{
    public class MonthData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public uint Month { get; set; }

        [NotNull]
        public uint Year { get; set; }

        [NotNull]
        public uint DayStatesPacked {  get; set; }

        [Ignore]
        public bool[] DayStates
        {
            get => UnpackDayStates(DayStatesPacked);
            set => DayStatesPacked = PackDayStates(value);
        }

        public bool GetDayStateAt(int index)
        {
            ValidateIndex(index);
            return (DayStatesPacked & (1u << index)) != 0;
        }

        // Set specific boolean at index 0–31
        public void SetDayStateAt(int index, bool value)
        {
            ValidateIndex(index);
            if (value)
                DayStatesPacked |= (1u << index);   // Set bit
            else
                DayStatesPacked &= ~(1u << index);  // Clear bit
        }

        private static void ValidateIndex(int index)
        {
            if (index < 0 || index >= 32)
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 31.");
        }

        private static uint PackDayStates(bool[] flags)
        {
            uint packed = 0;
            int length = Math.Min(flags.Length, 32);
            for (int i = 0; i < length; i++)
            {
                if (flags[i])
                    packed |= (1u << i);
            }
            return packed;
        }

        private static bool[] UnpackDayStates(uint packed)
        {
            var result = new bool[32];
            for (int i = 0; i < 32; i++)
            {
                result[i] = (packed & (1u << i)) != 0;
            }
            return result;
        }
    }
}
