﻿using Microsoft.EntityFrameworkCore;
using Voxel.Domain.Entities;
using Voxel.Infrastructure.Persistence.Configurations;

namespace Voxel.Infrastructure.Context
{
    public class VoxelContext : DbContext
    {
        public VoxelContext(DbContextOptions<VoxelContext> options) : base(options)
        {
        }

        // Isso diz ao banco: "Crie uma tabela de Usuarios baseada na minha classe"
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        }
    }
}
