﻿using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TodoApi.Domain.Todos;

namespace TodoApi.Infrastructure.Persistence
{
    public class TodoDbContext : DbContext
    {

        public TodoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Todo> Todos => Set<Todo>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Todo>()
                .HasKey(x => x.Id);

            builder.Entity<Todo>()
                .Property(x => x.Progress)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Todo>()
                .Property(x => x.Title)
                .IsRequired(true);

            builder.Entity<Todo>()
                .HasMany(x => x.TodoItems)
                .WithOne(x => x.Todo)
                .HasForeignKey(x => x.TodoId);
        }

    }
}
