﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using mobpsycho.Models;

#nullable disable

namespace mobpsycho.Migrations
{
    [DbContext(typeof(MobpsychoDbContext))]
    partial class MobpsychoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("mobpsycho.Models.Abilitie", b =>
                {
                    b.Property<int>("IdAbilitie")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAbilitie"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdCharacter")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdAbilitie");

                    b.HasIndex("IdCharacter");

                    b.ToTable("Abilitie", (string)null);
                });

            modelBuilder.Entity("mobpsycho.Models.Character", b =>
                {
                    b.Property<int>("IdCharacter")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCharacter"), 1L, 1);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlImg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdCharacter");

                    b.ToTable("Character", (string)null);
                });

            modelBuilder.Entity("mobpsycho.Models.User", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUser"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdUser");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("mobpsycho.Models.Abilitie", b =>
                {
                    b.HasOne("mobpsycho.Models.Character", "Character")
                        .WithMany("Abilities")
                        .HasForeignKey("IdCharacter")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Character");
                });

            modelBuilder.Entity("mobpsycho.Models.Character", b =>
                {
                    b.Navigation("Abilities");
                });
#pragma warning restore 612, 618
        }
    }
}
