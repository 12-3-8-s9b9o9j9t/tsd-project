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
    [Migration("20230525125943_Migration3")]
    partial class Migration3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SessionEntityUserEntity", b =>
                {
                    b.Property<int>("sessionsid")
                        .HasColumnType("integer");

                    b.Property<int>("usersid")
                        .HasColumnType("integer");

                    b.HasKey("sessionsid", "usersid");

                    b.HasIndex("usersid");

                    b.ToTable("SessionEntityUserEntity");
                });

            modelBuilder.Entity("SessionEntityUserStoryEntity", b =>
                {
                    b.Property<int>("sessionsid")
                        .HasColumnType("integer");

                    b.Property<int>("userStoriesid")
                        .HasColumnType("integer");

                    b.HasKey("sessionsid", "userStoriesid");

                    b.HasIndex("userStoriesid");

                    b.ToTable("SessionEntityUserStoryEntity");
                });

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

            modelBuilder.Entity("back.Entities.SessionEntity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("identifier")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("session");
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

                    b.Property<string>("tasks")
                        .IsRequired()
                        .HasColumnType("text");

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

                    b.Property<string>("tasks")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("userStoryProposition");
                });

            modelBuilder.Entity("SessionEntityUserEntity", b =>
                {
                    b.HasOne("back.Entities.SessionEntity", null)
                        .WithMany()
                        .HasForeignKey("sessionsid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("back.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("usersid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SessionEntityUserStoryEntity", b =>
                {
                    b.HasOne("back.Entities.SessionEntity", null)
                        .WithMany()
                        .HasForeignKey("sessionsid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("back.Entities.UserStoryEntity", null)
                        .WithMany()
                        .HasForeignKey("userStoriesid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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
