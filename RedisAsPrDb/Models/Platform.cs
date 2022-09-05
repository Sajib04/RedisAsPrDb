using System;
using System.ComponentModel.DataAnnotations;

namespace RedisAsPrDb.Models
{
    public class Platform
    {
        [Required]
        public string Id { get; set; } = $"Platform:{Guid.NewGuid().ToString()}";
        [Required]
        public string Name { get; set; } = String.Empty;
    }
}
