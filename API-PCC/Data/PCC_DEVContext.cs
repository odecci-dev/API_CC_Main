﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using API_PCC.Models;
using Microsoft.EntityFrameworkCore;

namespace API_PCC.Data;

public partial class PCC_DEVContext : DbContext
{
    public PCC_DEVContext(DbContextOptions<PCC_DEVContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ABirthType> ABirthTypes { get; set; }

    public virtual DbSet<ABloodComp> ABloodComps { get; set; }

    public virtual DbSet<ABreed> ABreeds { get; set; }

    public virtual DbSet<ABuffAnimal> ABuffAnimals { get; set; }

    public virtual DbSet<ATypeOwnership> ATypeOwnerships { get; set; }

    public virtual DbSet<ActionTbl> ActionTbls { get; set; }

    public virtual DbSet<HBuffHerd> HBuffHerds { get; set; }

    public virtual DbSet<HBuffaloType> HBuffaloTypes { get; set; }

    public virtual DbSet<HFarmerAffiliation> HFarmerAffiliations { get; set; }

    public virtual DbSet<HFeedingSystem> HFeedingSystems { get; set; }

    public virtual DbSet<HHerdType> HHerdTypes { get; set; }

    public virtual DbSet<ModuleTbl> ModuleTbls { get; set; }

    public virtual DbSet<TblApiTokenModel> TblApiTokenModels { get; set; }

    public virtual DbSet<TblAttempt> TblAttempts { get; set; }

    public virtual DbSet<TblCenterModel> TblCenterModels { get; set; }

    public virtual DbSet<TblRegistrationOtpmodel> TblRegistrationOtpmodels { get; set; }

    public virtual DbSet<TblStatusModel> TblStatusModels { get; set; }

    public virtual DbSet<TblUsersModel> TblUsersModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ABirthType>(entity =>
        {
            entity.ToTable("A_Birth_Type");

            entity.Property(e => e.BirthTypeCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Birth_Type_Code");
            entity.Property(e => e.BirthTypeDesc)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Birth_Type_Desc");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.DateDelete)
                .HasColumnType("date")
                .HasColumnName("Date_Delete");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
        });

        modelBuilder.Entity<ABloodComp>(entity =>
        {
            entity.ToTable("A_Blood_Comp");

            entity.Property(e => e.BloodCode)
                .IsRequired()
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Blood_Code");
            entity.Property(e => e.BloodDesc)
                .IsRequired()
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Blood_Desc");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.DateDelete)
                .HasColumnType("date")
                .HasColumnName("Date_Delete");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
        });

        modelBuilder.Entity<ABreed>(entity =>
        {
            entity.ToTable("A_Breed");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BreedCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Breed_Code");
            entity.Property(e => e.BreedDesc)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Breed_Desc");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.DateDelete)
                .HasColumnType("date")
                .HasColumnName("Date_Delete");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
        });

        modelBuilder.Entity<ABuffAnimal>(entity =>
        {
            entity.ToTable("A_Buff_Animal");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AnimalId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Animal_ID");
            entity.Property(e => e.BirthTypeCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Birth_Type_Code");
            entity.Property(e => e.BloodCode)
                .IsRequired()
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Blood_Code");
            entity.Property(e => e.BreedCode)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("Breed_Code");
            entity.Property(e => e.BuffaloType)
                .IsUnicode(false)
                .HasColumnName("Buffalo_type");
            entity.Property(e => e.CountryBirth)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Country_Birth");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateAcquisition)
                .HasColumnType("date")
                .HasColumnName("Date_Acquisition");
            entity.Property(e => e.DateDelete)
                .HasColumnType("date")
                .HasColumnName("Date_Delete");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("date")
                .HasColumnName("Date_of_Birth");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.HerdCode)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("Herd_Code");
            entity.Property(e => e.IdSystem)
                .IsUnicode(false)
                .HasColumnName("ID_System");
            entity.Property(e => e.Marking)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OriginAcquisition)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Origin_Acquisition");
            entity.Property(e => e.PedigreeRecords)
                .IsUnicode(false)
                .HasColumnName("Pedigree_Records");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.Rfid)
                .IsRequired()
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("RFID");
            entity.Property(e => e.Sex)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SireIdNum)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Sire_ID_Num");
            entity.Property(e => e.SireRegNum)
                .IsRequired()
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("Sire_Reg_Num");
            entity.Property(e => e.TypeOwnCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Type_Own_Code");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
        });

