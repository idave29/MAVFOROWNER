﻿namespace MAV.Common.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class Loan
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        //[JsonProperty("applicant")]
        //public object Applicant { get; set; }

        [JsonProperty("intern")]
        public object Intern { get; set; }

        [JsonProperty("loanDetails")]
        public ICollection<LoanDetails> LoanDetails { get; set; }
    }
}
