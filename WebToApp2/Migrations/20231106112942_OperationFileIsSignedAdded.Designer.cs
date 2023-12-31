﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebToApp2;

#nullable disable

namespace WebToApp2.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231106112942_OperationFileIsSignedAdded")]
    partial class OperationFileIsSignedAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebToApp2.Entities.AppFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("WebToApp2.Entities.Operation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GetFileResponseType")
                        .HasColumnType("int");

                    b.Property<string>("QrOperationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QrSignature")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("WebToApp2.Entities.OperationFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AppFileId")
                        .HasColumnType("int");

                    b.Property<bool>("IsSigned")
                        .HasColumnType("bit");

                    b.Property<int>("OperationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AppFileId");

                    b.HasIndex("OperationId");

                    b.ToTable("OperationFiles");
                });

            modelBuilder.Entity("WebToApp2.Entities.OperationFile", b =>
                {
                    b.HasOne("WebToApp2.Entities.AppFile", "AppFile")
                        .WithMany("OperationFiles")
                        .HasForeignKey("AppFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebToApp2.Entities.Operation", "Operation")
                        .WithMany("OperationFiles")
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppFile");

                    b.Navigation("Operation");
                });

            modelBuilder.Entity("WebToApp2.Entities.AppFile", b =>
                {
                    b.Navigation("OperationFiles");
                });

            modelBuilder.Entity("WebToApp2.Entities.Operation", b =>
                {
                    b.Navigation("OperationFiles");
                });
#pragma warning restore 612, 618
        }
    }
}
