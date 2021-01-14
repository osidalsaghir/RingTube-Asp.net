namespace RingTube.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ringtoneTag")]
    public partial class ringtoneTag
    {
        public int id { get; set; }

        public int tagID { get; set; }

        public int ringtoneID { get; set; }

        public virtual ringtone ringtone { get; set; }

        public virtual tag tag { get; set; }
    }
}
