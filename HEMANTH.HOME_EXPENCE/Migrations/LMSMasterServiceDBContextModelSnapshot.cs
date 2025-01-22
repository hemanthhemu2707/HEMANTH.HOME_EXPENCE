﻿// <auto-generated />
using System;
using IIITS.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HEMANTH.HOME_EXPENCE.Migrations
{
    [DbContext(typeof(LMSMasterServiceDBContext))]
    partial class LMSMasterServiceDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.Category.CategoryTableDBTypes", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("exp_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"), 1L, 1);

                    b.Property<string>("CatagoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("exp_name");

                    b.Property<string>("CatergoryDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("exp_desc");

                    b.HasKey("CategoryId");

                    b.ToTable("tblexpencecategory", "dbo");
                });

            modelBuilder.Entity("HEMANTH.HOME_EXPENCE.Repositories.DBConfig.AdminMaster.Family.FamilyTableDBTypes", b =>
                {
                    b.Property<int>("FamilyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("fam_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FamilyID"), 1L, 1);

                    b.Property<string>("FamilyDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("fam_description");

                    b.Property<int>("FamilyDoorNumber")
                        .HasColumnType("int")
                        .HasColumnName("fam_door_no");

                    b.Property<int>("FamilyEleBillNumber")
                        .HasColumnType("int")
                        .HasColumnName("fam_electric_bill_number");

                    b.Property<DateTime>("FamilyEntryDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("fam_entry_date");

                    b.Property<int>("FamilyFloorNo")
                        .HasColumnType("int")
                        .HasColumnName("fam_floor_no");

                    b.Property<string>("FamilyLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("fam_home_location");

                    b.Property<string>("FamilyMap")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("fam_location_map");

                    b.Property<string>("FamilyMobileNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("fam_home_owner_mobile");

                    b.Property<string>("FamilyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("fam_name");

                    b.Property<string>("FamilyOwnerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("fam_home_owner_name");

                    b.Property<string>("FamilyPhoto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("fam_home_photo");

                    b.Property<string>("FamilyRefrenceKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("fam_ref_key");

                    b.Property<string>("HomeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("fam_home_name");

                    b.HasKey("FamilyID");

                    b.ToTable("tblfamily", "dbo");
                });

            modelBuilder.Entity("IIITS.LMS.Repositories.GeneralTables.ExpenseDetailsTableDBTypes.ExpenseDetailsTableDBTypes", b =>
                {
                    b.Property<int>("ExpenceDetailsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("expd_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExpenceDetailsID"), 1L, 1);

                    b.Property<DateTime>("ExpenceDetailsEntryDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("expd_entry_date");

                    b.Property<int>("ExpenceDetailsExpID")
                        .HasColumnType("int")
                        .HasColumnName("expd_expe_id");

                    b.Property<int>("ExpenceFamilyId")
                        .HasColumnType("int")
                        .HasColumnName("expd_fam_id");

                    b.Property<double>("ExpencePersPrice")
                        .HasColumnType("float")
                        .HasColumnName("expd_exp_price");

                    b.Property<int>("ExpenceUserID")
                        .HasColumnType("int")
                        .HasColumnName("expd_us_id");

                    b.HasKey("ExpenceDetailsID");

                    b.ToTable("tblexpence_details", "dbo");
                });

            modelBuilder.Entity("IIITS.LMS.Repositories.GeneralTables.ExpenseMasterTableDBTypes.ExpenseMasterTableDBTypes", b =>
                {
                    b.Property<int>("ExpenceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("expense_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExpenceID"), 1L, 1);

                    b.Property<int>("ExpenceCategory")
                        .HasColumnType("int")
                        .HasColumnName("e_catergory_id");

                    b.Property<DateTime>("ExpenceDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("e_date");

                    b.Property<string>("ExpenceDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("e_desc");

                    b.Property<DateTime>("ExpenceEntryDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("e_entry_date");

                    b.Property<int>("ExpenceFamId")
                        .HasColumnType("int")
                        .HasColumnName("e_fam_id");

                    b.Property<string>("ExpenceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("exp_name");

                    b.Property<int>("ExpenceUserId")
                        .HasColumnType("int")
                        .HasColumnName("e_us_id");

                    b.Property<double>("ExpensePrice")
                        .HasColumnType("float")
                        .HasColumnName("e_price");

                    b.HasKey("ExpenceID");

                    b.ToTable("tblexpencemaster", "dbo");
                });

            modelBuilder.Entity("IIITS.LMS.Repositories.GeneralTables.Login.LoginDBTypes", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("us_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<int>("AdminStatus")
                        .HasColumnType("int")
                        .HasColumnName("us_is_admin");

                    b.Property<int>("UserApproveStatus")
                        .HasColumnType("int")
                        .HasColumnName("us_approve_status");

                    b.Property<string>("UserEmailAdress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("us_email_add");

                    b.Property<int>("UserFamilyId")
                        .HasColumnType("int")
                        .HasColumnName("us_family_id");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("us_name");

                    b.Property<string>("UserPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("us_pswd");

                    b.Property<string>("UserPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("us_phone_number");

                    b.HasKey("UserId");

                    b.ToTable("tbluser", "dbo");
                });
#pragma warning restore 612, 618
        }
    }
}
