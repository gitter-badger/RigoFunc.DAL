using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using RigoFunc.Api.Models;

namespace RigoFunc.Api.Migrations
{
    [DbContext(typeof(BloggingContext))]
    [Migration("20160526073037_InitialDatabase")]
    partial class InitialDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20901")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RigoFunc.Api.Models.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("RigoFunc.Api.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BlogId");

                    b.Property<string>("Content");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("RigoFunc.DAL.Internal.ChangeHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AfterJson")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 5000);

                    b.Property<string>("BeforeJson")
                        .HasAnnotation("MaxLength", 5000);

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("Kind");

                    b.Property<string>("SourceId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("Id");

                    b.ToTable("ChangeHistory");
                });

            modelBuilder.Entity("RigoFunc.Api.Models.Post", b =>
                {
                    b.HasOne("RigoFunc.Api.Models.Blog")
                        .WithMany()
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
