using System;
using System.Collections.Generic;
using System.Text;
using AssetTrackingEF.domain;
using Microsoft.EntityFrameworkCore;


namespace AssetTrackingEF.data{

    public class AssetContext:DbContext{
        public DbSet<Asset> Assets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-4TL0264;Initial Catalog=AssetTrack;Integrated Security=True");
        }
    }
}

