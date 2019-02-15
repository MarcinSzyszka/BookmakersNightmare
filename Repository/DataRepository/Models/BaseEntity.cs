using System;
using SQLite;

namespace DataRepository.Models
{
    public class BaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
