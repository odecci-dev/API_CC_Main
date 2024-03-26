﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using API_PCC.EntityModels;
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

    public virtual DbSet<HHerdClassification> HHerdClassifications { get; set; }

    public virtual DbSet<ModuleTbl> ModuleTbls { get; set; }

    public virtual DbSet<TblApiTokenModel> TblApiTokenModels { get; set; }

    public virtual DbSet<TblAttempt> TblAttempts { get; set; }

    public virtual DbSet<TblCenterModel> TblCenterModels { get; set; }

    public virtual DbSet<TblRegistrationOtpmodel> TblRegistrationOtpmodels { get; set; }

    public virtual DbSet<TblStatusModel> TblStatusModels { get; set; }

    public virtual DbSet<TblUsersModel> TblUsersModels { get; set; }

    public virtual DbSet<TblTokenModel> TblTokenModels { get; set; }

    public virtual DbSet<TblMailSenderCredential> TblMailSenderCredentials { get; set; }

    public virtual DbSet<TblFarmOwner> TblFarmOwners { get; set; }

    public virtual DbSet<SireModel> tblSireModels { get; set; }

    public virtual DbSet<DamModel> tblDamModels { get; set; }

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
            entity.Property(e => e.Status)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Status");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DateDeleted)
                .HasColumnType("date")
                .HasColumnName("Date_Deleted");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
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
            entity.Property(e => e.Status)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Status");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.DateDeleted)
                .HasColumnType("date")
                .HasColumnName("Date_Deleted");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
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
            entity.Property(e => e.Status)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Status");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag)
                .HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.DateDeleted)
                .HasColumnType("date")
                .HasColumnName("Date_Deleted");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
        });

        modelBuilder.Entity<ABuffAnimal>(entity =>
        {
            entity.ToTable("A_Buff_Animal");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AnimalIdNumber)
                .IsUnicode(false)
                .HasColumnName("Animal_ID_Number");
            entity.Property(e => e.AnimalName)
                .IsUnicode(false)
                .HasColumnName("Animal_Name");
            entity.Property(e => e.Photo)
                .IsUnicode(false)
                .HasColumnName("Photo");
            entity.Property(e => e.HerdCode)
                .IsUnicode(false)
                .HasColumnName("Herd_Code");
            entity.Property(e => e.RfidNumber)
                .IsUnicode(false)
                .HasColumnName("RFID_Number");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("date")
                .HasColumnName("Date_of_Birth");
            entity.Property(e => e.Sex)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.BreedCode)
                .IsUnicode(false)
                .HasColumnName("Breed_Code");
            entity.Property(e => e.BirthType)
                .IsUnicode(false)
                .HasColumnName("Birth_Type");
            entity.Property(e => e.CountryOfBirth)
                .IsUnicode(false)
                .HasColumnName("Country_Of_Birth");
            entity.Property(e => e.OriginOfAcquisition)
                .IsUnicode(false)
                .HasColumnName("Origin_Of_Acquisition");
            entity.Property(e => e.DateOfAcquisition)
                .HasColumnType("date")
                .HasColumnName("Date_Of_Acquisition");
            entity.Property(e => e.Marking)
                .IsUnicode(false);
            entity.Property(e => e.TypeOfOwnership)
                .IsUnicode(false)
                .HasColumnName("Type_Of_Ownership");
            entity.Property(e => e.BloodCode)
                .IsUnicode(false)
                .HasColumnName("Blood_Code");
            entity.Property(e => e.SireId)
                .IsUnicode(false)
                .HasColumnName("Sire_ID");
            entity.Property(e => e.DamId)
                .IsUnicode(false)
                .HasColumnName("Dam_Id");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.CreatedBy)
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("date")
                .HasColumnName("Created_Date");
            entity.Property(e => e.UpdatedBy)
                .IsUnicode(false)
                .HasColumnName("Updated_By");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("date")
                .HasColumnName("Update_Date");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.DateDeleted)
                .HasColumnType("date")
                .HasColumnName("Date_Deleted");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
        });

        modelBuilder.Entity<ATypeOwnership>(entity =>
        {
            entity.ToTable("A_Type_Ownership");

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
            entity.Property(e => e.Status)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Status");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By"); 
            entity.Property(e => e.DateDeleted)
                .HasColumnType("date")
                .HasColumnName("Date_Deleted");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
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
            entity.Property(e => e.ModuleId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Module_Id");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.UpdatedBy)
                .IsUnicode(false)
                .HasColumnName("Updated_By");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.DateDeleted)
                .HasColumnType("date")
                .HasColumnName("Date_Deleted");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
        });

        modelBuilder.Entity<HBuffHerd>(entity =>
        {
            entity.ToTable("H_Buff_Herd");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HerdName)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Herd_Name");
            entity.Property(e => e.HerdCode)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("Herd_Code");
            entity.Property(e => e.HerdSize).HasColumnName("Herd_Size");
            entity.Property(e => e.BreedTypeCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Breed_Type_COde");
            entity.Property(e => e.FarmAffilCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Farm_Affil_Code");
            entity.Property(e => e.HerdClassDesc)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("Herd_Class_Desc");
            entity.Property(e => e.FeedingSystemCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Feeding_System_Code");
            entity.Property(e => e.FarmManager)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Farm_Manager");
            entity.Property(e => e.FarmAddress)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Farm_Address");
            entity.Property(e => e.Owner)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Owner");
            entity.Property(e => e.Status)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Status");
            entity.Property(e => e.DateCreated)
                .IsRequired()
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.UpdatedBy)
                .IsUnicode(false)
                .HasColumnName("Updated_By");
            entity.Property(e => e.DateDeleted)
                .HasColumnType("date")
                .HasColumnName("Date_Deleted");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.OrganizationName)
                .IsUnicode(false)
                .HasColumnName("Organization_name");
            entity.Property(e => e.Center)
                .IsUnicode(false)
                .HasColumnName("Center");
            entity.Property(e => e.Photo)
                .IsUnicode(false)
                .HasColumnName("Photo");

        });

        modelBuilder.Entity<HBuffaloType>(entity =>
        {
            entity.ToTable("H_Buffalo_Type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BreedTypeCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Breed_Type_Code");
            entity.Property(e => e.BreedTypeDesc)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Breed_Type_Desc");
            entity.Property(e => e.Status)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Status");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.DateDeleted)
                .HasColumnType("date")
                .HasColumnName("Date_Deleted");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
        });

        modelBuilder.Entity<HFarmerAffiliation>(entity =>
        {
            entity.ToTable("H_Farmer_Affiliation");

            entity.Property(e => e.Id).HasColumnName("id");
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
            entity.Property(e => e.Status)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Status");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.DateDeleted)
                .HasColumnType("date")
                .HasColumnName("Date_Deleted");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
        });

        modelBuilder.Entity<HFeedingSystem>(entity =>
        {
            entity.ToTable("H_Feeding_System");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FeedingSystemCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Feeding_System_Code");
            entity.Property(e => e.FeedingSystemDesc)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Feeding_System_Desc");
            entity.Property(e => e.Status)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Status");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.DateDeleted)
                .HasColumnType("date")
                .HasColumnName("Date_Deleted");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
        });

            modelBuilder.Entity<HHerdClassification>(entity =>
            {
                entity.ToTable("H_Herd_Classification");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HerdClassCode)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("Herd_Class_Code");
            entity.Property(e => e.HerdClassDesc)
                .IsRequired()
                .HasMaxLength(17)
                .IsFixedLength()
                .HasColumnName("Herd_Class_Desc");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.UpdatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Updated_By");
                entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.DateDeleted)
                .HasColumnType("date")
                .HasColumnName("Date_Deleted");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.LevelFrom)
                .IsUnicode(false)
                .HasColumnName("Level_from");
            entity.Property(e => e.LevelTo)
                .IsUnicode(false)
                .HasColumnName("Level_to");
        });

        modelBuilder.Entity<ModuleTbl>(entity =>
        {
            entity.HasKey(e => e.ModuleId).HasName("PK_Module");

            entity.ToTable("Module_tbl");

            entity.Property(e => e.ModuleId)
                .HasColumnName("Module_Id");
            entity.Property(e => e.ModuleName)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Module_Name");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.UpdatedBy)
                .IsUnicode(false)
                .HasColumnName("Updated_By");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.DateDeleted)
                .HasColumnType("date")
                .HasColumnName("Date_Deleted");
            entity.Property(e => e.ParentModule)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Parent_Module");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
        });

        modelBuilder.Entity<TblApiTokenModel>(entity =>
        {
            entity.ToTable("tbl_ApiTokenModel");

            entity.Property(e => e.ApiToken).IsUnicode(false);
            entity.Property(e => e.Role).IsUnicode(false);
            entity.Property(e => e.Name).IsUnicode(false);
            entity.Property(e => e.Status).IsUnicode(false);
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
            entity.Property(e => e.CenterName)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.CenterDesc)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("Center_Desc");
            entity.Property(e => e.CreatedBy)
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.UpdatedBy)
                .IsUnicode(false)
                .HasColumnName("Updated_By");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.DateDeleted)
                .HasColumnType("date")
                .HasColumnName("Date_Deleted");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
        });

        modelBuilder.Entity<TblMailSenderCredential>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MailSenderCredential");

            entity.ToTable("tbl_MailSenderCredential");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);
            entity.Property(e => e.Password)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(255);
            entity.Property(e => e.DateCreated)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("DateCreated");
            entity.Property(e => e.Status)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Status");
            entity.Property(e => e.ExpiryDate)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("ExpiryDate");
        });

        modelBuilder.Entity<TblRegistrationOtpmodel>(entity =>
        {
            entity.ToTable("tbl_RegistrationOTPModel");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.Otp)
                .IsUnicode(false)
                .HasColumnName("OTP");
            entity.Property(e => e.Status)
                .IsUnicode(false)
                .HasColumnName("Status");
        });

        modelBuilder.Entity<TblStatusModel>(entity =>
        {
            entity.ToTable("tbl_StatusModel");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Status)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblTokenModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_TokenModel");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.ToTable("tbl_TokenModel");
            entity.Property(e => e.Token)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("Token");
            entity.Property(e => e.ExpiryDate)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("ExpiryDate");
            entity.Property(e => e.Status)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("Status");
            entity.Property(e => e.DateCreated)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("Date_Created");
        });

        modelBuilder.Entity<TblUsersModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UsersModel");

            entity.ToTable("tbl_UsersModel");

            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Fname).IsUnicode(false);
            entity.Property(e => e.Lname).IsUnicode(false);
            entity.Property(e => e.Mname).IsUnicode(false);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.Jwtoken)
                .IsUnicode(false)
                .HasColumnName("JWToken");
            entity.Property(e => e.FilePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Active)
                .IsUnicode(false)
                .HasColumnName("Active");
            entity.Property(e => e.Cno)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("Date_Created");
            entity.Property(e => e.UpdatedBy)
                .IsUnicode(false)
                .HasColumnName("Updated_By");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("date")
                .HasColumnName("Date_Updated");
            entity.Property(e => e.DeleteFlag).HasColumnName("Delete_Flag");
            entity.Property(e => e.DeletedBy)
                .IsUnicode(false)
                .HasColumnName("Deleted_By");
            entity.Property(e => e.DateDeleted)
                .HasColumnType("date")
                .HasColumnName("Date_Deleted");
            entity.Property(e => e.RestoredBy)
                .IsUnicode(false)
                .HasColumnName("Restored_By");
            entity.Property(e => e.DateRestored)
                .HasColumnType("date")
                .HasColumnName("Date_Restored");
            entity.Property(e => e.CenterId)
                .IsUnicode(false)
                .HasColumnName("CenterId");
            entity.Property(e => e.AgreementStatus)
                .IsUnicode(false)
                .HasColumnName("AgreementStatus");
            entity.Property(e => e.RememberToken)
                .IsUnicode(false)
                .HasMaxLength(255)
                .HasColumnName("RememberToken");
        });

        modelBuilder.Entity<TblFarmOwner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_FarmOwner");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.ToTable("tbl_FarmOwner");
            entity.Property(e => e.Name)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("Name");
            entity.Property(e => e.LastName)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("LastName");
            entity.Property(e => e.Address)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("Address");
            entity.Property(e => e.TelephoneNumber)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("TelephoneNumber");
            entity.Property(e => e. MobileNumber)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("MobileNumber");
            entity.Property(e => e.Email)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("Email");
        });

        modelBuilder.Entity<SireModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_SireModel");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.ToTable("tbl_SireModel");
            entity.Property(e => e.SireRegistrationNumber)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("Sire_Registration_Number");
            entity.Property(e => e.SireIdNumber)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("Sire_Id_Number");
            entity.Property(e => e.SireName)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("Sire_Name");
            entity.Property(e => e.BloodCode)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("Blood_Code");
        });

        modelBuilder.Entity<DamModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_DamModel");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.ToTable("tbl_DamModel");
            entity.Property(e => e.DamRegistrationNumber)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("Dam_Registration_Number");
            entity.Property(e => e.DamIdNumber)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("Dam_Id_Number");
            entity.Property(e => e.DamName)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("Dam_Name");
            entity.Property(e => e.BreedCode)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("Breed_Code");
            entity.Property(e => e.BloodCode)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasColumnName("Blood_Code");
        });

        OnModelCreatingGeneratedProcedures(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}