        modelBuilder.Entity<ATypeOwnership>(entity =>
        {
            entity.ToTable("A_Type_Ownership");

            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.DateDelete)
                .HasColumnType("date")
                .HasColumnName("Date_Delete");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.TypeOwnCode)
                .IsRequired()
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("Type_Own_Code");
            entity.Property(e => e.TypeOwnDesc)
                .IsRequired()
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("Type_Own_Desc");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
        });

        modelBuilder.Entity<ActionTbl>(entity =>
        {
            entity.HasKey(e => e.ActionId).HasName("PK_Action");

            entity.ToTable("Action_tbl");

            entity.Property(e => e.ActionId).HasColumnName("Action_Id");
            entity.Property(e => e.ActionName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Action_name");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.DateDelete)
                .HasColumnType("date")
                .HasColumnName("Date_Delete");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.ModuleId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Module_Id");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.UpdatedBy)
                .IsUnicode(false)
                .HasColumnName("Updated_By");
        });

        modelBuilder.Entity<HBuffHerd>(entity =>
        {
            entity.ToTable("H_Buff_Herd");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.BBuffCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("B_Buff_Code");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.DateDelete)
                .HasColumnType("date")
                .HasColumnName("Date_Delete");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.Email)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.FCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("F_Code");
            entity.Property(e => e.FarmAddress)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Farm_Address");
            entity.Property(e => e.FarmManager)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Farm_Manager");
            entity.Property(e => e.FeedCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Feed_Code");
            entity.Property(e => e.HTypeCode)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("H_Type_Code");
            entity.Property(e => e.HerdCode)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("Herd_Code");
            entity.Property(e => e.HerdName)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Herd_Name");
            entity.Property(e => e.HerdSize).HasColumnName("Herd_Size");
            entity.Property(e => e.MNo)
                .IsRequired()
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("M_No");
            entity.Property(e => e.Owner)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.TelNo)
                .IsRequired()
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("Tel_No");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
        });

        modelBuilder.Entity<HBuffaloType>(entity =>
        {
            entity.ToTable("H_Buffalo_Type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BBuffCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("B_Buff_Code");
            entity.Property(e => e.BBuffDesc)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("B_Buff_Desc");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.DateDelete)
                .HasColumnType("date")
                .HasColumnName("Date_Delete");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
        });

        modelBuilder.Entity<HFarmerAffiliation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_H");

            entity.ToTable("H_Farmer_Affiliation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.DateDelete)
                .HasColumnType("date")
                .HasColumnName("Date_Delete");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.FCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("F_Code");
            entity.Property(e => e.FDesc)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("F_Desc");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
        });

        modelBuilder.Entity<HFeedingSystem>(entity =>
        {
            entity.ToTable("H_Feeding_System");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.DateDelete)
                .HasColumnType("date")
                .HasColumnName("Date_Delete");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.FeedCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Feed_Code");
            entity.Property(e => e.FeedDesc)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Feed_Desc");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
        });

        modelBuilder.Entity<HHerdType>(entity =>
        {
            entity.ToTable("H_Herd_Type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.DateDelete)
                .HasColumnType("date")
                .HasColumnName("Date_Delete");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.HTypeCode)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("H_Type_Code");
            entity.Property(e => e.HTypeDesc)
                .IsRequired()
                .HasMaxLength(17)
                .IsFixedLength()
                .HasColumnName("H_Type_Desc");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
        });

        modelBuilder.Entity<ModuleTbl>(entity =>
        {
            entity.HasKey(e => e.ModuleId).HasName("PK_Module");

            entity.ToTable("Module_tbl");

            entity.Property(e => e.ModuleId).HasColumnName("Module_Id");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.DateDelete)
                .HasColumnType("date")
                .HasColumnName("Date_Delete");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.ModuleName)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Module_Name");
            entity.Property(e => e.ParentModule)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Parent_Module");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.UpdatedBy)
                .IsUnicode(false)
                .HasColumnName("Updated_By");
        });

        modelBuilder.Entity<TblApiTokenModel>(entity =>
        {
            entity.ToTable("tbl_ApiTokenModel");

            entity.Property(e => e.ApiToken).IsUnicode(false);
            entity.Property(e => e.Name).IsUnicode(false);
            entity.Property(e => e.Role).IsUnicode(false);
        });

        modelBuilder.Entity<TblAttempt>(entity =>
        {
            entity.ToTable("tbl_Attempts");

            entity.Property(e => e.Ipaddress)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IPAddress");
            entity.Property(e => e.Location)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblCenterModel>(entity =>
        {
            entity.ToTable("tbl_CenterModel");

            entity.Property(e => e.CenterDesc)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("Center_Desc");
            entity.Property(e => e.CenterName)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.DateDelete)
                .HasColumnType("date")
                .HasColumnName("Date_Delete");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.UpdatedBy)
                .IsUnicode(false)
                .HasColumnName("Updated_By");
        });

        modelBuilder.Entity<TblRegistrationOtpmodel>(entity =>
        {
            entity.ToTable("tbl_RegistrationOTPModel");

            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.Otp)
                .IsUnicode(false)
                .HasColumnName("OTP");
        });

        modelBuilder.Entity<TblStatusModel>(entity =>
        {
            entity.ToTable("tbl_StatusModel");

            entity.Property(e => e.Status)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblUsersModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UsersModel");

            entity.ToTable("tbl_UsersModel");

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.Cno)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.DateDelete)
                .HasColumnType("date")
                .HasColumnName("Date_Delete");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.FilePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Fname).IsUnicode(false);
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Jwtoken)
                .IsUnicode(false)
                .HasColumnName("JWToken");
            entity.Property(e => e.Lname).IsUnicode(false);
            entity.Property(e => e.Mname).IsUnicode(false);
            entity.Property(e => e.Password)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.UpdatedBy)
                .IsUnicode(false)
                .HasColumnName("Updated_By");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingGeneratedProcedures(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}