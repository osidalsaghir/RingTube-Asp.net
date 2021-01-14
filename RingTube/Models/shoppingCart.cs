namespace RingTube.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("shoppingCart")]
    public partial class shoppingCart
    {
        public int id { get; set; }

        public int userID { get; set; }

        public int ringtoneID { get; set; }

        public virtual ringtone ringtone { get; set; }

        public virtual user user { get; set; }
    }
}
