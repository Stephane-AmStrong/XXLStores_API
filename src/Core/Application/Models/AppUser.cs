﻿using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class AppUser : IdentityUser
    {
        //public AppUser()
        //{
        //    Tokens = new HashSet<UserToken>();
        //}


        public string ImgLink { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        //public virtual ICollection<UserToken> Tokens { get; set; }
    }
}
