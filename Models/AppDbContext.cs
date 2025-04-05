using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLbansach_tutorial.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Chitietdondathang> Chitietdondathangs { get; set; }

    public virtual DbSet<Chude> Chudes { get; set; }

    public virtual DbSet<Dondathang> Dondathangs { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Nhaxuatban> Nhaxuatbans { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sach> Saches { get; set; }

    public virtual DbSet<Tacgium> Tacgia { get; set; }

    public virtual DbSet<Vietsac> Vietsacs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=QLbansach;User ID=sa;Password=159357_Milkyway;TrustServerCertificate=True;MultipleActiveResultSets=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart__3213E83F5633DAEF");

            entity.ToTable("Cart");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Gia)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("gia");
            entity.Property(e => e.MaKh).HasColumnName("maKH");
            entity.Property(e => e.MaSach).HasColumnName("ma_sach");
            entity.Property(e => e.Sl).HasColumnName("sl");
            entity.Property(e => e.Tongtien)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("tongtien");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__Cart__maKH__440B1D61");

            entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.MaSach)
                .HasConstraintName("FK__Cart__ma_sach__44FF419A");
        });

        modelBuilder.Entity<Chitietdondathang>(entity =>
        {
            entity.HasKey(e => new { e.MaDonHang, e.Masach }).HasName("PK__CHITIETD__8B6CA765E7F6A286");

            entity.ToTable("CHITIETDONDATHANG");

            entity.Property(e => e.Soluong).HasColumnName("soluong");

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.Chitietdondathangs)
                .HasForeignKey(d => d.MaDonHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETDO__MaDon__4AB81AF0");

            entity.HasOne(d => d.MasachNavigation).WithMany(p => p.Chitietdondathangs)
                .HasForeignKey(d => d.Masach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETDO__Masac__4BAC3F29");
        });

        modelBuilder.Entity<Chude>(entity =>
        {
            entity.HasKey(e => e.MaCd).HasName("PK__CHUDE__27258E04BF620F66");

            entity.ToTable("CHUDE");

            entity.Property(e => e.MaCd).HasColumnName("MaCD");
            entity.Property(e => e.TenChuDe).HasMaxLength(100);
        });

        modelBuilder.Entity<Dondathang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__DONDATHA__129584ADD2657F76");

            entity.ToTable("DONDATHANG");

            entity.Property(e => e.MaKh).HasColumnName("maKH");
            entity.Property(e => e.Ngaygiao).HasColumnName("ngaygiao");
            entity.Property(e => e.Tinhtranggiaohang).HasMaxLength(50);

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Dondathangs)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__DONDATHANG__maKH__47DBAE45");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KHACHHAN__7A3ECFE46A6FD233");

            entity.ToTable("KHACHHANG");

            entity.Property(e => e.MaKh).HasColumnName("maKH");
            entity.Property(e => e.DiachiKh)
                .HasMaxLength(200)
                .HasColumnName("DiachiKH");
            entity.Property(e => e.DienthoaiKh)
                .HasMaxLength(20)
                .HasColumnName("DienthoaiKH");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.IdQuyen).HasColumnName("idQuyen");
            entity.Property(e => e.Matkhau).HasMaxLength(50);
            entity.Property(e => e.Taikhoan).HasMaxLength(50);

            entity.HasOne(d => d.IdQuyenNavigation).WithMany(p => p.Khachhangs)
                .HasForeignKey(d => d.IdQuyen)
                .HasConstraintName("FK__KHACHHANG__idQuy__3D5E1FD2");
        });

        modelBuilder.Entity<Nhaxuatban>(entity =>
        {
            entity.HasKey(e => e.MaNxb).HasName("PK__NHAXUATB__3A19482C6CB8DBB8");

            entity.ToTable("NHAXUATBAN");

            entity.Property(e => e.MaNxb).HasColumnName("MaNXB");
            entity.Property(e => e.Diachi).HasMaxLength(200);
            entity.Property(e => e.Dienthoai).HasMaxLength(20);
            entity.Property(e => e.TenNxb)
                .HasMaxLength(100)
                .HasColumnName("TenNXB");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3213E83F6FD4BC76");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TenQuyen).HasMaxLength(50);
        });

        modelBuilder.Entity<Sach>(entity =>
        {
            entity.HasKey(e => e.Masach).HasName("PK__SACH__9F923C88D3F6BACD");

            entity.ToTable("SACH");

            entity.Property(e => e.Anhbia).HasMaxLength(200);
            entity.Property(e => e.Giaban).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MaCd).HasColumnName("MaCD");
            entity.Property(e => e.MaNxb).HasColumnName("MaNXB");
            entity.Property(e => e.Mota).HasMaxLength(500);
            entity.Property(e => e.Ngaycapnhat).HasColumnName("ngaycapnhat");
            entity.Property(e => e.Tensach).HasMaxLength(100);

            entity.HasOne(d => d.MaCdNavigation).WithMany(p => p.Saches)
                .HasForeignKey(d => d.MaCd)
                .HasConstraintName("FK__SACH__MaCD__403A8C7D");

            entity.HasOne(d => d.MaNxbNavigation).WithMany(p => p.Saches)
                .HasForeignKey(d => d.MaNxb)
                .HasConstraintName("FK__SACH__MaNXB__412EB0B6");
        });

        modelBuilder.Entity<Tacgium>(entity =>
        {
            entity.HasKey(e => e.MaTg).HasName("PK__TACGIA__272500744B349F63");

            entity.ToTable("TACGIA");

            entity.Property(e => e.MaTg).HasColumnName("MaTG");
            entity.Property(e => e.Diachi).HasMaxLength(200);
            entity.Property(e => e.Dienthoai).HasMaxLength(20);
            entity.Property(e => e.TenTg)
                .HasMaxLength(100)
                .HasColumnName("TenTG");
            entity.Property(e => e.Tieusu).HasMaxLength(500);
        });

        modelBuilder.Entity<Vietsac>(entity =>
        {
            entity.HasKey(e => new { e.MaTg, e.Masach }).HasName("PK__VIETSAC__BEDC23BCC49286DC");

            entity.ToTable("VIETSAC");

            entity.Property(e => e.MaTg).HasColumnName("MaTG");
            entity.Property(e => e.Vaitro)
                .HasMaxLength(50)
                .HasColumnName("vaitro");
            entity.Property(e => e.Vitri).HasMaxLength(50);

            entity.HasOne(d => d.MaTgNavigation).WithMany(p => p.Vietsacs)
                .HasForeignKey(d => d.MaTg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VIETSAC__MaTG__5070F446");

            entity.HasOne(d => d.MasachNavigation).WithMany(p => p.Vietsacs)
                .HasForeignKey(d => d.Masach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VIETSAC__Masach__5165187F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
