﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Monkey.Sql.Model;

namespace Monkey.Sql.Migrations
{
    [DbContext(typeof(MonkeyDbContext))]
    [Migration("20190331205846_InitialData")]
    partial class InitialData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Monkey.Sql.Model.ActionParameterBinding", b =>
                {
                    b.Property<long>("ActionId");

                    b.Property<long>("RequestId");

                    b.Property<long?>("ControllerRequestId");

                    b.Property<bool>("IsFromBody");

                    b.Property<bool>("IsFromUrl");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("Order");

                    b.HasKey("ActionId", "RequestId");

                    b.HasIndex("ControllerRequestId");

                    b.ToTable("ActionParameterBindings");
                });

            modelBuilder.Entity("Monkey.Sql.Model.ControllerActionDescriptor", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("ControllerDescriptorId");

                    b.Property<long?>("ControllerResponseId");

                    b.Property<bool>("IsResponseCollection");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<long>("ResponseId");

                    b.Property<string>("Route")
                        .HasMaxLength(255);

                    b.Property<int>("Verb");

                    b.HasKey("Id");

                    b.HasIndex("ControllerDescriptorId");

                    b.HasIndex("ControllerResponseId");

                    b.ToTable("ControllerActions");
                });

            modelBuilder.Entity("Monkey.Sql.Model.ControllerDescriptor", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Route");

                    b.HasKey("Id");

                    b.ToTable("Controllers");
                });

            modelBuilder.Entity("Monkey.Sql.Model.ObjectProperty", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("DeclaringTypeId");

                    b.Property<bool>("IsCollection");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<long>("PropertyTypeId");

                    b.HasKey("Id");

                    b.HasIndex("DeclaringTypeId");

                    b.HasIndex("PropertyTypeId");

                    b.ToTable("ObjectProperties");
                });

            modelBuilder.Entity("Monkey.Sql.Model.ObjectType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDynamic");

                    b.Property<bool>("IsPrimitive");

                    b.Property<bool>("IsVoid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Namespace")
                        .HasMaxLength(255);

                    b.Property<string>("Usage")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.HasKey("Id");

                    b.ToTable("ObjectTypes");

                    b.HasDiscriminator<string>("Usage").HasValue("ObjectType");
                });

            modelBuilder.Entity("Monkey.Sql.Model.ProcedureBinding", b =>
                {
                    b.Property<long>("ProcedureId");

                    b.Property<long>("ResultId");

                    b.Property<bool>("IsResultCollection");

                    b.Property<int>("Mode");

                    b.HasKey("ProcedureId", "ResultId");

                    b.HasIndex("ResultId");

                    b.ToTable("ProcedureBindings");
                });

            modelBuilder.Entity("Monkey.Sql.Model.ProcedureDescriptor", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConnectionName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Schema")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("ProcedureDescriptors");
                });

            modelBuilder.Entity("Monkey.Sql.Model.ProcedureParameterBinding", b =>
                {
                    b.Property<long>("ParameterId");

                    b.Property<long>("PropertyId");

                    b.Property<long?>("ObjectPropertyId");

                    b.HasKey("ParameterId", "PropertyId");

                    b.HasIndex("ObjectPropertyId");

                    b.ToTable("ProcedureParameterBindings");
                });

            modelBuilder.Entity("Monkey.Sql.Model.ProcedureParameterDescriptor", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<long>("ProcedureId");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("ProcedureId");

                    b.ToTable("ProcedureParameterDescriptors");
                });

            modelBuilder.Entity("Monkey.Sql.Model.ProcedureResultColumn", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<long>("ProcedureId");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("ProcedureId");

                    b.ToTable("ProcedureResultDescriptors");
                });

            modelBuilder.Entity("Monkey.Sql.Model.ProcedureResultColumnBinding", b =>
                {
                    b.Property<long>("ResultColumnColumnId");

                    b.Property<long>("PropertyId");

                    b.Property<long?>("ObjectPropertyId");

                    b.HasKey("ResultColumnColumnId", "PropertyId");

                    b.HasIndex("ObjectPropertyId");

                    b.ToTable("ProcedureResultColumnBindings");
                });

            modelBuilder.Entity("Monkey.Sql.Model.Command", b =>
                {
                    b.HasBaseType("Monkey.Sql.Model.ObjectType");

                    b.HasDiscriminator().HasValue("Command");
                });

            modelBuilder.Entity("Monkey.Sql.Model.ControllerRequest", b =>
                {
                    b.HasBaseType("Monkey.Sql.Model.ObjectType");

                    b.HasDiscriminator().HasValue("Request");
                });

            modelBuilder.Entity("Monkey.Sql.Model.ControllerResponse", b =>
                {
                    b.HasBaseType("Monkey.Sql.Model.ObjectType");

                    b.HasDiscriminator().HasValue("Response");
                });

            modelBuilder.Entity("Monkey.Sql.Model.Query", b =>
                {
                    b.HasBaseType("Monkey.Sql.Model.ObjectType");

                    b.HasDiscriminator().HasValue("Query");
                });

            modelBuilder.Entity("Monkey.Sql.Model.Result", b =>
                {
                    b.HasBaseType("Monkey.Sql.Model.ObjectType");

                    b.HasDiscriminator().HasValue("Result");
                });

            modelBuilder.Entity("Monkey.Sql.Model.ActionParameterBinding", b =>
                {
                    b.HasOne("Monkey.Sql.Model.ControllerActionDescriptor", "Action")
                        .WithMany()
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Monkey.Sql.Model.ControllerRequest", "ControllerRequest")
                        .WithMany()
                        .HasForeignKey("ControllerRequestId");
                });

            modelBuilder.Entity("Monkey.Sql.Model.ControllerActionDescriptor", b =>
                {
                    b.HasOne("Monkey.Sql.Model.ControllerDescriptor")
                        .WithMany("Actions")
                        .HasForeignKey("ControllerDescriptorId");

                    b.HasOne("Monkey.Sql.Model.ControllerResponse", "ControllerResponse")
                        .WithMany()
                        .HasForeignKey("ControllerResponseId");
                });

            modelBuilder.Entity("Monkey.Sql.Model.ObjectProperty", b =>
                {
                    b.HasOne("Monkey.Sql.Model.ObjectType", "DeclaringType")
                        .WithMany("Properties")
                        .HasForeignKey("DeclaringTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Monkey.Sql.Model.ObjectType", "PropertyType")
                        .WithMany()
                        .HasForeignKey("PropertyTypeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Monkey.Sql.Model.ProcedureBinding", b =>
                {
                    b.HasOne("Monkey.Sql.Model.ProcedureDescriptor", "Procedure")
                        .WithMany()
                        .HasForeignKey("ProcedureId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Monkey.Sql.Model.Result", "Result")
                        .WithMany()
                        .HasForeignKey("ResultId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Monkey.Sql.Model.ProcedureParameterBinding", b =>
                {
                    b.HasOne("Monkey.Sql.Model.ObjectProperty", "ObjectProperty")
                        .WithMany()
                        .HasForeignKey("ObjectPropertyId");

                    b.HasOne("Monkey.Sql.Model.ProcedureParameterDescriptor", "Parameter")
                        .WithMany()
                        .HasForeignKey("ParameterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Monkey.Sql.Model.ProcedureParameterDescriptor", b =>
                {
                    b.HasOne("Monkey.Sql.Model.ProcedureDescriptor", "Procedure")
                        .WithMany()
                        .HasForeignKey("ProcedureId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Monkey.Sql.Model.ProcedureResultColumn", b =>
                {
                    b.HasOne("Monkey.Sql.Model.ProcedureDescriptor", "Procedure")
                        .WithMany()
                        .HasForeignKey("ProcedureId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Monkey.Sql.Model.ProcedureResultColumnBinding", b =>
                {
                    b.HasOne("Monkey.Sql.Model.ObjectProperty", "ObjectProperty")
                        .WithMany()
                        .HasForeignKey("ObjectPropertyId");

                    b.HasOne("Monkey.Sql.Model.ProcedureResultColumn", "ResultColumnColumn")
                        .WithMany()
                        .HasForeignKey("ResultColumnColumnId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
