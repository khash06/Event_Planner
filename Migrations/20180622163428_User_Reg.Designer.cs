﻿// <auto-generated />
using Belt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Belt.Migrations
{
    [DbContext(typeof(BeltContext))]
    [Migration("20180622163428_User_Reg")]
    partial class User_Reg
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("Belt.Models.Activities", b =>
                {
                    b.Property<int>("ActivitiesId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Creator");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Descripton");

                    b.Property<int>("Duration");

                    b.Property<int>("Participants");

                    b.Property<DateTime>("StartTime");

                    b.Property<string>("Title");

                    b.HasKey("ActivitiesId");

                    b.ToTable("activities");
                });

            modelBuilder.Entity("Belt.Models.Attendee", b =>
                {
                    b.Property<int>("AttendeeId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActivitiesId");

                    b.Property<int>("UserId");

                    b.HasKey("AttendeeId");

                    b.HasIndex("ActivitiesId");

                    b.HasIndex("UserId");

                    b.ToTable("attendess");
                });

            modelBuilder.Entity("Belt.Models.User_Reg", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.HasKey("UserId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Belt.Models.Attendee", b =>
                {
                    b.HasOne("Belt.Models.Activities", "Activities")
                        .WithMany("Reserver")
                        .HasForeignKey("ActivitiesId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Belt.Models.User_Reg", "User_Reg")
                        .WithMany("Attendee")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}