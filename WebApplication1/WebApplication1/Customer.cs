﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public class Customer
    {
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string Name { get; set; }
    }
}