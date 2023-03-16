﻿// <auto-generated />
using System;
using LegionWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LegionWebApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230316214827_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LegionWebApp.Models.GalleryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("GalleryItems");
                });

            modelBuilder.Entity("LegionWebApp.Models.Media", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("GalleryItemId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GalleryItemId");

                    b.ToTable("Media");
                });

            modelBuilder.Entity("LegionWebApp.Models.Image", b =>
                {
                    b.HasBaseType("LegionWebApp.Models.Media");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable("Image", (string)null);
                });

            modelBuilder.Entity("LegionWebApp.Models.Video", b =>
                {
                    b.HasBaseType("LegionWebApp.Models.Media");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Poster")
                        .HasColumnType("text");

                    b.ToTable("Video", (string)null);
                });

            modelBuilder.Entity("LegionWebApp.Models.Media", b =>
                {
                    b.HasOne("LegionWebApp.Models.GalleryItem", null)
                        .WithMany("Media")
                        .HasForeignKey("GalleryItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LegionWebApp.Models.Image", b =>
                {
                    b.HasOne("LegionWebApp.Models.Media", null)
                        .WithOne()
                        .HasForeignKey("LegionWebApp.Models.Image", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LegionWebApp.Models.Video", b =>
                {
                    b.HasOne("LegionWebApp.Models.Media", null)
                        .WithOne()
                        .HasForeignKey("LegionWebApp.Models.Video", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LegionWebApp.Models.GalleryItem", b =>
                {
                    b.Navigation("Media");
                });
#pragma warning restore 612, 618
        }
    }
}
