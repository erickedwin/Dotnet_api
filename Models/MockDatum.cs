using System;
using System.Collections.Generic;


namespace apinetcore5.Models
{
    public partial class MockDatum
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? Price { get; set; }
        public decimal? Lowprices { get; set; }
    }
}
