﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using back.DAL;

#nullable disable

namespace back.web.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230327143245_migr")]
    partial class migr
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("back.Entities.NoteEntity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<int>("UserEntityid")
                        .HasColumnType("integer");

                    b.Property<int>("UserStoryPropositionEntityid")
                        .HasColumnType("integer");

                    b.Property<int>("note")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.HasIndex("UserEntityid");

                    b.HasIndex("UserStoryPropositionEntityid");

                    b.ToTable("note");
                });

            modelBuilder.Entity("back.Entities.UserEntity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("user");
                });

            modelBuilder.Entity("back.Entities.UserStoryEntity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("estimatedCost")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("userStory");
                });

            modelBuilder.Entity("back.Entities.UserStoryPropositionEntity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("userStoryProposition");
                });

            modelBuilder.Entity("back.Entities.NoteEntity", b =>
                {
                    b.HasOne("back.Entities.UserEntity", "UserEntity")
                        .WithMany()
                        .HasForeignKey("UserEntityid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("back.Entities.UserStoryPropositionEntity", "UserStoryPropositionEntity")
                        .WithMany()
                        .HasForeignKey("UserStoryPropositionEntityid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserEntity");

                    b.Navigation("UserStoryPropositionEntity");
                });
#pragma warning restore 612, 618
        }
    }
}
