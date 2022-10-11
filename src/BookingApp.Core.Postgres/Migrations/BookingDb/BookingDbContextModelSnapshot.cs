﻿// <auto-generated />
using System;
using BookingApp.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookingApp.Core.Postgres.Migrations.BookingDb
{
    [DbContext(typeof(BookingDbContext))]
    partial class BookingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BookingApp.Core.Bookings.Models.Booking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CheckIn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CheckOut")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid>("PlaceId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PlaceId");

                    b.HasIndex(new[] { "CheckIn" }, "idx_booking_checkin_search");

                    b.HasIndex(new[] { "CheckOut" }, "idx_booking_checkout_search");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("BookingApp.Core.Bookings.Models.BookingGuest", b =>
                {
                    b.Property<Guid>("BookingId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("GuestId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsPrincipal")
                        .HasColumnType("boolean");

                    b.HasKey("BookingId", "GuestId");

                    b.ToTable("BookingGuests");
                });

            modelBuilder.Entity("BookingApp.Core.GuestContacts.Models.GuestContact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("GuestId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GuestId");

                    b.HasIndex(new[] { "Type" }, "idx_guestcontact_type_search");

                    b.ToTable("GuestContacts");
                });

            modelBuilder.Entity("BookingApp.Core.Guests.Models.Guest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Guests");
                });

            modelBuilder.Entity("BookingApp.Core.Places.Models.Place", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Places");
                });

            modelBuilder.Entity("BookingApp.Core.Bookings.Models.Booking", b =>
                {
                    b.HasOne("BookingApp.Core.Places.Models.Place", "Place")
                        .WithMany("Bookings")
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Place");
                });

            modelBuilder.Entity("BookingApp.Core.Bookings.Models.BookingGuest", b =>
                {
                    b.HasOne("BookingApp.Core.Bookings.Models.Booking", "Booking")
                        .WithMany("Guests")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingApp.Core.Guests.Models.Guest", "Guest")
                        .WithMany("Bookings")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Guest");
                });

            modelBuilder.Entity("BookingApp.Core.GuestContacts.Models.GuestContact", b =>
                {
                    b.HasOne("BookingApp.Core.Guests.Models.Guest", "Guest")
                        .WithMany("Contacts")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guest");
                });

            modelBuilder.Entity("BookingApp.Core.Bookings.Models.Booking", b =>
                {
                    b.Navigation("Guests");
                });

            modelBuilder.Entity("BookingApp.Core.Guests.Models.Guest", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Contacts");
                });

            modelBuilder.Entity("BookingApp.Core.Places.Models.Place", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
