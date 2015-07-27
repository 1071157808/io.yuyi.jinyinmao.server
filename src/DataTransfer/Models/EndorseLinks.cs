namespace DataTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EndorseLinks
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductIdentifier { get; set; }

        [Required]
        [StringLength(300)]
        public string EndorseImageLink { get; set; }

        [Required]
        [StringLength(300)]
        public string EndorseImageThumbnailLink { get; set; }
    }
}
