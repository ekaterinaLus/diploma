﻿using System.ComponentModel.DataAnnotations;


namespace DataStore.Entities
{
    public class ProjectsTags
    {
        

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
