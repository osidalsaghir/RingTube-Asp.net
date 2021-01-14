namespace RingTube.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RingTubeModel : DbContext
    {
        public RingTubeModel()
            : base("name=RingTubeDB")
        {
        }

        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<cutRingtone> cutRingtones { get; set; }
        public virtual DbSet<favList> favLists { get; set; }
        public virtual DbSet<purchased> purchaseds { get; set; }
        public virtual DbSet<ringtone> ringtones { get; set; }
        public virtual DbSet<ringtoneTag> ringtoneTags { get; set; }
        public virtual DbSet<shoppingCart> shoppingCarts { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tag> tags { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<category>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<category>()
                .HasMany(e => e.ringtones)
                .WithRequired(e => e.category)
                .HasForeignKey(e => e.catID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<cutRingtone>()
                .HasMany(e => e.ringtones)
                .WithRequired(e => e.cutRingtone)
                .HasForeignKey(e => e.urlCutID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ringtone>()
                .HasMany(e => e.purchaseds)
                .WithRequired(e => e.ringtone)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ringtone>()
                .HasMany(e => e.ringtoneTags)
                .WithRequired(e => e.ringtone)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ringtone>()
                .HasMany(e => e.shoppingCarts)
                .WithRequired(e => e.ringtone)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tag>()
                .HasMany(e => e.ringtoneTags)
                .WithRequired(e => e.tag)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.favLists)
                .WithRequired(e => e.user)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.purchaseds)
                .WithRequired(e => e.user)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.shoppingCarts)
                .WithRequired(e => e.user)
                .WillCascadeOnDelete(false);
        }
    }
}
