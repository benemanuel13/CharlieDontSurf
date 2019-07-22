﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharlieDontSurf.Models.Inventory
{
    public class ItemType
    {
        private int id;
        private string name;

        public ItemType(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }
    }
}