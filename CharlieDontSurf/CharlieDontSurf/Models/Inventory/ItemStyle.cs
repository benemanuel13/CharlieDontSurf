using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharlieDontSurf.Models.Inventory
{
    public class ItemStyle
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public ItemStyle(int id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}