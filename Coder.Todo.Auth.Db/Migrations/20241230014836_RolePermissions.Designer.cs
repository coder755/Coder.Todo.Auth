﻿// <auto-generated />
using System;
using Coder.Todo.Auth.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Coder.Todo.Auth.Db.Migrations
{
    [DbContext(typeof(AuthContext))]
    [Migration("20241230014836_RolePermissions")]
    partial class RolePermissions
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Coder.Todo.Auth.Db.Permission", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(16)
                        .HasColumnType("varbinary(16)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("IX_Permissions_Name");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Coder.Todo.Auth.Db.Role", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(16)
                        .HasColumnType("varbinary(16)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("IX_Roles_Name");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Coder.Todo.Auth.Db.RolePermission", b =>
                {
                    b.Property<byte[]>("RoleId")
                        .HasMaxLength(16)
                        .HasColumnType("varbinary(16)");

                    b.Property<byte[]>("PermissionId")
                        .HasMaxLength(16)
                        .HasColumnType("varbinary(16)");

                    b.HasKey("RoleId", "PermissionId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("Coder.Todo.Auth.Db.User", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(16)
                        .HasColumnType("varbinary(16)");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("binary(32)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("binary(16)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("IX_Users_Email");

                    b.HasIndex("Phone")
                        .IsUnique()
                        .HasDatabaseName("IX_Users_Phone");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasDatabaseName("IX_Users_UserName");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
