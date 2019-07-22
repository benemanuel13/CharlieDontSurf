using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharlieDontSurf.Models.Inventory
{
    public class ItemSize
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public ItemSize(int id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}