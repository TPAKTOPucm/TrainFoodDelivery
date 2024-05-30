﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TrainFoodDelivery.Data;

#nullable disable

namespace TrainFoodDelivery.Migrations
{
    [DbContext(typeof(TrainFoodDeliveryContext))]
    [Migration("20240530172656_add_photos_and_videos")]
    partial class add_photos_and_videos
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.26");

            modelBuilder.Entity("IngredientRecipe", b =>
                {
                    b.Property<int>("IngredientsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RecipesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("IngredientsId", "RecipesId");

                    b.HasIndex("RecipesId");

                    b.ToTable("IngredientRecipe");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Ingredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TrainNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WagonNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TrainNumber");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PaymentType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TicketId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.OrderRecipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RecipeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("RecipeId");

                    b.ToTable("OrderRecipes");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Photo", b =>
                {
                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<int>("RecipeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Path");

                    b.HasIndex("RecipeId");

                    b.ToTable("Photo");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Cost")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Netto")
                        .HasColumnType("TEXT");

                    b.Property<int>("NettoType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.ProductOrder", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProductId", "OrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("ProductOrders");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TrainNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WagonNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProductId")
                        .IsUnique();

                    b.HasIndex("TrainNumber");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SeatNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TrainNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("WagonNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TrainNumber");

                    b.HasIndex("UserId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Train", b =>
                {
                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("RouteId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WagonAmount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Number");

                    b.ToTable("Trains");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Video", b =>
                {
                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<int>("RecipeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Path");

                    b.HasIndex("RecipeId");

                    b.ToTable("Video");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.WagonProduct", b =>
                {
                    b.Property<int>("TrainNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WagonNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductAmount")
                        .HasColumnType("INTEGER");

                    b.HasKey("TrainNumber", "WagonNumber", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("WagonProducts");
                });

            modelBuilder.Entity("IngredientRecipe", b =>
                {
                    b.HasOne("TrainFoodDelivery.Models.Ingredient", null)
                        .WithMany()
                        .HasForeignKey("IngredientsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainFoodDelivery.Models.Recipe", null)
                        .WithMany()
                        .HasForeignKey("RecipesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Ingredient", b =>
                {
                    b.HasOne("TrainFoodDelivery.Models.Train", "Train")
                        .WithMany()
                        .HasForeignKey("TrainNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Train");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Order", b =>
                {
                    b.HasOne("TrainFoodDelivery.Models.Ticket", "Ticket")
                        .WithMany("Orders")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.OrderRecipe", b =>
                {
                    b.HasOne("TrainFoodDelivery.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainFoodDelivery.Models.Recipe", "Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Photo", b =>
                {
                    b.HasOne("TrainFoodDelivery.Models.Recipe", "Recipe")
                        .WithMany("Photos")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.ProductOrder", b =>
                {
                    b.HasOne("TrainFoodDelivery.Models.Order", "Order")
                        .WithMany("Products")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainFoodDelivery.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Recipe", b =>
                {
                    b.HasOne("TrainFoodDelivery.Models.Product", "Product")
                        .WithOne("Recipe")
                        .HasForeignKey("TrainFoodDelivery.Models.Recipe", "ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainFoodDelivery.Models.Train", "Train")
                        .WithMany()
                        .HasForeignKey("TrainNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Train");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Ticket", b =>
                {
                    b.HasOne("TrainFoodDelivery.Models.Train", "Train")
                        .WithMany()
                        .HasForeignKey("TrainNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainFoodDelivery.Models.User", "User")
                        .WithMany("Tickets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Train");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Video", b =>
                {
                    b.HasOne("TrainFoodDelivery.Models.Recipe", "Recipe")
                        .WithMany("Videos")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.WagonProduct", b =>
                {
                    b.HasOne("TrainFoodDelivery.Models.Product", "Product")
                        .WithMany("WagonProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainFoodDelivery.Models.Train", "Train")
                        .WithMany()
                        .HasForeignKey("TrainNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Train");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Order", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Product", b =>
                {
                    b.Navigation("Recipe");

                    b.Navigation("WagonProducts");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Recipe", b =>
                {
                    b.Navigation("Photos");

                    b.Navigation("Videos");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.Ticket", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("TrainFoodDelivery.Models.User", b =>
                {
                    b.Navigation("Tickets");
                });
#pragma warning restore 612, 618
        }
    }
}
