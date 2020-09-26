﻿// <auto-generated />
using System;
using Fabrit.Heroes.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fabrit.Heroes.Data.Migrations
{
    [DbContext(typeof(HeroesDbContext))]
    [Migration("20200826090059_modified password field")]
    partial class modifiedpasswordfield
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Fabrit.Heroes.Data.Entities.Hero.Hero", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ally")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MainPowerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OverallStrength")
                        .HasColumnType("int");

                    b.Property<int?>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MainPowerId");

                    b.HasIndex("TypeId");

                    b.ToTable("Heroes");
                });

            modelBuilder.Entity("Fabrit.Heroes.Data.Entities.Hero.HeroPower", b =>
                {
                    b.Property<int>("HeroId")
                        .HasColumnType("int");

                    b.Property<int>("PowerId")
                        .HasColumnType("int");

                    b.HasKey("HeroId", "PowerId");

                    b.HasIndex("PowerId");

                    b.ToTable("HeroPowers");
                });

            modelBuilder.Entity("Fabrit.Heroes.Data.Entities.Hero.HeroType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HeroTypes");
                });

            modelBuilder.Entity("Fabrit.Heroes.Data.Entities.Hero.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Fabrit.Heroes.Data.Entities.Power", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Element")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainTrait")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Strength")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Powers");
                });

            modelBuilder.Entity("Fabrit.Heroes.Data.Entities.Hero.Hero", b =>
                {
                    b.HasOne("Fabrit.Heroes.Data.Entities.Power", "MainPower")
                        .WithMany()
                        .HasForeignKey("MainPowerId");

                    b.HasOne("Fabrit.Heroes.Data.Entities.Hero.HeroType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId");
                });

            modelBuilder.Entity("Fabrit.Heroes.Data.Entities.Hero.HeroPower", b =>
                {
                    b.HasOne("Fabrit.Heroes.Data.Entities.Hero.Hero", "Hero")
                        .WithMany("Powers")
                        .HasForeignKey("HeroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fabrit.Heroes.Data.Entities.Power", "Power")
                        .WithMany()
                        .HasForeignKey("PowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
