namespace RingTube.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ringtone")]
    public partial class ringtone
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ringtone()
        {
            purchaseds = new HashSet<purchased>();
            ringtoneTags = new HashSet<ringtoneTag>();
            shoppingCarts = new HashSet<shoppingCart>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [Required]
        [StringLength(500)]
        public string dis { get; set; }

        [Required]
        [StringLength(50)]
        public string auther { get; set; }

        [Required]
        [StringLength(500)]
        public string url { get; set; }

        public int urlCutID { get; set; }

        public double Price { get; set; }

        public int catID { get; set; }

        public virtual category category { get; set; }

        public virtual cutRingtone cutRingtone { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<purchased> purchaseds { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ringtoneTag> ringtoneTags { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<shoppingCart> shoppingCarts { get; set; }
    }
}
