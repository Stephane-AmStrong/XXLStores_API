﻿using Application.Features.Categories.Queries.GetCategoryById;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.AppUsers.Queries.GetAppUserById
{
    public class GetAppUserViewModel
    {
        public virtual Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string ImgLink { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float Price { get; set; }
        public string StatusAppUser { get; set; }
        public int NoPriority { get; set; }
        public Guid CategoryId { get; set; }

        public GetCategoryViewModel Category { get; set; }

        public string AppUserId { get; set; }

        public GetAppUserViewModel AppUser { get; set; }

        public virtual ICollection<GetPlacesViewModel> Places { get; set; }
    }
}
