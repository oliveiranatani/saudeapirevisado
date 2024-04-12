﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using apisaude.Models;

namespace apisaude.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ClasseMed> ClasseMeds { get; set; }

    public virtual DbSet<Entrega> Entregas { get; set; }

    public virtual DbSet<Fabricante> Fabricantes { get; set; }

    public virtual DbSet<MatMed> MatMeds { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=saude;User Id=postgres;Password=senai901;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClasseMed>(entity =>
        {
            entity.HasKey(e => e.CodClasse).HasName("classe_med_pkey");

            entity.ToTable("classe_med");

            entity.HasIndex(e => e.CodClasse, "idx_cod_classe");

            entity.Property(e => e.CodClasse).HasColumnName("cod_classe");
            entity.Property(e => e.NomeClasse)
                .HasMaxLength(30)
                .HasColumnName("nome_classe");
        });

        modelBuilder.Entity<Entrega>(entity =>
        {
            entity.HasKey(e => e.CodEntrega).HasName("entrega_pkey");

            entity.ToTable("entrega");

            entity.HasIndex(e => e.CodEntrega, "idx_cod_entrega");

            entity.HasIndex(e => new { e.CodPaciente, e.CodMatMed }, "idx_cod_paciente_matmed");

            entity.Property(e => e.CodEntrega).HasColumnName("cod_entrega");
            entity.Property(e => e.CodMatMed).HasColumnName("cod_mat_med");
            entity.Property(e => e.CodPaciente).HasColumnName("cod_paciente");
            entity.Property(e => e.DataEntrega).HasColumnName("data_entrega");
            entity.Property(e => e.DataProEntrega).HasColumnName("data_pro_entrega");

            entity.HasOne(d => d.CodMatMedNavigation).WithMany(p => p.Entregas)
                .HasForeignKey(d => d.CodMatMed)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entrega_cod_mat_med_fkey");

            entity.HasOne(d => d.CodPacienteNavigation).WithMany(p => p.Entregas)
                .HasForeignKey(d => d.CodPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entrega_cod_paciente_fkey");
        });

        modelBuilder.Entity<Fabricante>(entity =>
        {
            entity.HasKey(e => e.CodFab).HasName("fabricante_pkey");

            entity.ToTable("fabricante");

            entity.HasIndex(e => e.CodFab, "idx_cod_fab");

            entity.Property(e => e.CodFab).HasColumnName("cod_fab");
            entity.Property(e => e.NomeFab)
                .HasMaxLength(80)
                .HasColumnName("nome_fab");
        });

        modelBuilder.Entity<MatMed>(entity =>
        {
            entity.HasKey(e => e.CodMatMed).HasName("mat_med_pkey");

            entity.ToTable("mat_med");

            entity.HasIndex(e => e.CodMatMed, "idx_cod_matmed");

            entity.Property(e => e.CodMatMed).HasColumnName("cod_mat_med");
            entity.Property(e => e.CodClasse).HasColumnName("cod_classe");
            entity.Property(e => e.CodFab).HasColumnName("cod_fab");
            entity.Property(e => e.NomeMatMed)
                .HasMaxLength(80)
                .HasColumnName("nome_mat_med");
            entity.Property(e => e.UnidadeMedida)
                .HasMaxLength(20)
                .HasColumnName("unidade_medida");

            entity.HasOne(d => d.CodClasseNavigation).WithMany(p => p.MatMeds)
                .HasForeignKey(d => d.CodClasse)
                .HasConstraintName("mat_med_cod_classe_fkey");

            entity.HasOne(d => d.CodFabNavigation).WithMany(p => p.MatMeds)
                .HasForeignKey(d => d.CodFab)
                .HasConstraintName("mat_med_cod_fab_fkey");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.CodMedico).HasName("medico_pkey");

            entity.ToTable("medico");

            entity.HasIndex(e => e.CodCrm, "idx_cod_crm");

            entity.HasIndex(e => e.CodMedico, "idx_cod_medico");

            entity.HasIndex(e => e.CodCrm, "medico_cod_crm_key").IsUnique();

            entity.Property(e => e.CodMedico).HasColumnName("cod_medico");
            entity.Property(e => e.CodCrm).HasColumnName("cod_crm");
            entity.Property(e => e.NomeMedico)
                .HasMaxLength(80)
                .HasColumnName("nome_medico");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.CodPaciente).HasName("paciente_pkey");

            entity.ToTable("paciente");

            entity.HasIndex(e => e.CartaoSus, "idx_cartao_sus");

            entity.HasIndex(e => e.CodPaciente, "idx_cod_paciente");

            entity.HasIndex(e => e.Cpf, "idx_cpf");

            entity.HasIndex(e => e.Cpf, "paciente_cpf_key").IsUnique();

            entity.Property(e => e.CodPaciente).HasColumnName("cod_paciente");
            entity.Property(e => e.Bairro)
                .HasMaxLength(30)
                .HasColumnName("bairro");
            entity.Property(e => e.CartaoSus).HasColumnName("cartao_sus");
            entity.Property(e => e.Cidade)
                .HasMaxLength(30)
                .HasColumnName("cidade");
            entity.Property(e => e.CodMed).HasColumnName("cod_med");
            entity.Property(e => e.Cpf).HasColumnName("cpf");
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .HasColumnName("email");
            entity.Property(e => e.NomePaciente)
                .HasMaxLength(80)
                .HasColumnName("nome_paciente");
            entity.Property(e => e.NumCasa).HasColumnName("num_casa");
            entity.Property(e => e.Rua)
                .HasMaxLength(50)
                .HasColumnName("rua");
            entity.Property(e => e.Telefone).HasColumnName("telefone");

            entity.HasOne(d => d.CodMedNavigation).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.CodMed)
                .HasConstraintName("paciente_cod_med_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
