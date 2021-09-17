﻿namespace MAV.Web.Data.Entities
{
    using System.Collections.Generic;
    public class Loan
    {
        public int Id { get; set; }
        public Applicant Applicant { get; set; }
        public Intern Intern { get; set; }
        public ICollection<LoanDetail> LoanDetails { set; get; }

    }
}
