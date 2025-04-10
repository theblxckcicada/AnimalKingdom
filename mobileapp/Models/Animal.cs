﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobileapp.Models
{
    public  class Animal
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public AnimalCategory Category { get; set; }
        public string Image { get; set; }
    }

    public enum AnimalCategory
    {
        Herbivore,
        Carnivore,
        Omnivore
    }
}
