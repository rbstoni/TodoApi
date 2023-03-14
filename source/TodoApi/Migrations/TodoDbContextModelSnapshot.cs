﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoApi.Infrastructure.Persistence;

#nullable disable

namespace TodoApi.Migrations
{
    [DbContext(typeof(TodoDbContext))]
    partial class TodoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TodoApi.Domain.Todos.Todo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CompletedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Progress")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Todos");
                });

            modelBuilder.Entity("TodoApi.Domain.Todos.TodoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Done")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TodoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TodoId");

                    b.ToTable("TodoItem");
                });

            modelBuilder.Entity("TodoApi.Domain.Todos.TodoItem", b =>
                {
                    b.HasOne("TodoApi.Domain.Todos.Todo", "Todo")
                        .WithMany("TodoItems")
                        .HasForeignKey("TodoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Todo");
                });

            modelBuilder.Entity("TodoApi.Domain.Todos.Todo", b =>
                {
                    b.Navigation("TodoItems");
                });
#pragma warning restore 612, 618
        }
    }
}
